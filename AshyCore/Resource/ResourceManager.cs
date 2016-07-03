// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using AshyCore.Debug;

namespace AshyCore.Resource
{
    delegate Resource ResorceConstructor(string path, ResourceTarget target, VFS.IFileSystem fs);

    /// <summary>
    /// Provides resources management.
    /// </summary>
    public class ResourceManager
    {
        #region Private Properties

        private readonly ResorceConstructor[]   _resourceConstructors;

        private Dictionary<string, Resource>    Resources { get; } = new Dictionary<string, Resource>();

        private VFS.IFileSystem                 FS { get; }

        #endregion


        #region Constructors

        public ResourceManager(VFS.IFileSystem _fs)
        {
            FS = _fs;
            _resourceConstructors = new ResorceConstructor[]
            {
                (path, target, fs)  => new ConfigResource        (path, target, fs),
                (path, target, fs)  => new LuaScriptResource     (path, target, fs),
                (path, target, fs)  => new MeshesResource        (path, target, fs),
                (path, target, fs)  => new TextureJpegResource   (path, target, fs),
                (path, target, fs)  => new TexturePngResource    (path, target, fs)
            };
        }

        #endregion


        #region Methods and indexer

        /// <summary>
        /// Returns resource which is automatically loading from <paramref name="path"/>.
        /// File extention should be missed.
        /// </summary>
        /// <example>
        /// this["Meshes/Wpn/ak_47"]; // returns Mesh object from "mesh/Wpn/ak_47.obj"
        /// </example>
        public object this[string path, ResourceTarget target]
        {
            get
            {
                if (Resources.ContainsKey(path))
                    return          ( Resources[path].RC );

                var resource        = CreateResourceInstance(path, target);
                if (resource == null)
                    throw           new ArgumentException($"Error during load resorce by path: \"{path}\".");

                Resources.Add       (path, resource);

                return              ( resource.RC );
            }
        }

        /// <summary>
        /// Alias to the indexer method.
        /// Returns resource which is automatically loading from <paramref name="path"/>.
        /// File extention should be missed.
        /// </summary>
        /// <typeparam name="TData">The type of the data to return (just static cast).</typeparam>
        public TData Get<TData>(string path, ResourceTarget target)
        {
            return                          ( (TData) this[path, target] );
        }

        /// <summary>
        /// Returns count of uncollected items.
        /// </summary>
        /// <param name="pred">
        /// <code>true</code> when resource should be freed.
        /// </param>
        public int CollectWaiting(Func<ResourceTarget, bool> pred)
        {
            var targets                     = Resources
                .Where                      ( rc => pred(rc.Value.Target) )
                .Select                     ( x => new WeakReference(x.Value.RC) )
                .ToList                     ();

            CollectWithoutWaiting           ( pred );
            Memory.Collect    ( false );

            return                          ( targets.Count(x => x.IsAlive) );
        }

        /// <param name="pred">
        /// <code>true</code> when resource should be freed.
        /// </param>
        public void CollectWithoutWaiting(Func<ResourceTarget, bool> pred)
        {
            foreach (var resource in Resources.Where(rc => pred(rc.Value.Target)))
            {
                Resources.Remove            ( resource.Key );
            }
        }

        /// <summary>
        /// Creates resource instance.
        /// </summary>
        private Resource CreateResourceInstance(string path, ResourceTarget target)
        {
            foreach (var resource in _resourceConstructors)
            {
                try
                {
                    var result              = resource(path, target, FS);
                    return                  ( result );
                }
                catch
                {
                    // ignored
                }
            }
            return                          ( null );
        }

        #endregion
    }
}