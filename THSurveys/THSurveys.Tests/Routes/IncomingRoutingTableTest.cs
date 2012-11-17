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
using System.Reflection;

namespace THSurveys.Tests.Routes
{
    [TestClass]
    public class IncomingRoutingTableTest
    {
        [TestMethod]
        public void In_Url_Root_And_blank_Maps_To_HomePage()
        {
            TestIncomingRouteOK("~/", "Home", "Index");
            TestIncomingRouteOK("~/Home/Index", "Home", "Index");
            TestIncomingRouteOK("~/Home/Contact", "Home", "Contact");
            TestIncomingRouteOK("~/Home/About", "Home", "About");
            TestIncomingRouteOK("~/Survey/Top5", "Home", "Index");
        }

        [TestMethod]
        public void In_Url_Basic_Survey_Maps_To_Survey_Controller()
        {
            TestIncomingRouteOK("~/Survey/List", "Survey", "List");
            TestIncomingRouteOK("~/Survey/Create", "Survey", "Create");
            TestIncomingRouteOK("~/Survey/Approve", "Survey", "Approve");
        }

        [TestMethod]
        public void In_Url_Approve_Survey_Post_Maps_To_SurveySubmit()
        {
            TestIncomingRouteOK("~/Survey/Approve/5", "Survey", "Approve", new { id = 5 }, "POST");
        }

        [TestMethod]
        public void In_Url_Question_Post_Maps_To_QuestionController()
        {
            //  This would be an ajax called method
            TestIncomingRouteOK("~/Question/GetLikertResponses/4", "Question", "GetLikertResponses", new { id = 4 }, "POST");
            TestIncomingRouteOK("~/Question/GetQuestionsForSurvey/5", "Question", "GetQuestionsForSurvey", new { id = 5 }, "POST");
        }


        [TestMethod]
        public void In_Url_Get_Analysis_Chart_Maps_To_Analysis_Controller()
        {
            TestIncomingRouteOK("~/Analysis/GetSurveyChart/5", "Analysis", "GetSurveyChart", new { id = 5 });
        }


        [TestMethod]
        public void Url_SurveyChoose_Maps_To_HomeChoose()
        {
            TestIncomingRouteOK("~/Survey/Choose", "Home", "Choose");
        }

        [TestMethod]
        public void Url_SurveyTake_without_SurveyI_Drops_through_To_Catchall()
        {
            TestIncomingRouteDropsToCatchall("~/Survey/Take");
        }

        [TestMethod]
        public void Url_SurveyTakeSurveyId_numeric_Maps_To_HomeTake_PassingSurveyId()
        {
            //  Arrange
            TestIncomingRouteOK("~/Survey/Take/5", "Home", "Take", new { SurveyId = 5 });
        }

        [TestMethod]
        public void Url_SurveyTakeSurveyId_AsString_Drops_Through_To_Catchall()
        {
            TestIncomingRouteDropsToCatchall("~/Survey/Take/asurveyId");
        }

        [TestMethod]
        public void Url_Surveys_Maps_To_SurveyList()
        {
            TestIncomingRouteOK("~/Surveys", "Survey", "List");
        }

        [TestMethod]
        public void Url_SurveyList_Maps_To_SurveyList()
        {
            TestIncomingRouteOK("~/Survey/List", "Survey", "List");
        }

        [TestMethod]
        public void Url_SurveyCreate_Maps_To_SurveyCreate()
        {
            TestIncomingRouteOK("~/Survey/Create", "Survey", "Create");
        }

        [TestMethod]
        public void Url_SurveyApprove_Maps_To_SurveyApprove()
        {
            TestIncomingRouteOK("~/Survey/Approve", "Survey", "Approve");
        }

        [TestMethod]
        public void Url_SurveyAddQuestion_without_SurveyId_Drops_through_To_Catchall()
        {
            TestIncomingRouteDropsToCatchall("~/Survey/AddQuestion");
        }

