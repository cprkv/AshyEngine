using System;
using System.Collections.Generic;
using AshyCore;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyCore.CoreCommands
{
    internal static class LoadLevel
    {
        /// <summary>
        /// Just collects unused resources.
        /// </summary>
        internal static EngineCommandResult Process()
        {
            if ( ! CoreAPI.I.CheckAllInitialized )
                return                          ( EngineCommandResult.Failed );

            int uncollected                     = Engine.I.RM.CollectWaiting(rtype => rtype >= Resource.ResourceTarget.LoadedLevel);
            if (uncollected != 0)
                Debug.Critical.CollectMemory    ( showLog: true );

            return                              ( EngineCommandResult.Success );
        }
    }

    internal static class DestroyLevel
    {
        internal static EngineCommandResult Process()
        {
            return                              ( LoadLevel.Process() );        // the same - just collect unused memory
        }
    }

    internal static class ChangeLevel
    {
        internal static EngineCommandResult Process()
        {
            return                              ( LoadLevel.Process() );        // the same - just collect unused memory
        }
    }

}
