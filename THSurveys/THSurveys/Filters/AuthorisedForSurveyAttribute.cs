using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Core.Interfaces;
using Core.Model;
using Infrastructure.Migrations;

namespace THSurveys.Filters
{
    public class AuthorisedForSurveyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //  Get the instance of the survey repository
            ISurveyRepository r = (ISurveyRepository)DependencyResolver.Current.GetService<ISurveyRepository>();
            //  Is it my survey?
            bool isMySurvey = r.IsMySurvey(filterContext.HttpContext.User.Identity.Name, (long)filterContext.ActionParameters["id"]);

            if (!isMySurvey || !filterContext.HttpContext.Request.IsAuthenticated)
                //  redirect to the Not Authorised Error page.
                filterContext.Result = new ViewResult
                {
                    ViewName = "ErrorNotAuthorised"
                };
        }
    }
}