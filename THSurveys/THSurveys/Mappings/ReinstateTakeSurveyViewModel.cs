using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using Core.Interfaces;
using THSurveys.Models.Home;

namespace THSurveys.Mappings
{
    public static class ReinstateTakeSurveyViewModel
    {
        public static TakeSurveyViewModel Map(TakeSurveyViewModel responses, ISurveyRepository surveyRepository, IQuestionRepository questionRepository)
        {
            Survey survey = surveyRepository.GetSurvey(responses.SurveyId);

            TakeSurveyViewModel viewModel = new TakeSurveyViewModel();

            //  Map the survey to the survey view model.
            viewModel.SurveyId = survey.SurveyId;
            viewModel.Title = survey.Title;
            viewModel.UserName = survey.User.UserName;
            viewModel.StatusDate = string.Format("{0:d}", survey.StatusDate);
            viewModel.CategoryDescription = survey.Category.Description;

            //  Map the questions to the view model
            viewModel.Questions = new List<SurveyQuestionsViewModel>();

            //foreach (Core.Model.Question q in survey.Questions)
            foreach (SurveyQuestionsViewModel q in responses.Questions)
            {
                //  Get the question and available responses from the repository.
                Core.Model.Question questionInfo = questionRepository.GetQuestion(Convert.ToInt64(q.QId_SeqNo.Split('_')[0]));

                SurveyQuestionsViewModel questionViewModel = new SurveyQuestionsViewModel();
                questionViewModel.QId_SeqNo = q.QId_SeqNo;

                questionViewModel.SequenceNumber = questionInfo.SequenceNumber;
                questionViewModel.Text = questionInfo.Text;
                questionViewModel.Answer = q.Answer;

                //  Map the responses to the question
                questionViewModel.Responses = new List<SurveyResponsesViewModel>();
                //  this causes a DBContext Disposed error if it's visited a 2nd time.
                foreach (Core.Model.AvailableResponse r in questionInfo.AvailableResponses)
                {
                    SurveyResponsesViewModel responsesViewModel = new SurveyResponsesViewModel();
                    responsesViewModel.QId_SeqNo = q.QId_SeqNo;
                    responsesViewModel.Answer = q.Answer.ToString();
                    responsesViewModel.LikertScaleNumber = r.LikertScaleNumber;
                    responsesViewModel.Text = r.Text;
                    questionViewModel.Responses.Add(responsesViewModel);
                }
                viewModel.Questions.Add(questionViewModel);
            }
            return viewModel;
        }
    }
}