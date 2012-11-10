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
    /// Class <c>Respondent</c> describes an instance of a Respondent
    /// and when they took a survey.  There is no personal infomration
    /// and should the same respondent take more than one survye, 
    /// another instance will be created.
    /// </summary>
    public abstract class Respondent
    {
        /// <summary>
        /// Gets or sets the Id of the Respondent
        /// </summary>
        /// <remarks>
        /// This does NOT indicate an individual but more the 
        /// individual taking and the date taken the survey
        /// </remarks>
        [Key]
        [DisplayName("Respondent Number")]
        public long RespondentId {get; set;}

        /// <summary>
        /// Gets or sets the data the respondent has taken the survey.
        /// </summary>
        [DisplayName("Date Taken")]
        [DataType(DataType.DateTime)]
        public DateTime DateTaken {get; set;}

        /// <summary>
        /// Navigation property to the Survey this response relates to. 
        /// </summary>
        public virtual Survey Survey { get; set; }

        ///// <summary>
        ///// Foreign key to the Survey, this respondent is taking
        ///// </summary>
        //[Required(ErrorMessage="The Respondent must select a Survey")]
        //public long SurveyId { get; set; }

        /// <summary>
        /// Gets or sets the actual responses to a particular survey response.
        /// </summary>
        public virtual ICollection<ActualResponse> Responses { get; set; }

    }
}
