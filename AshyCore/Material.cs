// 
// Created : 01.04.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCommon.Math;

namespace AshyCore
{
    public class Material
    {
        public Vec3 Color { get;  set; }
        public Texture Diffuse { get; private set; }
        public Texture Normal { get; private set; }
        public ShaderAlias Shader { get; private set; }

        public bool HasDiffuse => Diffuse != null;
        public bool HasNormal  => Normal  != null;

        private Material() {}

        public Material(Vec3 color, string shaderPath, string diffusePath, string normalPath)
        {
            Color   = color;

            Diffuse = diffusePath != null 
                ? CoreAPI.I.Core.RM.Get<Texture>($"Textures/{diffusePath}", Resource.ResourceTarget.LoadedLevel) 
                : null;

            Normal  = normalPath != null 
                ? CoreAPI.I.Core.RM.Get<Texture>($"Textures/{normalPath}", Resource.ResourceTarget.LoadedLevel) 
                : null;

            var shaderInfo = (shaderPath != null) 
                ? CoreAPI.I.Core.RM.Get<ConfigTable>($"Config/Shaders/{shaderPath}", Resource.ResourceTarget.LoadedLevel) 
                : null;

            Shader = shaderInfo == null
                ? null
                : new ShaderAlias(
                    shaderInfo["Shader", "VertexShader"],
                    shaderInfo["Shader", "FragmentShader"],
                    shaderInfo["Uniform"]);
        }

        public Material Copy()
        {
            var ret = new Material
            {
                Color = Color,
                Diffuse = Diffuse,
                Normal = Normal,
                Shader = Shader
            };
            return ret;
        }
    }
}