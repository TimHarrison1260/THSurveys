using System;
using System.Web.Mvc;           //  Access to Action filters
using THSurveys.Models.Home;    //  Access to the ViewModels.
using Core.Interfaces;          //  Access to the interfaces for Category Repository

namespace THSurveys.Filters
{
    [Obsolete("Do NOT use this filter under ANY circumstances, it causes a 'DbContext Disposed' error.", true)]
    public class MapSurveyToHomeSurveyListAttribute : ActionFilterAttribute
    {
        //  Inject the repository for the category.
        //
        //  NB!!!!!  WARNING !!!!!!!!!
        //  By the time this repository is used to access the categories, on the 2nd call to this 
        //  filter, the underlying DbContext has been disposed and therefore causes a "DbContext 
        //  disposed" exception, which appears on when the page is being rendered.
        //
        //  NEVER!!!!!!  NEVER!!!!!  NEVER!!!!!
        //  Do NOT, under any circumstances, use a DbContext from within an Action filter, either
        //  directly or via a repository.
        //
        //  For this mapper filter, the only reason for it was to add the categories so
        //  that they can be displayed in a drop down box.  Therefore, with that being the
        //  cause of the error, it negates the use of this filter.  Hence it's marked "Obsolete"
        //  The logic is transferred to the controller action method. (Hence this diatribe.
        //
        //
        private readonly ICategoryRepository _categoryRepository = (ICategoryRepository)DependencyResolver.Current.GetService(typeof(ICategoryRepository));

        /// <summary>
        /// Filter to apply the mapping between the Core.Survey business object
        /// and the Model.TopTenSurveysViewModel.  It uses Automapper to perform
        /// the mapping.  The configuration of Automapper is in the Mapping folder
        /// </summary>
        /// <param name="filterContext">The filterContext returned by the Action method, containing the Controller viewData.</param>
        /// <remarks>
        /// We override the OnActionExecuted so that the mapping is done AFTER the
        /// action method has executed.  The action method returns the Domain,Order
        /// object and this filter passes the mapped Model.OrderDetailViewModel 
        /// to the view prior to it being rendered.
        /// </remarks>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //  create an instance of the viewmodel class and map the incomming data to it
            HomeLChooseViewModel viewModel = new HomeLChooseViewModel();
            //  populate the viewModel
            viewModel.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Description");
            //replace the viewData in the filterContext.
            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}