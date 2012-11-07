using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;

namespace THSurveys.Models.Question
{
    public class AddQuestionsListViewModel
    {
        public long QuestionId { get; set; }

        [DisplayName("Number")]
        public long SequenceNumber { get; set; }

        [DisplayName("Question")]
        public string Text { get; set; }

        [DisplayName("Responses")]
        public string Title { get; set; }
    }
}