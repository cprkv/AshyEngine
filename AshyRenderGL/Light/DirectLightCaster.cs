//   
// Created : 08.07.2016
// Author  : qweqweqwe
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;
using System.Drawing;
using AshyCommon.Math;
using AshyUI.Util;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL.Light
{
    public class DirectLightCaster : ILightCaster
    {
        private float[] _floats;
        private float[] _float;
        // Correct this another time https://msdn.microsoft.com/ru-ru/library/windows/desktop/ee416324(v=vs.85).aspx
        public Mat4 Projection => Transform.CreateOrthoProjection(-100, 100, -100, 100, 0, 300);
        public Mat4 View => Transform.LookAt(Position, Direction, new Vec3(0, 1, 0));
        public Mat4 LightSpaceMatrix => View * Projection;

        public Vec3 Position { get; }
        public Vec3 Color { get; }
        public Vec3 Direction { get; }
        // pid -> struct member number -> uniform
        //public Dictionary<int, Dictionary<int, int>> Uniforms; 

        public DirectLightCaster(Vec3 pos, Vec3 dir, Vec3 color)
        {
            Position = pos;
            Direction = dir;
            Color = color;
            _floats = new[] { 0.25f };
            _float = new[] { 1f };
            
        }

        public void SetUniforms(int programId)
        {
            //if (Uniforms == null)
            //{
            //    Uniforms = new Dictionary<int, Dictionary<int, int>>
            //    {
            //        {
            //            programId,
            //            new Dictionary<int, int>
            //            {
            //                { 0, GL.GetUniformLocation(programId, "dirLight.direction")    },
            //                { 1, GL.GetUniformLocation(programId, "dirLight.ambientCoef")  },
            //                { 2, GL.GetUniformLocation(programId, "dirLight.diffuseCoef")  },
            //                { 3, GL.GetUniformLocation(programId, "dirLight.specularCoef") },
            //                { 4, GL.GetUniformLocation(programId, "dirLight.color")        },
            //            }
            //        }
            //    };
            //}
            GL.Uniform3(GL.GetUniformLocation(programId, "dirLight.direction"),     1, (Direction - Position).Values);
            GL.Uniform1(GL.GetUniformLocation(programId, "dirLight.ambientCoef"),   1, _floats);
            GL.Uniform1(GL.GetUniformLocation(programId, "dirLight.diffuseCoef"),   1, _float);
            GL.Uniform1(GL.GetUniformLocation(programId, "dirLight.specularCoef"),  1, _floats);
            GL.Uniform3(GL.GetUniformLocation(programId, "dirLight.color"),         1, Color.Values);
        }
    }
}