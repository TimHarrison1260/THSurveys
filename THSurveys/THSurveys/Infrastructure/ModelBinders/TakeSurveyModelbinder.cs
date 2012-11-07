using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using THSurveys.Models.Home;

namespace THSurveys.Infrastructure.ModelBinders
{
    public class TakeSurveyModelbinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //  Create a new model if it's not already defined.
            TakeSurveyViewModel model = (TakeSurveyViewModel)bindingContext.Model ?? new TakeSurveyViewModel();
            //  retrieve this information if it's there.  The view should always place a HiddenFor field containing
            //  any displayable data that must be displayed should the modelState be invalid.
            model.SurveyId = Convert.ToInt64(controllerContext.HttpContext.Request["SurveyId"]);
            model.Title = controllerContext.HttpContext.Request["Title"];
            model.UserName = controllerContext.HttpContext.Request["UserName"];
            model.StatusDate = controllerContext.HttpContext.Request["StatusDate"];
            model.CategoryDescription = controllerContext.HttpContext.Request["CategoryDescription"];

            model.Questions = new List<SurveyQuestionsViewModel>();

            var questions = controllerContext.HttpContext.Request["item.QuestionId"];
            string[] qs = questions.Split(',');
            foreach (var q in qs)
            {
                SurveyQuestionsViewModel question = new SurveyQuestionsViewModel();
                question.QuestionId = Convert.ToInt64(q);
                question.Text = controllerContext.HttpContext.Request["Text"]
                var responsename = "LikertScaleNumber_" + q;
                var response = controllerContext.HttpContext.Request[responsename];
                if (response == null)
                {
                    bindingContext.ModelState.AddModelError("", string.Format("You have not answered question {0}", q));
                    
                }
                else
                {

                }
                





            }


            return model;
        }
    }
}