using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Core.Model;
using THSurveys.Models.Question;
using AutoMapper;

namespace THSurveys.Filters
{
    public class MapQuestionToAddQuestionsListviewModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Question[] questions = (Question[]) filterContext.Controller.ViewData.Model;
            IEnumerable<AddQuestionsListViewModel> viewModel = Mapper.Map<Question[], IEnumerable<AddQuestionsListViewModel>>(questions);
            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}