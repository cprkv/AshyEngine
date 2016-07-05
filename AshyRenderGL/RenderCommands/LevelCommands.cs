//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCore.Debug;
using AshyCore.EngineAPI.EngineCommands;
using AshyCore.EntitySystem;

namespace AshyRenderGL.RenderCommands
{
    #region Helper

    internal static class LevelCmdHelper
    {
        internal static EngineCommandResult InitEntity(Entity entity)
        {
            return                      ( EngineCommandResult.Success );
        }
    }

    #endregion


    internal class AddEntity : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            var aec                     = (AshyCore.EngineAPI.EngineCommands.AddEntity) c;
            return                      ( LevelCmdHelper.InitEntity(aec.Entity) );
        }
    }

    internal class LoadLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            var ll                      = (AshyCore.EngineAPI.EngineCommands.LoadLevel) c;
            var res                     = EngineCommandResult.Success;

            foreach (var entity in ll.LoadingLevel.Entities)
            {
                res                     = LevelCmdHelper.InitEntity(entity).Worst(res);
            }

            return                      ( res );
        }
    }

    internal class DestroyLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            Memory.Collect              ( showLog: true );

            return                      ( EngineCommandResult.Success ); 
        }
    }
}