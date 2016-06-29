// 
// Created : 10.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using AshyCore;
using AshyCore.EntitySystem;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL
{
    /// <summary>
    /// Rendering module. Implementation for <see cref="RenderAPI"/>.
    /// </summary>
    /// <seealso cref="IRenderModule" /> todo move to engine
    //public class RenderModule : IRenderModule
    //{
    //    #region Properties

    //    public Render Render { get; private set; }

    //    public Version Version => new Version(GL.GetString(StringName.Version).Substring(0, 3)); 
        
    //    #endregion


    //    #region Inherited Methods

    //    public EventHandler<float> FrameIteration { get; set; }

    //    public void Start()
    //    {
    //        Render = new Render(FrameIteration);
    //        Render.Run();
    //    }

    //    public void LoadLevel(GameLevel gameLevel)
    //    {
    //        gameLevel.Load();
    //    }

    //    public void FreeLevelData()
    //    {

    //    }

    //    public void RegisterEntity(Entity entity)
    //    {

    //    }

    //    public void End()
    //    {
    //        Render.Release();
    //        RenderAPI.Instance.Log.Info("[Render] Ended.");
    //    }

    //    #endregion
    //}
}
