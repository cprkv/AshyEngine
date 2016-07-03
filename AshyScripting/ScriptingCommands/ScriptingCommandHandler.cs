//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System.Collections.Generic;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyScripting.ScriptingCommands
{
    internal class ScriptingCommandHandler : EngineCommandHandlerBase
    {
        public ScriptingCommandHandler()
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