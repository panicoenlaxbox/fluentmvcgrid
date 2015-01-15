using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FluentMvcGrid
{
    internal static class Utilities
    {
        internal static T EvalExpression<T>(Func<T> expression)
        {
            if (expression == null)
            {
                return default(T);
            }
            return expression();
        }

        internal static string EvalExpression<T>(Func<T, object> expression, T item)
        {
            if (expression == null)
            {
                return string.Empty;
            }
            var value = expression(item);
            if (IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return value.ToString();
        }

        internal static string EvalExpression(Func<object> expression)
        {
            if (expression == null)
            {
                return string.Empty;
            }
            var value = expression();
            if (IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return value.ToString();
        }

        internal static string GetText(string value, WhiteSpace whiteSpace)
        {
            return GetTextOrDefault(value, whiteSpace == WhiteSpace.Nbsp ? "&nbsp;" : "");
        }

        internal static bool IsNullOrWhiteSpace(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return true;
            }
            return false;
        }

        internal static void SetAttributes(TagBuilder tag, IEnumerable<Tuple<string, Func<dynamic, object>>> attributes)
        {
            foreach (var attribute in attributes)
            {
                var key = attribute.Item1;
                var expression = attribute.Item2;
                var value = EvalExpression(expression, null);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    tag.MergeAttribute(key, value, true);
                }
            }
        }

        private static string GetTextOrDefault(string value, string defaultValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}