using System.Web.Mvc;           //  Access to Action filters
using THSurveys.Models.Home;    //  Access to the ViewModels.
using Core.Interfaces;          //  Access to the interfaces for Category Repository

namespace THSurveys.Filters
{
    public class MapSurveyToHomeSurveyListAttribute : ActionFilterAttribute
    {
        //  Inject the repository for the category
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
            HomeListViewModel viewModel = new HomeListViewModel();
            //  populate the viewModel
            viewModel.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Description");
            //replace the viewData in the filterContext.
            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}