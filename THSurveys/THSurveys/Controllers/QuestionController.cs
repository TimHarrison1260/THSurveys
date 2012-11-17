using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.UI.DataVisualization.Charting;

using THSurveys.Models.Question;
using THSurveys.Models;             //  Access Survey and Categories
using Core.Interfaces;
using Core.Model;
using THSurveys.Filters;            //  Access to mapping attributes.

using AutoMapper;

namespace THSurveys.Controllers
{
    [Authorize]
    [HandleError]
    public class QuestionController : Controller
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IQuestionRepository _questionRepository;

        /// <summary>
        /// Constructor for SurveyController, we use DI to inject the instance of the repository.
        /// </summary>
        /// <param name="repository"></param>
        public QuestionController(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository, 
            ITemplateRepository templateRepository, IQuestionRepository questionRepository)
        {
            //  Throw exception if any of the dependencies are null.
            if (surveyRepository == null)
                throw new ArgumentNullException("SurveyRepository", "No valid Survey repository supplied to QuestionController.");
            if (categoryRepository == null)
                throw new ArgumentNullException("CategoryRepository", "No valid category repository supplied to QuestionController.");
            if (templateRepository == null)
                throw new ArgumentNullException("TemplateRepository", "No valid Template repository supplied to QuestionController.");
            if (questionRepository == null)
                throw new ArgumentNullException("QuestionRepository", "No valid Question repository supplied to QuestionController.");

            _surveyRepository = surveyRepository;
            _categoryRepository = categoryRepository;
            _templateRepository = templateRepository;
            _questionRepository = questionRepository;
        }


        [HttpGet]
        [Authorize(Roles = "User")]
        //[ChildActionOnly]
        public ActionResult Create(long surveyId)
        {
            //  TODO:   Create a Factory to create the AddQuestionViewModel taking in a surveyId.
            AddQuestionsViewModel question = new AddQuestionsViewModel();
            question.SurveyId = surveyId;

            Survey relatedSurvey = _surveyRepository.GetSurvey(surveyId);
            question.SurveyId = relatedSurvey.SurveyId;
            question.Title = relatedSurvey.Title;
            question.CategoryDescription = _categoryRepository.GetCategory(relatedSurvey.Category.CategoryId).Description;

            question.LikertTemplates = new SelectList(_templateRepository.GetLikertTemplates(), "Id", "Title");

            return View(question);
        }

        /// <summary>
        /// Crates a Question and adds it to the related Survey.  The viewModel contains the SurveyId.
        /// </summary>
        /// <param name="question">The AddQuestionsViewModel, containing the user input.</param>
        /// <returns>Returns to the AddQuestions view.</returns>
        /// <remarks>
        /// Intend to convert this to an Ajax call from the client, if there's time and the validation
        /// will still work
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "User")]
        //[ChildActionOnly]
        public ActionResult Create(AddQuestionsViewModel question)
        {
            if (ModelState.IsValid)
            {
                //  Map the info to a Question domain model.                            (USER INPUT)
                Question newQuestion = Mapper.Map<AddQuestionsViewModel, Question>(question);

                //  Add the related survey                                              (USER INPUT)
                newQuestion.Survey = _surveyRepository.GetSurvey(question.SurveyId);
                
                //  Create the Available from the selected TemplateResponses.           (USER SELECTION)
                var templateResponses = _templateRepository.GetLikertScaleResponses(question.LikertId).ToArray();
                newQuestion.AvailableResponses = Mapper.Map<TemplateResponse[], AvailableResponse[]>(templateResponses);

                //  Add the sequence number, unique within the survey                   (BUSINESS LOGIC)
                long seqNo = _questionRepository.GetLastSequenceNumber(question.SurveyId);
                newQuestion.SequenceNumber = ++seqNo;

                //  add the question to the model
                long id = _questionRepository.AddQuestion(newQuestion);

                return RedirectToAction("Create", new { surveyId = question.SurveyId });
            }
            else
            {
                //  TODO:   Create a Factory to create the AddQuestionViewModel taking in a surveyId.
                //  Re-instate the reference values.
                Survey relatedSurvey = _surveyRepository.GetSurvey(question.SurveyId);
                question.SurveyId = relatedSurvey.SurveyId;
                question.Title = relatedSurvey.Title;
                question.CategoryDescription = _categoryRepository.GetCategory(relatedSurvey.Category.CategoryId).Description;

                question.LikertTemplates = new SelectList(_templateRepository.GetLikertTemplates(), "Id", "Title");
                return View(question);
            }
        }

        /// <summary>
        /// Returns the templated responses for the supplied Likert Id.
        /// </summary>
        /// <param name="id">The id of the Likert Template</param>
        /// <returns>Partialview, containing the responses</returns>
        /// <remarks>
        /// Intended for use with an Ajax call from the interface.
        /// </remarks>
        [HttpPost]
        [MapResponseTemplateToResponsesViewModel]
        [AjaxActionOnly]
        public ActionResult GetLikertResponses(long id)
        {
            var responses = _templateRepository.GetLikertScaleResponses(id);
            return PartialView("_LikertResponses", responses.ToArray());
        }

        /// <summary>
        /// Returns a list of the questions defined for the specified SurveyId.
        /// </summary>
        /// <param name="id">The Id of the survey</param>
        /// <returns>PartialView, containing the questions.</returns>
        /// <remarks>
        /// Intended for use with an Ajax call from the interface.
        /// </remarks>
        [HttpPost]
        [MapQuestionToAddQuestionsListviewModel]
        [AjaxActionOnly]
        public ActionResult GetQuestionsForSurvey(long id)
        {
            var questions = _questionRepository.GetQuestionsForSurvey(id);
            return PartialView("_AddQuestionsList", questions.ToArray());
        }
    }
}
