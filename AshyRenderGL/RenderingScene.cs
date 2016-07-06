//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;
using System.Linq;
using AshyCore;
using AshyCore.EntitySystem;

namespace AshyRenderGL
{
    public class RenderingScene
    {
        #region Properties

        public Camera                               Camera { get; }

        public Dictionary<Entity, RenderComponent>  RenderableEntities { get; }



        #endregion


        #region Constructors

        public RenderingScene(IEnumerable<Entity> entities)
        {
            RenderableEntities      = entities
                .Where              ( e => e.HasComponent(ComponentType.Render) )
                .ToDictionary       ( x => x, x => x.Get<RenderComponent>(ComponentType.Render) );
        }


        #endregion


        
    }
}