        [TestMethod]
        public void Url_SurveyAddQuestion_with_text_SurveyId_Drops_through_To_Catchall()
        {
            TestIncomingRouteDropsToCatchall("~/Survey/AddQuestion/assurveyId");
        }

        [TestMethod]
        public void Url_SurveyAddQuestion_with_numeric_SurveyId_Maps_To_QuestionCreate()
        {
            TestIncomingRouteOK("~/Survey/AddQuestion/5", "Question", "Create", new { id = 5 });
        }

        [TestMethod]
        public void Url_SurveyAnalyse_without_SurveyId_Drops_through_To_Catchall()
        {
            TestIncomingRouteDropsToCatchall("~/Survey/Analyse");
        }

        [TestMethod]
        public void Url_SurveyAnalyse_with_text_SurveyId_Drops_through_To_Catchall()
        {
            TestIncomingRouteDropsToCatchall("~/Survey/Analyse/asurveyId");
        }

        [TestMethod]
        public void Url_SurveyAnalyse_with_numeric_SurveyId_Maps_To_AnalyseQuestionList()
        {
            TestIncomingRouteOK("~/Survey/Analyse/5", "Analysis", "QuestionList", new { id = 5 });
        }


        //  ************************************************************* 
        //  *   Test methods.
        //  *************************************************************

        /// <summary>
        /// Checks the Route for successful mapping.
        /// </summary>
        /// <param name="urlMask">The url being tested</param>
        /// <param name="controller">The controller name for testing</param>
        /// <param name="action">The action method name for testing</param>
        /// <param name="properties">Anonymous object containing any parameters being tested.</param>
        /// <param name="httpMethod">String representation of the HttpMethod associated with the request.</param>
        private void TestIncomingRouteOK(string urlMask, string controller, string action, object properties = null, string httpMethod = "GET")
        {
            //  Arrange
            RouteCollection routes = ArrangeRouteTest();
            HttpContextBase request = CreateMockHttpRequestContext(urlMask, httpMethod);
            //  Act
            RouteData routeData = routes.GetRouteData(request);
            //  Assert
            //  Check we have a route
            Assert.IsNotNull(routeData, "Should have found a route");
            //  Check the controller matched the expected controller
            Assert.AreEqual(controller, routeData.Values["controller"], string.Format("Should have found the '{0}' controller", controller));
            //  Check the action matched the expected action.
            Assert.AreEqual(action, routeData.Values["action"], string.Format("Should have found the '{0}' action.",action));
            //  Check any supplied properties matched
            if (!(properties == null))
            {
                //  Scan through the properties as they could contain other parms as well as any 'id' specified.
                //  Use reflection to find out what properties are passed in.
                PropertyInfo[] propertyInfo = properties.GetType().GetProperties();
                foreach (var p in propertyInfo)
                {
                    //  Check for any properties that don't match.
                    //  If the parameter is in the routeresult
                    if (routeData.Values.ContainsKey(p.Name))
                    {
                        string propertyValue = p.GetValue(properties).ToString();
                        Assert.AreEqual(propertyValue, routeData.Values[p.Name].ToString(), string.Format("Should have found the '{0}' property.", propertyValue));
                    }
                    else
                    {
                        //  We make sure this assert faile, because the required parameter is not there, so that it shows on the test.
                        Assert.IsTrue(routeData.Values.ContainsKey(p.Name), string.Format("Should have found parameter '{0}'.", p.Name));
                    }
                }
            }
        }


        private void TestIncomingRouteFailed(string urlMask, string httpMethod = "GET")
        {
            //  Arrange
            RouteCollection routes = ArrangeRouteTest();
            HttpContextBase request = CreateMockHttpRequestContext(urlMask, httpMethod);
            //  Act
            RouteData routeData = routes.GetRouteData(request);
            //  Assert
            //  Check we have either a null result of the route is null
            Assert.IsNull(routeData == null || routeData.Route == null);
        }


