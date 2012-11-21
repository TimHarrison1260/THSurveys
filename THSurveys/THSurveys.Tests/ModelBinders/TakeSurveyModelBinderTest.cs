using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting; //  Unit Testing modules
using System.Web;                                   //  HttpContextBase
using System.Web.Mvc;                               //  NameValueCollectionValueProvider
using THSurveys.Models.Home;                        //  TakeSurveyViewModel
using THSurveys.Infrastructure.ModelBinders;        //  TakeSurveyModelBinder
using Moq;                                          //  Moq
using System.Collections.Specialized;               //  NameValueCollection
using THSurveys.Tests.Model;                        //  MockData class


namespace THSurveys.Tests.ModelBinders
{
    [TestClass]
    public class TakeSurveyModelBinderTest
    {
        private readonly MockData _mockData = new MockData();

        [TestMethod]
        public void TakeSurveyBinderOKWithValidData()
        {
            //  Arrange
            //  1   Set up a name/value collection containing all view input values
            var formCollection = _mockData.SetTakeSurveyFormDataCompletedOK();
            //  2   Set up a ValueProvider for the name/value collection
            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            //  3   Get the Metadata from the ViewModel or class being bound
            var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(TakeSurveyViewModel));
            //  4   Instantiate the bindingContext for the model binder
            var bindingContext = new ModelBindingContext()
            {
                ModelName = "TakeSurvey",
                ValueProvider = valueProvider,
                ModelMetadata = metaData
            };

            //  5   Set up a mock Httpcontext and populate the Request["fieldname"].Return values
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request["SurveyId"]).Returns("3");
            mockHttpContext.Setup(c => c.Request["item.QId_SeqNo"]).Returns("1_1,2_2");
            mockHttpContext.Setup(c => c.Request["LikertScaleNumber_1_1"]).Returns("2");
            mockHttpContext.Setup(c => c.Request["LikertScaleNumber_2_2"]).Returns("3");

            //  6   Set up the ControllerContext, containing the HttpContext as as HttpContext property of the controllercontext
            //      Don't think we need to do this bit here, the formcollection has everything needed already, nothing needs to
            //      be supplied in addition.
            ControllerContext controllerContext = new ControllerContext();
            controllerContext.HttpContext = mockHttpContext.Object;

            //  7   Instantiate the model Binder taking the ControllerContext and BindingContext as its parameters
            var binder = new TakeSurveyModelbinder();

            //  Act
            //  8   Execute the bindingContext BindModel method to test the binder
            var result = (TakeSurveyViewModel) binder.BindModel(controllerContext, bindingContext);

            //  Assert
            //  9   Check the values of the output viewModel / class
            Assert.AreEqual(3, result.SurveyId, "Expected Survey3 to be returned");
            Assert.AreEqual("2", result.Questions.First().Answer, "Expected question1 to have answer 2");
            Assert.AreEqual("1_1", result.Questions.First().QId_SeqNo, "Expected question1 to have QId_seqNo of '1_1'");
            Assert.AreEqual("3", result.Questions.Last().Answer, "Expected question2 to have answer 3");
            Assert.AreEqual("2_2", result.Questions.Last().QId_SeqNo, "Expected question2 to have QId_SeqNo of '2_2'");
        }


        [TestMethod]
        public void TakeSurveySetsModelStateToFalseWithInvalidData()
        {
            //  Arrange
            //  1   Set up a name/value collection containing all view input values
            var formCollection = _mockData.SetTakeSurveyFormDataCompletedOK();
            //  2   Set up a ValueProvider for the name/value collection
            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            //  3   Get the Metadata from the ViewModel or class being bound
            var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(TakeSurveyViewModel));
            //  4   Instantiate the bindingContext for the model binder
            var bindingContext = new ModelBindingContext()
            {
                ModelName = "TakeSurvey",
                ValueProvider = valueProvider,
                ModelMetadata = metaData
            };

            //  5   Set up a mock Httpcontext and populate the Request["fieldname"].Return values
            //          with no response for question 1
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request["SurveyId"]).Returns("3");
            mockHttpContext.Setup(c => c.Request["item.QId_SeqNo"]).Returns("1_1,2_2");
//            mockHttpContext.Setup(c => c.Request["LikertScaleNumber_1_1"]).Returns("2");
            mockHttpContext.Setup(c => c.Request["LikertScaleNumber_2_2"]).Returns("3");

            //  6   Set up the ControllerContext, containing the HttpContext as as HttpContext property of the controllercontext
            //      Don't think we need to do this bit here, the formcollection has everything needed already, nothing needs to
            //      be supplied in addition.
            ControllerContext controllerContext = new ControllerContext();
            controllerContext.HttpContext = mockHttpContext.Object;

            //  7   Instantiate the model Binder taking the ControllerContext and BindingContext as its parameters
            var binder = new TakeSurveyModelbinder();

            //  Act
            //  8   Execute the bindingContext BindModel method to test the binder
            var result = (TakeSurveyViewModel)binder.BindModel(controllerContext, bindingContext);

            //  Assert
            //  9   Check the values of the output viewModel / class
            Assert.AreEqual(3, result.SurveyId, "Expected Survey3 to be returned");
            Assert.AreEqual("3", result.Questions.Last().Answer, "Expected question2 to have answer 3");
            Assert.AreEqual("2_2", result.Questions.Last().QId_SeqNo, "Expected question2 to have QId_SeqNo of '2_2'");

            //  Check the contents of the ModelState.
            var modelErrors = bindingContext.ModelState.Values.First();     //  Gives the Model level errors, if any
            Assert.IsFalse(bindingContext.ModelState.IsValid, "Expected the ModelState to be invalid");
            Assert.AreEqual(1, modelErrors.Errors.Count(), "Expected one Model Error (key='') in the ModelState");
            var theError = modelErrors.Errors.First();      // OK as we only have one error anyway, confirmed by previous Assert.
            Assert.AreEqual("You have not answered question 1", theError.ErrorMessage, "Expected message 'You have not answered question 1'");

        }



    }
}
