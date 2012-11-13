using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Services;
using Core.Model;

namespace Core.Factories
{
    public static class SurveyAnalysisServiceFactory
    {
        public static SurveyAnalysisService Create(Survey survey)
        {
            if (survey == null)
                throw new ArgumentNullException("Survey", "No valid Survey supplied to surveyAnalysisServiceFactory");
            return new ConcreteSurveyAnalysisService(survey);
        }
    }
}
