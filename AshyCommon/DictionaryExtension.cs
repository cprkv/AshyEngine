// 
// Created : 25.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Linq;

namespace AshyCommon
{
    /// <summary>
    /// Helper-class for <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public static class DictionaryExtension
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createFunc)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, createFunc(key));
            return dictionary[key];
        }

        public static TValue GetOrNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : class 
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : null;
        }

        public static bool Eq<TKey, TValue>(this Dictionary<TKey, TValue> dictA, Dictionary<TKey, TValue> dictB)
            where TKey : IEquatable<TKey>
            where TValue : IEquatable<TValue> 
        {
            return dictA.All(x => dictB.Any(y => x.Key.Equals(y.Key) && x.Value.Equals(y.Value))) 
                && dictB.All(x => dictA.Any(y => x.Key.Equals(y.Key) && x.Value.Equals(y.Value)));
        }

        [Obsolete("Not used", true)]
        public static bool ContainsKeyEq<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
            where TKey : IEquatable<TKey>
        {
            return dictionary.Any(x => x.Key.Equals(key));
        }
    }
}