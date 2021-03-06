﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Model
{
    public abstract class Question
    {
        /// <summary>
        /// Gets or sets the unique Id of the question.
        /// </summary>
        [Key]
        public long QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the number of the question within the survey.  
        /// It is unique within each survey.
        /// </summary>
        /// <remarks>
        /// Validation required:  The sequence number must be unique within the Survey
        /// A Custom validation rule will be required.
        /// </remarks>
        [DisplayName("Number")]
        [Required(ErrorMessage="The Sequence number cannot be blank or Zero.")]
        public long SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        [DisplayName("Text of Question")]
        [Required(ErrorMessage="The text for the question cannot be blank.")]
        public string Text { get; set; }

        /// <summary>
        /// Navigation property for Survey
        /// </summary>
        public virtual Survey Survey { get; set; }

        /// <summary>
        /// Gets or sets the collection of available responses to this question
        /// </summary>
        public virtual ICollection<AvailableResponse> AvailableResponses { get; set; }
    }
}