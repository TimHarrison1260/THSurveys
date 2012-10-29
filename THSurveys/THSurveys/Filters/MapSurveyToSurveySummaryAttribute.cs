using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;           //  Access to Action filters
using AutoMapper;               //  Access to Mapping tool
using Core.Model;               //  Access to the Domain model
using THSurveys.Models;         //  Access to the ViewModels.

namespace THSurveys.Filters
{
    public class MapSurveyToSurveySummaryAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Filter to apply the mapping between the Core.Survey business object
        /// and the Model.TopTenSurveysViewModel.  It uses Automapper to perform
        /// the mapping.  The configuration of Automapper is in the Mapping folder
        /// </summary>
        /// <param name="filterContext">The filterContext returned by the Action method, containing the Controller viewData.</param>
        /// <remarks>
        /// We override the OnActionExecuted so that the mapping is done AFTER the
        /// action method has executed.  The action method returns the Domain,Order
        /// object and this filter passes the mapped Model.OrderDetailViewModel 
        /// to the view prior to it being rendered.
        /// </remarks>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //  Get the view data from the Controller ViewData
            //List<Core.Model.Survey> surveys = (List<Core.Model.Survey>)filterContext.Controller.ViewData.Model;
            //var surveys = new Survey[1];
            var surveys = (Survey[])filterContext.Controller.ViewData.Model;

            //  create an instance of the viewmodel class and map the incomming data to it
            IList<SurveySummaryViewModel> viewModel = Mapper.Map<Survey[], IList<SurveySummaryViewModel>>(surveys);
            
            //replace the viewData in the filterContext.
            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}