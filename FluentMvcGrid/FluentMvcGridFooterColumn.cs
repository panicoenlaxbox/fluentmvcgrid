using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public class FluentMvcGridFooterColumn
    {
        private readonly List<Tuple<string, Func<dynamic, object>>> _attributes;
        private String _class;
        private int _colSpan;
        private Func<dynamic, object> _format;
        private Func<ColumnVisibility> _visibility;

        public FluentMvcGridFooterColumn()
        {
            _attributes = new List<Tuple<string, Func<dynamic, object>>>();
            _colSpan = 1;
            _visibility = (() => ColumnVisibility.Visible);
        }

        public FluentMvcGridFooterColumn AddAttribute(string key, Func<dynamic, object> expression)
        {
            _attributes.Add(new Tuple<string, Func<dynamic, object>>(key, expression));
            return this;
        }

        public FluentMvcGridFooterColumn Class(string value)
        {
            _class = value;
            return this;
        }

        public FluentMvcGridFooterColumn ColSpan(int value)
        {
            _colSpan = value;
            return this;
        }

        public FluentMvcGridFooterColumn Format(Func<dynamic, object> expression)
        {
            _format = expression;
            return this;
        }

        public FluentMvcGridFooterColumn Visibility(Func<ColumnVisibility> expression)
        {
            _visibility = expression;
            return this;
        }

        internal string Build(Configuration configuration)
        {
            var visibility = Utilities.EvalExpression(_visibility);
            if (visibility == ColumnVisibility.None)
            {
                return string.Empty;
            }
            var td = new TagBuilder("td");
            if (_colSpan > 1)
            {
                td.Attributes.Add("colspan", _colSpan.ToString());
            }
            var format = Utilities.EvalExpression(_format, null);
            td.InnerHtml = Utilities.GetText(format, configuration.GetWhiteSpace());
            if (!string.IsNullOrWhiteSpace(_class))
            {
                td.AddCssClass(_class);
            }
            Utilities.SetAttributes(td, _attributes);
            if (visibility == ColumnVisibility.Hidden)
            {
                td.Attributes.Add("style", "display: none;");
            }       
            return td.ToString();
        }

        internal int GetColSpan()
        {
            return _colSpan;
        }

        internal ColumnVisibility GetVisibility()
        {
            return _visibility();
        }

        internal bool IsRendered()
        {
            return _visibility() != ColumnVisibility.None;
        }
    }
}