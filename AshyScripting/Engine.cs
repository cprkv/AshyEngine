using AshyCore.EngineAPI;
using System;
using System.Collections.Generic;
using AshyCore.EntityManagement;
using AshyCore.EntitySystem;
using NLua;

namespace AshyScripting
{
    public class Engine : IScriptingEngine
    {
        #region Internal Usage

        internal static Engine I { get; set; }

        internal Lua LuaState { get; set; }

        internal Dictionary<Entity, LuaFunction> UpdateFunctions { get; set; }

        internal List<ScriptTrigger> Triggers { get; set; }

        #endregion


        #region IScriptingEngine

        public EngineStatus Status { get; internal set; }

        public ITrigger AttachTrigger(IZone zone, Script trigger, Entity e)
        {
            throw new NotImplementedException();
        }

        public ITrigger AttachTrigger(IZone zone, Script trigger, IEnumerable<Entity> entities)
        {
            throw new NotImplementedException();
        }

        public void Tick(float dtime)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
