using System;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public class FluentMvcGridFooterColumn
    {
        private int _colSpan = 1;
        private Func<dynamic, object> _format;

        public FluentMvcGridFooterColumn Format(Func<dynamic, object> expression)
        {
            _format = expression;
            return this;
        }

        public FluentMvcGridFooterColumn ColSpan(int value)
        {
            _colSpan = value;
            return this;
        }

        internal string Build()
        {
            var td = new TagBuilder("td");
            if (_colSpan > 0)
            {
                td.MergeAttribute("colspan", _colSpan.ToString());
            }
            var format = Utilities.EvalExpression(_format, null);
            if (!string.IsNullOrWhiteSpace(format))
            {
                td.InnerHtml = format;
            }
            return td.ToString();
        }
    }
}