using System;
using System.Collections.Generic;
using AshyCore.EngineAPI;
using AshyCore.EntitySystem;
using NLua;

namespace AshyScripting
{
    public class ScriptingAPI : IEngineAPI
    {
        public static EngineProxy I { get; private set; }
        
        public EngineStatus Preinitialize(EngineProxy baseEngine)
        {
            I                       = baseEngine;
            Engine.I                = I.Script as Engine;
            if (Engine.I == null) 
                return              ( EngineStatus.CriticalFailed );

            Engine.I.LuaState           = new Lua();
            Engine.I.Triggers           = new List<ScriptTrigger>();
            Engine.I.UpdateFunctions    = new Dictionary<Entity, LuaFunction>();

            return                  ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
            I.Core.Log.Info             ("--- Scripting Initialization ---");

            Engine.I.LuaState.LoadCLRPackage();
            Engine.I.LuaState.DoString(                                     // todo: check this list
                @" import               ('AshyCommon', 'AshyCommon.Math')
                   import               ('AshyCore',   'AshyCore')
                   import               ('AshyCore',   'AshyCore.Entity')"
                );

            I.Core.Log.Info             ("Scripting Initialization successful");
            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Free()
        {
            Engine.I.UpdateFunctions    = null;
            Engine.I.Triggers           = null;
            Engine.I.LuaState.Dispose   ();
            Engine.I.LuaState           = null;
            Engine.I                    = null;
            I                           = null;

            return                      ( EngineStatus.Free );
        }
    }
}
