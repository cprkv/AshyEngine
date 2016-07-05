using System;
using System.Collections.Generic;
using AshyCore.EngineAPI;
using AshyCore.EntitySystem;
using AshyScripting.ScriptingCommands;
using NLua;

namespace AshyScripting
{
    public class ScriptingAPI : IEngineAPI
    {
        public static EngineProxy I { get; private set; }
        
        public EngineStatus Preinitialize(EngineProxy baseEngine)
        {
            I                           = baseEngine;
            Engine.I                    = I.Script as Engine;
            if (Engine.I == null) 
                return                  ( EngineStatus.CriticalFailed );

            Engine.I.CommandHandler     = new ScriptingCommandHandler();

            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
            I.Core.Log.Info             ("AshyScripting: Initialization successful");
            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Free()
        {
            Engine.I?.DestroyWorld      ();
            Engine.I                    = null;
            I                           = null;

            return                      ( EngineStatus.Free );
        }
    }
}
