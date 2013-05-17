using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class EnumerableExtensions
    {
        public static string ToCommaSeperatedString<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return "";

            var sb = new StringBuilder();
            foreach (var enu in enumerable)
            {
                var type = enu.GetType();
                if (type.IsPrimitive || type == typeof (string))
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
    }
}
