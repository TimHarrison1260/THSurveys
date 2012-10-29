using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Model
{
    public class Category
    {
        /// <summary>
        /// Gets or sets the Id of the Category
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the Title of the Category
        /// </summary>
        [DisplayName("Description")]
        [Required(ErrorMessage = "The Category Description cannot be left blank")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Category Description must be between 1 and 50 characters in length")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a reference to the associated surveys
        /// </summary>
        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
