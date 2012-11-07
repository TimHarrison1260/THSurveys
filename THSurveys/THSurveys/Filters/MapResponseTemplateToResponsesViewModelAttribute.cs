using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;           //  Access to Action filters
using AutoMapper;               //  Access to Mapping tool
using Core.Model;               //  Access to the Domain model
using THSurveys.Models.Question;         //  Access to the ViewModels.

namespace THSurveys.Filters
{
    public class MapResponseTemplateToResponsesViewModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Map the Templated Responses for the Likert Scale to the Responses view Model.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var responses = (TemplateResponse[])filterContext.Controller.ViewData.Model;

            //  create an instance of the viewmodel class and map the incomming data to it
            IList<LikertResponseViewModel> viewModel = Mapper.Map<TemplateResponse[], IList<LikertResponseViewModel>>(responses);

            //replace the viewData in the filterContext.
            filterContext.Controller.ViewData.Model = viewModel;
        }
    }

}