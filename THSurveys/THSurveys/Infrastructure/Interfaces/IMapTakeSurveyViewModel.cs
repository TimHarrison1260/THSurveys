using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;
using Core.Interfaces;
using THSurveys.Models.Home;

namespace THSurveys.Infrastructure.Interfaces
{
    public interface IMapTakeSurveyViewModel
    {
        Survey Map(TakeSurveyViewModel Responses);
    }
}
