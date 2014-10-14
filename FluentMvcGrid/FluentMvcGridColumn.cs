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
        private Func<ColumnVisibility> _display;
        private Func<T, object> _format;
        private Func<string> _headerClass;
        private string _headerText;
        private string _name;
        private string _sortBy;
        private bool _sortable;

        public FluentMvcGridColumn()
        {
            _display = (() => ColumnVisibility.Visible);
        }

        public FluentMvcGridColumn<T> AddAttribute(string key, Func<T, object> expression)
        {
            _attributes.Add(new Tuple<string, Func<T, object>>(key, expression));
            return this;
        }

        public FluentMvcGridColumn<T> Class(Func<T, object> expression)
        {
            _class = expression;
            return this;
        }

        public FluentMvcGridColumn<T> Visibility(Func<ColumnVisibility> expression)
        {
            _display = expression;
            return this;
        }

        public FluentMvcGridColumn<T> Format(Func<T, object> expression)
        {
            _format = expression;
            return this;
        }

        public FluentMvcGridColumn<T> HeaderClass(Func<string> expression)
        {
            _headerClass = expression;
            return this;
        }

        public FluentMvcGridColumn<T> HeaderText(string value)
        {
            _headerText = value;
            return this;
        }

        public FluentMvcGridColumn<T> Name(string value)
        {
            _name = value;
            return this;
        }

        public FluentMvcGridColumn<T> SortBy(string value)
        {
            _sortBy = value;
            return this;
        }

        public FluentMvcGridColumn<T> Sortable(bool value)
        {
            _sortable = value;
            return this;
        }

        internal string BuildContent(T item, Configuration configuration)
        {
            var td = new TagBuilder("td");
            var display = Utilities.EvalExpression(_display);
            if (display == ColumnVisibility.None)
            {
                return string.Empty;
            }
            var format = Utilities.EvalExpression(_format, item);
            td.InnerHtml = Utilities.GetText(format, configuration.GetWhiteSpace());
            var @class = Utilities.EvalExpression(_class, item);
            if (!string.IsNullOrWhiteSpace(@class))
            {
                td.AddCssClass(@class);
            }
            td.Attributes.Add("data-role", "column");
            var name = !string.IsNullOrWhiteSpace(_name) ? _name : _sortBy;
            if (!string.IsNullOrWhiteSpace(name))
            {
                td.Attributes.Add("data-column-name", name);
            }
            foreach (var attribute in _attributes)
            {
                var key = attribute.Item1;
                var expression = attribute.Item2;
                var value = Utilities.EvalExpression(expression, item);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    td.MergeAttribute(key, value, true);
                }
            }
            if (display == ColumnVisibility.Hidden)
            {
                td.Attributes.Add("style", "display: none;");
            }
            return td.ToString();
        }

        internal string BuildHeader(Configuration configuration, Uri url)
        {
            var th = new TagBuilder("th");
            var display = Utilities.EvalExpression(_display);
            if (display == ColumnVisibility.None)
            {
                return string.Empty;
            }
            if (_sortable)
            {
                var a = new TagBuilder("a")
                {
                    InnerHtml = _headerText
                };
                var parameters = HttpUtility.ParseQueryString(url.Query);
                parameters.Remove("page");
                var sort = parameters["sort"];
                var sortDir = "ASC";
                if (!string.IsNullOrWhiteSpace(sort) && sort == _sortBy)
                {
                    sortDir = parameters["sortdir"];
                    sortDir = sortDir == "ASC" ? "DESC" : "ASC";
                }
                parameters["sort"] = _sortBy;
                parameters["sortdir"] = sortDir;
                var href = url.LocalPath + "?" + parameters;
                a.Attributes.Add("href", href);
                th.InnerHtml = a.ToString();
            }
            else
            {
                th.InnerHtml = Utilities.GetText(_headerText, configuration.GetWhiteSpace());
            }
            var headerClass = Utilities.EvalExpression(_headerClass);
            if (!string.IsNullOrWhiteSpace(headerClass))
            {
                th.AddCssClass(headerClass);
            }
            if (display == ColumnVisibility.Hidden)
            {
                th.Attributes.Add("style", "display: none;");
            }
            return th.ToString();
        }
    }
}