[assembly: WebActivator.PreApplicationStartMethod(typeof(THSurveys.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(THSurveys.App_Start.NinjectWebCommon), "Stop")]

namespace THSurveys.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using IoCMappings;          //  Access the project where the mappings are performed.  

    //  We MUST get RID of this reference as it breaks app structure.  Ninject binding requires it at the moment.
    //  It means we have a reference in the GUi layer, and this is a BIG NO_NO!!!!!!!!!!!!!!!!!
    //  The Problem is we must find a place to hold the ioC mappings, that doesn't break the layered approach
    //  but means we can remove the reference.  The necessary location would be the Core layer as the GUI should
    //  only ever reference tis layer, but it would necessitate a reference to the Infrastructure layer there, 
    //  and this is a BIG NO_NO too! The Infrastructure layer can reference the Core but NOT the other way around.
    //  Perhaps a seperate roject for the IoC container mappings on its own.  This introduces more projects to the
    //  solution but removes the need to reference both layers.  It also gives a nice, single location, for all 
    //  IoC mappings and while it cannot actually be in the Infrastructure project, because it requires a reference
    //  to the UI layer (More BIG NO_NO's!) it cannot really be in the core as it is infrastructure type code.
    //
    //  Accepting this is how I'll code it, I'll try and do it in such a way as I can try and inject the mappings
    //  so that it makes it easier to swap DI and IoC frameworks. (IS THIS REALLY PRACTICAL, as the code within
    //  each IoC project would be specific to the IoC being used.  Through an Interface perhaps.  
    //    using Infrastructure;               

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //  Call the static binder in the IoCMappings project
            //  so that we can abstract out the references to the
            //  application Core and Infrastructure projects from
            //  the UI.
            Mappings.RegisterServices(kernel);

            //  Add additional binding stuff so that we can inject 
            //  dependencies within the THSurveys project.  This is
            //  avoids having to reference THSurveys project from 
            //  within the IoC project, because a reference THSurveys
            //  from there causes a circular reference, BAD!!!

            //  Allow the instsance of the TakeSurveyViewModelToSurvey mapper class.
            kernel.Bind<THSurveys.Infrastructure.Interfaces.IMapTakeSurveyViewModel>().To<THSurveys.Mappings.MapTakeSurveyViewModelToSurvey>();
            kernel.Bind<THSurveys.Infrastructure.Interfaces.IReinstateTakeSurveyViewModel>().To<THSurveys.Mappings.ReinstateTakeSurveyViewModel>();
        }        
    }
}
