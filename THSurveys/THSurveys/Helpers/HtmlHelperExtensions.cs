using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;          //  Allows the Expression within the parameter list of the helper.
using System.Web;
using System.Web.Mvc;

namespace THSurveys.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Extension method to inject an index number into the name attribute
        /// so that multiple groups of radio buttons can be displayed on a 
        /// page, and they don't interfere with each other.
        /// </summary>
        /// <returns>MvcHtmlString containing the markup for the radio button.</returns>
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object value, string groupId, string checkedValue, object htmlAttributes)
        {
            var member = (MemberExpression)expression.Body;

            //  The prefix to the property (item.propertyName) is accessible from 
            //  expression.member.name at runtime, but the Name property is not available
            //  from the expression at compile time.  
            //  TODO: investigate how to access the prefix so it can correctly be combined with the 'id' and 'name' attributes.            
            var itemName = member.Member.Name;
            
            string idAttr = itemName;
            string nameAttr = itemName + "_" + groupId;
            string valueAttr = value.ToString();

            TagBuilder tag = new TagBuilder("input");

            //  Mark the radio button as checked if the answer (checkedValue) is the same as the value
            //  supplied.  This allows the selected readio button to be reinstated when redisplaying 
            //  the control.
            if ((checkedValue != null)  && (checkedValue == value.ToString()))
                tag.Attributes.Add("checked", "checked");

            tag.Attributes.Add("id", idAttr);
            tag.Attributes.Add("name", nameAttr);
            tag.Attributes.Add("type", "radio");
            tag.Attributes.Add("value", valueAttr);
            MvcHtmlString result = new MvcHtmlString(tag.ToString());
            return result;
        }

    }
}
