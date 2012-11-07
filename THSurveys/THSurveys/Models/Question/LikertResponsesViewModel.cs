using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;

namespace THSurveys.Models.Question
{
    public class LikertResponseViewModel
    {
        [DisplayName("Option")]
        public long LikertScaleNumber { get; set; }

        [DisplayName("Test attached to Response")]
        public string Text { get; set; }
    }
}