using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Model
{
    public class TemplateResponse   
    {
        /// <summary>
        /// Gets of sets the unique Id of the Response record.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Likert Template this response relates to.
        /// </summary>
        /// <remarks>
        /// This allows EF to generate a Foreign key between this
        /// template response and its parent LikertTemplate.
        /// </remarks>
        public long LikertId { get; set; }

        /// <summary>
        /// Gets or sets the sequence number of the question within the survey.  
        /// It's unique within each questions, and strictly sequential.
        /// </summary>
        [Required(ErrorMessage="The response number must be completed")]
        [Range(1,10,ErrorMessage="Scale Number must be between 2 and 10.")]
        public long LikertScaleNumber { get; set; }

        /// <summary>
        /// Gets or sets the Text of the Response (in the Likert Scale)
        /// </summary>
        [DisplayName("Test attached to Response")]
        [Required(ErrorMessage="The text for the response cannot be left blank.")]
        public string Text { get; set; }

        /// <summary>
        /// Navigation property to the Likert Scale Template to which this 
        /// response belongs.
        /// </summary>
        public virtual LikertTemplate LikertTemplate { get; set; }
    
    }
}