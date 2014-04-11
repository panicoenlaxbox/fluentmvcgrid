using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    public class FluentMvcGridFooterColumn
    {
        private readonly List<Tuple<string, Func<dynamic, object>>> _attributes = new List<Tuple<string, Func<dynamic, object>>>();
        private int _colSpan = 1;
        private Func<dynamic, object> _format;
        private String _class;

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

        public FluentMvcGridFooterColumn Class(string value)
        {
            _class = value;
            return this;
        }
        
        internal int GetColSpan()
        {
            return _colSpan;
        }

        public FluentMvcGridFooterColumn AddAttribute(string key, Func<dynamic, object> expression)
        {
            _attributes.Add(new Tuple<string, Func<dynamic, object>>(key, expression));
            return this;
        }

        internal string Build(Configuration configuration)
        {
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
            return td.ToString();
        }
    }
}