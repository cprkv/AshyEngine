// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// 

using System;
using System.Globalization;
using System.Linq;

namespace AshyCommon
{
    public static class StringExtention
    {
        /// <summary>
        /// Parses file path to array with delimiters '/' or '\'.
        /// </summary>
        /// <example>
        /// "Meshes/Wpn/Ak-47.obj" -> { "Meshes", "Wpn", "Ak-47.obj" }
        /// </example>
        public static string[] ParseFilePath(this string str)
        {
            return str.Split('/', '\\');
        }

        /// <summary>
        /// Parses file extention.
        /// </summary>
        /// <example>
        /// "Meshes/Wpn/Ak-47.obj" -> "obj"
        /// </example>
        public static string ParseFileExtention(this string str)
        {
            return str.Split('.').Last();
        }

        /// <summary>
        /// Converts a string like "312" in the integer 312.
        /// If you can not convert - would be 'null'.
        /// </summary>
        public static int? AsInt(this string str)
        {
            int result;
            return int.TryParse(str.Trim(), out result) == false
                ? (int?)null
                : result;
        }

        /// <summary>
        /// Converts a string like "312" in the integer 312.
        /// If you can not convert - would be 'null'.
        /// </summary>
        public static float? AsFloat(this string str)
        {
            var cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            float result;
            return float.TryParse(str.Trim(), NumberStyles.Any, cultureInfo, out result) == false
                ? (float?)null
                : result;
        }

        public static bool? AsBool(this string str)
        {
            return str.Trim() == "1" ? true : str.Trim() == "0" ? false : (bool?) null;
        }

        public static TEnum AsEnum<TEnum>(this string str)
        {
            return (TEnum) Enum.Parse(typeof (TEnum), str);
        }
    }
}