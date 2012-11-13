using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using THSurveys.Models.Analysis;

namespace THSurveys.Infrastructure.Factories
{
    public abstract class MVCChartFactory
    {
        public abstract MVCChart Create(ChartData chartdata);
    }
}