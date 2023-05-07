using System;
using System.Collections.Generic;

namespace System.Collections.Generic
{
    internal static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return dictionary[key] = factory(key);
        }
    }
}
