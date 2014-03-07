using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public class FluentMvcGridColumn<T>
    {
        private readonly List<Tuple<string, Func<T, object>>> _attributes = new List<Tuple<string, Func<T, object>>>();
        private Func<T, object> _class;
        private Func<T, object> _format;
        private Func<string> _headerClass;
        private string _headerText;
        private string _sort;
        private bool _sortable;

        public FluentMvcGridColumn<T> AddAttribute(string key, Func<T, object> expression)
        {
            _attributes.Add(new Tuple<string, Func<T, object>>(key, expression));
            return this;
        }

        public FluentMvcGridColumn<T> HeaderText(string headerText)
        {
            _headerText = headerText;
            return this;
        }

        public FluentMvcGridColumn<T> SortBy(string sort)
        {
            _sort = sort;
            return this;
        }

        public FluentMvcGridColumn<T> Format(Func<T, object> expression)
        {
            _format = expression;
            return this;
        }

        public FluentMvcGridColumn<T> Sortable(bool sortable)
        {
            _sortable = sortable;
            return this;
        }

        public FluentMvcGridColumn<T> Class(Func<T, object> expression)
        {
            _class = expression;
            return this;
        }

        public FluentMvcGridColumn<T> HeaderClass(Func<string> expression)
        {
            _headerClass = expression;
            return this;
        }

        internal string BuildContent(T item)
        {
            var td = new TagBuilder("td");
            var format = Utilities.EvalExpression(_format, item);
            if (!string.IsNullOrWhiteSpace(format))
            {
                td.InnerHtml = format;
            }
            var @class = Utilities.EvalExpression(_class, item);
            if (!string.IsNullOrWhiteSpace(@class))
            {
                td.AddCssClass(@class);
            }
            foreach (var attribute in _attributes)
            {
                var key = attribute.Item1;
                var expression = attribute.Item2;
                var value = Utilities.EvalExpression(expression, item);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    td.MergeAttribute(key, value);
                }
            }
            return td.ToString();
        }

        internal string BuildHeader()
        {
            var th = new TagBuilder("th");
            if (_sortable)
            {
                var a = new TagBuilder("a")
                {
                    InnerHtml = _headerText
                };
                var parameters = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query);
                parameters.Remove("page");
                var sort = parameters["sort"];
                var sortDir = "ASC";
                if (!string.IsNullOrWhiteSpace(sort) && sort == _sort)
                {
                    sortDir = parameters["sortdir"];
                    sortDir = sortDir == "ASC" ? "DESC" : "ASC";
                }
                parameters["sort"] = _sort;
                parameters["sortdir"] = sortDir;
                var href = HttpContext.Current.Request.Path + "?" + parameters;
                a.Attributes.Add("href", href);
                th.InnerHtml = a.ToString();
            }
            else
            {
                th.InnerHtml = _headerText;
            }
            var headerClass = Utilities.EvalExpression(_headerClass);
            if (!string.IsNullOrWhiteSpace(headerClass))
            {
                th.AddCssClass(headerClass);
            }
            return th.ToString();
        }
    }
}