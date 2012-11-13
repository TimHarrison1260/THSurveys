using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using THSurveys.Models.Analysis;

namespace THSurveys.Infrastructure.Factories
{
    public class SurveyChartFactory : MVCChartFactory
    {
        public override MVCChart Create(ChartData chartData)
        {
            return new SurveyQuestionChart(chartData);
        }
    }
}