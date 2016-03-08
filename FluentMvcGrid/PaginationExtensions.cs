using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public static class PaginationExtensions
    {
        public static PaginationSummary GetSummaryPagination(this HtmlHelper htmlHelper, int pageIndex, int totalCount, int pageSize)
        {
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            var summary = new PaginationSummary(
                pageCount,
                pageIndex,
                totalCount,
                pageSize,
                new PaginationSummaryItem(pageIndex > 1, pageIndex > 1 ? 1 : (int?)null),
                new PaginationSummaryItem(pageIndex > 1, pageIndex > 1 ? pageIndex - 1 : (int?)null),
                new PaginationSummaryItem(pageIndex + 1 <= pageCount, pageIndex + 1 <= pageCount ? pageIndex + 1 : (int?)null),
                new PaginationSummaryItem(pageIndex < pageCount, pageIndex < pageCount ? pageCount : (int?)null));
            return summary;
        }

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