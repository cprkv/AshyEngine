// 
// Created : 13.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AshyCore.VFS
{
    /// <summary>
    /// Represents standard file system.
    /// </summary>
    /// <seealso cref="IFileSystem" />
    public class BasicFileSystem : IFileSystem
    {
        #region Properties

        public string Prefix { get; }

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicFileSystem"/> class.
        /// </summary>
        /// <param name="prefix">The prefix to path. Should include '\' character.</param>
        public BasicFileSystem(string prefix)
        {
            Prefix = prefix;
        }

        #endregion


        #region Methods

        public List<string> ReadLines(string path)
        {
            return File.ReadLines(Prefix + path).ToList();
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(Prefix + path);
        }

        public byte[] ReadBytes(string path)
        {
            return File.ReadAllBytes(Prefix + path);
        }

        public void WriteLines(string path, string[] lines)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        } 

        #endregion
    }
}