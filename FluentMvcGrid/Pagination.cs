using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    internal static class Pagination
    {
        public static HtmlString GetDefaultPagination(int pageIndex, int totalCount, int pageSize, PaginationSizing paginationSizing, PaginationAligment paginationAligment, int numericLinksCount, bool paginationInfo, object htmlAttributes, BootstrapVersion bootstrapVersion, string onClick, bool href, Uri currentUrl, string[] removedParameters, Dictionary<string, string> addedParameters)
        {
            if (currentUrl == null)
            {
                currentUrl = HttpContext.Current.Request.Url;
            }

            var pageCount = CalculatePageCount(pageSize, totalCount);
            if (pageCount == 1)
            {
                return new HtmlString("");
            }

            var div = new TagBuilder("div");
            var ul = new TagBuilder("ul");
            TagBuilder parentTag;

            if (bootstrapVersion == BootstrapVersion.Bootstrap2)
            {
                div.AddCssClass("pagination");
                if (paginationAligment != PaginationAligment.Left)
                {
                    div.AddCssClass(paginationAligment == PaginationAligment.Center ? "pagination-centered" : "pagination-right");
                }
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


            var parameters = Utilities.ParseQueryString(currentUrl.Query);

            RemoveParameters(parameters, removedParameters);
            AddParameters(parameters, addedParameters);
            var page = 1;
            parameters["page"] = page.ToString();

            var path = currentUrl.LocalPath;

            var url = Utilities.AppendParametersToUrl(path, parameters);
            ul.InnerHtml += GetPaginationItem("&laquo;", url, liClass, FluentMvcGridResources.First, onClick, href, page);

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
                    page = j;
                    parameters["page"] = page.ToString();
                    url = Utilities.AppendParametersToUrl(path, parameters);
                    ul.InnerHtml += GetPaginationItem(text, url, liClass, null, onClick, href, page);
                }
                else
                {
                    url = "";
                    ul.InnerHtml += GetPaginationItem(text, url, liClass, null, onClick, href, null);
                }
            }

            liClass = pageIndex == pageCount ? "disabled" : "";
            page = pageCount;
            parameters["page"] = page.ToString();
            url = Utilities.AppendParametersToUrl(path, parameters);
            ul.InnerHtml += GetPaginationItem("&raquo;", url, liClass, FluentMvcGridResources.Last, onClick, href, page);

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

        public static HtmlString GetPagerPagination(int pageIndex, int totalCount, int pageSize, bool alignedLinks, object htmlAttributes, string onClick, bool href, Uri currentUrl, string[] removedParameters, Dictionary<string, string> addedParameters)
        {
            if (currentUrl == null)
            {
                currentUrl = HttpContext.Current.Request.Url;
            }

            var pageCount = CalculatePageCount(pageSize, totalCount);
            if (pageCount == 1)
            {
                return new HtmlString("");
            }

            var parameters = HttpUtility.ParseQueryString(currentUrl.Query);

            RemoveParameters(parameters, removedParameters);
            AddParameters(parameters, addedParameters);

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
            int page;
            if (pageIndex == 1)
            {
                url = "#";
                liClass += " disabled";
                ul.InnerHtml += GetPaginationItem(text, url, liClass.Trim(), null, onClick, href, null);
            }
            else
            {
                page = pageIndex - 1;
                parameters["page"] = page.ToString();
                url = Utilities.AppendParametersToUrl(path, parameters);
                ul.InnerHtml += GetPaginationItem(text, url, liClass.Trim(), null, onClick, href, page);
            }

            //Next
            page = pageIndex + 1;
            parameters["page"] = page.ToString();
            text = FluentMvcGridResources.Next;
            liClass = "";
            if (pageIndex + 1 > pageCount)
            {
                url = "";
                liClass = "disabled";
            }
            else
            {
                url = Utilities.AppendParametersToUrl(path, parameters);
            }
            if (alignedLinks)
            {
                text = text + " &rarr;";
                liClass += " next";
            }
            ul.InnerHtml += GetPaginationItem(text, url, liClass, null, onClick, href, page);

            return new HtmlString(ul.ToString());
        }

        private static void RemoveParameters(NameValueCollection collection, string[] parameters)
        {
            if (parameters == null)
            {
                return;
            }
            foreach (var parameter in parameters)
            {
                collection.Remove(parameter);
            }
        }

        private static void AddParameters(NameValueCollection collection, Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                return;
            }
            foreach (var parameter in parameters)
            {
                if (!collection.AllKeys.Contains(parameter.Key))
                {
                    collection.Add(parameter.Key, parameter.Value);
                }
            }
        }

        private static int CalculatePageCount(int pageSize, int totalCount)
        {
            return (int)Math.Ceiling((double)totalCount / pageSize);
        }

        private static string GetPaginationItem(string text, string url, string liClass, string title, string onClick, bool href, int? page)
        {
            var a = new TagBuilder("a")
            {
                InnerHtml = text
            };
            if (!string.IsNullOrWhiteSpace(url))
            {
                a.Attributes.Add("href", string.IsNullOrWhiteSpace(onClick) ? url : "#");
                if (!string.IsNullOrEmpty(onClick) && page != null)
                {
                    string javascript;
                    if (href)
                    {
                        javascript = string.Format("javascript:{0}(\"{1}\",{2});return false;", onClick, url, page);
                    }
                    else
                    {
                        javascript = string.Format("javascript:{0}({1});return false;", onClick, page);
                    }                    
                    a.Attributes.Add("onclick", javascript);
                }
            }
            var li = new TagBuilder("li");
            if (!string.IsNullOrWhiteSpace(liClass))
            {
                li.AddCssClass(liClass);
            }
            if (page != null)
            {
                li.Attributes.Add("data-page", page.ToString());
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