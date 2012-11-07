using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace THSurveys.Models.Question
{
    public class AddQuestionsViewModel
    {
        //  Survey Header information. (Display only)
        public long SurveyId { get; set; }

        [DisplayName("Survey Title")]
        public string Title { get; set; }

        [DisplayName("Category")]
        public string CategoryDescription { get; set; }

        //  Question information, User Entry required.
        [DisplayName("Question")]
        [Required(ErrorMessage="You must supply some text for the question.")]
        public string Text { get; set; }

        //  Likert Template (for Selection only)
        [DisplayName("Likert Response Template")]
        [Required(ErrorMessage="You must select a template for the responses.")]
        public long LikertId { get; set; }
        public SelectList LikertTemplates { get; set; }
    }
}