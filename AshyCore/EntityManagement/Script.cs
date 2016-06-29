// 
// Created : 05.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 
namespace AshyCore.EntityManagement
{
    /// <summary>
    /// Represents any text Script.
    /// </summary>
    public class Script
    {
        #region Properties

        /// <summary>
        /// Path to Script.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Text of Script.
        /// </summary>
        public string Text { get; }

        #endregion


        #region Constructors

        public Script(string path, string text)
        {
            Path = path;
            Text = text;
        }

        #endregion
    }
}