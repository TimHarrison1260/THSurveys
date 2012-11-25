using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Interfaces;          //  Interfaces.
using Core.Model;               //  for mapping survey results
using Core.Factories;           //  for mapping survey results
using THSurveys.Filters;        //  Access to the mapping and other filters
using THSurveys.Models.Home;    //  Access the viewmodels supporting the home controller and views
using THSurveys.Models.Shared;  
using THSurveys.Mappings;       //  for mapping survey results.
using THSurveys.Infrastructure.Interfaces;

namespace THSurveys.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        /// <summary>
        /// Holds an instance of the Domain Model Repositories
        /// </summary>
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IRespondentRepository _respondentRepository;

        private readonly IMapTakeSurveyViewModel _takeSurveyViewModelMapper;
        private readonly IReinstateTakeSurveyViewModel _reinstateTakeWurveyViewModelMapper;

        /// <summary>
        /// HomeController Constructor which injects the instance of the Domain Model Repositories.
        /// </summary>
        /// <param name="repository">Instance of the Domain Model Repository</param>
        public HomeController(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository, IQuestionRepository questionRepository, IRespondentRepository respondentRepository
            , IMapTakeSurveyViewModel takeSurveyViewModeMapper, IReinstateTakeSurveyViewModel reinstateTakeSurveyViewModelMapper)
        {
            if (surveyRepository == null)
                throw new ArgumentNullException("repository", "No valid repository supplied to HomeController");
            if (categoryRepository == null)
                throw new ArgumentNullException("CategoryRepository","No valid Category repository supplied to HomeController");
            if (questionRepository == null)
                throw new ArgumentNullException("QuestionRepository", "No valid Question repository supplied to HomeController");
            if (respondentRepository == null)
                throw new ArgumentNullException("RespondentRepository", "No valid Respondent repository supplied to HomeController");
            if (takeSurveyViewModeMapper == null)
                throw new ArgumentNullException("TakeSurveyviewModelMapper", "No valid mapper for TakeSurveyViewModel supplied to HomeController");
            if (reinstateTakeSurveyViewModelMapper == null)
                throw new ArgumentNullException("ReinstateTakeSurveyViewModelMapper", "No valie mapper for ReinstateTakeSurveyViewModel supplied to HomeController");

            _surveyRepository = surveyRepository;
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
            _respondentRepository = respondentRepository;
            _takeSurveyViewModelMapper = takeSurveyViewModeMapper;
            _reinstateTakeWurveyViewModelMapper = reinstateTakeSurveyViewModelMapper;
        }

        [MapSurveyToSurveySummary]
        public ActionResult Index()
        {
            ViewBag.Message = "Select a Survey Title from the list below to participate in the survey";
            ViewBag.Title = "Top 5 most popular Surveys";
            var surveys = _surveyRepository.GetTopTenSurveys().ToArray();
            return View(surveys);
        }


        public ActionResult Choose()
        {
            ViewBag.Message = "Select a category to display the list of available surveys";
            ViewBag.Title = "Available Surveys";
            //  create an instance of the viewmodel class and map the incomming data to it
            HomeLChooseViewModel viewModel = new HomeLChooseViewModel();
            //  populate the viewModel wth the categories
            viewModel.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Description");
            //  Now pass the viewModel to the view, avoids sending the null to the view, which avoids the 
            //  null reference exceptions being suppressed automatically and improves performance.
            return View(viewModel);
        }

        
        /// <summary>
        /// Responds to change in selection of categories.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// No AntiForgery stuff as nothing is updated.
        /// </remarks>
        [HttpPost]
        [MapSurveyToSurveySummary]
        [AjaxActionOnly]
        public ActionResult SurveyList(long? id)
        {
            var surveys = (id == null ? _surveyRepository.GetAvailableSurveys() : _surveyRepository.GetSurveysForCategory(Convert.ToInt64(id)));
            return PartialView("_SurveySummaryList", surveys.ToArray());
        }


        [MapSurveyToTakeSurveyViewModel]
        public ActionResult Take(int id)
        {
            var survey = _surveyRepository.GetSurvey(id);
            return View(survey);
        }

        /// <summary>
        /// Update the responses to a survey.
        /// </summary>
        /// <remarks>
        /// Updates possible, so validate the AntiForgery stuff
        /// </remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Take(TakeSurveyViewModel Responses)
        {
            if (ModelState.IsValid)
            {
                //  Map the responses to the Survey.
                Survey surveyWithResults = _takeSurveyViewModelMapper.Map(Responses);
                
                //  Now update the DbContext
                Survey[] surveys = new Survey[] { surveyWithResults };
                _surveyRepository.UpdateSurveys(surveys);

                //  Display the Thank you page.
                return RedirectToAction("ThankYou", new { id = surveyWithResults.SurveyId });
            }

            //  An error, so reconstruct the survey details and return to the view
            TakeSurveyViewModel viewModel = _reinstateTakeWurveyViewModelMapper.Map(Responses);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ThankYou(int id)
        {
            //  Get latest result for the survey: this is the one just taken.
            var survey = _surveyRepository.GetSurvey(id);
            SurveyResultsViewModel viewModel = MapSurveyToSurveyResults.MapLatestResult(survey, _respondentRepository, _questionRepository);
            return View(viewModel);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }


        public ActionResult PageNotfound()
        {
            ViewBag.Title = "Sorry, an error has occurred";
            ViewBag.Message = "The page you were looking for could not be found";
            return View();
        }

    }
}
