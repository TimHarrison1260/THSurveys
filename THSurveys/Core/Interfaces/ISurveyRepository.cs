using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Interfaces
{
    /// <summary>
    /// Class <c>ISurveyRepository</c> exposes the interface to the
    /// Survey Repository, provides access to the Surveys datastore
    /// </summary>
    public interface ISurveyRepository
    {
        IQueryable<Survey> GetTopTenSurveys();

        IQueryable<Survey> GetSurveysForCategory(long categoryId);

        IQueryable<Survey> GetSurveysForUser(string userName);

        IQueryable<Survey> GetAvailableSurveys();

        IQueryable<Survey> GetSurveysForApproval();

        IQueryable<Survey> GetAllSurveys();

        Survey GetSurvey(long id);

        long CreateSurvey(Survey survey);

        void UpdateSurveys(IList<Survey> survey);

        bool IsMySurvey(string userName, long surveyId);
    }
}
