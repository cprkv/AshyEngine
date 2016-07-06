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
                Dir                 = new Vec3(1, 1, 1).Norm(),
                Eye                 = new Vec3(-3.910404f, 6.532519f, -8.444711f),
                Right               = new Vec3(-0.821857f, 0.000000f, -0.051417f)
            };

            RenderableEntities      = entities
                .Where              ( e => e.HasComponent(ComponentType.Render) )
                .ToDictionary       ( x => x, x => x.Get<RenderComponent>(ComponentType.Render) );
        }

        #endregion
    }
}