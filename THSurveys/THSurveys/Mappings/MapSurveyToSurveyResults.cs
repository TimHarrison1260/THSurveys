using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using Core.Interfaces;
using THSurveys.Models.Shared;

namespace THSurveys.Mappings
{
    public static class MapSurveyToSurveyResults
    {
        public static SurveyResultsViewModel MapLatestResult(Survey survey,IRespondentRepository respondentRepository, IQuestionRepository questionRepository)
        {
            //  Get the latest respondents answers.
            ICollection<Respondent> latestRespondents = new List<Respondent>();
            latestRespondents.Add(respondentRepository.GetLatestResultForSurvey(survey.SurveyId));

            //  Instantiate the view model
            SurveyResultsViewModel surveyResults = new SurveyResultsViewModel();
            //  map the Survey information
            surveyResults.SurveyId = survey.SurveyId;
            surveyResults.Title = survey.Title;
            //  Map the results information.
            surveyResults.Results = MapResponses(latestRespondents, questionRepository);

            return surveyResults;
        }

        public static SurveyResultsViewModel MapAllResults(Survey survey, IQuestionRepository questionRepository)
        {
            //  Instantiate the view model
            SurveyResultsViewModel surveyResults = new SurveyResultsViewModel();
            //  map the Survey information
            surveyResults.SurveyId = survey.SurveyId;
            surveyResults.Title = survey.Title;
            //  Map the results information.
            surveyResults.Results = MapResponses(survey.Respondents, questionRepository);

            return surveyResults;
        }

        private static IList<SurveyResultsDetailsviewModel> MapResponses(ICollection<Respondent> Respondents, IQuestionRepository questionRepository)
        {
            IList<SurveyResultsDetailsviewModel> results = new List<SurveyResultsDetailsviewModel>();

            foreach (Core.Model.Respondent respondent in Respondents)
            {
                SurveyResultsDetailsviewModel result = new SurveyResultsDetailsviewModel();
                result.DateTaken = string.Format("{0:d}", respondent.DateTaken.ToString());
                result.Answers = new List<SurveyResultsAnswersViewModel>();

                foreach (Core.Model.ActualResponse response in respondent.Responses)
                {
                    //  Get the descriptions for the questions and corresponding answer
                    string[] resultDescriptions = questionRepository.GetQuestionAndAnswerDescriptions(response.Question, response.Response);
                    //  Instatiate and populate the result
                    SurveyResultsAnswersViewModel answer = new SurveyResultsAnswersViewModel();
                    answer.QuestionNumber = Convert.ToInt64(resultDescriptions[0]);
                    answer.QuestionText = resultDescriptions[1];
                    answer.AnswerNumber = response.Response;
                    answer.AnswerText = resultDescriptions[2];
                    //  Add the result to the answers
                    result.Answers.Add(answer);
                }
                results.Add(result);
            }
            return results;
        }
    }
}