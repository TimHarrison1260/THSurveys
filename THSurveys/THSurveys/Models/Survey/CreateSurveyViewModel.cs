using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace THSurveys.Models
{
    public class CreateSurveyViewModel
    {
        public long SurveyId { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage="Survey Title cannot be blank, please enter a Title")]
        [StringLength(100,ErrorMessage="Title can be a maximum of 100 chracters")]
        public string Title { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage="You must assign a Category to the Survey")]
        public long CategoryId {get; set;}

        [DisplayName("Owner")]
        [Required(ErrorMessage="The Survey must be assigned to an Owner")]
        public int Owner {get;set;}

        public int Status {get; set;}

        public DateTime? StatusDate {get; set;}

        public SelectList CategoryList {get; set;}

//        public SelectList StatusList {get; set;}
    }
}