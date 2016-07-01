// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;

namespace AshyCore.Resource
{
    /// <summary>
    /// Targets for resource binding.
    /// </summary>
    public enum ResourceTarget
    {
        Engine,
        PrivateUI,
        PrivateRender,
        PrivateScript,
        PrivatePhysics,
        World,
        LoadedLevel,
        LoadedLevelPrivateUI,
        LoadedLevelPrivateRender,
        LoadedLevelPrivateScript,
        LoadedLevelPrivatePhysics,
    }

    /// <summary>
    /// Represents resource, which you can load from <see cref="AshyCore.VFS.IFileSystem"/>.
    /// Manages loading (parsing), unloading file.
    /// </summary>
    public abstract class Resource : IDisposable
    {
        #region Properties
        
        /// <summary>
        /// Returns loaded resource.
        /// </summary>
        public object RC { get; private set; }

        /// <summary>
        /// Resource binding target.
        /// Resource will be released only then released its target.
        /// </summary>
        public ResourceTarget Target { get; }

        #endregion


        #region Constructors

        protected Resource(string path, ResourceTarget target, VFS.IFileSystem fs)
        {
            Target  = target;
            RC      = Load(path, fs);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Loads resource from the specified path. Parses it to <see cref="Resource"/> property.
        /// </summary>
        /// <param name="path">Path to file which contains specified type of resource.</param>
        /// <remarks>
        /// <paramref name="path"/> is not contains:
        ///  - path to gamedata,
        ///  - folder name inside gamedata,
        ///  - file extention.
        /// </remarks>
        public abstract object Load(string path, VFS.IFileSystem fs);

        public void Dispose()
        {
            RC = null;
        }

        #endregion
    }
}