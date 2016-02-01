using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public static class PaginationExtensions
    {
        public static HtmlString GetDefaultPagination(
            this HtmlHelper htmlHelper,
            int pageIndex,
            int totalCount,
            int pageSize,
            PaginationSizing paginationSizing,
            PaginationAligment paginationAligment,
            int numericLinksCount,
            bool paginationInfo,
            object htmlAttributes,
            BootstrapVersion bootstrapVersion,
            string onClick,
            bool href,
            Uri currentUrl,            
            string[] removedParameters,
            Dictionary<string, string> addedParameters)
        {
            return Pagination.GetDefaultPagination(pageIndex, totalCount, pageSize, paginationSizing, paginationAligment, numericLinksCount, paginationInfo, htmlAttributes, bootstrapVersion, onClick, href, currentUrl, removedParameters, addedParameters);
        }

        public static HtmlString GetPagerPagination(
            this HtmlHelper htmlHelper,
            int pageIndex,
            int totalCount,
            int pageSize,
            bool alignedLinks,
            object htmlAttributes,
            string onClick,
            bool href,
            Uri currentUrl,
            string[] removedParameters,
            Dictionary<string, string> addedParameters)
        {
            return Pagination.GetPagerPagination(pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes, onClick, href, currentUrl, removedParameters, addedParameters);
        }
    }
}