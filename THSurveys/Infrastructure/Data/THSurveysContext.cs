using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;           //  Accesss to Entity Framework
using Core.Model;                   //  Access to the business entities.

namespace Infrastructure
{
    public class THSurveysContext : DbContext
    {
        /// <summary>
        /// ctor: ensure the THSurveysContext is used as the connection string.
        /// </summary>
        public THSurveysContext()
            : base("THSurveysContext")
        {
        }

        /// <summary>
        /// SimpleMembership, UserProfiles table.
        /// </summary>
        //public DbSet<UserProfile> UserProfiles { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets of sets the DbSet within the DbContext for the Categories of Surveys
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets or sets the DbSet within the DbContext for the Surveys entity.
        /// </summary>
        public DbSet<Survey> Surveys { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets or sets the DbSet within the DbContext for the Questions Entity.
        /// </summary>
        public DbSet<Question> Questions { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets or sets the DbSet within the DbContext for the Available Responses Entity.
        /// </summary>
        public DbSet<AvailableResponse> AvailableResponses { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets of sets the DbSet within the DbContext for the Respondents Entity.
        /// </summary>
        public DbSet<Respondent> Respondents { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets or sets the DbSet within the DbContext for the Actual Responses Entity.
        /// </summary>
        public DbSet<ActualResponse> ActualResponses { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets or sets the DbSet within the DbContext for the Likert Response Templates.
        /// </summary>
        public DbSet<LikertTemplate> LikertTemplates { get; set; }

        /// <summary>
        /// Entity Framework, Code First.
        /// Gets or sets the DbSet within the DbContext for the responses for a Likert Response Template.
        /// </summary>
        public DbSet<TemplateResponse> TemplateResponses { get; set; }

    }
}
