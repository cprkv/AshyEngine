//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCommon;
using AshyCore;
using AshyCore.EntitySystem;
using OpenTK.Graphics.OpenGL4;
using Uniform = System.Collections.Generic.KeyValuePair<string, float[]>;


namespace AshyRenderGL.Techniques.GL4
{
    public class DiffuseStage : IStage
    {
        private RenderingScene scene;

        public bool Init(RenderingScene renderingScene)
        {
            throw new System.NotImplementedException();
        }

        public void Free()
        {
            throw new System.NotImplementedException();
        }

        public void Simulate(float dtime)
        {
            throw new System.NotImplementedException();
        }

        public void Render()
        {
            foreach (var ent in scene.RenderableEntities)
            {
                GL.BindVertexArray(Engine.I.Device.Buffers[ent.Value.Mesh].VertexArrayObjectId);

                // set shader
                var shaderProgram = Engine.I.Device.ShaderPrograms[ent.Value.Material.Shader];
                if (!shaderProgram.CanBeUsed) continue;
                shaderProgram.Use();
                shaderProgram.SetUniform(
                    new Uniform("lightPos", scene.LightPosition.Values),
                    new Uniform("viewProjectionMat", scene.Camera.ViewProj.Values),
                    new Uniform("modelMat", ent.Key.TransformMatrix.Values),
                    new Uniform("ambientLight", (ent.Value.Material.Color).Values)
                    );

                // bind texture
                if (ent.Value.Material.HasDiffuse)  Engine.I.Device.Textures[ent.Value.Material.Diffuse].Bind(0);
                if (ent.Value.Material.HasNormal)   Engine.I.Device.Textures[ent.Value.Material.Normal].Bind(1);

                GL.DrawArrays(PrimitiveType.Triangles, 0, ent.Value.Mesh.VertIndices.Length);
            }
        }
    }
}