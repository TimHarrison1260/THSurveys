using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Services
{
    public class ConcreteSurveyAnalysisService: SurveyAnalysisService
    {
        public ConcreteSurveyAnalysisService(Survey survey)
            : base(survey)
        {
        }
    }
}
