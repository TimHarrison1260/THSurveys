using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Model
{
    /// <summary>
    /// Class <c>Survey</c> contains the information for a survey
    /// </summary>
    public class Survey
    {
        /// <summary>
        /// Gets or sets the unique ID of the survey.
        /// </summary>
        [Key]
        public long SurveyId { get; set; }

        /// <summary>
        /// Gets or sets the Title of the survey
        /// </summary>
        [DisplayName("Title")]
        [Required(ErrorMessage = "Title of the Survey cannot be left blank.")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Owner that owns the survye.
        /// </summary>
        /// <remarks>
        /// Only the owner of the survey can make changes to it.
        /// </remarks>
        [DisplayName("Owner")]
        [Required(ErrorMessage = "The Owner of the Survey cannot be left blank.")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the Category the survey belongs to.
        /// </summary>
        [DisplayName("Category")]
        [Required(ErrorMessage="The survey must belong to a Category.  It cannot be left blank")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the current status of the survey
        /// </summary>
        [DisplayName("Status")]
        [Required(ErrorMessage="The Survey must have a Status.")]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time the status of the survey was changed
        /// </summary>
        [DisplayName("Date Status Changed")]
        [Required(ErrorMessage="The date must be completed")]
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// Gets or sets an indicator that identifies the Survey as a Template or
        /// actual survey.  True indicates a Template, False (default) indicates
        /// an actual survey.
        /// </summary>
        [DisplayName("Template")]
        public bool IsTemplate { get; set; }

        /// <summary>
        /// Gets or sets the questions for the survey
        /// </summary>
        public virtual ICollection<Question> Questions { get; set; }

        /// <summary>
        /// Gets or sets the collection of respondents who have taken the survey.
        /// </summary>
        public virtual ICollection<Respondent> Respondents { get; set; }

        /// <summary>
        /// Navigation property for CategoryId
        /// </summary>
        public virtual Category Category { get; set; }

    }
}
