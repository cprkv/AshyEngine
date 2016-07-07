//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;
using System.Linq;
using AshyCommon.Math;
using AshyCore;
using AshyCore.EntitySystem;

namespace AshyRenderGL
{
    public class RenderingScene
    {
        #region Properties

        public Camera                               Camera { get; }

        public Dictionary<Entity, RenderComponent>  RenderableEntities { get; }

        public Vec3                                 LightPosition { get; }

        #endregion


        #region Constructors

        public RenderingScene(IEnumerable<Entity> entities)
        {
            LightPosition           = Vec3.Parse(RenderAPI.I.Game.Level.LevelInfo["LevelProperties", "DirectionalLight"]);
            Camera                  = new Camera
            {
                Width               = Engine.I.GameWindow.Width,
                Height              = Engine.I.GameWindow.Height,
                Dir                 = new Vec3(1, 0, 1).Norm(),
                Eye                 = new Vec3(-3.910404f, 6.532519f, -8.444711f),
                Right               = new Vec3(-0.821857f, 0.000000f, -0.051417f)
            };
            Engine.I.GameWindow.RenderFrame += dtime =>
            {
                Camera.InterpolationDeg += 0.001f;
                if (Camera.InterpolationDeg > 1)
                    Camera.InterpolationDeg = 0;
                Camera.Eye = Camera.BezierCurve.Interpolate(Camera.InterpolationDeg);
                Camera.Dir = (RenderAPI.I.Game.Level.GetEntity("factory_0001").Position - Camera.Eye).Norm();
            };

            Engine.I.GameWindow.Resize += size =>
            {
                Camera.Width        = size.Width;
                Camera.Height       = size.Height;
            };

            RenderableEntities      = entities
                .Where              ( e => e.HasComponent(ComponentType.Render) )
                .ToDictionary       ( x => x, x => x.Get<RenderComponent>(ComponentType.Render) );
        }

        #endregion
    }
}