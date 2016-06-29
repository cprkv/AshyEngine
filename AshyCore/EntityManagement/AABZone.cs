// 
// Created : 05.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCommon.Math;
using AshyCore.EntitySystem;

namespace AshyCore.EntityManagement
{
    /// <summary>
    /// Axis-align box zone.
    /// </summary>
    /// <seealso cref="AshyCore.EntityManagement.IZone" />
    public class AABZone : IZone
    {
        #region Properties

        /// <summary>
        /// Length of zone by X.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Length of zone by Z.
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// Length of zone by Y (height of box).
        /// </summary>
        public float H { get; }

        public Vec3 Center { get; }

        #endregion


        #region Constructors

        public AABZone(Vec3 center, float x, float z, float h)
        {
            Center = center;
            X = x;
            Z = z;
            H = h;
        }

        #endregion

        
        #region Methods

        public bool IsInside(Entity e)
        {
            return e.Position.X < Center.X + X / 2.0f 
                && e.Position.Y < Center.Y + H / 2.0f
                && e.Position.Z < Center.Z + Z / 2.0f

                && e.Position.X > Center.X - X / 2.0f
                && e.Position.Y > Center.Y - H / 2.0f
                && e.Position.Z > Center.Z - Z / 2.0f;
        }

        #endregion
    }
}