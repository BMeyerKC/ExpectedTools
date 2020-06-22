using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace ExpectedTools
{
    public enum CacheExpirationType
    {
        Absolute,
        Sliding
    }

    public static class CacheLayer
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        // static readonly System.Collections.Concurrent.BlockingCollection<string> InProgressAdds = new System.Collections.Concurrent.BlockingCollection<string>();

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public static T Get<T>(string key) where T : class
        {
            T item = default(T);

            if (Cache.Contains(key))
                item = (T)Cache[key];

            return item;
        }

        /// <summary>
        /// Retrieve a cached item or create it with F if not yet in cache
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <param name="key">key</param>
        /// <param name="f">function that returns T</param>
        /// <param name="expirationType">sliding/absolute</param>
        /// <param name="expiration">ttl</param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> f, CacheExpirationType expirationType, TimeSpan expiration) where T : class
        {
            var policy = MakePolicy(expirationType, ref expiration);

            //Holy crap! this guy is super cool. http://blog.falafel.com/working-system-runtime-caching-memorycache/

            var newValue = new Lazy<T>(f);
            var oldValue = Cache.AddOrGetExisting(key, newValue, policy) as Lazy<T>;
            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                // Handle cached lazy exception by evicting from cache. Thanks to Denis Borovnev for pointing this out!
                Cache.Remove(key);
                throw;
            }
        }

        private static CacheItemPolicy MakePolicy(CacheExpirationType expirationType, ref TimeSpan expiration)
        {
            var policy = new CacheItemPolicy();
            if (expirationType == CacheExpirationType.Absolute)
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now.Add(expiration);
            }
            else
            {
                policy.SlidingExpiration = expiration;
            }
            return policy;
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(T objectToCache, string key, CacheItemPolicy policy) where T : class
        {
            Cache.Add(key, objectToCache, policy);
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(T objectToCache, string key, CacheExpirationType expirationType, TimeSpan expiration) where T : class
        {
            var policy = MakePolicy(expirationType, ref expiration);

            Cache.Add(key, objectToCache, policy);
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs (default expiraton 30 minutes from last use)
        /// </summary>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add(object objectToCache, string key)
        {
            Cache.Add(key, objectToCache, new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(30) });
        }

        /// <summary>
        /// Insert/Update value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Set<T>(T objectToCache, string key, CacheItemPolicy policy) where T : class
        {
            if (default(T) != objectToCache)
                Cache.Set(key, objectToCache, policy);
        }

        /// <summary>
        /// Insert/Update value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Set<T>(T objectToCache, string key, CacheExpirationType expirationType, TimeSpan expiration) where T : class
        {
            if (default(T) != objectToCache)
            {
                var policy = new CacheItemPolicy();
                if (expirationType == CacheExpirationType.Absolute)
                {
                    policy.AbsoluteExpiration = DateTimeOffset.Now.Add(expiration);
                }
                else
                {
                    policy.SlidingExpiration = expiration;
                }

                Cache.Set(key, objectToCache, policy);
            }
        }

        /// <summary>
        /// Insert value/Update into the cache using
        /// appropriate name/value pairs (default expiraton 30 minutes from last use)
        /// </summary>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Set(object objectToCache, string key)
        {
            if (objectToCache != null)
                Cache.Set(key, objectToCache, new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(30) });
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(string key)
        {
            Cache.Remove(key);
        }

        public static void ClearAll()
        {
            var cacheItems = Cache.ToList();

            foreach (KeyValuePair<String, Object> a in cacheItems)
            {
                Cache.Remove(a.Key);
            }
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return Cache.Get(key) != null;
        }

        /// <summary>
        /// Gets all cached items as a list by their key.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAll()
        {
            return Cache.Select(keyValuePair => keyValuePair.Key).ToList();
        }
    }
}