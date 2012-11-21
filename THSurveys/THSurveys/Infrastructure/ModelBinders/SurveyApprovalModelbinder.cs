using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THSurveys.Infrastructure.ModelBinders
{
    /// <summary>
    /// This is not even started really.  Not the best solution
    /// to the prolem.  
    /// </summary>
    /// <remarks>
    /// Leave this here but NOTE, it is NOT part of the fuctioning
    /// solution.
    /// </remarks>
    [Obsolete("Do not require to use this",true)]
    public class SurveyApprovalModelbinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return base.BindModel(controllerContext, bindingContext);
        }


        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, 
            System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(bool?) && propertyDescriptor.Name == "item.Approve")
            {
                var request = HttpContext.Current.Request;
                var approve = request["item.Approve"];



            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

    }
}