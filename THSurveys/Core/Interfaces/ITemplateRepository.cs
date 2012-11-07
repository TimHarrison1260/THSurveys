using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Interfaces
{
    public interface ITemplateRepository
    {
        IQueryable<LikertTemplate> GetLikertTemplates();
        IQueryable<TemplateResponse> GetLikertScaleResponses(long LikertId);
    }
} 