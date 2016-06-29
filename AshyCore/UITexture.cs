// 
// Created : 08.04.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Collections.Generic;
using AshyCommon.Math;

namespace AshyCore
{
    public class UITexture
    {
        public Mesh Mesh { get; }

        public Texture Texture { get; }

        public ShaderAlias Shader { get; }

        public UITexture(Texture texture)
        {
            Mesh = new Mesh(
                vertices:    new Vec3[] { new Vec3(-1f, 1f, 0.5f), new Vec3(-1f, -1f, 0.5f), new Vec3(1f, -1f, 0.5f), new Vec3(1f, 1f, 0.5f) },
                uvw:         new Vec3[] { new Vec3( 0, -1, 1), new Vec3( 0,  0, 1), new Vec3(1,  0, 1), new Vec3(1, -1, 1) },
                normals:     new Vec3[] { new Vec3( 0, 1, 1), new Vec3( 0,  0, 1), new Vec3(1,  0, 1), new Vec3(1, 1, 1) },
                vertIndices: new uint[] { 0, 1, 2,  2, 3, 0 },
                uvwIndices:  new uint[] { 0, 1, 2,  2, 3, 0 },
                normIndices: new uint[] { 0, 1, 2,  2, 3, 0 }
                );
            Texture = texture;
            Shader = new ShaderAlias("basicUITexture", "basicUITexture", new Dictionary<string, string>());
        }
    }
}
