using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject;              //  to get a reference to the Ninject Kernel interface

using Core.Interfaces;      //  to get a reference to the application interfaces
using Infrastructure;       //  to get a reference to the aplication infrastructure concrete classes.
using Infrastructure.Repositories;  // get a reference to the application infrastructure concrete repository classes


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

            //  Bind the ISurveyRepository to its concrete implementation SurveyRepository.
            kernel.Bind<Core.Interfaces.ISurveyRepository>().To<Infrastructure.Repositories.SurveyRepository>();
            //  Bind the ICategoryRepository to its concrete implementation CategoryRepository.
            kernel.Bind<Core.Interfaces.ICategoryRepository>().To<Infrastructure.Repositories.CategoryRepository>();
        }
    }
}
