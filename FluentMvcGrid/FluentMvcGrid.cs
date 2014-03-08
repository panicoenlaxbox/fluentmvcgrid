using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public class FluentMvcGrid<T> : IHtmlString
    {
        private readonly List<FluentMvcGridColumn<T>> _columns = new List<FluentMvcGridColumn<T>>();
        private readonly List<FluentMvcGridFooterColumn> _footerColumns = new List<FluentMvcGridFooterColumn>();
        private readonly IEnumerable<T> _items;
        private readonly FluentMvcGridPagination _pagination = new FluentMvcGridPagination();
        private string _class;
        private string _id;
        private Func<T, object> _rowClass;
        private Func<dynamic, object> _eof;
        private readonly List<Tuple<string, Func<dynamic, object>>> _attributes = new List<Tuple<string, Func<dynamic, object>>>();
        private Func<dynamic, object> _htmlBefore;
        private Func<dynamic, object> _htmlAfter;
        private BootstrapVersion _bootstrapVersion = BootstrapVersion.Bootstrap3;

        public FluentMvcGrid(IEnumerable<T> items)
        {
            _items = items;
        }

        public FluentMvcGrid<T> Bootstrap(BootstrapVersion version)
        {
            _bootstrapVersion = version;
            return this;
        }

        public FluentMvcGrid<T> AddAttribute(string key, Func<dynamic, object> expression)
        {
            _attributes.Add(new Tuple<string, Func<dynamic, object>>(key, expression));
            return this;
        }

        public string ToHtmlString()
        {
            // IHtmlString
            // @
            return Build();
        }

        public FluentMvcGrid<T> Id(string id)
        {
            _id = id;
            return this;
        }

        public FluentMvcGrid<T> Class(string @class)
        {
            _class = @class;
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

        public FluentMvcGrid<T> AddFooterColumn(Action<FluentMvcGridFooterColumn> footerColumn)
        {
            var newFooterColumn = new FluentMvcGridFooterColumn();
            _footerColumns.Add(newFooterColumn);
            footerColumn(newFooterColumn);
            return this;
        }

        public FluentMvcGrid<T> Pagination(Action<FluentMvcGridPagination> pagination)
        {
            pagination(_pagination);
            return this;
        }

        internal string Build()
        {
            if (!_items.Any())
            {
                return Utilities.EvalExpression(_eof, null);
            }
            var table = new TagBuilder("table");
            if (!string.IsNullOrWhiteSpace(_id))
            {
                table.MergeAttribute("id", _id);
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
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            foreach (var column in _columns)
            {
                tr.InnerHtml += column.BuildHeader();
            }
            thead.InnerHtml = tr.ToString();
            table.InnerHtml += thead.ToString();

            var tfoot = new TagBuilder("tfoot");

            if (_footerColumns.Any())
            {
                if (_footerColumns.Count == 1)
                {
                    _footerColumns.First().ColSpan(_columns.Count);
                }
                tr = new TagBuilder("tr");
                tr.MergeAttribute("data-role", "footer");
                foreach (var footerColumn in _footerColumns)
                {
                    tr.InnerHtml += footerColumn.Build();
                }
                tfoot.InnerHtml += tr.ToString();
            }

            if (_pagination.IsEnabled())
            {
                tr = new TagBuilder("tr");
                tr.MergeAttribute("data-role", "pagination");
                var td = new TagBuilder("td");
                td.MergeAttribute("colspan", _columns.Count.ToString());
                td.InnerHtml = _pagination.Build(_bootstrapVersion);
                if (!string.IsNullOrWhiteSpace(td.InnerHtml))
                {
                    tr.InnerHtml = td.ToString();
                    tfoot.InnerHtml += tr.ToString();
                }
            }

            if (!string.IsNullOrWhiteSpace(tfoot.InnerHtml))
            {
                table.InnerHtml += tfoot.ToString();
            }

            var tbody = new TagBuilder("tbody");
            foreach (var item in _items)
            {
                tr = new TagBuilder("tr");
                var rowClass = Utilities.EvalExpression(_rowClass, item);
                if (!string.IsNullOrWhiteSpace(rowClass))
                {
                    tr.AddCssClass(rowClass);
                }
                foreach (var column in _columns)
                {
                    tr.InnerHtml += column.BuildContent(item);
                }
                tbody.InnerHtml += tr.ToString();
            }
            table.InnerHtml += tbody.ToString();

            foreach (var attribute in _attributes)
            {
                var key = attribute.Item1;
                var expression = attribute.Item2;
                var value = Utilities.EvalExpression(expression, null);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    table.MergeAttribute(key, value);
                }
            }

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