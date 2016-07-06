// 
// Created : 27.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AshyCommon;
using AshyCore;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL
{
    /// 1. new ShaderProgram(...) or Parse(...)
    /// 2. Compile()
    /// 3. Use() and SetUniform(...) when it renders
    /// ...
    /// n. Free() when it ends
    public class ShaderProgram
    {
        #region Properties

        public int                          ProgramId { get; set; }
        public Shader[]                     Shaders { get; }
        public bool                         CanBeUsed { get; private set; }
        public Dictionary<string, string>   UniformTypes { get; }                 // uniform name -> type
        public Dictionary<string, int>      UniformLocation { get; private set; }    // uniform name -> location

        #endregion
        
        
        #region Constructors

        public ShaderProgram(Dictionary<string, string> uniformTypes, params Shader[] shaders)
        {
            ProgramId               = GL.CreateProgram();
            UniformTypes            = uniformTypes;
            Shaders                 = shaders;
            CanBeUsed               = false;
        }

        #endregion


        #region Public methods

        public void Use()
        {
            if (!CanBeUsed)
                return;
            GL.UseProgram           ( ProgramId );
        }

        public ShaderProgram Compile()
        {
            Shaders.ForEach(s =>
            {                         
                GL.ShaderSource     ( s.Id, s.Source );
                GL.CompileShader    ( s.Id );
                GL.AttachShader     ( ProgramId, s.Id );
                VerifyShaderStep    ( s.Id );
                GL.DeleteShader     ( s.Id );
                VerifyProgramStep   ();
            });

            GL.LinkProgram          ( ProgramId );
            VerifyProgramStep       ();
            CanBeUsed               = true;
            UniformLocation         = UniformTypes
                .ToDictionary       ( u => u.Key, u => GL.GetUniformLocation(ProgramId, u.Key) );

            return                  ( this );
        }

        public void Free()
        {
            GL.DeleteProgram        ( ProgramId );
        }

        public void SetUniform(params KeyValuePair<string, float[]>[] uniforms)
        {
            foreach (var u in uniforms)
            {
                int loc             = UniformLocation[u.Key];
                string utype        = UniformTypes[u.Key];
                if (utype == "Vec3")
                {
                    GL.Uniform3     ( loc, 1, u.Value );
                }
                if (utype == "Mat4")
                {
                    GL.UniformMatrix4( loc, 1, false, u.Value );
                }
            }
        }

        #endregion


        #region Private methods

        private void VerifyShaderStep(int shaderId)
        {
            string info             = GL.GetShaderInfoLog(shaderId);
            if (info != "")
                throw new Exception ( info );
        }

        private void VerifyProgramStep()
        {
            string info             = GL.GetProgramInfoLog(ProgramId);
            if (info != "")
                throw new Exception ( info );
        }

        #endregion


        #region Static methods

        public static ShaderProgram Parse(ShaderAlias shader)
        {
            Shader vertex           = new Shader($"Shaders/{shader.VertexShader}.vs",   ShaderType.VertexShader);
            Shader fragment         = new Shader($"Shaders/{shader.FragmentShader}.fs", ShaderType.FragmentShader);

            return                  ( new ShaderProgram(shader.Uniform, vertex, fragment) );
        }

        #endregion
    }
}