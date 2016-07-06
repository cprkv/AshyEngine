//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using OpenTK.Graphics.OpenGL4;
using Uniform = System.Collections.Generic.KeyValuePair<string, float[]>;

namespace AshyRenderGL.Techniques.GL4
{
    public class SkyboxStage : IStage
    {
        private Skybox              _skybox;
        private bool                initialized;
        private RenderingScene      scene;

        public bool Init(RenderingScene renderingScene)
        {
            try
            {
                var skyboxName      = RenderAPI.I.Game.Level.LevelInfo["LevelProperties", "Skybox"];
                var skyboxData      = RenderAPI.I.Game.Level.LevelInfo[skyboxName];
                _skybox             = Skybox.Load(
                                            skyboxData["right"], 
                                            skyboxData["left"], 
                                            skyboxData["top"], 
                                            skyboxData["bottom"], 
                                            skyboxData["back"], 
                                            skyboxData["front"] );
                initialized         = true;
                scene               = renderingScene;
            }
            catch (Exception e)
            {
                RenderAPI.I.Core.Log.Error("Can't load skybox", e);
#if DEBUG
                throw;
#endif
                initialized         = false;
            }

            return                  ( initialized );
        }

        public void Free()
        {
            _skybox                 = null;
            initialized             = false;
        }

        public void Simulate(float dtime)
        {
            // todo: make clouds move here 
        }

        public void Render()
        {
            if( ! initialized)      return;
            GL.Disable              ( EnableCap.DepthTest );
            GL.Disable              ( EnableCap.CullFace );
            _skybox.ShaderProgram.Use();
            _skybox.ShaderProgram.SetUniform(
                new Uniform("viewProjectionMat",    scene.Camera.View.Transpose().ClipRotation().Values),
                new Uniform("modelMat",             scene.Camera.Proj.Values)
                );

            _skybox.BindAndRender   ();

            GL.Disable              ( EnableCap.CullFace );
            GL.Enable               ( EnableCap.DepthTest );
        }
    }
}