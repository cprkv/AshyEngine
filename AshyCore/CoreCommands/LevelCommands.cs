using System;
using System.Collections.Generic;
using System.Linq;
using AshyCommon;
using AshyCommon.Math;
using AshyCore;
using AshyCore.Debug;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;
using AshyCore.EntitySystem;

namespace AshyCore.CoreCommands
{
    #region Helper

    internal static class LevelCommandsHelper
    {
        /// <summary>
        /// Just collects unused resources.
        /// </summary>
        internal static EngineCommandResult CollectResources()
        {
            Engine.I.RM.CollectWaiting( rtype => rtype >= Resource.ResourceTarget.LoadedLevel );
            Memory.Collect          ( showLog: true );
            Memory.CompactHeap      ();


            return                  ( EngineCommandResult.Success );
        }
    }

    #endregion


    internal class LoadLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            CoreAPI.I.Game.Level.Entities
                .Where              ( e => e.HasComponent(ComponentType.Geom) )
                .Select             ( e => e.Get<GeomComponent>(ComponentType.Geom).Mesh )
                .ForEach            ( m => m.Free() );
            var result              = LevelCommandsHelper.CollectResources();

            var aliveMeshes         = Mesh.HoldingData.Count(h => h.IsAlive);
            var aliveMeshesData     = BufferDataDesctiption.HoldingData.Count(h => h.IsAlive);
            Engine.I.Log.Info       ($"Alive meshes: {aliveMeshes}, Alive meshes data: {aliveMeshesData}");

            return                  ( result );
        }
    }
    
    internal class DestroyLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            return                  ( LevelCommandsHelper.CollectResources() );
        }
    }
    
    internal class ChangeLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            return                  ( LevelCommandsHelper.CollectResources() );
        }
    }
}
