using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Core.Model;                       //  Survey model
using Core.Services;                    //  Survey Analyse service
using Core.Factories;                   //  Analyser factory
using THSurveys.Models.Analysis;        //  analysis View Model

namespace THSurveys.Mappings
{
    /// <summary>
    /// This class maps a Survey to a SurveyAnalysisViewModel 
    /// </summary>
    /// <remarks>
    /// It is a static class as there is no state information required
    /// It doesn't use automapper as the structure is complex and requires
    /// access to the Core.Service SurveyAnalysisService.
    /// </remarks>
    public static class MapSurveyToSurveyAnalysisViewModel
    {
        public static SurveyAnalysisViewModel Map(Survey survey)
        {
            //  Instantiate the analysis service
            var analyser = SurveyAnalysisServiceFactory.Create(survey);

            //  Map the survey level stuff.
            var viewModel = new SurveyAnalysisViewModel();
            viewModel.SurveyId = survey.SurveyId;
            viewModel.Title = survey.Title;
            viewModel.FirstResponseDate = analyser.DateOfFirstResponse();
            viewModel.LastResponseDate = analyser.DateOfLatestResponse();
            viewModel.NumberOfRespondents = analyser.TotalRespondents();

            //  Map the question level stuff.
            viewModel.Questions = new List<QuestionAnalysisViewModel>();
            foreach (var q in survey.Questions)
            {
                var question = new QuestionAnalysisViewModel();
                question.QuestionId = q.QuestionId;
                question.SequenceNumber = q.SequenceNumber;
                question.Text = q.Text;
                question.NumberOfResponses = analyser.TotalRespondents();

                //  Map the Response stuff.
                question.Analysis = new List<ResponseAnalysisViewModel>();
                foreach (var r in q.AvailableResponses)
                {
                    var response = new ResponseAnalysisViewModel();
                    response.QuestionId = q.QuestionId;
                    response.Text = r.Text;
                    response.LikertScaleNumber = r.LikertScaleNumber;
                    response.NumberOfResponses = analyser.TotalForQuestionResponse(response.QuestionId, response.LikertScaleNumber);
                    response.Percentage = string.Format("{0:f2}", analyser.PercentageForQuestionResponse(response.QuestionId, response.LikertScaleNumber));

                    //  Add the responses to the analysis for the question
                    question.Analysis.Add(response);
                }

                //  Add the question to the viewModel
                viewModel.Questions.Add(question);
            }

            return viewModel;
        }

    }
}