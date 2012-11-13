using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Core.Model;                       //  Survey model
using Core.Services;                    //  Survey Analyse service
using Core.Factories;                   //  Analyser factory
using THSurveys.Models.Analysis;        //  chartData model

namespace THSurveys.Mappings
{
    public static class MapSurveyToSurveyChart
    {
        public static ChartData Map(Survey Survey, long QuestionId)
        {
            //  Instantiate the analysis service
            var analyser = SurveyAnalysisServiceFactory.Create(Survey);
            //  instantiate the chartdata;
            ChartData data = new ChartData();

            //  Get the question object from the survey
            var question = Survey.Questions.FirstOrDefault(x => x.QuestionId == QuestionId);

            //  Map the data
            data.Title = question.Text;                             //  Load question text
            data.Width = 400;
            //  Set the height of the chart
            data.Height = (question.AvailableResponses.Count() * 30) + 150;

            data.Results = new List<ChartInformation>();

            long totalRespondents = analyser.TotalRespondents();    //  Load total
            foreach (var answer in question.AvailableResponses)
            {
                var result = new ChartInformation();
                result.responses = (analyser.TotalForQuestionResponse(question.QuestionId, answer.LikertScaleNumber) / totalRespondents);
                result.Text = answer.Text;
                data.Results.Add(result);
            }

            return data;
        }
    }
}