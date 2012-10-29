using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Model
{
    public class LikertTemplate
    {
        /// <summary>
        /// Gets or sets the unique Identifier of the Likert Response Template
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Title of the Likert Response Template
        /// </summary>
        [DisplayName("Scale Description")]
        [Required(ErrorMessage="Title for Template cannot be blank.")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the collection of templated responses for this template.
        /// </summary>
        public virtual ICollection<TemplateResponse> Responses { get; set; } 
    }
}