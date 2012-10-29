using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;                   //  Entity Framework
using Core.Model;                           //  Access the business entities
using System;                               //  Access to [Obsolete] attribute, marks the class 

namespace Infrastructure
{
    /// <summary>
    /// MVC3 EF4.  Doesn't work with EF4.5+.  The process of database migrations has taken over.
    /// Class <c>GCUSurveysInitialiser</c> is created so that, while using EF Code First,
    /// we can seed the database with test data.  In particular, some sample 
    /// surveys and some Response Templates
    /// </summary>
    /// <remarks>
    /// This works for MVC3 but NOT MVC4.  It uses DBMigrations which seem rather
    /// awkward to get used to.  Hopefully things will improve as I research it.
    /// 
    /// This class has been marked as 'Obsolete' as it relates to MVC3 EF4 and 
    /// this Solution makes use of MVC4 and EF4.5;
    /// 
    /// </remarks>
    [Obsolete("For database initialisation, use Entity Migrations within Package Manager Console.",true)]
    public class THSurveysInitialiser : DropCreateDatabaseIfModelChanges<THSurveysContext>
    {
        /// <summary>
        /// We override the Seed method, which is called when the data model has changed 
        /// and the underlying database has been dropped and recreated.
        /// </summary>
        /// <param name="context">the DbContext for the GCUSurveys data model</param>
        /// <remarks>
        /// This will only be called when a method actually tries to access the
        /// underlying database
        /// </remarks>
        protected override void Seed(THSurveysContext context)
        {
            TemplateResponse responseYes = new TemplateResponse()
            {
                LikertScaleNumber = 1,
                Text = "Yes"
            };
            TemplateResponse responseNo = new TemplateResponse()
            {
                LikertScaleNumber = 2,
                Text = "No"
            };
            ICollection<TemplateResponse> responsesYesNo = new Collection<TemplateResponse>();
            responsesYesNo.Add(responseYes);
            responsesYesNo.Add(responseNo);
            LikertTemplate templateYesNo = new LikertTemplate()
            {
                Title = "Yes No",
                Responses = responsesYesNo
            };
            context.LikertTemplates.Add(templateYesNo);
            context.SaveChanges();
        }
    }
}
