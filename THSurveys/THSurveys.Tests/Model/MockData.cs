﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;       //  NameValueCollection
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;
using THSurveys.Models.Home;

namespace THSurveys.Tests.Model
{
    public class MockData
    {
        //  Collections to hold mock data.
        private IList<Survey> _surveys = new List<Survey>();
        private IList<Category> _categories = new List<Category>();
        private IList<Question> _questions = new List<Question>();
        private IList<AvailableResponse> _availableResponses = new List<AvailableResponse>();

        /// <summary>
        /// Ctor: populate the mock data collections
        /// </summary>
        public MockData()
        {
            setCategories();
            setSurveys();
            setQuestionsForSurvey3(_surveys.FirstOrDefault(s => s.SurveyId == 3));
        }

        /// <summary>
        /// Return Live surveys for Home/Index controller test
        /// </summary>
        /// <returns></returns>
        public IQueryable<Survey> GetLiveSurveys()
        {
            //  AsQueryable<> converts the IEnumerable<> that's returned from Linq to Objects
            //  to an IQueryable<> that is generated by Linq to Entities (SQL) which is what
            //  the repository returns, so the controller will expect it too.

            //  Use the .Where as this returns a survey.  The .Select returns a bool which
            //  cannot be converted to IQueryable<> as required by the repository.
            return _surveys.AsQueryable<Survey>().Where(s => s.Status == 2 && !s.IsTemplate);
        }

        /// <summary>
        /// Return instance of Survey class containing SurveyId of 3.  Various tests.
        /// </summary>
        /// <returns></returns>
        public Survey GetSurvey3()
        {
            var survey = _surveys.AsQueryable<Survey>().Where(s => s.SurveyId == 3).FirstOrDefault();
            return survey;
        }

        /// <summary>
        /// Returns an initialised Respondent class.
        /// </summary>
        /// <returns></returns>
        public Respondent CreateRespondent()
        {
            var respondent = new ConcreteRespondent();
            respondent.DateTaken = DateTime.Parse("19/11/2012");
            respondent.Responses = new Collection<ActualResponse>();
            return respondent;
        }

        /// <summary>
        /// REturns an initialised Response class
        /// </summary>
        /// <returns></returns>
        public ActualResponse CreateResponse()
        {
            return new ConcreteActualResponse();
        }

        /// <summary>
        /// Returns a populated TakeSurveyViewModel class
        /// </summary>
        /// <returns></returns>
        public TakeSurveyViewModel SetTakeSurveyViewModel_3()
        {
            TakeSurveyViewModel vM = new TakeSurveyViewModel();
            vM.CategoryDescription = null;
            vM.StatusDate = null;
            vM.SurveyId = 3;
            vM.Title = null;
            vM.UserName = null;
            vM.Questions = new List<SurveyQuestionsViewModel>();
            //  Add response to question1
            SurveyQuestionsViewModel response1 = new SurveyQuestionsViewModel();
            response1.Answer = "5";
            response1.QId_SeqNo="1_1";
            response1.SequenceNumber = 0;
            response1.Responses = null;
            response1.Text=null;
            vM.Questions.Add(response1);
            //  Add response to question 2
            SurveyQuestionsViewModel response2 = new SurveyQuestionsViewModel();
            response2.Answer = "5";
            response2.QId_SeqNo="1_2";
            response2.SequenceNumber = 0;
            response2.Responses = null;
            response2.Text=null;
            vM.Questions.Add(response2);
            //  Return it
            return vM;
        }

        /// <summary>
        /// Returns the populated question class.  ReinstateTakeSurveyViewModel test
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Question GetQuestion(long questionId)
        {
            var question = _questions.AsQueryable<Question>().Where(q => q.QuestionId == questionId).First();
            return question;
        }

        /// <summary>
        /// form data for TakeSurveyModelBinder - Data is complete
        /// </summary>
        /// <returns></returns>
        public NameValueCollection SetTakeSurveyFormDataCompletedOK()
        {
            //  1.  Always get SurveyId
            //  2.  Always get item.QId_SeqNo for each question (QuestionId and SequenceNumber)
            //  3.  Will get LikertScaleNumber_p_q if a response has been selected, otherwise it will not appear in the request.
            //  Will get a combination of either 2 or 2 and 3 for each question within the survey.
            var formData = new NameValueCollection()
            {
                { "SurveyId", "3"},
                { "item.QId_SeqNo", "1_1"},
                { "LikertScaleNumber_1_1", "2" },
                { "item.QId_SeqNo", "2_2" },
                { "LikertScaleNumber_2_2", "3" }
            };
            return formData;
        }







        //  ****************************************************************
        //  *   Private methods for populating the various test data.
        //  ****************************************************************

