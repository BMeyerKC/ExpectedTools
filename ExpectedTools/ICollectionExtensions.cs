using System.Collections;

namespace System
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Implements a foreach for ICollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <remarks>DO NOT modify the collection in the Action (e.g. add/remove)</remarks>
        public static ICollection Each<T>(this ICollection collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }

            return collection;
        }

    }
}
