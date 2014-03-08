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
            BootstrapVersion bootstrapVersion)
        {
            return Pagination.GetDefaultPagination(pageIndex, totalCount, pageSize, paginationSizing, numericLinksCount,
                paginationInfo, htmlAttributes, bootstrapVersion);
        }

        public static HtmlString GetPagerPagination(
            this HtmlHelper htmlHelper, 
            int pageIndex, 
            int totalCount,
            int pageSize, 
            bool alignedLinks, 
            object htmlAttributes)
        {
            return Pagination.GetPagerPagination(pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes);
        }
    }
}