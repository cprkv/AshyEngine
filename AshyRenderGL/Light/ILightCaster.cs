//   
// Created : 08.07.2016
// Author  : qweqweqwe
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCommon.Math;

namespace AshyRenderGL.Light
{
    public interface ILightCaster
    {
        Vec3 Position { get; }
        Vec3 Color { get; }
        Mat4 LightSpaceMatrix { get; }
    }
}