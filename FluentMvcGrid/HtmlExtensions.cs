using System.Collections.Generic;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public static class HtmlExtensions
    {
        public static FluentMvcGrid<T> FluentMvcGrid<T>(this HtmlHelper helper, IEnumerable<T> items)
        {
            return new FluentMvcGrid<T>(items);
        }
    }
}