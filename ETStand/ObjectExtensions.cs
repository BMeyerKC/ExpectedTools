using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System
{
    public static class ObjectExtensions
    {
        #region Checks
        /// <summary>
        /// returns true if the object is null, = DBNull.Value or is an empty string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object o)
        {
            return (o == null) ||
                (o == DBNull.Value) ||
                (o is string && string.IsNullOrEmpty((string)o));
        }
        #endregion

        #region Conversion To
        /// <summary>
        /// Converts the nullable struct to a string that is formatted, 
        /// if o is null, returns string.empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToString<T>(this Nullable<T> o, string format) where T : struct
        {
            return o.HasValue ? ("{0:" + format + "}").FormatThis(o.Value) : string.Empty;
        }

        /// <summary>
        /// runs tostring but if it's empty it returns default
        /// if o IsNullOrEmpty, returns def
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string ToStringOrDefault(this object o, string def)
        {
            if (o.IsNullOrEmpty())
                return def;

            return o.ToString();
        }

        /// <summary>
        /// Converts the object to the specified struct, 
        /// if o IsNullOrEmpty, returns default(T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object o)
        {
            return ConvertTo<T>(o, default(T));
        }

        /// <summary>
        /// Converts the object to the specified struct
        /// if o IsNullOrEmpty, returns def
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object o, T def)
        {
            if (o.IsNullOrEmpty())
                return def;

            Type t = typeof(T);
            Type u = Nullable.GetUnderlyingType(t);

            return (T)Convert.ChangeType(o, u ?? t);
        }

        public static string ToXml(this object obj)
        {
            return ToXml(obj, (XmlAttributeOverrides)null, true);
        }

        public static string ToXml(this object obj, string rootname)
        {
            XmlAttributes attributes = new XmlAttributes() { XmlRoot = new XmlRootAttribute(rootname) };
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            overrides.Add(obj.GetType(), attributes);

            return ToXml(obj, overrides, true);
        }

        public static string ToXml(this object obj, XmlAttributeOverrides overrides, bool omitNameSpaces)
        {
            XmlSerializerNamespaces nm = omitNameSpaces ? new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName("") }) : null;
            return ToXml(obj, overrides, nm);
        }

        public static string ToXml(this object obj, XmlAttributeOverrides overrides, XmlSerializerNamespaces nm)
        {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter sw = XmlWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                new XmlSerializer(obj.GetType(), overrides).Serialize(sw, obj, nm);
            }
            return sb.ToString();
        }

        public static T FromXml<T>(this string xml) where T : class
        {
            Type type = typeof(T);
            if (string.IsNullOrEmpty(xml))
            {
                return Activator.CreateInstance(type) as T;
            }
            else
            {
                XmlSerializer xs = new XmlSerializer(type);
                StringReader sr = new StringReader(xml);
                XmlReader xr = XmlReader.Create(sr, new XmlReaderSettings() { IgnoreWhitespace = true });
                return xs.Deserialize(xr) as T;
            }
        }

        #endregion

        #region Safe Chained Access
        //From http://devtalk.net/csharp/chained-null-checks-and-the-maybe-monad/
        /// <summary>
        /// returns the result of the evaluator if the object is not null
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="o"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }

        /// <summary>
        /// returns the result of the evaluator if the object is not null, otherwise returns default(TResult) or string.Empty for strings
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="o"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TInput : class
        {

            if (o == null) return typeof(TResult) == typeof(string) ? (TResult)Convert.ChangeType(string.Empty, typeof(TResult)) : default(TResult);
            return evaluator(o);
        }

        /// <summary>
        /// returns the result of the evaluator if the object is not null, otherwise returns valueIfNull
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="o"></param>
        /// <param name="evaluator"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult valueIfNull)
            where TInput : class
        {
            if (o == null) return valueIfNull;
            return evaluator(o);
        }

        /// <summary>
        /// returns the object if the object is not null and evaluator == true
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="o"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        /// <summary>
        /// returns the object if the object is not null and evaluator == false
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="o"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        /// <summary>
        /// if the object is not null, executes the action, returns object 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="o"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }

        public static TInput TrimAllStrings<TInput>(this TInput item)
            where TInput : class
        {
            if (item != null)
            {
                item.GetType().GetProperties()
                    .Where(p => (p.PropertyType == typeof(string) || p.PropertyType == typeof(String)))
                    .Each(p => p.Do(pi => pi.SetValue(item, pi.GetValue(item, null).Return(v => v.ToString().Trim()), null)));
            }

            return item;
        }
        #endregion

        public static bool IsBetween<T>(this T value, T low, T high) where T : IComparable<T>
        {
            return value.CompareTo(low) >= 0 && value.CompareTo(high) <= 0;
        }

        public static bool In<T>(this T value, params T[] values)
        {
            return values.Contains(value);
        }

        public static dynamic ToExpando(this object o)
        {
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>;
            if (o.GetType() == typeof(ExpandoObject)) return o;
            if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(i => d.Add(i));
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }

        /// <summary>
        /// Turns the object into a Dictionary
        /// </summary>
        public static IDictionary<string, object> ToDictionary(this object thingy)
        {
            return (IDictionary<string, object>)thingy.ToExpando();
        }
    }
}
