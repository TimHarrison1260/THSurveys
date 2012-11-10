using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Core.Interfaces;
using Core.Factories;
using Core.Model;
using THSurveys.Models.Home;

namespace THSurveys.Mappings
{
    public static class MapTakeSurveyViewModelToSurvey
    {
        public static Survey Map(TakeSurveyViewModel Responses, ISurveyRepository surveyRepository)
        {
            //  Update the results of the survey in the db
            var surveyWithResults = surveyRepository.GetSurvey(Responses.SurveyId);
            //  create ne respondent
            Respondent respondent = RespondentFactory.Create();
            respondent.Survey = surveyWithResults;
            //  add all the questions and answers to the actual resonses
            foreach (SurveyQuestionsViewModel q in Responses.Questions)
            {
                var answer = ActualResponseFactory.Create();
                answer.Question = Convert.ToInt64(q.QId_SeqNo.Split('_')[0]);
                answer.Response = Convert.ToInt64(q.Answer);
                answer.Respondent = respondent;
                //  Add answer to actual responses
                respondent.Responses.Add(answer);
            }

            //  Now dd the respondent and the anwsers to the survey
            surveyWithResults.Respondents.Add(respondent);

            return surveyWithResults;
        }
    }
}