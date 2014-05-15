using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Paging
{
   
    public static class AjaxHelperExtension
    {
        public static IHtmlString PagedActionLink(this AjaxHelper helper, string linkText, int pageNumber, int pageSizeToDisplay, string actionName, string controllerName, string cssClass, AjaxOptions ajaxOptions)
        {
            
            var pageLinkValueDictionary = new RouteValueDictionary {{"page", pageNumber}, {"count", pageSizeToDisplay}};
            if (actionName != null)
            {
                if (pageLinkValueDictionary.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                }
                pageLinkValueDictionary.Add("action", actionName);
            }

            if (controllerName != null)
            {
                if (pageLinkValueDictionary.ContainsKey("controller"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "controllerName");
                }
                pageLinkValueDictionary.Add("controller", controllerName);
            }
         

            return helper.ActionLink(linkText, actionName, controllerName, pageLinkValueDictionary, ajaxOptions, new { @class = cssClass });

        }

        public static string ReplaceQueryStringValue(this string url, string key, string value)
        {
            return Regex.Replace(
                url,
                @"([?&]" + key + ")=[^?&]+",
                "$1=" + value);
        }
    }
}
