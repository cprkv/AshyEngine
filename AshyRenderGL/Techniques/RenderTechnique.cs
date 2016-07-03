//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;

namespace AshyRenderGL.Techniques
{
    public class RenderTechnique
    {
        private Queue<Stage> _stages;

        public Scene Scene { get; internal set; }

        public RenderTechnique(Queue<Stage> stages)
        {
            _stages         = stages;
        }

        public void Render()
        {
            
        }
    }
}