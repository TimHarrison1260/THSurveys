using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace THSurveys
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
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
                name: "Surveys",
                url: "Surveys",
                defaults: new { controller = "Survey", action = "List" }
                );

            routes.MapRoute(
                name: "SurveyCategories",
                url: "Survey/Categories/{id}",
                defaults: new { controller = "Home", action = "SurveyList", id = UrlParameter.Optional },
                constraints: new { id = @"^[0-9]{0,4}" } // specific actions and SurveyId numeric, if defined.
                );

            routes.MapRoute(
                name: "SurveyWithOptionalId",
                url: "Survey/{action}/{id}",
                defaults: new { controller = "Home" },
                constraints: new { action = "^Take$|^ThankYou$", id = @"^[0-9]{1,4}" } // specific actions and SurveyId numeric, if defined.
                );  //  Regex: allows only numerics, but with length or 0 to 4.  0 is consistent with it being optional.

            routes.MapRoute(
                name: "SurveyTakePost",
                url: "Survey/{action}/{id}",
                defaults: new { controller = "Home" },
                constraints: new { action = "^Take$", httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "ChooseSurvey",
                url: "Survey/{action}",
                defaults: new { controller = "Home" },
                constraints: new { action = "^Choose" }
                );

            routes.MapRoute(
                name: "SurveyAnalysis",
                url: "Survey/Analyse/{id}",
                defaults: new { controller = "Analysis", action = "QuestionList" },
                constraints: new { id = @"^[0-9]{1,4}" }
                );

            routes.MapRoute(
                 name: "QuestionMaintenance",
                 url: "Survey/AddQuestion/{id}",
                 defaults: new { controller = "Question", action = "Create" },
                 constraints: new { id = @"^[0-9]{1,4}" } //  The survey MUST be numeric.
                 );

            routes.MapRoute(
                name: "DefaultWithNumericId",
                url: "{controller}/{action}/{id}",
                defaults: new { },
                constraints: new { controller = "^Survey$|^Question$|^Analysis$", id = @"^[0-9]{1,4}" }
                );



            //  HomePage    Route to Home/Index
            //  Combine DefaultWithNoId and DefaultWithObject.
            routes.MapRoute(
                name: "DefaultWithNonNumericId",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = "^Home$|^Survey$", action = "^Index$|^About$|^Contact$|^List$|^Create$|^Approve$" }
                );

            //  Special route for the GetAnalysisChart as it drops through to here anyway and it must be routed
            routes.MapRoute(
                name: "GetChart",
                url: "{controller}/{action}/{id}",
                defaults: new { },      //  id is not optional
                constraints: new { controller = "^Analysis$", action = "^GetSurveyChart$" }
                );

            //  Route the Account stuff
            routes.MapRoute(
                name: "AccountStuff",
                url: "{controller}/{action}/{id}",
                defaults: new { id = UrlParameter.Optional },
                constraints: new { controller = "^Account$" }
                );




            //  Catchall    Route to HttpNotFound page
            routes.MapRoute(
                name: "catchall",
                url: "{*url}",
                defaults: new { controller = "Home", action = "PageNotFound" }
                );


            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}