using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace THSurveys.Models.Survey
{
    public class AddQuestionsViewModel
    {
        //  Survey Header information.
        public long SurveyId { get; set; }

        public string Title { get; set; }

        public string CategoryDescription { get; set; }

        //  Question information
        public long QuestionId { get; set; }
        public string Text { get; set; }
        public int SequenceNumber { get; set; }

//        public 


    }
}