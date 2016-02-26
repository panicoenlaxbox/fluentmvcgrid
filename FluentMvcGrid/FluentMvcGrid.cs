using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public class FluentMvcGrid<T> : IHtmlString
    {
        private readonly List<Tuple<string, Func<dynamic, object>>> _attributes;
        private readonly List<FluentMvcGridColumn<T>> _columns;
        private readonly Configuration _configuration;
        private readonly List<FluentMvcGridFooterColumn> _footerColumns;
        private readonly FluentMvcGridPagination _pagination;
        private string _class;
        private Func<dynamic, object> _eof;
        private Func<dynamic, object> _htmlAfter;
        private Func<dynamic, object> _htmlBefore;
        private string _id;
        private IEnumerable<T> _items;
        private Func<T, object> _rowClass;
        private bool _showHeadersIfEof;
        private Uri _url;

        public FluentMvcGrid(IEnumerable<T> items)
        {
            _attributes = new List<Tuple<string, Func<dynamic, object>>>();
            _columns = new List<FluentMvcGridColumn<T>>();
            _configuration = new Configuration();
            _footerColumns = new List<FluentMvcGridFooterColumn>();
            _items = items;
            _pagination = new FluentMvcGridPagination();
            _url = HttpContext.Current.Request.Url;
        }

        private bool NothingToShow
        {
            get { return !_items.Any() && !_showHeadersIfEof; }
        }

        #region IHtmlString members

        public string ToHtmlString()
        {
            // @
            return Build();
        }

        #endregion

        public FluentMvcGrid<T> AddAttribute(string key, Func<dynamic, object> expression)
        {
            _attributes.Add(new Tuple<string, Func<dynamic, object>>(key, expression));
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

        public FluentMvcGrid<T> Class(string value)
        {
            _class = value;
            return this;
        }

        public FluentMvcGrid<T> Configuration(Action<Configuration> configuration)
        {
            configuration(_configuration);
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

        public FluentMvcGrid<T> HtmlAfter(Func<dynamic, object> expression)
        {
            _htmlAfter = expression;
            return this;
        }

        public FluentMvcGrid<T> HtmlBefore(Func<dynamic, object> expression)
        {
            _htmlBefore = expression;
            return this;
        }

        public FluentMvcGrid<T> Id(string value)
        {
            _id = value;
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

        public FluentMvcGrid<T> RowClass(Func<T, object> expression)
        {
            _rowClass = expression;
            return this;
        }

        public override string ToString()
        {
            // Html.Raw
            return Build();
        }

        public FluentMvcGrid<T> Url(Uri url)
        {
            _url = url;
            return this;
        }

        private string Build()
        {
            if (_items == null)
            {
                _items = Enumerable.Empty<T>();
            }

            if (NothingToShow)
            {
                return Utilities.EvalExpression(_eof, null);
            }

            var table = new TagBuilder("table");

            SetRequiredAttributes(table);

            SetHeader(table);

            if (!_items.Any() && _showHeadersIfEof)
            {
                SetBodyWhenEof(table);
                return table.ToString();
            }

            SetFooter(table);

            SetContent(table);

            SetAttributes(table);

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

        private string GetCurrentUrl(Uri url)
        {
            var parameters = Utilities.ParseQueryString(url.Query);
            if (string.IsNullOrWhiteSpace(parameters["page"]) && _items.Any())
            {
                parameters.Add("page", "1");
            }
            if (parameters.Keys.Count > 0)
            {
                return Utilities.AppendParametersToUrl(url.LocalPath, parameters);
            }
            return url.LocalPath;
        }

        private int GetNumberOfVisibleColumns()
        {
            return _columns.Count(c => c.GetVisibility() == ColumnVisibility.Visible);
        }

        private void SetAttributes(TagBuilder table)
        {
            Utilities.SetAttributes(table, _attributes);
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
                tr.Attributes.Add("data-role", "row");
                foreach (var column in _columns)
                {
                    tr.InnerHtml += column.BuildContent(item, _configuration);
                }
                tbody.InnerHtml += tr.ToString();
            }
            table.InnerHtml += tbody.ToString();
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

        private void SetFooterColumns(TagBuilder tfoot)
        {
            if (_footerColumns.Any())
            {
                FixColSpanIfUniqueFooterColumn();
                var tr = new TagBuilder("tr");
                tr.Attributes.Add("data-role", "footer");
                _footerColumns.ForEach(item =>
                {
                    tr.InnerHtml += item.Build(_configuration);
                });
                var numberOfColSpan = _footerColumns
                    .Where(fc => fc.GetVisibility() == ColumnVisibility.Visible)
                    .Sum(fc => fc.GetColSpan());
                var numberOfVisibleColumns = GetNumberOfVisibleColumns();
                if (numberOfColSpan < numberOfVisibleColumns)
                {
                    tr.InnerHtml += string.Format("<td colspan='{0}'>{1}</td>",
                        numberOfVisibleColumns - numberOfColSpan,
                        Utilities.GetText("", _configuration.GetWhiteSpace()));
                }
                tfoot.InnerHtml += tr.ToString();
            }
        }

        private void FixColSpanIfUniqueFooterColumn()
        {
            if (_footerColumns.Count == 1 && _footerColumns.First().GetColSpan() <= 1)
            {
                _footerColumns.First().ColSpan(GetNumberOfVisibleColumns());
            }
        }

        private void SetHeader(TagBuilder table)
        {
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            foreach (var column in _columns)
            {
                tr.InnerHtml += column.BuildHeader(_url, _configuration);
            }
            thead.InnerHtml = tr.ToString();
            table.InnerHtml += thead.ToString();
        }

        private void SetPagination(TagBuilder tfoot)
        {
            if (_pagination.GetEnabled())
            {
                var tr = new TagBuilder("tr");
                tr.Attributes.Add("data-role", "pagination");
                var td = new TagBuilder("td");
                td.Attributes.Add("colspan", _columns.Count.ToString());
                td.InnerHtml = _pagination.Build(_configuration, _url);
                if (!string.IsNullOrWhiteSpace(td.InnerHtml))
                {
                    tr.InnerHtml = td.ToString();
                    tfoot.InnerHtml += tr.ToString();
                }
            }
        }

        private void SetRequiredAttributes(TagBuilder table)
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
            table.Attributes.Add("data-current-url", GetCurrentUrl(_url));
        }
    }
}