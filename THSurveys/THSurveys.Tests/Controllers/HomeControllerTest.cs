using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;                          //  Mocking library

using THSurveys;
using THSurveys.Controllers;
using THSurveys.Models.Home;
using Core.Interfaces;
using Core.Model;
using THSurveys.Tests.Model;


namespace THSurveys.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private MockData mockdata = new MockData();

        [TestMethod]
        public void IndexReturnsListOfSurveys()
        {
            // Arrange
            HomeController controller = ArrangeController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.AreEqual("Top 5 most popular Surveys", result.ViewBag.Title);
            //  Assert the correct type                                             Check the correct type of data.
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Survey[]), "Expected an Survey[]");
            //  Retrieve the data model from the view                               Extract the data from the view.
            var model = result.ViewData.Model as IEnumerable<Survey>;
            //  Assert the corrent number of surveys in the model                   Check the amount of data.
            Assert.AreEqual(model.Count(), 2, "Wrong number of surveys returned");
            //  Assert the first element has the correct surveyId.                  Check the first data record.
            Assert.AreEqual(3, model.FirstOrDefault<Survey>().SurveyId, "First live survey is Not SurveyId of 3.");
        }

        [TestMethod]
        public void IndexReturnsEmptyViewIfNoData()
        {
            // Arrange
            HomeController controller = ArrangeController(true);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.AreEqual("Top 5 most popular Surveys", result.ViewBag.Title);
            //  Assert the correct type                                             Check the correct type of data.
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Survey[]), "Expected an Survey[]");
            //  Retrieve the data model from the view                               Extract the data from the view.
            var model = result.ViewData.Model as IEnumerable<Survey>;
            //  Assert the corrent number of surveys in the model                   Check the amount of data.
            Assert.AreEqual(model.Count(), 0, "Wrong number of surveys returned");
        }


        [TestMethod]
        public void TakeRedirectsToThankYouWithResultsOnSuccess()
        {
            //  Arrange
            //      Instantiate the controller, with the necessary mocked dependencies.
            HomeController controller = ArrangeController();
            //      Input:  This can be null as we don't test the fields in the mapping, this is mocked instead.
            TakeSurveyViewModel viewModel = new TakeSurveyViewModel();

            //  Act
            RedirectToRouteResult result = controller.Take(viewModel) as RedirectToRouteResult;

            //  Assert
            //      Check result is actually returned.
            Assert.IsNotNull(result, "No redirect result returned");
            //      Check the route values, not the Controller, it's defaulted to the current: Home.
            Assert.AreEqual("ThankYou", result.RouteValues["action"], "Expected the route action to be 'ThankYou'");
            Assert.AreEqual(3L, result.RouteValues["id"], "Expected the Id to be '3'");
        }


        [TestMethod]
        public void TakeReturnsViewIfModelInvalid()
        {
            //  Arrange
            //      Instantiate the controller, with the necessary mocked dependencies.
            HomeController controller = ArrangeController();
            //      Input:  This can be null as we don't test the fields in the mapping, this is mocked instead.
            TakeSurveyViewModel viewModel = new TakeSurveyViewModel();

            //  Introduce an error to the modelstate so a test to return the view is possible
            controller.ModelState.AddModelError("", "An error condition to set the modelstate.isvalid to false");

            ViewResult result = controller.Take(viewModel) as ViewResult;

            //  Assert
            //      Check a review is actually returned
            Assert.IsNotNull(result, "Expected a view to be returned");
            //      Check the viewName is ""
            Assert.AreEqual("", result.ViewName, "incorrect view returned");
            //      Check the model type
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(TakeSurveyViewModel), "Incorrect ViewData Model returned");
            //      Check the valud of the surveyid
            TakeSurveyViewModel model = (TakeSurveyViewModel)result.ViewData.Model;
            Assert.AreEqual(3L, model.SurveyId, "Incorrect data returned in the ViewData model");
        }

        
        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = ArrangeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = ArrangeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }



        private HomeController ArrangeController(bool ReturnEmptyData = false)
        {
            //  Mock the repositories (dependencies of the controller)
            var mockSurveyRepository = new Mock<ISurveyRepository>();
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            var mockQuestionRepository = new Mock<IQuestionRepository>();
            var mockRespondentRepository = new Mock<IRespondentRepository>();
            var mockTakeSurveyViewModelMapper = new Mock<THSurveys.Infrastructure.Interfaces.IMapTakeSurveyViewModel>();
            var mockReinstateTakeSurveyVMMapper = new Mock<THSurveys.Infrastructure.Interfaces.IReinstateTakeSurveyViewModel>();

            //  Setup the Mock behaviour
            if (!ReturnEmptyData)
            {
                //  Mock the necessary calls for the Home/Index controller action
                mockSurveyRepository.Setup(r => r.GetTopTenSurveys()).Returns(mockdata.GetLiveSurveys());

                //  Mock dependencies for the Home/Take controller test.
                //  The respository update returns a void so we need return nothing, but still need to be able to mock the call.
                mockSurveyRepository.Setup(r => r.UpdateSurveys(It.IsAny<Survey[]>()));     //  UpdateSurveys return is 'void'.
                //  Mock the mapped survey with a Survey class.  No fields are referenced in the Take controller
                //  so it doesn't matter what Survey class is returned.  Therefore we'll return survey 3.
                mockTakeSurveyViewModelMapper.Setup(m => m.Map(It.IsAny<TakeSurveyViewModel>())).Returns(mockdata.GetSurvey3());
                //  Mock the reinstated survey with a TakeSurveyViewModel, we can provide the same viewmodel as used in the 
                //  input to the Map tests.  We can check only the fields that are populated as independent test confirm that
                //  tha mapping process works.  therefore, in this test we're only checking that the correct view is returned
                //  and don't need to check all details of the viewmodel.
                mockReinstateTakeSurveyVMMapper.Setup(m => m.Map(It.IsAny<TakeSurveyViewModel>())).Returns(mockdata.SetTakeSurveyViewModel_3());
            }
            else
            {
                //  Mock the necessary calls for the Home/Index controller action where there are no surveys in the model.
                mockSurveyRepository.Setup(r => r.GetTopTenSurveys()).Returns(new List<Core.Model.Survey>().AsQueryable<Core.Model.Survey>());
            }

            /// Instantiate the controller
            return new HomeController(mockSurveyRepository.Object,
                mockCategoryRepository.Object, mockQuestionRepository.Object, mockRespondentRepository.Object,
                mockTakeSurveyViewModelMapper.Object, mockReinstateTakeSurveyVMMapper.Object);
        }











    }
}
