using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interfaces;
using Core.Model;

using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

using THSurveys.Models;
using THSurveys.Filters;

namespace THSurveys.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Constructor for SurveyController, we use DI to inject the instance of the repository.
        /// </summary>
        /// <param name="repository"></param>
        public SurveyController(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository)
        {
            //  Throw exception if any of the dependencies are null.
            if (surveyRepository == null)
                throw new ArgumentNullException("SurveyRepository", "No valid Survey repository supplied to SurveyController.");
            if (categoryRepository == null)
                throw new ArgumentNullException("CategoryRepository", "No valid category repository supplied to SurveyController.");

            _surveyRepository = surveyRepository;
            _categoryRepository = categoryRepository;
        }


        //
        // GET: /Survey/

        [AllowAnonymous]
        [MapSurveyToSurveySummary]
        public ActionResult Index()
        {
            ViewBag.Title = "List of Surveys for 'UserName'";
            ViewBag.Message = "Select a Category to list the surveys within it.";
            var surveys = _surveyRepository.GetAllSurveys().ToArray();
            return View(surveys);
        } 

        //
        // GET: /Survey/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Survey/Create

        public ActionResult Create()
        {
            CreateSurveyViewModel survey = new CreateSurveyViewModel();
            //  Load the viewModel with the appropriate stuff for a new Survey.
            //survey.StatusDate = DateTime.Now;
            //survey.CategoryId = 0;
            //survey.Owner = 1;       //  TODO: get the userId from the login.
            //survey.Status = (int)SurveyStatusEnum.Incomplete;
            survey.CategoryList = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Description");

            return View(survey);
        }

        //
        // POST: /Survey/Create

        [HttpPost]
        public ActionResult Create(Core.Model.Survey survey)
        {
            //  Default StatusDate for the moment to today
            //survey.StatusDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                //  Map to a Survey object from viewModel
                //      Refactor this code to a custom model binder for this 
                //Survey newSurvey = new Survey();
                //newSurvey.Id = survey.Id;                       //  Will be set by the database.
                //newSurvey.Category = survey.ca;  //  Get the Category for the Id, to include in the model
                //newSurvey.Owner = survey.Owner;                 //  This will be retrieved from the logged on user cookie
                //newSurvey.Status = survey.Status;               //  This will always be set to Incomplete for a Create, so no need for user to set it.
                //newSurvey.StatusDate = (DateTime)survey.StatusDate; //  This gets set to now, on the GET create method
                //newSurvey.Title = survey.Title;                 //  About the only user input

                _surveyRepository.CreateSurvey(survey);      //  Add the survey to the model.

                return RedirectToAction("Index");               //  Go back to the Survey index page.
            }
            else
            {
                //  Get the categories again, they're not retained between pages.
                survey.CategoryList = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Description");
                return View();
            }
        }

        //
        // GET: /Survey/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Survey/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Survey/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Survey/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
