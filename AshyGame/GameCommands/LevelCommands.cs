//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyGame.GameCommands
{
    internal class AddEntity : IEngineCommandHandler
    {
        EngineCommandResult IEngineCommandHandler.Execute(IEngineCommand c)
        {
            var aec                     = (AshyCore.EngineAPI.EngineCommands.AddEntity) c;
            Engine.I.Level.Spawn        ( aec.Entity );
            return                      ( EngineCommandResult.Success );
        }
    }

    internal class LoadLevel : IEngineCommandHandler
    {
        EngineCommandResult IEngineCommandHandler.Execute(IEngineCommand c)
        {
            try
            {
                var ll                  = (AshyCore.EngineAPI.EngineCommands.LoadLevel) c;
                Engine.I.Level          = ll.LoadingLevel;
                Engine.I.Level.Load     ();
            }
            catch (Exception e)
            {
                GameAPI.I.Core.Log.Error("--- Load level command failed in AshyGame module ---", e);
#if DEBUG
                throw;
#else
                return                  ( EngineCommandResult.CriticalFailed );
#endif
            }
            return                      ( EngineCommandResult.Success );
        }
    }

    internal class DestroyLevel : IEngineCommandHandler
    {
        EngineCommandResult IEngineCommandHandler.Execute(IEngineCommand c)
        {
            Engine.I.Level              = null;
            return                      ( EngineCommandResult.Success ); 
        }
    }
}