using System.Collections.Generic;

namespace System
{
    public static class IListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeExtensions.ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T Random<T>(this IList<T> list)
        {
            return list[new Random(DateTime.Now.Millisecond).Next(list.Count - 1)];
        }

        public static T Random<T>(this IList<T> list, int seed)
        {
            return list[new Random(seed).Next(list.Count - 1)];
        }
    }
}