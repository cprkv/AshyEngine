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

        public RenderingScene   RenderingScene { get; private set; }

        public bool             IsInitialized { get; private set; }

        /// <summary>
        /// Initializes render stages. Loads level data.
        /// </summary>
        /// <returns><code>true</code>, if no fails.</returns>
        public virtual bool Init(RenderingScene renderingScene)
        {
            RenderingScene      = renderingScene;
            var initResult      = Stages
                .Select         ( stage => stage.Init(RenderingScene) )
                .All            ( stageInit => stageInit );
            IsInitialized       = true;

            return              ( initResult );
        }

        public void Free()
        {
            Stages.ForEach      ( stage => stage.Free() );
            IsInitialized       = false;
        }

        public void Simulate(float dtime)
        {
            Stages.ForEach      ( stage => stage.Simulate(dtime) );
        }

        public virtual void Render()
        {
            Stages.ForEach      ( stage => stage.Render() );
        }
    }
}