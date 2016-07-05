// 
// Created : 29.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Drawing;
using System.IO;
using System.Reflection;

namespace AshyCore.Resource
{
    /// <summary>
    /// Represents jpeg file format (.jpg) resource.
    /// </summary>
    /// <see cref="Resource.RC"/> returns <see cref="Texture"/>.
    public class TextureJpegResource : Resource
    {
        #region Propteries

        public static string FileExtension = "jpg";

        #endregion


        #region Constructors

        public TextureJpegResource(string path, ResourceTarget target, VFS.IFileSystem fs) 
            : base(path, target, fs)
        { }

        #endregion


        #region Public methods

        public override object Load(string path, VFS.IFileSystem fs)
        {
            var bytes = fs.ReadBytes($"{path}.{FileExtension}");
            Texture texture;
            using (var stream = new MemoryStream(bytes))
            {
                var bmp = new Bitmap(stream);
                texture = new Texture(bmp, path);
            }
            return texture;
        }

        #endregion
    }

    /// <summary>
    /// Represents png file format (.png) resource.
    /// </summary>
    /// <see cref="Resource.RC"/> returns <see cref="Texture"/>.
    public class TexturePngResource : Resource
    {
        #region Propteries

        public static string FileExtension = "png";

        #endregion


        #region Constructors

        public TexturePngResource(string path, ResourceTarget target, VFS.IFileSystem fs) 
            : base(path, target, fs)
        { }

        #endregion


        #region Public methods

        public override object Load(string path, VFS.IFileSystem fs)
        {
            var bytes = fs.ReadBytes($"{path}.{FileExtension}");
            Texture texture;
            using (var stream = new MemoryStream(bytes))
            {
                var bmp = new Bitmap(stream);
                texture = new Texture(bmp, path);
            }
            return texture;
        }

        #endregion
    }
}