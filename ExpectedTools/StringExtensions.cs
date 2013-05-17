namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Makes an identical call to string.Format(<paramref name="str"/>,<paramref name="args"/>)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatThis(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        /// <summary>
        /// Makes an identical call to string.IsNullOrEmpty(<paramref name="str"/>)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
