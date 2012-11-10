using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Core.Model;
using Core.Interfaces;
using Infrastructure;   

namespace Infrastructure.Repositories
{
    public class RespondentRepository : IRespondentRepository
    {
        private readonly THSurveysContext _unitOfWork;

        public RespondentRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork", "Null DbContext suppllied");
            _unitOfWork = unitOfWork as THSurveysContext;
        }

        public Respondent GetLatestResultForSurvey(long id)
        {
            //  Get the id of the latest respondent.
            var LastRespondent = _unitOfWork.Respondents
                .Where(r => r.Survey.SurveyId == id)
                .Max(r => r.RespondentId);

            //  Use eager loading from the context to ensure all required 
            //  information is available.  Keep separate from ;GetSurvey' 
            //  so we can load the results and leave out the unnecessary
            //  survey question information.
            var latestResult = _unitOfWork.Respondents
                .Include(s => s.Responses)
                .First(s => s.RespondentId == LastRespondent);

            return latestResult;
        }
    }
}
