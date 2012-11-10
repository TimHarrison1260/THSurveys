using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Interfaces
{
    public interface IQuestionRepository
    {
        IQueryable<Question> GetQuestionsForSurvey(long SurveyId);
        IQueryable<AvailableResponse> GetResponsesForQuestion(long questionId);
        Question GetQuestion(long questionId);
        string[] GetQuestionAndAnswerDescriptions(long questionId, long response);
        long AddQuestion(Question question);
        long GetLastSequenceNumber(long surveyId);
    }
}
