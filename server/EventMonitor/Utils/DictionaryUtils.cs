using System;
using System.Collections.Generic;

namespace EventMonitor.Utils
{
    public class DictionaryUtils
    {
        public static bool Add<T, X>(Dictionary<T, X> collection, T key, X value)
        {
            try
            {
                lock (collection) collection.Add(key, value);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Remove<T, X>(Dictionary<T, X> collection, T key)
        {
            try
            {
                lock (collection) collection.Remove(key);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
        