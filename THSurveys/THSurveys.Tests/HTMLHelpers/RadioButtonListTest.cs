using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;               //  MVC String
using THSurveys.Models.Home;
using THSurveys.Helpers;
using Moq;

namespace THSurveys.Tests.HTMLHelpers
{
    [TestClass]
    public class RadioButtonListTest
    {
        [TestMethod]
        public void RadioButtonListRendersOK()
        {
            //  Arrange
            //  1   Set up an intance of the viewModelit works with
            SurveyResponsesViewModel responses = new SurveyResponsesViewModel();
            //      Populate with data
            responses.Answer = "5";
            responses.LikertScaleNumber = 1L;
            responses.QId_SeqNo = "1_1";
            responses.Text = "Strongly Disagree";

//            var mockhttpContext = new Mock<HttpContextBase>();        // not used in helper so don't need it.
            var mockViewContext = new ViewContext();
            var mockViewDataContainer = new FakeViewDataContainer();

            //  2   instantiate an HtmlHelper
            //  NB! NB! NB! NB! NB! NB! NB!
            //  This is a STRONGLY TYPED Html Helper, as therefore we need to instantiate an html helper with a model.
            //  NB! NB! NB! NB! NB! NB! NB!
            HtmlHelper<SurveyResponsesViewModel> helper = new HtmlHelper<SurveyResponsesViewModel>(mockViewContext, mockViewDataContainer);

            //  Act
            MvcHtmlString result = HtmlHelperExtensions.RadioButtonListFor(helper, (r => responses.LikertScaleNumber) , responses.LikertScaleNumber, responses.QId_SeqNo, responses.Answer, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToHtmlString().Contains("input"), "Expected input. tag");
            Assert.AreEqual(true, result.ToHtmlString().Contains("name"), "Expected name attribute");
            Assert.IsFalse(result.ToHtmlString().Contains("checked"), "Should not have been a checked attribute");
            
        }

        [TestMethod]
        public void RadioButtonListRendersChecked()
        {
            //  Arrange
            //  1   Set up an intance of the viewModelit works with
            SurveyResponsesViewModel responses = new SurveyResponsesViewModel();
            //      Populate with data
            responses.Answer = "1";
            responses.LikertScaleNumber = 1L;
            responses.QId_SeqNo = "1_1";
            responses.Text = "Strongly Disagree";

            var mockViewContext = new ViewContext();
            var mockViewDataContainer = new FakeViewDataContainer();

            //  2   instantiate an HtmlHelper
            //  NB! NB! NB! NB! NB! NB! NB!
            //  This is a STRONGLY TYPED Html Helper, as therefore we need to instantiate an html helper with a model.
            //  NB! NB! NB! NB! NB! NB! NB!
            HtmlHelper<SurveyResponsesViewModel> helper = new HtmlHelper<SurveyResponsesViewModel>(mockViewContext, mockViewDataContainer);

            //  Act
            MvcHtmlString result = HtmlHelperExtensions.RadioButtonListFor(helper, (r => responses.LikertScaleNumber), responses.LikertScaleNumber, responses.QId_SeqNo, responses.Answer, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToHtmlString().Contains("input"), "Expected input. tag");
            Assert.AreEqual(true, result.ToHtmlString().Contains("name"), "Expected name attribute");
            Assert.AreEqual(true, result.ToHtmlString().Contains("checked"), "Expected checked attribute");
        }


        private class FakeViewDataContainer : IViewDataContainer
        {
            private ViewDataDictionary _viewData = new ViewDataDictionary();

            public ViewDataDictionary ViewData { get { return _viewData; } set { _viewData = value; } }
        }
    }
}
