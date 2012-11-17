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
        public void HomeIndex_Maps_To_RootUrl()
        {
            //  Arrange and Act.
            string outgoingRoute = ArrangeAndActOutgoingRouteTest(null, "Index", "Home");

            //  Assert
            Assert.AreEqual("/Survey/Top5", outgoingRoute, "Expected the home route, '/'.");
        }

        [TestMethod]
        public void SeeWhereTheyGo()
        {
            string outgoingUrl_1 = ArrangeAndActOutgoingRouteTest(null, "Index", "Home");
            string outgoingUrl_2 = ArrangeAndActOutgoingRouteTest(null, "Index", "Survey");
            string outgoingUrl_3 = ArrangeAndActOutgoingRouteTest(null, "Approve", "Survey");
            string outgoingUrl_4 = ArrangeAndActOutgoingRouteTest(null, "About", "Home");
            string outgoingUrl_5 = ArrangeAndActOutgoingRouteTest(null, "Contact", "Home");
            string outgoingUrl_6 = ArrangeAndActOutgoingRouteTest(null, "Create", "Question");  //  Add the surveyId to it.
            string outgoingUrl_N = ArrangeAndActOutgoingRouteTest(null, "action", "controller");

            Assert.IsTrue(true);
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
        private string ArrangeAndActOutgoingRouteTest(string routeName, string action, string controller)
        {
            //  Arrange

            RouteCollection routes = new RouteCollection();
            //  This would reference MVC app itself but we don't want to break that
            //  so well copy the route table once they are tested and point this
            //  to the MVc app.  Meanwhile it points to the RoutingTable class within
            //  this test project.
            THSurveys.Tests.Routes.RoutingTable.registerRoutes(routes);

            //  Mock the HttpContext
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath)
                .Returns<string>(null);
            mockHttpContext.Setup(c => c.Request.HttpMethod)
                .Returns("GET");
            mockHttpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>()))
                .Returns<string>(s => s);
            
            //  Initialise the routeData
            RouteData routeData = new RouteData();
            //  setup the mock request, using the mockHttpContext and routedata.
            RequestContext request = new RequestContext(mockHttpContext.Object, routeData);

            //  Act
//            string result = HtmlHelper.GenerateLink(request, routes, "", routeName, "Index", "Home", null, null);
            string result = UrlHelper.GenerateUrl(routeName, action, controller, null, routes, request, false);

            return result;
        }
    }
}
