using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interfaces;          //  Interfaces.
using THSurveys.Filters;        //  Access to the mapping and other filters
using THSurveys.Models.Home;    //  Access the viewmodels supporting the home controller and views

namespace THSurveys.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Holds an instance of the Domain Model Repository
        /// </summary>
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;

        /// <summary>
        /// HomeController Constructor which injects the instance of the Domain Model Repository.
        /// </summary>
        /// <param name="repository">Instance of the Domain Model Repository</param>
        public HomeController(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository, IQuestionRepository questionRepository)
        {
            if (surveyRepository == null)
                throw new ArgumentNullException("repository", "No valid repository supplied to HomeController");
            if (categoryRepository == null)
                throw new ArgumentNullException("CategoryRepository","No valid Category repository supplied to HomeController");
            if (questionRepository == null)
                throw new ArgumentNullException("QuestionRepository", "No valid Question repository supplied to HomeController");

            _surveyRepository = surveyRepository;
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
        }

        [MapSurveyToSurveySummary]
        public ActionResult Index()
        {
            ViewBag.Message = "Select a Survey Title from the list below to participate in the survey";
            ViewBag.Title = "Top 5 most popular Surveys";
            var surveys = _surveyRepository.GetTopTenSurveys().ToArray();
            return View(surveys);
        }

        [MapSurveyToHomeSurveyList]
        public ActionResult List()
        {
            ViewBag.Message = "Select a category to display the list of available surveys";
            ViewBag.Title = "Available Surveys";
            return View();
        }

        //[ChildActionOnly]
        [HttpPost]
        [MapSurveyToSurveySummary]
        public ActionResult SurveyList(long? id)
        {
            var surveys = (id == null ? _surveyRepository.GetAvailableSurveys() : _surveyRepository.GetSurveysForCategory(Convert.ToInt64(id)));
            return PartialView("_SurveySummaryList", surveys.ToArray());
        }


        [MapSurveyTopTakeSurveyViewModel]
        public ActionResult TakeSurvey(int id)
        {
            var survey = _surveyRepository.GetSurvey(id);
            return View(survey);
        }
        
        [HttpPost]
        public ActionResult TakeSurvey(TakeSurveyViewModel Responses)
        {
            if (ModelState.IsValid)
            {
                //  Update the results of the survey in the db



                //  Return to list for now, put up thank you page.
                return RedirectToAction("List");
            }
            return View(Responses);
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
