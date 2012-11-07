using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using Core.Model;

namespace THSurveys.Models
{
    public class CreateSurveyViewModel
    {
        [DisplayName("Title")]
        [Required(ErrorMessage="Survey Title cannot be blank, please enter a Title")]
        [StringLength(100,ErrorMessage="Title can be a maximum of 100 chracters")]
        public string Title { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage="You must assign a Category to the Survey")]
        public long CategoryId {get; set;}

        public SelectList CategoryList { get; set; }
    }
}