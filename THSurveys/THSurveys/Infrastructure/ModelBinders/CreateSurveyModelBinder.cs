using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebMatrix.WebData;
//using System.Web.Security;

using Core.Model;
using Core.Interfaces;

namespace THSurveys.Infrastructure.ModelBinders
{
    /// <summary>
    /// Class <c>CreateSurveyModelBinder</c> is responsible for binding the
    /// CreateSurveyViewModel class to the Core.Model.Survey class for the
    /// Create Survey page.
    /// </summary>
    public class CreateSurveyModelBinder : IModelBinder
    {
        private ICategoryRepository _repository;
        /// <summary>
        /// Constructor to allow dependency (repository) to be injected.
        /// </summary>
        /// <param name="repository"></param>
        public CreateSurveyModelBinder(ICategoryRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository is null");
            _repository = repository;

            //            _repository = DependencyResolver.Current.GetService<Core.Interfaces.ICategoryRepository>();

        }

        /// <summary>
        /// Responsible for binding the view model to the domain model
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //  check valid contexts are passed in.
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext", "controller context is missing.");
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext", "binding context is missing.");

            //  Find the Title and Category from the incomming bindingContext
            string title = TryGet<string>(bindingContext, "Title");
            string categoryId = TryGet<string>(bindingContext, "Categoryid");
            int i =  Convert.ToInt32(categoryId);
            //  Get the default values for the rest of the Survey model properties
            Category category = _repository.GetCategory(i);
            string userName = controllerContext.HttpContext.User.Identity.Name;
            int owner = WebSecurity.GetUserId(userName);
            //  May need to have method to get the Id for a Username to set the Owner

            int status = (int)SurveyStatusEnum.Incomplete;
            DateTime statusDate = DateTime.Now;

            Survey survey = new Survey()
            {
                Title = title,
                CategoryId = category.CategoryId,
                UserId = owner,
                Status = status,
                StatusDate = statusDate
            };

            //  Return the bound model
            return survey;
        }

        /// <summary>
        /// copied from CustomModelBinderDemo.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindingContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private T TryGet<T>(ModelBindingContext bindingContext, string key) where T : class
        {
            if (String.IsNullOrEmpty(key))
                return null;

            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix == true)
                valueResult = bindingContext.ValueProvider.GetValue(key);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            if (valueResult == null)
                return null;

            try
            {
                return (T)valueResult.ConvertTo(typeof(T));
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
                return null;
            }
        }
    }
}