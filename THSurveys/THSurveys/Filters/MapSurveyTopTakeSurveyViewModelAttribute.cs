using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Model;
using Core.Interfaces;
using THSurveys.Models.Home;

namespace THSurveys.Filters
{
    public class MapSurveyTopTakeSurveyViewModelAttribute : ActionFilterAttribute
    {
        //  inject a reference to the Questions Repository (Ninject Dependency resolver);
        private IQuestionRepository _questionRepository = (IQuestionRepository) DependencyResolver.Current.GetService(typeof(IQuestionRepository));

        /// <summary>
        /// Map the Survey to the TakeSurveyViewModel.
        /// </summary>
        /// <remarks>
        /// This mapping is done explicitly as the target structure is quite complex
        /// and the amount of exceptions required if AutoMapper were used would not
        /// have a significant effect on the amount of code.
        /// Additionally, the top level represents a single survey, but the
        /// lower levels represent collections of objects.
        /// </remarks>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Survey survey = (Survey) filterContext.Controller.ViewData.Model;
            
            TakeSurveyViewModel viewModel = new TakeSurveyViewModel();

            //  Map the survey to the survey view model.
            viewModel.SurveyId = survey.SurveyId;
            viewModel.Title = survey.Title;
            viewModel.UserName = survey.User.UserName;
            viewModel.StatusDate = string.Format("{0:d}", survey.StatusDate);
            viewModel.CategoryDescription = survey.Category.Description;

            //  Map the questions to the view model
            viewModel.Questions = new List<SurveyQuestionsViewModel>();
            foreach (Core.Model.Question q in survey.Questions)
            {
                SurveyQuestionsViewModel questionViewModel = new SurveyQuestionsViewModel();
                questionViewModel.QuestionId = q.QuestionId;
                questionViewModel.SequenceNumber = q.SequenceNumber;
                questionViewModel.Text = q.Text;

                //  Map the responses to the question
                questionViewModel.Responses = new List<SurveyResponsesViewModel>();
                //  this causes a DBContext Disposed error if it's visited a 2nd time.
                foreach (Core.Model.AvailableResponse r in _questionRepository.GetResponsesForQuestion(q.QuestionId))
                {
                    SurveyResponsesViewModel responsesViewModel = new SurveyResponsesViewModel();
                    //responsesViewModel.SurveyId = survey.SurveyId;
                    responsesViewModel.Id = q.QuestionId;
                    responsesViewModel.LikertScaleNumber = r.LikertScaleNumber;
                    responsesViewModel.Text = r.Text;
                    //responsesViewModel.Selected = false;
                    questionViewModel.Responses.Add(responsesViewModel);
                }
                viewModel.Questions.Add(questionViewModel);
            }
            
            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}