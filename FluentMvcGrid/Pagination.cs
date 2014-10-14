using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    internal static class Pagination
    {
        public static HtmlString GetDefaultPagination(int pageIndex, int totalCount, int pageSize, PaginationSizing paginationSizing, int numericLinksCount, bool paginationInfo, object htmlAttributes, BootstrapVersion bootstrapVersion)
        {
            return GetDefaultPagination(pageIndex, totalCount, pageSize, paginationSizing, numericLinksCount,
                paginationInfo, htmlAttributes, bootstrapVersion, HttpContext.Current.Request.Url);
        }

        public static HtmlString GetDefaultPagination(int pageIndex, int totalCount, int pageSize, PaginationSizing paginationSizing, int numericLinksCount, bool paginationInfo, object htmlAttributes, BootstrapVersion bootstrapVersion, Uri currentUrl)
        {
            var pageCount = CalculatePageCount(pageSize, totalCount);
            if (pageCount == 1)
            {
                return new HtmlString("");
            }

            var div = new TagBuilder("div");
            var ul = new TagBuilder("ul");
            TagBuilder parentTag = null;

            if (bootstrapVersion == BootstrapVersion.Bootstrap2)
            {
                div.AddCssClass("pagination");
                parentTag = div;
            }
            else
            {
                ul.AddCssClass("pagination");
                parentTag = ul;
            }

            var attributes = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (attributes != null)
            {
                if (attributes.ContainsKey("class"))
                {
                    parentTag.AddCssClass(attributes["class"].ToString());
                }
                parentTag.MergeAttributes(attributes);
            }

            switch (paginationSizing)
            {
                case PaginationSizing.Large:
                    parentTag.AddCssClass("pagination-" + (bootstrapVersion == BootstrapVersion.Bootstrap2 ? "pagination-large" : "pagination-lg"));
                    break;
                case PaginationSizing.Small:
                    parentTag.AddCssClass("pagination-" + (bootstrapVersion == BootstrapVersion.Bootstrap2 ? "pagination-small" : "pagination-sm"));
                    break;
                case PaginationSizing.Mini:
                    if (bootstrapVersion == BootstrapVersion.Bootstrap2)
                    {
                        parentTag.AddCssClass("pagination pagination-mini");
                    }
                    break;
            }

            var liClass = pageIndex == 1 ? "disabled" : "";

            var parameters = HttpUtility.ParseQueryString(currentUrl.Query);

            if (string.IsNullOrWhiteSpace(parameters["page"]))
            {
                parameters.Add("page", "");
            }
            parameters["page"] = "1";

            var path = currentUrl.LocalPath;

            var url = path + "?" + parameters;
            ul.InnerHtml += GetPaginationItem("&laquo;", url, liClass, FluentMvcGridResources.First);

            var num = pageCount - 1;
            var num1 = pageIndex + numericLinksCount / 2;
            var num2 = num1 - numericLinksCount + 1;
            if (num1 > num)
            {
                num2 = num2 - (num1 - num);
                num1 = num;
            }
            if (num2 < 0)
            {
                num1 = Math.Min(num1 + -num2, num);
                num2 = 0;
            }
            for (var i = num2; i <= num1; i++)
            {
                var j = i + 1;
                var text = j.ToString(CultureInfo.InvariantCulture);
                liClass = pageIndex == j ? "active" : "";
                if (j != pageIndex)
                {
                    parameters["page"] = j.ToString(CultureInfo.InvariantCulture);
                    url = path + "?" + parameters;
                    ul.InnerHtml += GetPaginationItem(text, url, liClass, "");
                }
                else
                {
                    url = "#";
                    ul.InnerHtml += GetPaginationItem(text, url, liClass, "");
                }
            }

            liClass = pageIndex == pageCount ? "disabled" : "";
            parameters["page"] = pageCount.ToString(CultureInfo.InvariantCulture);
            url = path + "?" + parameters;
            ul.InnerHtml += GetPaginationItem("&raquo;", url, liClass, FluentMvcGridResources.Last);

            if (paginationInfo)
            {
                var info = string.Format(FluentMvcGridResources.PaginationInfo, pageIndex, pageCount, totalCount);
                ul.InnerHtml += string.Format("<li class='disabled'><span>{0}</span></li>", info);
            }

            if (bootstrapVersion == BootstrapVersion.Bootstrap2)
            {
                div.InnerHtml = ul.ToString();
                return new HtmlString(div.ToString());
            }

            return new HtmlString(ul.ToString());
        }

        public static HtmlString GetPagerPagination(int pageIndex, int totalCount, int pageSize, bool alignedLinks, object htmlAttributes)
        {
            return GetPagerPagination(pageIndex, totalCount, pageSize, alignedLinks, htmlAttributes, HttpContext.Current.Request.Url);
        }

        public static HtmlString GetPagerPagination(int pageIndex, int totalCount, int pageSize, bool alignedLinks, object htmlAttributes, Uri currentUrl)
        {
            var pageCount = CalculatePageCount(pageSize, totalCount);
            if (pageCount == 1)
            {
                return new HtmlString("");
            }

            var parameters = HttpUtility.ParseQueryString(currentUrl.Query);

            if (string.IsNullOrWhiteSpace(parameters["page"]))
            {
                parameters.Add("page", "");
            }

            var path = currentUrl.LocalPath;

            //Previous
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pager");
            var attributes = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (attributes != null)
            {
                if (attributes.ContainsKey("class"))
                {
                    ul.AddCssClass(attributes["class"].ToString());
                }
                ul.MergeAttributes(attributes);
            }
            var text = FluentMvcGridResources.Previous;
            var liClass = "";
            if (alignedLinks)
            {
                text = "&larr; " + text;
                liClass += " previous";
            }
            string url;
            if (pageIndex == 1)
            {
                url = "#";
                liClass += " disabled";
            }
            else
            {
                parameters["page"] = (pageIndex - 1).ToString(CultureInfo.InvariantCulture);
                url = path + "?" + parameters;
            }
            ul.InnerHtml += GetPaginationItem(text, url, liClass.Trim(), "");

            //Next
            parameters["page"] = (pageIndex + 1).ToString(CultureInfo.InvariantCulture);
            text = FluentMvcGridResources.Next;
            liClass = "";
            if (pageIndex + 1 > pageCount)
            {
                url = "#";
                liClass = "disabled";
            }
            else
            {
                url = path + "?" + parameters;
            }
            if (alignedLinks)
            {
                text = text + " &rarr;";
                liClass += " next";
            }
            ul.InnerHtml += GetPaginationItem(text, url, liClass, "");

            return new HtmlString(ul.ToString());
        }

        private static int CalculatePageCount(int pageSize, int totalCount)
        {
            return (int)Math.Ceiling((double)totalCount / pageSize);
        }

        private static string GetPaginationItem(string text, string url, string liClass, string title)
        {
            var a = new TagBuilder("a")
            {
                InnerHtml = text
            };
            if (!string.IsNullOrWhiteSpace(url))
            {
                a.Attributes.Add("href", url);
            }
            var li = new TagBuilder("li");
            if (!string.IsNullOrWhiteSpace(liClass))
            {
                li.AddCssClass(liClass);
            }
            li.InnerHtml += a.ToString();
            if (!string.IsNullOrWhiteSpace(title))
            {
                li.Attributes.Add("title", title);
            }
            return li.ToString();
        }
    }
}