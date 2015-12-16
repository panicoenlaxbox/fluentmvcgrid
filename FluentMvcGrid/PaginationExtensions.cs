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
            PaginationAligment paginationAligment,
            int numericLinksCount,
            bool paginationInfo,
            object htmlAttributes,
            BootstrapVersion bootstrapVersion,
            string onClick,
            Uri currentUrl)
        {
            return Pagination.GetDefaultPagination(pageIndex, totalCount, pageSize, paginationSizing, paginationAligment, numericLinksCount, paginationInfo, htmlAttributes, bootstrapVersion, onClick, currentUrl);
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
            string onClick)
        {
            return GetDefaultPagination(htmlHelper, pageIndex, totalCount, pageSize, paginationSizing, paginationAligment, numericLinksCount, paginationInfo, htmlAttributes, bootstrapVersion, onClick,HttpContext.Current.Request.Url);
        }

        public static HtmlString GetPagerPagination(
            this HtmlHelper htmlHelper,
            int pageIndex,
            int totalCount,
            int pageSize,
            bool alignedLinks,
            object htmlAttributes,
            string onClick,
            Uri currentUrl)
        {
            return Pagination.GetPagerPagination(pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes, onClick, currentUrl);
        }

        public static HtmlString GetPagerPagination(
            this HtmlHelper htmlHelper,
            int pageIndex,
            int totalCount,
            int pageSize,
            bool alignedLinks,
            object htmlAttributes,
            string onClick)
        {
            return GetPagerPagination(htmlHelper, pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes, onClick, HttpContext.Current.Request.Url);
        }
    }
}