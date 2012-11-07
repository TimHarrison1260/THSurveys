using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;

namespace Infrastructure.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly THSurveysContext _unitOfWork;

        public TemplateRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("No UnitOfWork supplied");
            _unitOfWork = unitOfWork as THSurveysContext;
        }


        public IQueryable<Core.Model.LikertTemplate> GetLikertTemplates()
        {
            var templates = (from t in _unitOfWork.LikertTemplates
                             select t);
            return templates;
        }

        public IQueryable<Core.Model.TemplateResponse> GetLikertScaleResponses(long LikertId)
        {
            var responses = (from r in _unitOfWork.TemplateResponses
                             where r.LikertTemplate.Id == LikertId
                             select r);
            return responses;
        }
    }
}
