//  
// Created  : 28.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

namespace AshyCore.EntitySystem
{
    public enum ComponentType
    {
        Geom,
        Physics,
        Render,
        Script
    }

    /// <summary>
    /// Decribes component for <see cref="Entity"/>.
    /// </summary>
    public interface IComponent
    {
        ComponentType Type { get; }
    }
}