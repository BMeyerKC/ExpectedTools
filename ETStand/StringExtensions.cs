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

        public static string ToBase64(this string str)
        {
            return Text.Encoding.UTF8.EncodeBase64(str);
        }

        public static string FromBase64(this string str)
        {
            return Text.Encoding.UTF8.DecodeBase64(str);
        }

        public static byte[] ToByteArray(this string str)
        {
            return Text.Encoding.ASCII.GetBytes(str);
        }

        public static string RemoveFromEnd(this string str, string suffix)
        {
            var removeLength = str.Length - suffix.Length;
            if (removeLength < 0) return str;

            return str.Remove(removeLength);
        }
    }
}