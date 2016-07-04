//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;
using System.Linq;
using AshyCommon;

namespace AshyRenderGL.Techniques
{
    public class RenderTechnique : IStage
    {
        protected Queue<IStage> Stages;

        public Scene            Scene { get; private set; }

        /// <summary>
        /// Initializes render stages. Loads level data.
        /// </summary>
        /// <returns><code>true</code>, if no fails.</returns>
        public bool Init(Scene scene)
        {
            Scene               = scene;
            return Stages
                .Select         ( stage => stage.Init(Scene) )
                .All            ( initResult => initResult );
        }

        public void Free()
        {
            Stages.ForEach      ( stage => stage.Free() );
        }

        public void Simulate(float dtime)
        {
            Stages.ForEach      ( stage => stage.Simulate(dtime) );
        }

        public void Render()
        {
            Stages.ForEach      ( stage => stage.Render() );
        }
    }
}