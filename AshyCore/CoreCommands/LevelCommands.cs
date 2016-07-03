using System;
using System.Collections.Generic;
using AshyCore;
using AshyCore.Debug;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;

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
            int uncollected         = Engine.I.RM.CollectWaiting(rtype => rtype >= Resource.ResourceTarget.LoadedLevel);
            if (uncollected != 0)
                Memory.Collect      ( showLog: true );

            return                  ( EngineCommandResult.Success );
        }
    }

    #endregion


    internal class LoadLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            return                  ( LevelCommandsHelper.CollectResources() );
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
