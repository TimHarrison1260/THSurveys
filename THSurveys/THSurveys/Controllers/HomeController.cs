using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interfaces;          //  Interfaces.
using THSurveys.Filters;        //  Access to the mapping and other filters

namespace THSurveys.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Holds an instance of the Domain Model Repository
        /// </summary>
        private readonly ISurveyRepository _repository;

        /// <summary>
        /// HomeController Constructor which injects the instance of the Domain Model Repository.
        /// </summary>
        /// <param name="repository">Instance of the Domain Model Repository</param>
        public HomeController(ISurveyRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "No valid repository supplied to HomeController");
            _repository = repository;
        }

        [MapSurveyToSurveySummary]
        public ActionResult Index()
        {
            ViewBag.Message = "Select a Survey Title from the list below to participate in the survey";
            ViewBag.Title = "Available Surveys";
            var surveys = _repository.GetTopTenSurveys().ToArray();
            return View(surveys);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
