using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Core.Interfaces;
using Core.Factories;
using Core.Model;
using THSurveys.Models.Home;
using THSurveys.Infrastructure.Interfaces;

namespace THSurveys.Mappings
{
    public class MapTakeSurveyViewModelToSurvey : IMapTakeSurveyViewModel
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly RespondentFactory _respondentFactory;
        private readonly ActualResponseFactory _actualResponseFactory;

        public MapTakeSurveyViewModelToSurvey(ISurveyRepository surveyRepository, RespondentFactory respondentFactory, ActualResponseFactory actualResponseFactory)
        {
            if (surveyRepository == null)
                throw new ArgumentNullException("SurveyRepository", "No valid survey repository supplied");
            if (respondentFactory == null)
                throw new ArgumentNullException("RespondentFactory", "No valid respondent Factory supplied");
            if (actualResponseFactory == null)
                throw new ArgumentNullException("ActualResponseFactory", "No valid actual response faction supplied");
            _respondentFactory = respondentFactory;
            _actualResponseFactory = actualResponseFactory;
            _surveyRepository = surveyRepository;
        }

        public Survey Map(TakeSurveyViewModel Responses)
        {
            //  Update the results of the survey in the db
            var surveyWithResults = _surveyRepository.GetSurvey(Responses.SurveyId);
            //  create ne respondent
            Respondent respondent = _respondentFactory.Create();
            respondent.Survey = surveyWithResults;
            //  add all the questions and answers to the actual resonses
            foreach (SurveyQuestionsViewModel q in Responses.Questions)
            {
                var answer = _actualResponseFactory.Create();
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