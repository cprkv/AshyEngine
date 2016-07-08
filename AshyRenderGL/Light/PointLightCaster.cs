//   
// Created : 08.07.2016
// Author  : qweqweqwe
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCommon.Math;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL.Light
{
    public class PointLightCaster : ILightCaster
    {
        public Vec3 Position { get; }
        public Vec3 Color { get; }
        public Mat4 LightSpaceMatrix { get; }
        private string[,] _uniforms;

        public float Constant { get; }
        public float Linear { get; }
        public float Quadratic { get; }
        public float AmbientCoef { get; }
        public float DiffuseCoef { get; }
        public float SpecularCoef { get; }

        public float zNear = 1;
        public float zFar = 25;
        public Mat4 Proj => Transform.PerspectiveFOV(90f.ToRadians(), Engine.I.GameWindow.Width / (float)Engine.I.GameWindow.Height, zNear, zFar);

        public PointLightCaster(Vec3 pos, Vec3 color)
        {
            Position        = pos;
            Color           = color;
            Constant        = 1f;
            Linear          = 0.07f;
            Quadratic       = 0.017f;
            AmbientCoef     = 0.85f;
            DiffuseCoef     = 1f;
            SpecularCoef    = 0.55f;
            _uniforms       = new string[5, 8];
            for (int i = 0; i < 5; i++)
            {
                _uniforms[i, 0] = $"pointLights[{i}].position"    ;
                _uniforms[i, 1] = $"pointLights[{i}].color"       ;
                _uniforms[i, 2] = $"pointLights[{i}].constant"    ;
                _uniforms[i, 3] = $"pointLights[{i}].linear"      ;
                _uniforms[i, 4] = $"pointLights[{i}].quadratic"   ;
                _uniforms[i, 5] = $"pointLights[{i}].ambientCoef" ;
                _uniforms[i, 6] = $"pointLights[{i}].diffuseCoef" ;
                _uniforms[i, 7] = $"pointLights[{i}].specularCoef";
           }
        }

        public void SetUniforms(int programId, int arrayId)
        {
            GL.Uniform3(GL.GetUniformLocation(programId, _uniforms[arrayId, 0]), 1, Position.Values);
            GL.Uniform3(GL.GetUniformLocation(programId, _uniforms[arrayId, 1]), 1, Color.Values);
            GL.Uniform1(GL.GetUniformLocation(programId, _uniforms[arrayId, 2]), 1, new[] { Constant });
            GL.Uniform1(GL.GetUniformLocation(programId, _uniforms[arrayId, 3]), 1, new[] { Linear });
            GL.Uniform1(GL.GetUniformLocation(programId, _uniforms[arrayId, 4]), 1, new[] { Quadratic });
            GL.Uniform1(GL.GetUniformLocation(programId, _uniforms[arrayId, 5]), 1, new[] { AmbientCoef });
            GL.Uniform1(GL.GetUniformLocation(programId, _uniforms[arrayId, 6]), 1, new[] { DiffuseCoef });
            GL.Uniform1(GL.GetUniformLocation(programId, _uniforms[arrayId, 7]), 1, new[] { SpecularCoef });
        }
    }
}