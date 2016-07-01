// 
// Created : 05.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Reflection;
using AshyCore.EntityManagement;

namespace AshyCore.Resource
{
    /// <summary>
    /// Represents lua script (.lua) resource.
    /// <see cref="Resource.RC"/> returns <see cref="Script"/>.
    /// </summary>
    public class LuaScriptResource : Resource
    {
        #region Propteries

        public static readonly string FileExtension = "lua";

        #endregion


        #region Constructors

        public LuaScriptResource(string path, ResourceTarget target, VFS.IFileSystem fs)
            : base(path, target, fs)
        { }

        #endregion


        #region Public methods

        public override object Load(string path, VFS.IFileSystem fs)
        {
            var text    = fs.ReadAllText($"{path}.{FileExtension}");
            return      ( new Script(path, text) );
        }

        #endregion
    }
}