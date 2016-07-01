//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using AshyCore;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyGame.GameCommands
{
    internal static class AddEntity
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.AddEntity c)
        {
            if ( ! CoreAPI.I.CheckAllInitialized || c == null)
                return                  ( EngineCommandResult.Failed );

            Engine.I.Level.Spawn        ( c.Entity );
            return                      ( EngineCommandResult.Success );
        }
    }

    internal static class LoadLevel
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.LoadLevel c)
        {
            if ( ! CoreAPI.I.CheckAllInitialized || c == null )
                return                  ( EngineCommandResult.Failed );

            try
            {
                Engine.I.Level          = c.LoadingLevel;
                Engine.I.Level.Load     ();
            }
            catch (Exception e)
            {
                GameAPI.I.Core.Log.Error("--- Load level failed in AshyGame module ---", e);
#if DEBUG
                throw;
#else
                return                  ( EngineCommandResult.CriticalFailed );
#endif
            }
            return                      ( EngineCommandResult.Success );
        }
    }

    internal static class DestroyLevel
    {
        internal static EngineCommandResult Process()
        {
            Engine.I.Level              = null;
            return                      ( EngineCommandResult.Success ); 
        }
    }

    internal static class ChangeLevel
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.ChangeLevel c)
        {
            DestroyLevel.Process();
            if ( ! CoreAPI.I.CheckAllInitialized || c == null )
                return                  ( EngineCommandResult.Failed );

            try
            {
                Engine.I.Level          = c.LoadingLevel;
                Engine.I.Level.Load     ();
            }
            catch (Exception e)
            {
                GameAPI.I.Core.Log.Error("--- Change level failed in AshyGame module ---", e);
#if DEBUG
                throw;
#else
                return                  ( EngineCommandResult.CriticalFailed );
#endif
            }
            return                      ( EngineCommandResult.Success );
        }
    }
}