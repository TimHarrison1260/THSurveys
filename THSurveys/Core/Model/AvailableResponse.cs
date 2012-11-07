using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Model
{
    public class AvailableResponse
    {
        /// <summary>
        /// Gets of sets the unique Id of the Response record.
        /// </summary>
        [Key]
        public long Id { get; set; }

        ///// <summary>
        ///// Gets or sets the Id of the Question the Answer relates to.
        ///// </summary>
        ///// <remarks>
        ///// This will allow EF to generate a foreign key between the availabe Response
        ///// and the parent question.
        ///// </remarks>
        //public long QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the sequence number of the question within the survey.  
        /// It's unique within each questions, and strictly sequential.
        /// </summary>
        /// <remarks>
        /// Validation attribute required to ensure this number is unique
        /// and stricly sequential within the Question.
        /// </remarks>
        [DisplayName("Response")]
        [Required(ErrorMessage="The sequence number cannot be left blank.")]
        public long LikertScaleNumber { get; set; }

        /// <summary>
        /// Gets or sets the Text of the Response (in the Likert Scale)
        /// </summary>
        [DisplayName("Response Text")]
        [Required(ErrorMessage="The text of the response cannot be left blank.")]
        public string Text { get; set; }

        /// <summary>
        /// Navigation property to related Question
        /// </summary>
        public virtual Question Question { get; set; }


    }
}