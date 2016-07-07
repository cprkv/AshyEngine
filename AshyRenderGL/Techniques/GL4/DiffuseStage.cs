//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using AshyCommon;
using AshyCore;
using AshyCore.EntitySystem;
using OpenTK.Graphics.OpenGL4;
using Uniform = System.Collections.Generic.KeyValuePair<string, float[]>;


namespace AshyRenderGL.Techniques.GL4
{
    public class DiffuseStage : IStage
    {
        private RenderingScene  scene;
        private Device          _device;

        public bool Init(RenderingScene renderingScene)
        {
            scene                               = renderingScene;
            _device                             = Engine.I.Device;
            try
            {
                foreach (var ent in scene.RenderableEntities)
                {
                    _device.LoadMesh            ( ent.Value.Mesh );
                    _device.LoadShader          ( ent.Value.Material.Shader );

                    if (ent.Value.Material.HasDiffuse)
                        _device.LoadTexture     ( ent.Value.Material.Diffuse );

                    if (ent.Value.Material.HasNormal)
                        _device.LoadTexture     ( ent.Value.Material.Normal );
                }
                return                          ( true );
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                RenderAPI.I.Core.Log.Error("Can't initialize diffuse stage.", e);
            }
            return                              ( false );
        }

        public void Free()
        {
            // free
        }

        public void Simulate(float dtime)
        {
            // empty
        }

        public void Render()
        {
            foreach (var ent in scene.RenderableEntities)
            {
                GL.BindVertexArray              (_device.Buffers[ent.Value.Mesh].VertexArrayObjectId );

                // set shader
                var shaderProgram               = _device.ShaderPrograms[ent.Value.Material.Shader];
                if (!shaderProgram.CanBeUsed) continue;
                shaderProgram.Use               ();
                shaderProgram.SetUniform        (
                                                    new Uniform("lightPos", scene.LightPosition.Values),
                                                    new Uniform("viewProjectionMat", scene.Camera.ViewProj.Values),
                                                    new Uniform("modelMat", ent.Key.TransformMatrix.Values),
                                                    new Uniform("ambientLight", (ent.Value.Material.Color).Values)
                                                );

                // bind texture
                if (ent.Value.Material.HasDiffuse)
                    _device.Textures[ent.Value.Material.Diffuse].Bind(0);

                if (ent.Value.Material.HasNormal)
                    _device.Textures[ent.Value.Material.Normal].Bind(1);

                GL.DrawArrays(PrimitiveType.Triangles, 0, ent.Value.Mesh.VertIndices.Length);
            }
        }
    }
}