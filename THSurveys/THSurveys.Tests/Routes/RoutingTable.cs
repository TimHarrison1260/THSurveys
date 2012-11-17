using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace THSurveys.Tests.Routes
{
    public static class RoutingTable
    {
        public static void registerRoutes(RouteCollection routes)
        {
            //  Ignore any *.axd files
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //  Routes specific to Survey application.
            routes.MapRoute(
                name: "Top5Surveys",
                url: "Survey/Top5",
                defaults: new { controller = "Home", action = "Index" }
                );  

            routes.MapRoute(
                name: "TakeSurvey",
                url: "Survey/{action}/{SurveyId}",
                defaults: new { controller = "Home"},
                constraints: new { action = "^Take|^Choose", SurveyId = @"^[0-9]{0,4}" } // specific actions and SurveyId numeric, if defined.
                );  //  Regex: allows only numerics, but with length or 0 to 4.  0 is consistent with it being optional.

            //routes.MapRoute(
            //    name: "ChooseSurvey",
            //    url: "Survey/{action}",
            //    defaults: new { controller = "Home" },
            //    constraints: new { action = "^Choose" } // specific actions and SurveyId numeric, if defined.
            //    );  //  Regex: allows only numerics, but with length or 0 to 4.  0 is consistent with it being optional.

            //routes.MapRoute(
            //    name: "SurveyMantenance",
            //    url: "Survey/{action}",
            //    defaults: new { controller = "Survey" },
            //    constraints: new { action = "^List|^Create|^Approve" } // specific actions and SurveyId numeric, if defined.
            //    );

            //routes.MapRoute(
            //    name: "QuestionMaintenance",
            //    url: "Survey/AddQuestion/{SurveyId}",
            //    defaults: new { controller = "Question", action = "Create" },
            //    constraints: new { SurveyId = @"^[0-9]{0,4}" } //  The survey MUST be numeric.
            //    );

            //routes.MapRoute(
            //    name: "SurveyAnalysis",
            //    url: "Survey/Analyse/{SurveyId}",
            //    defaults: new { controller = "Analysis", action = "QuestionList" },
            //    constraints: new { SurveyId = @"^[0-9]{0,4}" }
            //    );

            //routes.MapRoute(
            //    name: "Surveys",
            //    url: "Surveys",
            //    defaults: new { controller = "Survey", action = "List" }
            //    );




            routes.MapRoute(
                name: "DefaultWithNumericId",
                url: "{controller}/{action}/{id}",
                defaults: new {},
                constraints: new { controller = "^Survey$|^Question$|^Analysis$", id = @"^[0-9]{1,4}" } //, action = "^Index$|^About$|^Contact$|^List$|^Create$|^Approve$" }
                );
            //defaults: new { controller = "Home", action = "Index" },

            //  HomePage    Route to Home/Index
            //  Combine DefaultWithNoId and DefaultWithObject.
            routes.MapRoute(
                name: "DefaultWithNonNumericId",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id=UrlParameter.Optional },
                constraints: new { controller = "^Home$|^Survey$", action = "^Index$|^About$|^Contact$|^List$|^Create$|^Approve$" }
                );

            //  Catchall    Route to HttpNotFound page
            routes.MapRoute(
                name: "catchall",
                url: "{*url}",
                defaults: new { controller = "Home", action = "PageNotFound"}
                );
        }
    }
}
