﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Web.Ajax.Paging
{
    /// <summary>
    /// Represents a pager control on a web page.
    /// </summary>
    /// <remarks>
    /// Adapted from an early version of the paged list available at http://pagedlist.codeplex.com/.
    /// </remarks>
    public class Pager
    {
        #region Fields

        private ViewContext viewContext;
        private readonly int pageSize;
        private readonly int currentPage;
        private readonly int totalItemCount;
        private readonly System.Web.Routing.RouteValueDictionary linkWithoutPageValuesDictionary;
        private readonly AjaxHelper _ajaxHelper;
        private readonly AjaxOptions _ajaxOptionsValues;
        private string _actionName;
        private string _controllerName;      
        private string _filter;

        #endregion
        delegate string GeneratePageLinkDelegate(string linkText, string name, int pageNumber, int pageSizeToDisplay);

        private GeneratePageLinkDelegate _generatePageLink;
        #region Properties

        // [jelled] Added global properties to control markup.

        /// <summary>
        /// The number of page links to display on the pager. The default is 10.
        /// </summary>
        public static int NumPageLinksToDisplay { get; set; }

        /// <summary>
        /// The page sizes to display that the user can change. The default is 10, 25 and 50.
        /// </summary>
        public static IEnumerable<int> PageSizesToDisplay { get; set; }

        /// <summary>
        /// The parameter for the page number in the URL. The default is "page".
        /// </summary>
        public static string PageNumberParameterName { get; set; }

        /// <summary>
        /// The parameter for the page size in the URL. The default is "count".
        /// </summary>
        public static string PageSizeParameterName { get; set; }

        /// <summary>
        /// The name of the CSS style for the div element that contains the pager. The default is "pager".
        /// </summary>
        public static string StyleNamePagerDiv { get; set; }

        /// <summary>
        /// The name of the CSS style for the span element that contains the pages. The default is "pages".
        /// </summary>
        public static string StyleNamePagesSpan { get; set; }

        /// <summary>
        /// The name of the CSS style for the span element that contains the page info. The default is "pageinfo".
        /// </summary>
        public static string StyleNamePageInfoSpan { get; set; }

        /// <summary>
        /// The name of the CSS style for the span element that contains the page sizes. The default is "pagesizes".
        /// </summary>
        public static string StyleNamePageSizesSpan { get; set; }

        /// <summary>
        /// The name of the CSS style for the currently selected link. The default is "current".
        /// </summary>
        public static string StyleNameCurrentLink { get; set; }

        /// <summary>
        /// The name of the CSS style for a disabled link. The default is "disabled".
        /// </summary>
        public static string StyleNameDisabledLink { get; set; }

        /// <summary>
        /// The format string for the page info, where {0} is the first shown item, {1} is the last shown item and {2} is the total items. The default is "{0} - {1} / {2}".
        /// </summary>
        public static string PageInfoFormat { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="Pager"/> class.
        /// </summary>
        static Pager()
        {
            NumPageLinksToDisplay = 5;
            PageNumberParameterName = "page";
            PageSizeParameterName = "count";
            PageSizesToDisplay = new int[] { 3, 4, 5 };
            StyleNamePagerDiv = "pager";
            StyleNamePagesSpan = "pages";
            StyleNamePageInfoSpan = "pageinfo";
            StyleNamePageSizesSpan = "pagesizes";
            StyleNameCurrentLink = "current";
            StyleNameDisabledLink = "disabled";
            PageInfoFormat = "{0} - {1} / {2}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pager"/> class.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <param name="valuesDictionary">The values dictionary.</param>
        public Pager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
        {
            this.viewContext = viewContext;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            this.linkWithoutPageValuesDictionary = valuesDictionary;
        }

        public Pager(ViewContext viewContext, string name, int pageSize, int currentPage, int totalItemCount, AjaxHelper ajaxHelper, string actionName, string controllerName, AjaxOptions ajaxOptions)
        {
            this.viewContext = viewContext;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            this._ajaxOptionsValues = ajaxOptions;
            this._ajaxHelper = ajaxHelper;
            this._actionName = actionName;
            this._controllerName = controllerName;
            this.linkWithoutPageValuesDictionary = new RouteValueDictionary();
            this._filter = name;
        }

        public Pager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, AjaxHelper ajaxHelper, string actionName, string controllerName, AjaxOptions ajaxOptions)
        {
            this.viewContext = viewContext;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            this._ajaxOptionsValues = ajaxOptions;
            this._ajaxHelper = ajaxHelper;
            this._actionName = actionName;
            this._controllerName = controllerName;
            this.linkWithoutPageValuesDictionary = new RouteValueDictionary();
      
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Renders the HTML.
        /// </summary>
        /// <returns>The HTML for the pager.</returns>
        public MvcHtmlString RenderHtml()
        {
            int pageCount = (int)Math.Ceiling(this.totalItemCount / (double)this.pageSize);

            // [jelled] Changed so that no pager displays if not necessary.
            if (pageCount <= 1)
            {
                return new MvcHtmlString(string.Empty);// string.Empty;
            }

            var sb = new StringBuilder();
            sb.AppendFormat("<div class=\"{0}\">", StyleNamePagerDiv);

            // Pages
            sb.AppendFormat("<span class=\"{0}\">", StyleNamePagesSpan);
            RenderPagerHtml(sb, pageCount);
            sb.Append("</span>");

            // Page Info
            sb.AppendFormat("<span class=\"{0}\">", StyleNamePageInfoSpan);
            RenderPageInfoHtml(sb);
            sb.Append("</span>");

            // Page Sizes
            //sb.AppendFormat("<span class=\"{0}\">", StyleNamePageSizesSpan);
            //RenderPageSizerHtml(sb);
            //sb.Append("</span>");

            // Close
            sb.Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        private void RenderPagerHtml(StringBuilder sb, int pageCount)
        {
            int nrOfPagesToDisplay = NumPageLinksToDisplay;

            // Previous
            if (this.currentPage > 1)
            {
                sb.Append(GeneratePageLink("<", this.currentPage - 1));
            }
            else
            {
                sb.AppendFormat("<span class=\"{0}\">&lt;</span>", StyleNameDisabledLink);
            }

            int start = 1;
            int end = pageCount;

            if (pageCount > nrOfPagesToDisplay)
            {
                int middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                int below = (this.currentPage - middle);
                int above = (this.currentPage + middle);

                if (below < 4)
                {
                    above = nrOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 4))
                {
                    above = pageCount;
                    below = (pageCount - nrOfPagesToDisplay);
                }

                start = below;
                end = above;
            }

            if (start > 3)
            {
                sb.Append(GeneratePageLink("1", 1));
                sb.Append(GeneratePageLink("2", 2));
                sb.Append("...");
            }
            for (int i = start; i <= end; i++)
            {
                if ((i) == this.currentPage || (this.currentPage == 0 && i == 1 ))
                {
                    sb.AppendFormat("<span class=\"{0}\">{1}</span>", StyleNameCurrentLink, i);
                }
                else
                {
                    sb.Append(GeneratePageLink(i.ToString(CultureInfo.CurrentCulture), i));
                }
            }
            if (end < (pageCount - 3))
            {
                sb.Append("...");
                sb.Append(GeneratePageLink((pageCount - 1).ToString(CultureInfo.CurrentCulture), pageCount - 1));
                sb.Append(GeneratePageLink(pageCount.ToString(CultureInfo.CurrentCulture), pageCount));
            }

            // Next
            if (this.currentPage < pageCount)
            {
                sb.Append(GeneratePageLink(">", (this.currentPage + 1)));
            }
            else
            {
                sb.AppendFormat("<span class=\"{0}\">&gt;</span>", StyleNameDisabledLink);
            }
        }

        private void RenderPageInfoHtml(StringBuilder sb)
        {
            var totalItems = this.totalItemCount;
            var firstItem = 1 + (((this.currentPage > 0) ?  (this.currentPage - 1): 0) * this.pageSize);
            var lastItem = Math.Min(totalItems, firstItem + this.pageSize - 1);
            sb.AppendFormat(PageInfoFormat, firstItem, lastItem, totalItems);
        }

        private void RenderPageSizerHtml(StringBuilder sb)
        {
            foreach (var pageSizeToDisplay in PageSizesToDisplay)
            {
                if (pageSizeToDisplay == this.pageSize)
                {
                    sb.AppendFormat("<span class=\"{0}\">{1}</span>", StyleNameCurrentLink, pageSizeToDisplay);
                }
                else
                {
                    // Always go to page 1 when changing page size.
                    if (null != _ajaxOptionsValues) _generatePageLink = GenerateAjaxedPageLink;
                    else _generatePageLink = GeneratePageLink;
                         sb.Append(_generatePageLink(pageSizeToDisplay.ToString(CultureInfo.CurrentCulture),null, 1, pageSizeToDisplay));
                }
            }
        }


        private string GeneratePageLink(string linkText, string name, int pageNumber, int pageSizeToDisplay)
        {
            var pageLinkValueDictionary = new RouteValueDictionary(this.linkWithoutPageValuesDictionary)
                {
                    {PageNumberParameterName, pageNumber},
                    {PageSizeParameterName, pageSizeToDisplay}
                };
            var virtualPathData = RouteTable.Routes.GetVirtualPath(this.viewContext.RequestContext, pageLinkValueDictionary);

            if (virtualPathData != null)
            {
                string linkFormat = "<a class=\"btn\" href=\"{0}\">{1}</a>";
                return String.Format(CultureInfo.InvariantCulture, linkFormat, HttpUtility.HtmlAttributeEncode(virtualPathData.VirtualPath), linkText);
            }
            else
            {
                return null;
            }
        }

        private string GeneratePageLink(string linkText, int pageNumber)
        {
            if (this._ajaxOptionsValues != null) return GenerateAjaxedPageLink(linkText, _filter, pageNumber , this.pageSize)
            ;
            
            return GeneratePageLink(linkText, null,pageNumber, this.pageSize);
        }


       

        #endregion
        #region [AjaxPaging]

        private string GenerateAjaxedPageLink(string linkText, string name,int pageNumber, int pageSizeToDisplay)
        {
            var pageLinkValueDictionary = new RouteValueDictionary { {"name", name},{ "pageNo", pageNumber }, { "count", pageSizeToDisplay } };

            var opt = new AjaxOptions();
            _ajaxOptionsValues.Url = _ajaxOptionsValues.Url.ReplaceQueryStringValue("pageNo", pageNumber.ToString(CultureInfo.InvariantCulture));
            return _ajaxHelper.ActionLink(linkText, _actionName, _controllerName, pageLinkValueDictionary, _ajaxOptionsValues,new {@class = "btn"}).ToString();



        }
        #endregion [AjaxPaging]
    }

     
}