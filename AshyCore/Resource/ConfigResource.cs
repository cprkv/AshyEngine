// 
// Created  : 18.03.2016
// Author   : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Linq;
using System.Reflection;
using IniParser.Parser;

namespace AshyCore.Resource
{
    /// <summary>
    /// Configuration resource describes any ini-file resource.
    /// <see cref="Resource.RC"/> returns <see cref="ConfigTable"/>.
    /// </summary>
    public class ConfigResource : Resource
    {
        #region Fields

        public static readonly string FileExtension = "ini";

        internal static readonly IniDataParser Parser = new IniDataParser();

        #endregion


        #region Constructors

        public ConfigResource(string path, ResourceTarget target, VFS.IFileSystem fs) 
            : base(path, target, fs)
        { }

        #endregion


        #region Methods 

        public override object Load(string path, VFS.IFileSystem fs)
        {
            var obj         = fs.ReadLines($"{path}.{FileExtension}");
            var fileData    = obj.Aggregate("", (a, b) => a + b + "\r\n");
            return          ( new ConfigTable(Parser.Parse(fileData)) );
        } 

        #endregion
    }
}