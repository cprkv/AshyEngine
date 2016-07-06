//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;

namespace AshyRenderGL.Techniques.GL4
{
    public class SkyboxStage : IStage
    {
        private Skybox              _skybox;

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

                return              ( true );
            }
            catch (Exception e)
            {
                RenderAPI.I.Core.Log.Error("Can't load skybox", e);
#if DEBUG
                throw;
#endif
                return              ( false );
            }
        }

        public void Free()
        {
            _skybox                 = null;
        }

        public void Simulate(float dtime)
        {
            // todo: make clouds move here 
        }

        public void Render()
        {
            _skybox?.BindAndRender  ();
        }
    }
}