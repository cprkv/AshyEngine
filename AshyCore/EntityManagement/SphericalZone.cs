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
    /// Describes any zone repeseted by sphere.
    /// </summary>
    /// <seealso cref="AshyCore.EntityManagement.IZone" />
    public class SphericalZone : IZone
    {
        #region Properties

        /// <summary>
        /// Radius of the spherical zone.
        /// </summary>
        public float Radius { get; }

        public Vec3 Center { get; }

        #endregion


        #region Constructors

        public SphericalZone(Vec3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        #endregion


        #region Methods

        public bool IsInside(Entity e) => (e.Position - Center).Len < Radius;

        #endregion
    }
}