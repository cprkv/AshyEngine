//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCore.EngineAPI.EngineCommands;

namespace AshyPhysics.PhysicsCommands
{
    public class PhysicsCommandHandler : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            EngineCommandResult result;
            switch ( c.Type )
            {
                case EngineCommandType.LoadLevel:
                    result      = LoadLevel.Process(c as AshyCore.EngineAPI.EngineCommands.LoadLevel);
                    break;

                case EngineCommandType.DestroyLevel:
                    result      = DestroyLevel.Process();
                    break;

                case EngineCommandType.ChangeLevel:
                    result      = ChangeLevel.Process(c as AshyCore.EngineAPI.EngineCommands.ChangeLevel);
                    break;

                case EngineCommandType.AddEntity:
                    result      = AddEntity.Process(c as AshyCore.EngineAPI.EngineCommands.AddEntity);
                    break;

                // nothing to do
                case EngineCommandType.OpenConsole:
                case EngineCommandType.CloseConsole:
                case EngineCommandType.PauseGame:
                default:
                    result      = EngineCommandResult.Success;
                    break;
            }

            return              ( result );
        }
    }
}