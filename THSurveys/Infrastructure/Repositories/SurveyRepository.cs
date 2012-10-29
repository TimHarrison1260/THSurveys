using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;
using Core.Interfaces;

namespace Infrastructure.Repositories
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly THSurveysContext _db = new THSurveysContext();

        //public SurveyRepository(GCUSurveys db)
        //{
        //    if (db == null)
        //        throw new NullReferenceException("No DbContext supplied to repository");
        //    _db = db;
        //}

        public IQueryable<Survey> GetTopTenSurveys()
        {
            //  TODO:   Alter the linq to return the top ten, no all as this does
            var surveys = (from s in _db.Surveys
                           where s.Status == 2 && s.IsTemplate == false
                           select s);
            return surveys;
        }


        public IQueryable<Survey> GetAvailableSurveys()
        {
            var surveys = (from s in _db.Surveys
                           where s.Status == 2 && s.IsTemplate == false
                           select s);
            return surveys;
        }

        public IQueryable<Survey> GetAllSurveys()
        {
            var surveys = (from s in _db.Surveys
                           where s.IsTemplate == false
                           select s);
            return surveys;
        }

        public Survey GetSurvey(long id)
        {
            var survey = (from s in _db.Surveys
                          where s.SurveyId == id && s.IsTemplate == false
                          select s).FirstOrDefault();
            return survey;
        }

        public void CreateSurvey(Survey survey)
        {
            _db.Surveys.Add(survey);
            _db.SaveChanges();
        }
    }
}
