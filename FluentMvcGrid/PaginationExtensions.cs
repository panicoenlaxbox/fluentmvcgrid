using System;
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
            int numericLinksCount,
            bool paginationInfo,
            object htmlAttributes,
            BootstrapVersion bootstrapVersion,
            Uri currentUrl)
        {
            return Pagination.GetDefaultPagination(pageIndex, totalCount, pageSize, paginationSizing, numericLinksCount, paginationInfo, htmlAttributes, bootstrapVersion, currentUrl);
        }

        public static HtmlString GetDefaultPagination(
            this HtmlHelper htmlHelper, 
            int pageIndex, 
            int totalCount,
            int pageSize, 
            PaginationSizing paginationSizing, 
            int numericLinksCount, 
            bool paginationInfo,
            object htmlAttributes, 
            BootstrapVersion bootstrapVersion)
        {
            return GetDefaultPagination(htmlHelper, pageIndex, totalCount, pageSize, paginationSizing, numericLinksCount, paginationInfo, htmlAttributes, bootstrapVersion, HttpContext.Current.Request.Url);
        }

        public static HtmlString GetPagerPagination(
            this HtmlHelper htmlHelper,
            int pageIndex,
            int totalCount,
            int pageSize,
            bool alignedLinks,
            object htmlAttributes,
            Uri currentUrl)
        {
            return Pagination.GetPagerPagination(pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes, currentUrl);
        }

        public static HtmlString GetPagerPagination(
            this HtmlHelper htmlHelper, 
            int pageIndex, 
            int totalCount,
            int pageSize, 
            bool alignedLinks, 
            object htmlAttributes)
        {
            return GetPagerPagination(htmlHelper,pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes, HttpContext.Current.Request.Url);
        }
    }
}