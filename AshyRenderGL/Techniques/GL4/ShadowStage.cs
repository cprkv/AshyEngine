//   
// Created : 07.07.2016
// Author  : qweqweqwe
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using System.Collections.Generic;
using AshyCommon.Math;
using AshyCore;
using AshyCore.EntitySystem;
using AshyRenderGL.Light;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL.Techniques.GL4
{
    public class ShadowStage : IStage
    {
        private DepthMap _depthMap;
        private RenderingScene _scene;
        private DirectLightCaster _directLight;
        private ShaderProgram _shaderProgram;

        public bool Init(RenderingScene renderingScene)
        {
            _scene = renderingScene;

            var _device = Engine.I.Device;
            try
            {
                foreach (var ent in renderingScene.RenderableEntities)
                {
                    _device.LoadMesh(ent.Value.Mesh);
                    _device.LoadShader(ent.Value.Material.Shader);

                    if (ent.Value.Material.HasDiffuse)
                        _device.LoadTexture(ent.Value.Material.Diffuse);

                    if (ent.Value.Material.HasNormal)
                        _device.LoadTexture(ent.Value.Material.Normal);
                }
                _depthMap       = new DepthMap(renderingScene);
                _directLight    = new DirectLightCaster(pos: new Vec3(90, 90, -150), dir: Vec3.Zero, color: new Vec3(234.0f / 255.0f, 215.0f / 255.0f, 213.0f / 255.0f));
                _depthMap.FillDepthMap(_directLight.LightSpaceMatrix);

                _shaderProgram  = DepthMap.LoadSpecificShader("shadow&light");
                return (true);
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                RenderAPI.I.Core.Log.Error("Can't initialize diffuse stage.", e);
            }
            return (false);
        }

        public void Free()
        {
            _depthMap.Release();
        }

        public void Simulate(float dtime)
        {
            // empty
        }

        public void Render()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Enable(EnableCap.DepthTest);
            GL.ActiveTexture(TextureUnit.Texture10);
            GL.BindTexture(TextureTarget.Texture2D, _depthMap.DepthMapId);
            GL.ActiveTexture(0);

            // set shader

            if (!_shaderProgram.CanBeUsed) throw new Exception("FillDepthMap: shader can't be used");
            _shaderProgram.Use();

            List<PointLightCaster> lightPoints = new List<PointLightCaster>
            {
                //new PointLightCaster(new Vec3(0, 20f, 5f), new Vec3(1, 0, 0)),
                //new PointLightCaster(new Vec3(20, 5f, 5), new Vec3(0, 1, 0)),
                //new PointLightCaster(new Vec3(0, 0f, 20), new Vec3(0, 0, 1)),
                //new PointLightCaster(new Vec3(20, 20f, 20f), new Vec3(1, 1, 1)),
                //new PointLightCaster(new Vec3(-20, 0f, 20f), new Vec3(1, 1, 0))
            };

            _directLight.SetUniforms(_shaderProgram.ProgramId);
            foreach (var entity in _scene.RenderableEntities)
            {
                var renderComponent = entity.Value;

                GL.BindVertexArray(Engine.I.Device.Buffers[renderComponent.Mesh.Id].VertexArrayObjectId);

                GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram.ProgramId, "ModelMat"), 1, false, entity.Key.TransformMatrix.Values);
                GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram.ProgramId, "ProjectView"), 1, false, _scene.Camera.ViewProj.Values);
                GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram.ProgramId, "DepthMVP"), 1, false, (entity.Key.TransformMatrix * _directLight.LightSpaceMatrix).Values);
                GL.Uniform3(GL.GetUniformLocation(_shaderProgram.ProgramId, "viewPos"), 1, _scene.Camera.Eye.Values);
                GL.Uniform1(GL.GetUniformLocation(_shaderProgram.ProgramId, "numPointLight"), 1, new[] { lightPoints.Count });

                for (var i = 0; i < lightPoints.Count; i++)
                {
                    lightPoints[i].SetUniforms(_shaderProgram.ProgramId, i);
                }

                // bind texture
                if (renderComponent.Material.HasDiffuse)    Engine.I.Device.Textures[renderComponent.Material.Diffuse].Bind(0);
                if (renderComponent.Material.HasNormal)     Engine.I.Device.Textures[renderComponent.Material.Normal].Bind(1);

                GL.DrawArrays(PrimitiveType.Triangles, 0, renderComponent.Mesh.IndexLength);
            }
            //_shaderProgram.Free();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.DepthTest);
        }
    }
}