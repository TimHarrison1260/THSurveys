using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

using Ninject;              //  to get a reference to the Ninject Kernel interface

using Core.Interfaces;      //  to get a reference to the application interfaces
using Infrastructure;       //  to get a reference to the aplication infrastructure concrete classes.
using Infrastructure.Repositories;  // get a reference to the application infrastructure concrete repository classes

//using System.Data.Entity;           //  Access to DbContext: EF.


namespace IoCMappings
{
    /// <summary>
    /// Static class <c>Mappings</c> is responsible for maintaning the DI mappings
    /// for the application.  It exists to abstract the bindings for the 
    /// Ninject IoC, from the UI and therefore the requirement for the UI to 
    /// reference the Infrastructure project.
    /// </summary>
    public static class Mappings
    {
        /// <summary>
        /// Static <c>RegisterServices</c> allows the necessary IoC bindings for the 
        /// application to be set.
        /// </summary>
        /// <param name="kernel">A reference to the Static instance of the Ninject IoC Kernel</param>
        public static void RegisterServices (IKernel kernel)
        {
            //  TODO: Add the bindings for the application here......

            //  Allow Ninject to manage the instance of the DbContext so that we can get access to the same instance.
            //kernel.Bind<Infrastructure.THSurveysContext>().ToSelf();
            kernel.Bind<Core.Interfaces.IUnitOfWork>().To<Infrastructure.THSurveysContext>().InScope(c => System.Web.HttpContext.Current);

            //  Bind the ISurveyRepository to its concrete implementation SurveyRepository.
            kernel.Bind<Core.Interfaces.ISurveyRepository>().To<Infrastructure.Repositories.SurveyRepository>();
            //  Bind the ICategoryRepository to its concrete implementation CategoryRepository.
            kernel.Bind<Core.Interfaces.ICategoryRepository>().To<Infrastructure.Repositories.CategoryRepository>();
            //  Bind the IUserRepository to its concrete implementation UserRepository.
            kernel.Bind<Core.Interfaces.IUserRepository>().To<Infrastructure.Repositories.UserRepository>();
            //  Bind the ITemplateRepository to its concrete implenetation TemplateRepository
            kernel.Bind<Core.Interfaces.ITemplateRepository>().To<Infrastructure.Repositories.TemplateRepository>();
            //  Bind the IQuestionRepository to its concrete implementation QuestionRepository
            kernel.Bind<Core.Interfaces.IQuestionRepository>().To<Infrastructure.Repositories.QuestionRepository>();
            //  Bind the IRespondentRepository to its concrete implementation RespondentRepository.
            kernel.Bind<Core.Interfaces.IRespondentRepository>().To<Infrastructure.Repositories.RespondentRepository>();

            //  Bind the abstract factory for Respondents.
            kernel.Bind<Core.Factories.RespondentFactory>().To<Core.Factories.ConcreteResponentFactory>();
            //  Bind the abstract factory for ActualResponses.
            kernel.Bind<Core.Factories.ActualResponseFactory>().To<Core.Factories.ConcreteActualResponseFactory>();
        }
    }
}
