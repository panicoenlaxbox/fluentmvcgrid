using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public class FluentMvcGrid<T> : IHtmlString
    {
        private readonly List<Tuple<string, Func<dynamic, object>>> _attributes = new List<Tuple<string, Func<dynamic, object>>>();
        private readonly List<FluentMvcGridColumn<T>> _columns = new List<FluentMvcGridColumn<T>>();
        private readonly List<FluentMvcGridFooterColumn> _footerColumns = new List<FluentMvcGridFooterColumn>();
        private IEnumerable<T> _items;
        private readonly FluentMvcGridPagination _pagination = new FluentMvcGridPagination();
        private string _class;
        private Func<dynamic, object> _eof;
        private Func<dynamic, object> _htmlAfter;
        private Func<dynamic, object> _htmlBefore;
        private string _id;
        private Func<T, object> _rowClass;
        private bool _showHeadersIfEof;
        private readonly Configuration _configuration;

        public FluentMvcGrid(IEnumerable<T> items)
        {
            _items = items;
            _configuration = new Configuration();
        }

        public string ToHtmlString()
        {
            // @
            return Build();
        }

        [Obsolete("Use Configuration method")]
        public FluentMvcGrid<T> Bootstrap(BootstrapVersion value)
        {
            _configuration.Bootstrap(value);
            return this;
        }

        public FluentMvcGrid<T> Configuration(Action<Configuration> configuration)
        {
            configuration(_configuration);
            return this;
        }

        public FluentMvcGrid<T> AddAttribute(string key, Func<dynamic, object> expression)
        {
            _attributes.Add(new Tuple<string, Func<dynamic, object>>(key, expression));
            return this;
        }

        public FluentMvcGrid<T> Id(string value)
        {
            _id = value;
            return this;
        }

        public FluentMvcGrid<T> Class(string value)
        {
            _class = value;
            return this;
        }

        public FluentMvcGrid<T> RowClass(Func<T, object> expression)
        {
            _rowClass = expression;
            return this;
        }

        public FluentMvcGrid<T> Eof(Func<dynamic, object> expression)
        {
            _eof = expression;
            return this;
        }

        public FluentMvcGrid<T> HeadersIfEof(bool value)
        {
            _showHeadersIfEof = value;
            return this;
        }

        public FluentMvcGrid<T> HtmlBefore(Func<dynamic, object> expression)
        {
            _htmlBefore = expression;
            return this;
        }

        public FluentMvcGrid<T> HtmlAfter(Func<dynamic, object> expression)
        {
            _htmlAfter = expression;
            return this;
        }

        public FluentMvcGrid<T> AddColumn(Action<FluentMvcGridColumn<T>> column)
        {
            var newColumn = new FluentMvcGridColumn<T>();
            _columns.Add(newColumn);
            column(newColumn);
            return this;
        }

        public FluentMvcGrid<T> AddColumn(string headerText, Func<T, object> expression)
        {
            var newColumn = new FluentMvcGridColumn<T>();
            newColumn.HeaderText(headerText).Format(expression);
            _columns.Add(newColumn);
            return this;
        }

        public FluentMvcGrid<T> AddColumn(string headerText, Func<T, object> expression, string sortBy)
        {
            var newColumn = new FluentMvcGridColumn<T>();
            newColumn.HeaderText(headerText).Format(expression).Sortable(true).SortBy(sortBy);
            _columns.Add(newColumn);
            return this;
        }

        public FluentMvcGrid<T> AddColumn(string headerText, Func<T, object> expression, Func<T, object> @class)
        {
            var newColumn = new FluentMvcGridColumn<T>();
            newColumn.HeaderText(headerText).Format(expression).Class(@class);
            _columns.Add(newColumn);
            return this;
        }

        public FluentMvcGrid<T> AddColumn(string headerText, Func<T, object> expression, Func<T, object> @class, string sortBy)
        {
            var newColumn = new FluentMvcGridColumn<T>();
            newColumn.HeaderText(headerText).Format(expression).Class(@class).Sortable(true).SortBy(sortBy);
            _columns.Add(newColumn);
            return this;
        }

        public FluentMvcGrid<T> AddFooterColumn(Action<FluentMvcGridFooterColumn> footerColumn)
        {
            var newFooterColumn = new FluentMvcGridFooterColumn();
            _footerColumns.Add(newFooterColumn);
            footerColumn(newFooterColumn);
            return this;
        }

        public FluentMvcGrid<T> Pagination(int pageIndex, int pageSize, int totalCount)
        {
            _pagination.PageIndex(pageIndex).PageSize(pageSize).TotalCount(totalCount);
            return this;
        }

        public FluentMvcGrid<T> Pagination(Action<FluentMvcGridPagination> pagination)
        {
            pagination(_pagination);
            return this;
        }

        private string GetCurrentUrl()
        {
            var parameters = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query);
            if (string.IsNullOrWhiteSpace(parameters["page"]) && _items.Any())
            {
                parameters.Add("page", "1");
            }
            if (parameters.Keys.Count > 0)
            {
                return HttpContext.Current.Request.Path + "?" + parameters;
            }
            return HttpContext.Current.Request.Path;
        }

        private void SetGeneralAttributes(TagBuilder table)
        {
            if (!string.IsNullOrWhiteSpace(_id))
            {
                table.Attributes.Add("id", _id);
            }
            if (!string.IsNullOrWhiteSpace(_class))
            {
                if (_class.Split(new[] { ' ' }).All(p => p.ToLower() != "table"))
                {
                    _class = "table " + _class;
                }
                table.AddCssClass(_class);
            }
            else
            {
                table.AddCssClass("table");
            }
            table.Attributes.Add("data-current-url", GetCurrentUrl());
        }

        private void SetHeader(TagBuilder table)
        {
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            foreach (var column in _columns)
            {
                tr.InnerHtml += column.BuildHeader(_configuration);
            }
            thead.InnerHtml = tr.ToString();
            table.InnerHtml += thead.ToString();
        }

        private void SetFooterColumns(TagBuilder tfoot)
        {
            if (_footerColumns.Any())
            {
                if (_footerColumns.Count == 1)
                {
                    var footerColumn = _footerColumns.First();
                    if (footerColumn.GetColSpan() <= 1)
                    {
                        footerColumn.ColSpan(_columns.Count);
                    }
                }
                var tr = new TagBuilder("tr");
                tr.Attributes.Add("data-role", "footer");
                var numberOfColSpan = 0;
                foreach (var footerColumn in _footerColumns)
                {
                    numberOfColSpan += footerColumn.GetColSpan();
                    tr.InnerHtml += footerColumn.Build(_configuration);
                }
                if (numberOfColSpan < _columns.Count)
                {
                    tr.InnerHtml += string.Format("<td colspan='{0}'></td>", _columns.Count - numberOfColSpan);
                }
                tfoot.InnerHtml += tr.ToString();
            }
        }

        private void SetPagination(TagBuilder tfoot)
        {
            if (_pagination.GetEnabled())
            {
                var tr = new TagBuilder("tr");
                tr.Attributes.Add("data-role", "pagination");
                var td = new TagBuilder("td");
                td.Attributes.Add("colspan", _columns.Count.ToString());
                td.InnerHtml = _pagination.Build(_configuration);
                if (!string.IsNullOrWhiteSpace(td.InnerHtml))
                {
                    tr.InnerHtml = td.ToString();
                    tfoot.InnerHtml += tr.ToString();
                }
            }
        }

        private void SetContent(TagBuilder table)
        {
            var tbody = new TagBuilder("tbody");
            foreach (var item in _items)
            {
                var tr = new TagBuilder("tr");
                var rowClass = Utilities.EvalExpression(_rowClass, item);
                if (!string.IsNullOrWhiteSpace(rowClass))
                {
                    tr.AddCssClass(rowClass);
                }
                foreach (var column in _columns)
                {
                    tr.InnerHtml += column.BuildContent(item, _configuration);
                }
                tbody.InnerHtml += tr.ToString();
            }
            table.InnerHtml += tbody.ToString();
        }

        private void SetBodyWhenEof(TagBuilder table)
        {
            var eof = Utilities.EvalExpression(_eof, null);
            if (!string.IsNullOrWhiteSpace(eof))
            {
                var tbody = new TagBuilder("tbody");
                var tr = new TagBuilder("tr");
                var td = new TagBuilder("td");
                td.Attributes.Add("colspan", _columns.Count.ToString());
                td.InnerHtml = eof;
                tr.InnerHtml = td.ToString();
                tbody.InnerHtml = tr.ToString();
                table.InnerHtml += tbody.ToString();
            }
        }

        private void SetFooter(TagBuilder table)
        {
            var tfoot = new TagBuilder("tfoot");

            SetFooterColumns(tfoot);
            SetPagination(tfoot);

            if (!string.IsNullOrWhiteSpace(tfoot.InnerHtml))
            {
                table.InnerHtml += tfoot.ToString();
            }
        }

        private string Build()
        {
            if (_items == null)
            {
                _items = Enumerable.Empty<T>();
            }

            if (!_items.Any() && !_showHeadersIfEof)
            {
                return Utilities.EvalExpression(_eof, null);
            }

            var table = new TagBuilder("table");

            SetGeneralAttributes(table);
            SetHeader(table);

            if (!_items.Any() && _showHeadersIfEof)
            {
                SetBodyWhenEof(table);
                return table.ToString();
            }

            SetFooter(table);

            SetContent(table);
            Utilities.SetAttributes(table, _attributes);

            var htmlBefore = Utilities.EvalExpression(_htmlBefore, null);
            var htmlAfter = Utilities.EvalExpression(_htmlAfter, null);
            if (!string.IsNullOrWhiteSpace(htmlBefore) || !string.IsNullOrWhiteSpace(htmlAfter))
            {
                var returnValue = "";
                if (!string.IsNullOrWhiteSpace(htmlBefore))
                {
                    returnValue += htmlBefore;
                }
                returnValue += table.ToString();
                if (!string.IsNullOrWhiteSpace(htmlAfter))
                {
                    returnValue += htmlAfter;
                }
                return returnValue;
            }

            return table.ToString();
        }

        public override string ToString()
        {
            // Html.Raw
            return Build();
        }
    }
}