using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THSurveys.Filters
{
    /// <summary>
    /// Action Filter <c>[AjaxActionOnlyAttribute]</c> is intended to allow an
    /// action method to respond only to Ajax calls, in much the same way
    /// that [ChildActionOnlyAttribute] restricts methods to being called 
    /// as child actions.
    /// This should stop the methods being called directly from the address
    /// bar with the appropriate Url.  (Security issue)
    /// </summary>
    public class AjaxActionOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.Result = new HttpNotFoundResult();
        }
    }
}