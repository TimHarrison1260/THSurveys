using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;           //  give access to the lambd expression version of .Include for eager loading (EF)

using Core.Model;
using Core.Interfaces;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Class <c>SurveyRepository</c> is responsible for managing
    /// all access to survey information within the DbContext.
    /// </summary>
    public class SurveyRepository : ISurveyRepository
    {
        //  Holds the injected instance of the DbContext.
        private readonly THSurveysContext _unitOfWork;

        /// <summary>
        /// ctor: to inject DbContext dependency, unit Ninject.
        /// </summary>
        /// <param name="unitOfWork"></param>
        public SurveyRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new NullReferenceException("Null UnitOfWork supplied");
            _unitOfWork = unitOfWork as THSurveysContext;
        }

        /// <summary>
        /// Get the to ten surveys based upon the number of respondents
        /// </summary>
        /// <returns></returns>
        public IQueryable<Survey> GetTopTenSurveys()
        {
            //  TODO:   Alter the linq to return the top 5, not all as this does
            var surveys = (from s in _unitOfWork.Surveys
                           where s.Status == 2 && s.IsTemplate == false
                           select s);
            return surveys;
        }

        /// <summary>
        /// Get the surveys for a supplied CategoryId
        /// </summary>
        /// <param name="categoryId">The Id of the category</param>
        /// <returns></returns>
        public IQueryable<Survey> GetSurveysForCategory(long categoryId)
        {
            var surveys = (from s in _unitOfWork.Surveys
                           where s.Status == 2 && s.IsTemplate == false && s.Category.CategoryId == categoryId
                           select s);
            return surveys;
        }

        public IQueryable<Survey> GetSurveysForUser(string userName)
        {
            var surveys = (from s in _unitOfWork.Surveys
                           where s.IsTemplate == false && s.User.UserName == userName
                           select s);
            return surveys;
        }

        /// <summary>
        /// Get all available Live surveys
        /// </summary>
        /// <returns></returns>
        public IQueryable<Survey> GetAvailableSurveys()
        {
            //var surveys = (from s in _unitOfWork.Surveys
            //               where s.Status == 2 && s.IsTemplate == false
            //               select s);
            var surveys = _unitOfWork.Surveys
                .Where(s => s.Status == 2 && !s.IsTemplate);
            return surveys;
        }

        public IQueryable<Survey> GetSurveysForApproval()
        {
            var surveys = (from s in _unitOfWork.Surveys
                           where s.Status == 1 && s.IsTemplate == false
                           orderby s.StatusDate ascending
                           orderby s.User.UserName ascending
                           orderby s.Category.Description ascending
                           orderby s.Title ascending
                           select s);
            return surveys;
        }


        /// <summary>
        /// Get all surveys defined, regardless of status.        
        /// <returns></returns>
        public IQueryable<Survey> GetAllSurveys()
        {
            var surveys = _unitOfWork.Surveys
                .Where(s => !s.IsTemplate);
            return surveys;
        }

        /// <summary>
        /// Get a specific survey for a supplied Surveyid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Survey GetSurvey(long id)
        {
            //  Eager load from the context to ensure all required information
            //  is available and it's done as efficiently as possible using a 
            //  single SQL statement.
            var survey = _unitOfWork.Surveys
                .Include(s => s.Questions)
                .Include(s => s.Questions.Select(r => r.AvailableResponses))
                .Include(s => s.Respondents)
//                .Include("Questions")
//                .Include("Questions.AvailableResponses")
                .FirstOrDefault(s => s.SurveyId == id && !s.IsTemplate);
            return survey;
        }


        /// <summary>
        /// Create a survey, Add it to the DbContext, for persisting 
        /// in the underlying database
        /// </summary>
        /// <param name="survey"></param>
        public long CreateSurvey(Survey survey)
        {
            _unitOfWork.Surveys.Add(survey);
            _unitOfWork.SaveChanges();

            return survey.SurveyId;
        }

        /// <summary>
        /// Updats the survey models supplied in the DbContext
        /// </summary>
        /// <param name="surveys"></param>
        public void UpdateSurveys(IList<Survey> surveys)
        {
            _unitOfWork.SaveChanges();
        }
    }
}
