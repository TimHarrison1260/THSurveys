using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Core.Model;
using THSurveys.Models.Survey;

namespace THSurveys.Filters
{
    public class MapSurveyToApprovalListViewModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            IEnumerable<Survey> surveys = (IEnumerable<Survey>)filterContext.Controller.ViewData.Model;
            IEnumerable<ApprovalListViewModel> viewModel = Mapper.Map<IEnumerable<Survey>, IEnumerable<ApprovalListViewModel>>(surveys);
            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}