// 
// Created : 17.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;

namespace AshyCore.Debug
{
    public static class Debug
    {
        /// <summary>
        /// Safety call function without any exceptions.
        /// </summary>
        public static void ProtectedCall<T>(Action<T> func, T args)
        {
            // todo: make it safety
            func(args);
        }
    }
}