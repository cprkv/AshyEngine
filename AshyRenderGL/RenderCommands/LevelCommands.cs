//   
// Created : 04.07.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCore.Debug;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;
using AshyCore.EntitySystem;

namespace AshyRenderGL.RenderCommands
{
    #region Helper

    internal static class LevelCmdHelper
    {
        internal static EngineCommandResult InitEntity(Entity entity)
        {
            // todo: initialize entity on RenderingScene
            return                      ( EngineCommandResult.Success );
        }
    }

    #endregion


    internal class AddEntity : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            var aec                     = (AshyCore.EngineAPI.EngineCommands.AddEntity) c;
            var res                     = LevelCmdHelper.InitEntity(aec.Entity);

            return                      ( res );
        }
    }

    internal class LoadLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            var ll                      = (AshyCore.EngineAPI.EngineCommands.LoadLevel) c;
            var scene                   = new RenderingScene(ll.LoadingLevel.Entities);
            var isLoaded                = Engine.I.RenderTechnique.Init( scene );

            if (isLoaded)
                Engine.I.Status         = EngineStatus.LoadedWorld;

            return                      ( isLoaded ? EngineCommandResult.Success : EngineCommandResult.Failed );
        }
    }

    internal class DestroyLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            Memory.Collect              ( showLog: true );

            Engine.I.RenderTechnique?.Free();

            Engine.I.Status             = EngineStatus.ReadyToUse;
            return                      ( EngineCommandResult.Success ); 
        }
    }
}