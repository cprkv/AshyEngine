// 
// Created : 05.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using AshyCore.EntitySystem;

namespace AshyCore.EntityManagement
{
    /// <summary>
    /// Represents zone trigger.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Action, which happens then the specified entity or any entity is gets in the <see cref="Zone"/>.
        /// </summary>
        Action<Entity> OnInAction { get; }

        /// <summary>
        /// Action, which happens then the specified entity or any entity gets out of <see cref="Zone"/>.
        /// </summary>
        Action<Entity> OnOutAction { get; }

        /// <summary>
        /// Special zone that determines where is entity: inside or outside.
        /// </summary>
        IZone Zone { get; }
    }
}