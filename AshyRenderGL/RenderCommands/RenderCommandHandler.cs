//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyRenderGL.RenderCommands
{
    internal class RenderCommandHandler : EngineCommandHandlerBase
    {
        public RenderCommandHandler()
        {
            Executers = new Dictionary<EngineCommandType, IEngineCommandHandler>()
            {
                { EngineCommandType.LoadLevel,      new LoadLevel() },
                { EngineCommandType.AddEntity,      new AddEntity() },
                { EngineCommandType.DestroyLevel,   new DestroyLevel() },
            };
        }
    }
}