        private void setSurveys()
        {

            var survey = Core.Factories.SurveyFactory.CreateSurvey();
            survey.SurveyId = 1;
            survey.Title = "A First Survey (incomplete)";
            survey.Status = (int)SurveyStatusEnum.Incomplete;
            survey.StatusDate = DateTime.Parse("01/11/2012");
            survey.User = new UserProfile { UserName = "Tim" };
            survey.IsTemplate = false;
            survey.Category = _categories.FirstOrDefault(r => r.CategoryId == 1);
            survey.Respondents = new List<Respondent>();
            survey.Questions = new List<Question>();
            _surveys.Add(survey);

            survey = Core.Factories.SurveyFactory.CreateSurvey();
            survey.SurveyId = 2;
            survey.Title = "A Second Survey (approval)";
            survey.Status = (int)SurveyStatusEnum.Approval;
            survey.StatusDate = DateTime.Parse("02/11/2012");
            survey.User = new UserProfile { UserName = "Tim" };
            survey.IsTemplate = false;
            survey.Category = _categories.FirstOrDefault(r => r.CategoryId == 2);
            survey.Respondents = new List<Respondent>();            
            survey.Questions = new List<Question>();
            _surveys.Add(survey);

            survey = Core.Factories.SurveyFactory.CreateSurvey();
            survey.SurveyId = 3;
            survey.Title = "A Third Survey (Live)";
            survey.Status = (int)SurveyStatusEnum.Live;
            survey.StatusDate = DateTime.Parse("10/11/2012");
            survey.User = new UserProfile { UserName = "Tim" };
            survey.IsTemplate = false;
            survey.Category = _categories.FirstOrDefault(r => r.CategoryId == 1);
            survey.Respondents = new List<Respondent>();
            survey.Questions = new List<Question>();
            _surveys.Add(survey);

            survey = Core.Factories.SurveyFactory.CreateSurvey();
            survey.SurveyId = 4;
            survey.Title = "A Fourth Survey (Live)";
            survey.Status = (int)SurveyStatusEnum.Live;
            survey.StatusDate = DateTime.Parse("11/11/2012");
            survey.User = new UserProfile { UserName = "Tim" };
            survey.IsTemplate = false;
            survey.Category = _categories.FirstOrDefault(r => r.CategoryId == 2);
            survey.Respondents = new List<Respondent>();
            survey.Questions = new List<Question>();
            _surveys.Add(survey);
        }

        private void setCategories()
        {
            _categories.Add(new Core.Model.Category()
            {
                CategoryId = 1,
                Description = "First Category",
                Surveys = new List<Survey>()
            });

            _categories.Add(new Core.Model.Category()
            {
                CategoryId = 2,
                Description = "Second Category",
                Surveys = new List<Survey>()
            });
        }




        private void setQuestionsForSurvey3(Survey survey)
        {
            Question q1 = Core.Factories.QuestionFactory.CreateQuestion();
            q1.QuestionId = 1;
            q1.SequenceNumber = 1;
            q1.Text = "Text for Question 1";
            q1.Survey = survey;
            q1.AvailableResponses = new List<AvailableResponse>();
            //  Add the responses to the question.
            setAvailableResponsesForQuestion(survey.SurveyId, q1);
            //  Add to the Survey and the list of questions
            _questions.Add(q1);
            survey.Questions.Add(q1);

            Question q2 = Core.Factories.QuestionFactory.CreateQuestion();
            q2.QuestionId = 2;
            q2.SequenceNumber = 2;
            q2.Text = "Text for Question 2";
            q2.Survey = survey;
            q2.AvailableResponses = new List<AvailableResponse>();
            //  Add the responses to the question.
            setAvailableResponsesForQuestion(survey.SurveyId, q2);

            //  Add to the Survey and the list of questions.
            _questions.Add(q2);
            survey.Questions.Add(q2);
        }


        private void setAvailableResponsesForQuestion(long surveyId, Question question)
        {
            if (surveyId == 3 && question.QuestionId == 1)
            {
                AvailableResponse r1 = new AvailableResponse()
                {
                    Id = 1,
                    LikertScaleNumber = 1,
                    Text = "Strongly Disagree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 2,
                    LikertScaleNumber = 2,
                    Text = "Disagree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 3,
                    LikertScaleNumber = 3,
                    Text = "Neither Agree nor Disagree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 4,
                    LikertScaleNumber = 4,
                    Text = "Agree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 5,
                    LikertScaleNumber = 5,
                    Text = "Strongly Agree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);
            }

            if (surveyId == 3 && question.QuestionId == 2)
            {
                AvailableResponse r1 = new AvailableResponse()
                {
                    Id = 6,
                    LikertScaleNumber = 1,
                    Text = "Strongly Disagree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 7,
                    LikertScaleNumber = 2,
                    Text = "Disagree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 8,
                    LikertScaleNumber = 3,
                    Text = "Neither Agree nor Disagree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 9,
                    LikertScaleNumber = 4,
                    Text = "Agree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);

                r1 = new AvailableResponse()
                {
                    Id = 10,
                    LikertScaleNumber = 5,
                    Text = "Strongly Agree",
                    Question = question
                };
                _availableResponses.Add(r1);
                question.AvailableResponses.Add(r1);
            }

        }

       
    }
}