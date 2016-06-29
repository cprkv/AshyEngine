// 
// Created : 29.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AshyCore
{
    /// <summary>
    /// Class-wrapper for <see cref="System.Drawing.Bitmap"/>.
    /// </summary>
    public class Texture : IEquatable<Texture>
    {
        #region Private fields

        private string Name { get; }

        public Bitmap Bitmap { get; }

        #endregion


        #region Constructors

        public Texture(Bitmap bitmap, string name)
        {
            Bitmap = bitmap;
            Name = name;
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Processes bitmap data.
        /// </summary>
        public void ProcessData(Action<BitmapData> proc)
        {
            BitmapData data = Bitmap.LockBits(
                new Rectangle(0, 0, Bitmap.Width, Bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb
                );

            proc(data);

            Bitmap.UnlockBits(data);
        }

        public bool Equals(Texture other)
        {
            return Name == other.Name;
        }

        #endregion
    }
}