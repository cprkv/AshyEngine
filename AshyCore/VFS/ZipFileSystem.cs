// 
// Created : 13.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace AshyCore.VFS
{
    /// <summary>
    /// Represents zip file system.
    /// </summary>
    /// <seealso cref="IFileSystem" />
    public class ZipFileSystem : CriticalFinalizerObject, IFileSystem
    {
        #region Properties

        private ZipArchive Archive { get; }

        #endregion


        #region Constructors

        public ZipFileSystem(string pathToZipFile)
        {
            Archive = ZipFile.OpenRead(pathToZipFile);
        }

        #endregion

        
        #region Methods

        public List<string> ReadLines(string path)
        {
            return Encoding.UTF8
                .GetString(ReadBytes(path))
                .Split(new[] { "\r\n" }, StringSplitOptions.None)
                .SelectMany(s => s.Split(new[] { "\n" }, StringSplitOptions.None))
                .ToList();
        }

        public string ReadAllText(string path)
        {
            var bytes = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(ReadBytes(path)));
            var convertedBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), bytes);
            var newBytes = new byte[convertedBytes.Length - 1];
            for (int i = 1; i < convertedBytes.Length; i++)
            {
                newBytes[i - 1] = convertedBytes[i];
            }
            var converted = Encoding.GetEncoding("ISO-8859-1").GetString(newBytes);
            return converted;
        }

        public byte[] ReadBytes(string path)
        {
            var stream = Archive.GetEntry(path.Replace('\\', '/')).Open();
            byte[] buff = new byte[stream.Length];
            stream.Read(buff, 0, (int)stream.Length);
            stream.Dispose();
            return buff;
        }

        public void WriteLines(string path, string[] lines)
        {
            throw new NotImplementedException();
        }

        public string[] GetDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Archive.Dispose();
        }

        ~ZipFileSystem()
        {
            Archive.Dispose();
        }

        #endregion
    }
}