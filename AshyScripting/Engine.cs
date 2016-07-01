using AshyCore.EngineAPI;
using System;
using System.Collections.Generic;
using AshyCore.EntityManagement;
using AshyCore.EntitySystem;
using NLua;
using AshyCommon;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyScripting
{
    public class Engine : IScriptingEngine
    {
        #region Internal Usage

        internal static Engine                      I { get; set; }

        internal Lua                                LuaState { get; set; }

        internal Dictionary<Entity, LuaFunction>    UpdateFunctions { get; set; }

        internal List<ScriptTrigger>                Triggers { get; set; }

        internal void CreateWorld()
        {
            LuaState                    = new Lua();
            Triggers                    = new List<ScriptTrigger>();
            UpdateFunctions             = new Dictionary<Entity, LuaFunction>();

            LuaState.LoadCLRPackage     ();
            LuaState.DoString(                                     
                @" import               ('AshyCommon', 'AshyCommon.Math')
                   import               ('AshyCore',   'AshyCore')
                   import               ('AshyCore',   'AshyCore.Entity')"
                );
        }

        internal void DestroyWorld()
        {
            UpdateFunctions             = null;
            Triggers                    = null;
            LuaState?.Dispose           ();
            LuaState                    = null;
        }

        #endregion


        #region IScriptingEngine

        public EngineStatus             Status { get; internal set; }

        public IEngineCommandHandler    CommandHandler { get; internal set; }

        public ITrigger AttachTrigger(IZone zone, Script trigger, Entity e)
        {
            var scriptTrigger           = new PrivateScriptTrigger(LuaState, zone, trigger, e);
            Triggers.Add                ( scriptTrigger );

            return                      ( scriptTrigger );
        }

        public ITrigger AttachTrigger(IZone zone, Script trigger, IEnumerable<Entity> entities)
        {
            var scriptTrigger           = new ScriptTrigger(LuaState, zone, trigger, entities);
            Triggers.Add                ( scriptTrigger );

            return                      ( scriptTrigger );
        }

        public void Tick(float dtime)
        {
            Triggers.ForEach            ( x => x.AcceptTrigger() );
            UpdateFunctions.ForEach     ( f => f.Value.Call(f.Key, dtime) );
        }

        #endregion
    }
}
