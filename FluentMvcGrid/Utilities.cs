using System;

namespace FluentMvcGrid
{
    internal static class Utilities
    {
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
    }
}