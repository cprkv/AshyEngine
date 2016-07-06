//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  
namespace AshyRenderGL.Techniques
{
    public interface IStage
    {
        /// <summary>
        /// Special method for loading RenderingScene data.
        /// </summary>
        /// <param name="renderingScene"> Loading RenderingScene to graphics card. </param>
        /// <returns> <code>true</code>, if no fails. </returns>
        bool Init(RenderingScene renderingScene);

        void Free();

        void Simulate(float dtime);

        void Render();
    }
}