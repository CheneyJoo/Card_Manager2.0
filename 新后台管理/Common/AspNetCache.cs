
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Common
{
   public static class AspNetCache
    {
       

        private static System.Web.Caching.Cache cache;

        private static object cacheLocker;
   

        static AspNetCache()
        {
            AspNetCache.cacheLocker = new object();
            cache = HttpRuntime.Cache;
        }

      
        public static void Clear()
        {
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                cache.Remove(enumerator.Key.ToString());
            }
        }

        public static object Get(string key)
        {
            return cache.Get(key);
        }

        public static T Get<T>(string key)
        {
            return (T)cache.Get(key);
        }

        public static void Insert(string key, object value)
        {
            lock (AspNetCache.cacheLocker)
            {
                if (cache.Get(key) != null)
                {
                    cache.Remove(key);
                }
                cache.Insert(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">seconds</param>
		public static void Insert(string key, object value, int cacheTime)
        {
            var caches = cache;
            DateTime now = DateTime.Now;
            caches.Insert(key, value, null, now.AddSeconds(cacheTime), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        public static void Insert(string key, object value, DateTime cacheTime)
        {
            cache.Insert(key, value, null, cacheTime, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        public static void Remove(string key)
        {
            cache.Remove(key);
        }

        public static bool Exist(string key)
        {
            bool isExist = false;
            isExist = cache.Get(key) == null ? false : true;
            return isExist;
        }
    }
}