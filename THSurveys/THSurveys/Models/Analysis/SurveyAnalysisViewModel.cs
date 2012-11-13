using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace THSurveys.Models.Analysis
{
    public class SurveyAnalysisViewModel
    {
        public long SurveyId { get; set; }
        [DisplayName("Survey title")]
        public string Title { get; set; }
        [DisplayName("Respondents")]
        public long NumberOfRespondents { get; set; }
        [DisplayName("First Taken")]
        public DateTime FirstResponseDate { get; set; }
        [DisplayName("Latest Response")]
        public DateTime LastResponseDate { get; set; }

        public IList<QuestionAnalysisViewModel> Questions { get; set; }
    }

    public class QuestionAnalysisViewModel
    {
        public long QuestionId { get; set; }
        [DisplayName("Number")]
        public long SequenceNumber { get; set; }
        [DisplayName("Question")]
        public string Text {get; set;}
        [DisplayName("Responses")]
        public long NumberOfResponses {get; set;}

        public IList<ResponseAnalysisViewModel> Analysis { get; set; }
    }

    public class ResponseAnalysisViewModel
    {
        public long QuestionId {get; set;}
        public long LikertScaleNumber { get; set; }
        public string Text {get; set;}
        public double NumberOfResponses { get; set; }
        public string Percentage { get; set; }
    }
}