using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Factories
{
    public static class SurveyFactory
    {
        public static Survey CreateSurvey()
        {
            //  Create a new instance of the Survey,
            Survey survey = new ConcreteSurvey();

            //  Initialse the default value and leave the others to be set by client
            survey.Status = (int)SurveyStatusEnum.Incomplete;
            survey.StatusDate = DateTime.Now;
            survey.IsTemplate = false;
            survey.Questions = null;
            survey.Respondents = null;

            return survey;
        }
    }
}
