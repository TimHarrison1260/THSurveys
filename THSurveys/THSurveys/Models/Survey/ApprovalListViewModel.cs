using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.Web.Mvc;

namespace THSurveys.Models.Survey
{
    /// <summary>
    /// Class <c>ApprovalListViewModel</c> represents the display
    /// part of the List of surveys awaiting approval.
    /// </summary>
    //[ModelBinder(typeof(THSurveys.Infrastructure.ModelBinders.SurveyApprovalModelbinder))]
    public class ApprovalListViewModel
    {
        [DisplayName("Date")]
        public DateTime StatusDate { get; set; }
        [DisplayName("User")]
        public string UserName { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Category")]
        public string CategoryDescription { get; set; }
        //  By referencing the Input part here, we separate
        //  the two portions of the viewModel.
        public ApprovalInputViewModel Input { get; set; }

        /// <summary>
        /// Nested Class <c>ApprovalInputViewModel</c> represents the input
        /// part of the List of surveys awaiting approval.  This
        /// is the returned part of the viewModel that the 
        /// controller action will process on the HttpPost.
        /// </summary>
        public class ApprovalInputViewModel
        {
            /// <summary>
            /// Id of the Survey being approved.  This keeps the
            /// approve in step with the survey being approved.
            /// </summary>
            public long SurveyId { get; set; }
            /// <summary>
            /// Boolean to indicate whether the survey is being
            /// approved.
            /// </summary>
            [DisplayName("Approve")]
            public bool Approve { get; set; }
        }
    }

}