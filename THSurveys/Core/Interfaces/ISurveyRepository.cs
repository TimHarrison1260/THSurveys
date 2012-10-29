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

        IQueryable<Survey> GetAvailableSurveys();

        IQueryable<Survey> GetAllSurveys();

        Survey GetSurvey(long id);

        void CreateSurvey(Survey survey);
    }
}
