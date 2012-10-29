using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

using Core.Model;
using THSurveys.Models;


namespace THSurveys.Filters
{
    public class MapSurveyToCreateSurveyVMAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var survey = (Survey)filterContext.Controller.ViewData.Model;

            var surveyVM = new CreateSurveyViewModel();
            base.OnActionExecuted(filterContext);
        }

    }
}