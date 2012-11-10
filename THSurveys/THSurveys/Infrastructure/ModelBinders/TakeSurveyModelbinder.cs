using System;
using System.Collections.Generic;
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

            //  retrieve the information from the view.
            model.SurveyId = Convert.ToInt64(controllerContext.HttpContext.Request["SurveyId"]);

            //  Set new questions model
            model.Questions = new List<SurveyQuestionsViewModel>();

            //  Process each quetion in the survey
            foreach (string qs in (controllerContext.HttpContext.Request["item.QId_SeqNo"]).Split(','))
            {
                //  Split questionId and SequenceNumber from input field.
                string[] str = qs.Split('_');
                string q = str[0];
                string s = str[1];
                //  Instantiate the question in the viewModel
                SurveyQuestionsViewModel question = new SurveyQuestionsViewModel();
                question.QId_SeqNo = qs;

                //  Find the selected response
                var response = controllerContext.HttpContext.Request["LikertScaleNumber_" + qs];
                if (response == null)
                {
                    //  It's not there, so add error to modelstate: the user hasn't answered the question.
                    bindingContext.ModelState.AddModelError("", string.Format("You have not answered question {0}", s));
                    bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue("LikertScaleNumber_" + qs));
                    question.Answer = "";
                }
                else
                {
                    //  Add the answer to the question viewModel.
                    question.Answer = response;
                }
                //  Add the question to the survey
                model.Questions.Add(question);
            }
            return model;
        }
    }
}