//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//using Core.Model;
//using Core.Interfaces;
//using THSurveys.Models;

//namespace THSurveys.Filters
//{
//    public class CreateSurveyMapAttribute : ActionFilterAttribute
//    {
//        private readonly ICategoryRepository _repository;

//        public CreateSurveyMapAttribute (ICategoryRepository repository)
//        {
//            if (repository == null)
//                throw new ArgumentNullException("No valid repository supplied");
//            _repository = repository;
//        }

//        /// <summary>
//        /// Map this from the CreateSurveyViewModel to the Core.Model.Survey class
//        /// </summary>
//        /// <param name="filterContext"></param>
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            //  Strip viewmodel out of context
//            CreateSurveyViewModel viewModel = (CreateSurveyViewModel)filterContext.Controller.ViewData.Model;
//            //  Map this to the Core.ModelSurvey class

//            Survey newSurvey = new Survey();
//            newSurvey.Id = viewModel.Id;                       //  Will be set by the database.
//            newSurvey.Category = _repository.GetCategory(viewModel.CategoryId);  //  Get the Category for the Id, to include in the model
//            newSurvey.Owner = viewModel.Owner;                 //  This will be retrieved from the logged on user cookie
//            newSurvey.Status = viewModel.Status;               //  This will always be set to Incomplete for a Create, so no need for user to set it.
//            newSurvey.StatusDate = (DateTime)viewModel.StatusDate; //  This gets set to now, on the GET create method
//            newSurvey.Title = viewModel.Title;                 //  About the only user input

//            //  Replace the model in the context
//            filterContext.Controller.ViewData.Model = newSurvey;
            
////            base.OnActionExecuting(filterContext);
//        }



//    }
//}