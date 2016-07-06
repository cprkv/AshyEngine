//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;

namespace AshyRenderGL.Techniques.GL4
{
    public class LevelTechnique : RenderTechnique
    {
        public LevelTechnique()
        {
            Stages              = new Queue<IStage>(3);

            Stages.Enqueue      ( new SkyboxStage() );
            //Stages.Enqueue      ( new DiffuseStage() );
            //Stages.Enqueue      ( new ParticleStage() );
        }
    }
}