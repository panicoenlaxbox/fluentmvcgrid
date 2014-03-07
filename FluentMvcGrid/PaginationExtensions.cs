using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public static class PaginationExtensions
    {
        public static HtmlString GetDefaultPagination(this HtmlHelper htmlHelper, int pageIndex, int totalCount,
            int pageSize, PaginationSizing paginationSizing, int numericLinksCount, bool paginationInfo,
            object htmlAttributes)
        {
            return Pager.GetDefaultPagination(pageIndex, totalCount, pageSize, paginationSizing, numericLinksCount,
                paginationInfo, htmlAttributes);
        }

        public static HtmlString GetPagerPagination(this HtmlHelper htmlHelper, int pageIndex, int totalCount,
            int pageSize, bool alignedLinks, object htmlAttributes)
        {
            return Pager.GetPagerPagination(pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes);
        }
    }
}