using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Migrations;
using Core.Model;
using System.Collections.ObjectModel;


namespace Infrastructure
{
    /// <summary>
    /// MVC4, EF4.5+, Allow database migrations between various levels with EV Code first.
    /// </summary>
    public class THSurveyMigrations : DbMigrationsConfiguration<THSurveysContext>
    {
        /// <summary>
        /// ctor: set the automaticMigrations to true
        /// </summary>
        public THSurveyMigrations()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// Override Seed so that we can populate the changed
        /// database with data.
        /// </summary>
        /// <param name="context"></param>
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
            context.LikertTemplates.AddOrUpdate(templateYesNo);
            context.SaveChanges();            
            
            //base.Seed(context);
        }
    }
}