        private void TestIncomingRouteDropsToCatchall(string urlMask, string httpMethod = "GET")
        {
            //  Arrange
            RouteCollection routes = ArrangeRouteTest();
            HttpContextBase request = CreateMockHttpRequestContext(urlMask, httpMethod);
            //  Act
            RouteData routeData = routes.GetRouteData(request);
            //  Assert
            //  Check we have a route
            Assert.IsNotNull(routeData, "Should have found a route");
            //  Check the controller matched the expected controller
            Assert.AreEqual("Home", routeData.Values["controller"], "Should have found the 'Home' controller");
            //  Check the action matched the expected action.
            Assert.AreEqual("PageNotFound", routeData.Values["action"], "Should have found the 'PageNotFound' action.");
            //  Check any supplied properties matched
        }



        //  ************************************************************* 
        //  *   Private helper methods.
        //  *************************************************************
        
        /// <summary>
        /// Aranges the Routes table, 
        /// </summary>
        /// <returns>
        /// An instance of the RouteData from the routing 
        /// engine for the supplied mask.
        /// </returns>
        private RouteCollection ArrangeRouteTest ()
        {
            RouteCollection routes = new RouteCollection();
            //  This would reference MVC app itself but we don't want to break that
            //  so well copy the route table once they are tested and point this
            //  to the MVc app.  Meanwhile it points to the RoutingTable class within
            //  this test project.
            THSurveys.Tests.Routes.RoutingTable.registerRoutes(routes);
            return routes;
        }

        /// <summary>
        /// This creates the mock HttpRequestContext used for all 
        /// Incoming route tests.
        /// </summary>
        /// <param name="urlMask"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        private HttpContextBase CreateMockHttpRequestContext(string urlMask = null, string httpMethod = "GET")
        {
            //  Mock the HttpContext
            var mockHttpRequest = new Mock<HttpContextBase>();
            //  Mock the route from the request.
            mockHttpRequest.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath)
                .Returns(urlMask);
            mockHttpRequest.Setup(c => c.Request.HttpMethod).Returns(httpMethod);
            //  Return the RequestContext
            return mockHttpRequest.Object;
        }


        ///// <summary>
        ///// Test if the result from the GetRoutedata returned expected
        ///// results.  All components must match to return true for the
        ///// method.  Therefore it is possible to test this result for
        ///// TRUE to ensure a successful Route test.
        ///// </summary>
        ///// <param name="routeResult"></param>
        ///// <param name="controller"></param>
        ///// <param name="action"></param>
        ///// <param name="properties"></param>
        ///// <returns></returns>
        ///// <remarks>
        ///// Based upon Freeman and Sanderson,pp332 and lecture notes.  It's 
        ///// coded slightly differently so that I can understand it easier.  
        ///// </remarks>
        //private bool TestIncomingRouteResult(RouteData routeResult, string controller, string action, object properties = null)
        //{
        //    //  Check the controller returned.
        //    bool controllerResult = String.Equals(routeResult.Values["controller"].ToString(), controller,StringComparison.InvariantCultureIgnoreCase);
        //    //  Check the action returned.
        //    bool actionResult = String.Equals(routeResult.Values["action"].ToString(), action, StringComparison.InvariantCultureIgnoreCase);
        //    //  check any properties supplied
        //    bool propertiesResult = true;
        //    if (properties == null)
        //    {
        //        //  Scan through the properties as they could contain other parms as well as any 'id' specified.
        //        //  Use reflection to find out what properties are passed in.
        //        PropertyInfo[] propertyInfo = properties.GetType().GetProperties();
        //        foreach (var p in propertyInfo)
        //        {
        //            //  Check for any properties that don't match.
        //            //  If the parameter is in the routeresult
        //            if (routeResult.Values.ContainsKey(p.Name))
        //            {
        //                propertiesResult = String.Equals(routeResult.Values[p.Name].ToString(), p.GetValue(properties).ToString(), StringComparison.InvariantCultureIgnoreCase);
        //                if (!propertiesResult)
        //                    break;
        //            }
        //        }
        //    }
        //    //  All must be true 
        //    return (controllerResult && actionResult && propertiesResult);
        //}

    }
}
