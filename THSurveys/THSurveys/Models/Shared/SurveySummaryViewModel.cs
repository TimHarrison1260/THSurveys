using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace THSurveys.Models
{
    /// <summary>
    /// Class <c>SurveySummaryViewNodel</c> is intended to display
    /// the summary information about a survey.
    /// It is only ever intended for display use only.
    /// </summary>
    public class SurveySummaryViewModel
    {
        /// <summary>
        /// Gets or sets the Id of the Survey
        /// </summary>
        [DisplayName("Survey Id")]
        public long SurveyId { get; set; }

        /// <summary>
        /// Gets or set the Title of the Survey
        /// </summary>
        [DisplayName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Owner of the project
        /// </summary>
        [DisplayName("Owner")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets of sets the description of the currently survey status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date the survey was published / approved
        /// </summary>
        [DisplayName("Date")]
        public DateTime? StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the number of responses to the survey
        /// </summary>
        [DisplayName("Number of Responses")]
        public int Responses { get; set; }

        /// <summary>
        /// Gets or sets the category the survey belongs to.
        /// </summary>
        [DisplayName("Category")]
        public string Category { get; set; }
    }
}