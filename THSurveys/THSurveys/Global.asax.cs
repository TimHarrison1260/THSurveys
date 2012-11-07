using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc;

//using Infrastructure.Repositories;
using THSurveys.Models;
using THSurveys.Models.Home;
using THSurveys.Models.Survey;

namespace THSurveys
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //  Cleare out the default viewengines and register only the Razor view engine.
            //  This should cut down the response time for finding the view, as Tesco says: 
            //  "Every little helps".
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());


            //  TODO: configure a routine to populate the database.  (Possibly).

            //  Call automaper configuration
            Mappings.AutoMapperConfiguration.Configure();

            //  Add the custom model binders.
            ModelBinders.Binders.Add(typeof(TakeSurveyViewModel),
                new THSurveys.Infrastructure.ModelBinders.TakeSurveyModelbinder());
            ModelBinders.Binders.Add(typeof(ApprovalListViewModel),
                new THSurveys.Infrastructure.ModelBinders.SurveyApprovalModelbinder());
        }
    }
}