// 
// Created : 04.04.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

namespace AshyCommon.Math
{
    /// <summary>
    /// Describes any transformation with object.
    /// </summary>
    /// <remarks>
    /// The problem is that applying <see cref="Position"/>, <see cref="Rotation"/>, <see cref="Scale"/> isn't commutative.
    /// </remarks>
    public interface ITransformBase
    {
        Vec3 Position { get; }
        Quat Rotation { get; }
        Vec3 Scale { get; }
    }
}