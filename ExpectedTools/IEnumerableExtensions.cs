using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class IEnumerableExtensions
    {
        public static string ToCommaSeperatedString<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return "";

            var sb = new StringBuilder();
            foreach (var enu in enumerable)
            {
                var type = enu.GetType();
                if (type.IsPrimitive || type == typeof(string))
                {
                    sb.AppendFormat("{0},", enu);
                }
                else
                {
                    foreach (var prop in enu.GetType().GetProperties())
                    {
                        sb.AppendFormat("{0},{1},".FormatThis(prop.Name, prop.GetValue(enu, null)));
                    }
                }

            }
            return sb.ToString().TrimEnd(char.Parse(","));
        }

        /// <summary>
        /// Implements a foreach for IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <remarks>DO NOT modify the collection in the Action (e.g. add/remove)</remarks>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null) return enumerable;
            foreach (T item in enumerable)
            {
                action(item);
            }

            return enumerable;
        }

    }
}
