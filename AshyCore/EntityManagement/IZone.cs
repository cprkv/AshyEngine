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
    /// Any types of zones where could be character in or out.
    /// </summary>
    public interface IZone
    {
        /// <summary>
        /// Center of the zone.
        /// </summary>
        Vec3 Center { get; }

        /// <summary>
        /// Determines whether the specified entity is: 
        /// inside if <code>true</code> or outside if <code>falce</code>.
        /// </summary>
        bool IsInside(Entity e);
    }
}