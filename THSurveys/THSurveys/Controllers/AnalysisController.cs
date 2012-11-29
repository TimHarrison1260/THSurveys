using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Core.Factories;
using Core.Services;
using Core.Interfaces;
using THSurveys.Mappings;
using THSurveys.Models.Analysis;
using THSurveys.Infrastructure.Factories;
using THSurveys.Filters;

namespace THSurveys.Controllers
{
    [Authorize]
    public class AnalysisController : Controller
    {
        /// <summary>
        /// Holds an instance of the Domain Model Repositories
        /// </summary>
        private readonly ISurveyRepository _surveyRepository;

        /// <summary>
        /// AnalysisController Constructor which injects the instance of the Domain Model Repositories.
        /// </summary>
        /// <param name="repository">Instance of the Domain Model Repository</param>
        public AnalysisController(ISurveyRepository surveyRepository) 
        {
            if (surveyRepository == null)
                throw new ArgumentNullException("SurveyRepository", "No valid Survey repository supplied to AnalysisController");
            _surveyRepository = surveyRepository;
        }

        //
        // GET: /Analysis/
        [Authorize(Roles="User")]
        [AuthorisedForSurvey]
        public ActionResult QuestionList(long id)
        {
            //  Get the survey results
            var survey = _surveyRepository.GetSurvey(id);

            //  Map them to the SurvayAnalysisViewModel
            var viewModel = MapSurveyToSurveyAnalysisViewModel.Map(survey);

            //  Render the view
            return View(viewModel);
        }

        /// <summary>
        /// This is NOT an Ajax call, but serviced by the src attribute
        /// of the img tag.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles="User")]
        [AuthorisedForSurvey]
        public FileResult GetSurveyChart(long id, long questionId)
        {
            //  TODO: New method on Survey
            var survey = _surveyRepository.GetSurvey(id);

            //  Create the data for the chart:  Map surey Results to Chart Data;
            ChartData data = MapSurveyToSurveyChart.Map(survey, questionId);

            //  Create the survey chart object (taking the data model in, throwing exception if it's not there.
            MVCChartFactory factory = new SurveyChartFactory();
            var surveychart = factory.Create(data);

            //  launch the GeneratChart method to create the MemoryStream (an in-memory image file.
            System.IO.MemoryStream image = surveychart.GenerateChartImage(data.Width, data.Height);

            //  return a File object containing the image in the form of the memoryStream object
            return File(image.GetBuffer(), @"image/png", "SurveyChart.png");
        }
    
    }
}
