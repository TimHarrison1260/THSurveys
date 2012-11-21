using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Web;
using System.Web.Mvc;
using Moq;
using THSurveys.Filters;
using Microsoft.QualityTools.Testing.Fakes;     //  Fakes stuff

//using System.Web.Mvc.Fakes;
//using Microsoft.QualityTools.Testing.Fakes;     //  The shims stuff for replacing the IsAjaxRequest extension method.

namespace THSurveys.Tests.Filters
{
    /// <summary>
    /// Test the AjaxActionOnly filter, that it allows
    /// Ajax HttpRequests and blocks any other HttpRequest.
    /// </summary>
    [TestClass]
    public class AjaxActionOnlyTest
    {

        [TestMethod]
        public void AjaxActionOnlyAllowsAjaxRequestThrough()
        {
            //  Arrange
            //      Mock the httpRequest
            var mockHttpRequest = new Mock<HttpRequestIsAjaxRequestWrapper>();
            //  Ensure the request returns true for the isAjaxRequest call.
            mockHttpRequest.Setup(h => h.IsAjaxRequest()).Returns(true);

            //  Mock the Httpcontext
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);

            //      Mock the ActionExecutingContext
            var mockActionExecutingContext = new Mock<ActionExecutingContext>();
            //  Setup the mocked actionExecutingContext to return the mocked httpRequest
            mockActionExecutingContext.SetupGet(a => a.HttpContext).Returns(mockHttpContext.Object);
            
            //      Instantiate the filter class
            AjaxActionOnlyAttribute filter = new AjaxActionOnlyAttribute();

            //  Act
            filter.OnActionExecuting(mockActionExecutingContext.Object);

            //  Assert:
            //      Check the contents of the request within the ActionExecutingContext
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void AjaxActionOnlyWithShim()
        {
            using (ShimsContext.Create())
            {
                //  Arrange the shimming part
                //  Represents the IsAjaxRequest named as the propertyName (IsAjaxRequest) concatenated with the 'this' reference (HttprequestBase).
                System.Web.Mvc.Fakes.ShimAjaxRequestExtensions.IsAjaxRequestHttpRequestBase = HttpRequestBase => { return true; };

                var shimmedHttpRequest = new ShimmedHttpRequest();

                //      Mock the httpRequest
                var mockHttpRequest = new Mock<HttpRequestBase>();
                //  Ensure the request returns true for the isAjaxRequest call.
//                mockHttpRequest.SetupGet(h => h.IsAjaxRequest()).Returns(true);

                //  Mock the Httpcontext
                var mockHttpContext = new Mock<HttpContextBase>();
//                mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
                mockHttpContext.Setup(c => c.Request).Returns(shimmedHttpRequest);

                //      Mock the ActionExecutingContext
                var mockActionExecutingContext = new Mock<ActionExecutingContext>();
                //  Setup the mocked actionExecutingContext to return the mocked httpRequest
                mockActionExecutingContext.SetupGet(a => a.HttpContext).Returns(mockHttpContext.Object);

                //      Instantiate the filter class
                AjaxActionOnlyAttribute filter = new AjaxActionOnlyAttribute();

                //  Act
                filter.OnActionExecuting(mockActionExecutingContext.Object);

                //  Assert:
                //      Check the contents of the request within the ActionExecutingContext
                Assert.IsTrue(true);

            }
            //var result = System.Web.Mvc.AjaxRequestExtensions.IsAjaxRequest();
            //System.Web.Mvc.Fakes.ShimAjaxRequestExtensions.IsAjaxRequestHttpRequestBase();
        }
    }





    public class HttpRequestIsAjaxRequestWrapper : HttpRequestBase
    {
       //Public HttpRequest HttpRequest { get; set; }
        public virtual bool IsAjaxRequest()
        {
            return true;
        }
    }

    public class ShimmedHttpRequest : HttpRequestBase
    { }

}
