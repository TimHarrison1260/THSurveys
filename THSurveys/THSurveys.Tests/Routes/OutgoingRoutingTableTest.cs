using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace THSurveys.Tests.Routes
{
    [TestClass]
    public class OutgoingRoutingTableTest
    {
        [TestMethod]
        public void Out_Url_HomeIndex_Maps_To_RootUrl()
        {
            OutGoingRouteOK("/Survey/Top5", "Home", "Index", null);
        }

        [TestMethod]
        public void Out_Url_Home_SurveyList_CategoryId_Maps_To_Survey_Categories()
        {
            OutGoingRouteOK("/Survey/Categories/5", "Home", "SurveyList", new { id = "5" }, "GET");
        }

        [TestMethod]
        public void Out_Url_Survey_Approve_Maps_To_Survey_Approve()
        {
            OutGoingRouteOK("/Survey/Approve", "Survey", "Approve", null);
        }

        [TestMethod]
        public void Out_Url_Home_About_Maps_To_Home_About()
        {
            OutGoingRouteOK("/Home/About", "Home", "About", null);
        }

        [TestMethod]
        public void Out_Url_Home_Contact_Maps_To_Home_Contact()
        {
            OutGoingRouteOK("/Home/Contact", "Home", "Contact", null);
        }

        [TestMethod]
        public void Out_Url_Question_Create_Maps_To_Survey_AddQuestion()
        {
            OutGoingRouteOK("/Survey/AddQuestion/5", "Question", "Create", new { id = 5 });
        }

        [TestMethod]
        public void Out_Url_Analysis_QuestionList_Maps_To_Survey_Analyse()
        {
            OutGoingRouteOK("/Survey/Analyse/5", "Analysis", "QuestionList", new { id = 5 });
        }

        [TestMethod]
        public void Out_Url_Home_Take_Maps_To_Survey_Take()
        {
            OutGoingRouteOK("/Survey/Take/5", "Home", "Take", new { id = 5 });
        }

        [TestMethod]
        public void Out_Url_Survey_List_Maps_To_Surveys()
        {
            OutGoingRouteOK("/Surveys", "Survey", "List", null);
        }

        [TestMethod]
        public void Out_Url_Survey_Create_Maps_To_Survey_Create()
        {
            OutGoingRouteOK("/Survey/Create", "Survey", "Create", null);
        }

        [TestMethod]
        public void Out_Url_Question_GetLikertResponses_Maps_To_Queston_GetLikertResponses()
        {
            OutGoingRouteOK("/Question/GetLikertResponses/7", "Question", "GetLikertResponses", new { id = 7 });
        }

        [TestMethod]
        public void Out_Url_Question_GetQuestionsforSurvey_Maps_To_Question_GetQuestionsForSurvey()
        {
            OutGoingRouteOK("/Question/GetQuestionsForSurvey/5", "Question", "GetQuestionsForSurvey", new { id = 5 });
        }


        [TestMethod]
        public void Out_Url_Survey_Submit_Maps_To_Survey_Submit()
        {
            OutGoingRouteOK("/Survey/Submit/5", "Survey", "Submit", new { id = 5 });
        }

        [TestMethod]
        public void Out_Url_Home_SurveyList_Maps_To_Survey_Categories()
        {
            OutGoingRouteOK("/Survey/Categories/9", "Home", "SurveyList", new { id = 9 });
        }

        /// <summary>
        /// The special case
        /// </summary>
        [TestMethod]
        public void Out_Url_Analysis_GetSurveyChart_Maps_To_Analysis_GetSurveyChart_With_Parameters()
        {
            OutGoingRouteOK("/Analysis/GetSurveyChart/4?questionId=17", "Analysis", "GetSurveyChart", new { id = 4, questionId = 17 });
        }





        //  ************************************************************
        //  *   Unit Test Routines for OutGoing Urls
        //  ************************************************************

        private void OutGoingRouteOK(string expectedUrl, string controller, string action, object parameters, string httpMethod = "GET")
        {
            //  Arrange
            RouteCollection routes = ArrangeRouteTest();
            RequestContext context = new RequestContext(CreateMockHttpResponseContext(httpMethod), new RouteData());

            //  Act
            //            string result = HtmlHelper.GenerateLink(request, routes, "", routeName, "Index", "Home", null, null);
            string result = UrlHelper.GenerateUrl(null, action, controller, new RouteValueDictionary(parameters), routes, context, false);

            

            //  Assert
            Assert.AreEqual(expectedUrl, result, string.Format("Expected the home route, '{0}'.", expectedUrl));
        }



        /// <summary>
        /// Aranges the Routes table, mocks the HttpContext and returns
        /// the RouteData, by executing the GetRouteData method using
        /// the mock HttpContext.
        /// </summary>
        /// <param name="urlMask">The Url being tested</param>
        /// <returns>
        /// An intstance of the RouteData from the routing 
        /// engine for the supplied mask.
        /// </returns>
        private RouteCollection ArrangeRouteTest()
        {
            RouteCollection routes = new RouteCollection();
            //  This would reference MVC app itself but we don't want to break that
            //  so well copy the route table once they are tested and point this
            //  to the MVc app.  Meanwhile it points to the RoutingTable class within
            //  this test project.
            //THSurveys.Tests.Routes.RoutingTable.registerRoutes(routes);
            THSurveys.RouteConfig.RegisterRoutes(routes);

            return routes;
        }



        /// <summary>
        /// This creates the mock HttpRequestContext used for all 
        /// Incoming route tests.
        /// </summary>
        /// <param name="urlMask"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        private HttpContextBase CreateMockHttpResponseContext(string httpMethod = "GET")
        {
            //  Mock the HttpContext
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath)
                .Returns<string>(null);
            mockHttpContext.Setup(c => c.Request.HttpMethod)
                .Returns(httpMethod);
            mockHttpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>()))
                .Returns<string>(s => s);
            //  Return the RequestContext
            return mockHttpContext.Object;
        }





    }
}
