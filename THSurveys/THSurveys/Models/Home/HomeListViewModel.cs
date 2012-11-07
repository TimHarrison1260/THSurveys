using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace THSurveys.Models.Home
{
    public class HomeListViewModel
    {
        //  holds the id of the selected category
        //  make it nullable so a blank value can be received
        public long? CategoryId { get; set; }

        //  Selection of categories to filter the surveys.
        //  Specify 'All..' to show all surveys.
        public SelectList Categories {get; set;}
    }
}