using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THSurveys.Models.Home
{
    public class TakeSurveyViewModel
    {
        public long SurveyId { get; set; }
        public string Title { get; set; }
        public string CategoryDescription { get; set; }
        public string UserName { get; set; }
        public string StatusDate { get; set; }
        public IList<SurveyQuestionsViewModel> Questions { get; set; }
    }

    public class SurveyQuestionsViewModel
    {
        public long QuestionId { get; set; }
        public long SequenceNumber { get; set; }
        public string Text { get; set; }
        public long Answer { get; set; }
        public IList<SurveyResponsesViewModel> Responses { get; set; }
    }
    
    public class SurveyResponsesViewModel
    {
        //public long SurveyId { get; set; }
        public long Id { get; set; }
        public long LikertScaleNumber { get; set; }
        public string Text { get; set; }
        //public bool Selected { get; set; }
    }
}