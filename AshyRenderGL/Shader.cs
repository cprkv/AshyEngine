// 
// Created : 18.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCore;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL
{
    public class Shader
    {
        #region Properties

        public string Source { get; }
        public int Id { get; }
        public ShaderType Type { get; }

        #endregion


        #region Constructors

        public Shader(string filename, ShaderType type)
        {
            Id = GL.CreateShader(type);
            Source = RenderAPI.I.Core.FS.ReadAllText(filename);
            Type = type;
        }

        #endregion
    }
}