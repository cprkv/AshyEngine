// 
// Created : 13.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

using System;
using System.Collections.Generic;

namespace AshyCommon
{
    public static class ListExtension
    {
        /// <summary>
        /// Applies <paramref name="action"/> for each element of <paramref name="collection"/>.
        /// </summary>
        /// <remarks>Active method.</remarks>
        public static void ForEach<TCollection>(this IEnumerable<TCollection> collection, Action<TCollection> action)
        {
            foreach (var e in collection)
            {
                action(e);
            }
        }
    }
}