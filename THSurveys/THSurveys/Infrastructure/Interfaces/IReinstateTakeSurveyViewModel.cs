using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using THSurveys.Models.Home;

namespace THSurveys.Infrastructure.Interfaces
{
    public interface IReinstateTakeSurveyViewModel
    {
        TakeSurveyViewModel Map(TakeSurveyViewModel responses);
    }
}
