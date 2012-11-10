using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THSurveys.Models.Shared
{
    public class SurveyResultsViewModel
    {
        public long SurveyId { get; set; }
        public string Title { get; set; }
        public IList<SurveyResultsDetailsviewModel> Results { get; set; }
    }

    public class SurveyResultsDetailsviewModel
    {
        public string DateTaken { get; set; }
        public IList<SurveyResultsAnswersViewModel> Answers { get; set; }
    }

    public class SurveyResultsAnswersViewModel
    {
        public long QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public long AnswerNumber { get; set; }
        public string AnswerText { get; set; }
    }
}