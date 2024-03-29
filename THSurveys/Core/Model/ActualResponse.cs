﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Model
{
    public abstract class ActualResponse
    {
        /// <summary>
        /// Gets or sets the unique ID for the response
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the Respondent taking the survey
        /// </summary>
        [DisplayName("Respondent")]
        public long RespondentId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Question the Answer relates to.
        /// </summary>
        [DisplayName("Question")]
        public long Question { get; set; }

        /// <summary>
        /// Gets or sets the answer to the question.  this holds the response
        /// number from the likert scale the question references.
        /// </summary>
        [DisplayName("Response")]
        [Required(ErrorMessage = "The answer to the question cannot be left blank.")]
        public long Response { get; set; }

        /// <summary>
        /// Navigation property to the Respondent taking the survey
        /// </summary>
        public virtual Respondent Respondent { get; set; }
    }
}