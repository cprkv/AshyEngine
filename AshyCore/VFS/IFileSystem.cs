// 
// Created : 13.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;

namespace AshyCore.VFS
{
    /// <summary>
    /// Describes any file system, used in AshyEngine. 
    /// </summary>
    /// <remarks>Should be thread safe! That's why this class can't be static.</remarks>
    public interface IFileSystem : IDisposable
    {
        /// <summary>
        /// Reads all file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Readed lines.</returns>
        List<string> ReadLines(string path);

        /// <summary>
        /// Reads all file to one string object.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Readed lines.</returns>
        string ReadAllText(string path);

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <param name="path">Path to file.</param>
        byte[] ReadBytes(string path);

        /// <summary>
        /// Write lines to file. Rewrites file or creates it.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="lines">The lines.</param>
        void WriteLines(string path, string[] lines);
    }
}