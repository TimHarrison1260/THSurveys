using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Core.Interfaces;
using Core.Model;

namespace Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly THSurveysContext _unitOfWork;
            
        public QuestionRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("UnitOfWork", "No valid unitOfWork supplied to QuestionRepository");
            _unitOfWork = unitOfWork as THSurveysContext;
        }

        public IQueryable<Question> GetQuestionsForSurvey(long SurveyId)
        {
            var questions = (from q in _unitOfWork.Questions
                             where q.Survey.SurveyId == SurveyId
                             select q);
            return questions;
        }

        public IQueryable<AvailableResponse> GetResponsesForQuestion(long questionId)
        {
            var responses = (from r in _unitOfWork.AvailableResponses
                             where r.Question.QuestionId == questionId
                             select r);
            return responses;
        }


        public Question GetQuestion(long questionId)
        {
            var question = _unitOfWork.Questions
                .Include(q => q.AvailableResponses)
                .FirstOrDefault(q => q.QuestionId == questionId);
            return (Question)question;
        }


        public string[] GetQuestionAndAnswerDescriptions(long questionId, long response)
        {
            var d = _unitOfWork.AvailableResponses
                .Where(r => r.Question.QuestionId == questionId && r.LikertScaleNumber == response)
                .Select(r => new {number = r.Question.SequenceNumber, question = r.Question.Text, answer = r.Text })
                .First();
            string[] descriptions = new string[] {d.number.ToString(), d.question, d.answer };
            return new string[] {d.number.ToString(), d.question, d.answer };
        }


        public long GetLastSequenceNumber(long SurveyId)
        {
            //  See if we have any questions already
            //  The Max function throws exception if 
            //  the query returns NULL;
            if ((from q in _unitOfWork.Questions
                 where q.Survey.SurveyId == SurveyId
                 select q).Count() == 0)
            {
                return 0L;
            }
            else
            {
                return (from q in _unitOfWork.Questions
                        where q.Survey.SurveyId == SurveyId
                        select q.SequenceNumber).Max();
            }
        }

        public long AddQuestion(Question question)
        {
            _unitOfWork.Questions.Add(question);
            _unitOfWork.SaveChanges();
            return question.QuestionId;     //  Return the Id of the question.
        }
    }
}
