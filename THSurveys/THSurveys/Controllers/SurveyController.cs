using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using Core.Interfaces;
using Core.Model;

using THSurveys.Models;
using THSurveys.Models.Survey;
using THSurveys.Filters;

namespace THSurveys.Controllers
{
    [Authorize]
    [HandleError]
    public class SurveyController : Controller
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor for SurveyController, we use DI to inject the instance of the repository.
        /// </summary>
        /// <param name="repository"></param>
        public SurveyController(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            //  Throw exception if any of the dependencies are null.
            if (surveyRepository == null)
                throw new ArgumentNullException("SurveyRepository", "No valid Survey repository supplied to SurveyController.");
            if (categoryRepository == null)
                throw new ArgumentNullException("CategoryRepository", "No valid category repository supplied to SurveyController.");
            if (userRepository == null)
                throw new ArgumentNullException("UserRepository", "No valid User repository supplied to SurveyController.");

            _surveyRepository = surveyRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }


        //
        // GET: /Survey/

        [Authorize(Roles="User")]
        [MapSurveyToSurveySummary]
        public ActionResult Index()
        {
            ViewBag.Title = string.Format("List of Surveys for {0}", User.Identity.Name);
            ViewBag.Message = string.Empty;
            var surveys = _surveyRepository.GetSurveysForUser(User.Identity.Name).ToArray();
            return View(surveys);
        } 

        //
        // GET: /Survey/Create
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Create()
        {
            CreateSurveyViewModel survey = new CreateSurveyViewModel();
            survey.CategoryList = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Description");

            return View(survey);
        }

        //
        // POST: /Survey/Create
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Create(CreateSurveyViewModel survey)
        {
            if (ModelState.IsValid)
            {
                //var newSurvey = AutoMapper.Mapper.Map<CreateSurveyViewModel, Survey>(survey);
                var newSurvey = Core.Factories.SurveyFactory.CreateSurvey();

                //  Add the info enter by user
                newSurvey.Title = survey.Title;
                //  Add the Category, from the selected one, and the User from the HttpContext
                //  so we can add the new survey.  These values are not availble within the 
                //  Business model
                newSurvey.User = _userRepository.GetUserByName(HttpContext.User.Identity.Name);
                newSurvey.Category = _categoryRepository.GetCategory(survey.CategoryId);

                //  Add the new survey to the model, and retrieve its key so we can progress to the Add Questions
                long id = _surveyRepository.CreateSurvey(newSurvey);

                return RedirectToAction("Create", "Question", new { surveyId = id });
            }
            else
            {
                //  Get the categories again, they're not retained between pages.
                survey.CategoryList = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Description");
                return View(survey);
            }
        }


        [HttpPost]
        [AjaxActionOnly]
        public ActionResult Submit(long surveyId)
        {
            string result = "Failed";
            var survey = _surveyRepository.GetSurvey(surveyId);
            if (survey.SubmitForApproval())
            {
                Survey[] surveys = new Survey[] { survey };
                _surveyRepository.UpdateSurveys(surveys);
                result = "OK";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles="Administrator")]
        [MapSurveyToApprovalListViewModel]
        public ActionResult Approve()
        {
            var surveys = _surveyRepository.GetSurveysForApproval();
            return View(surveys);
        }


        [HttpPost]
        [Authorize(Roles="Administrator")]
        public ActionResult Approve(IEnumerable<ApprovalListViewModel.ApprovalInputViewModel> Input)
        {
            if (ModelState.IsValid)
            {
                IList<Survey> approvedSurveys = new List<Survey>();
                foreach (var inputApproval in Input)
                {
                    if (inputApproval.Approve)
                    {
                        Survey survey = _surveyRepository.GetSurvey(inputApproval.SurveyId);
                        survey.Approve();
                        approvedSurveys.Add(survey);
                    }
                }
                _surveyRepository.UpdateSurveys(approvedSurveys);
                //  Return to display to allow more surveys to be approved.
                return RedirectToAction("Approve");
            }
            else
            {
                return View(Input);
            }
        }

    }
}
