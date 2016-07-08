//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL.Techniques.GL4
{
    public class LevelTechnique : RenderTechnique
    {
        public LevelTechnique()
        {
            Stages              = new Queue<IStage>(3);

            Stages.Enqueue      ( new SkyboxStage() );
            //Stages.Enqueue      ( new DiffuseStage() );
            Stages.Enqueue      ( new ShadowStage() );
            //Stages.Enqueue      ( new ParticleStage() );
        }

        public override bool Init(RenderingScene renderingScene)
        {
            GL.Enable           ( EnableCap.DepthTest );
            GL.Disable          ( EnableCap.CullFace );
            GL.Enable           ( EnableCap.Texture2D );
            GL.BlendFunc        ( BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha );
            GL.Enable           ( EnableCap.Blend );
            GL.Enable           ( IndexedEnableCap.Blend, 0 );
            
            GL.ClearColor       ( Color.FromArgb(50, 50, 50) );
            GL.Hint             ( HintTarget.PerspectiveCorrectionHint, HintMode.Nicest );

            return ( base.Init(renderingScene) );
        }

        public override void Render()
        {
            GL.Clear            ( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
            GL.Viewport         ( 0, 0, Engine.I.GameWindow.Width, Engine.I.GameWindow.Height );
            base.Render         ();
            GL.BindVertexArray  ( 0 );
            GL.BindBuffer       ( BufferTarget.ArrayBuffer, 0 );
            GL.BindBuffer       ( BufferTarget.ElementArrayBuffer, 0 );
            GL.UseProgram       ( 0 );
        }
    }
}