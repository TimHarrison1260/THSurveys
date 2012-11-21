using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebMatrix.WebData;
//using System.Web.Security;

using Core.Model;
using Core.Interfaces;
using THSurveys.Models;

namespace THSurveys.Infrastructure.ModelBinders
{
    /// <summary>
    /// Class <c>CreateSurveyModelBinder</c> is responsible for binding the
    /// CreateSurveyViewModel class to the Core.Model.Survey class for the
    /// Create Survey page.
    /// </summary>
    /// <remarks>
    /// 
    /// Do NOT use repositories within a model binder.
    /// When using the 'unit of work' pattern to 
    /// inject the same instance of a repository into
    /// various components of the mvc app, doing so
    /// can lead to "DBContext has been disposed"
    /// errors.  NOT good practice.
    /// </remarks>
    [Obsolete("Do Not use this modelbinder", true)]
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

            //  See Sandersen for explanation of this coding and why we do it.
            //  See if model is there to update, create one if there isn't
            CreateSurveyViewModel model = (CreateSurveyViewModel)bindingContext.Model ??
                (CreateSurveyViewModel)DependencyResolver.Current.GetService(typeof(CreateSurveyViewModel));
            //  See if the value provider has the required prefix, which would be 'CreateSurveyViewModel'
            bool hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            string searchPrefix = (hasPrefix) ? bindingContext.ModelName + "." : "";

            //  Set the values of the model fields.
            model.Title = TryGet<string>(bindingContext, searchPrefix, "Title");

            model.CategoryId = Convert.ToInt64(TryGet<string>(bindingContext, searchPrefix, "Categoryid"));
            //  Do not populate the navigation property for an ADD operation, otherwise we may get an exception
            //  when adding the Survey to the DbContext.
            //  "An Entity Object cannot reference multiple instances of IEntityChangeTracker".  This happens
            //  when two items within the same eneity model are populated from objects from separate instance
            //  of the DbContext.
//            model.Category = _repository.GetCategory(model.CategoryId);
  //          model.Category = null;

            //  Set the default values for the rest of the Survey model properties

            string userName = controllerContext.HttpContext.User.Identity.Name;
 //           model.Owner = WebSecurity.GetUserId(controllerContext.HttpContext.User.Identity.Name);
   //         model.Status = (int)SurveyStatusEnum.Incomplete;
     //       model.StatusDate = DateTime.Now;

            //  Return the bound model
            return model;
        }

        /// <summary>
        /// copied from CustomModelBinderDemo.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindingContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private T TryGet<T>(ModelBindingContext bindingContext, string prefix, string key) where T : class
        {
            //  didn't find anything, no key supplied
            if (String.IsNullOrEmpty(key))
                return null;

            //  get the result from the value provider.
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(prefix + key);
            //  try setting the model value, invokes the vadation rules.
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
            //  No such value supplied
            if (valueResult == null)
                return null;
            //  convert the result to its type
            try
            {
                return (T)valueResult.ConvertTo(typeof(T));
            }
            catch (Exception ex)
            {
                //  convertion not possible, return null and set a modelerror.
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
                return null;
            }
        }
    }
}