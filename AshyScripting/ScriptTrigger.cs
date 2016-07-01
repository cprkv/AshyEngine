// 
// Created : 05.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Linq;
using AshyCore;
using AshyCore.EntityManagement;
using AshyCore.EntitySystem;
using NLua;
using Lua = NLua.Lua;

namespace AshyScripting
{
    /// <summary>
    /// Script trigger for the specified entities.
    /// </summary>
    /// <seealso cref="AshyCore.EntityManagement.ITrigger" />
    public class ScriptTrigger : ITrigger
    {
        #region Properties

        public Action<Entity>   OnInAction  => e => _onInActionLua.Call(e);
        public Action<Entity>   OnOutAction => e => _onOutActionLua.Call(e);
        public IZone            Zone { get; }

        private readonly LuaFunction                _onInActionLua;
        private readonly LuaFunction                _onOutActionLua;
        private readonly IEnumerable<Entity>        _entities;
        private readonly Dictionary<Entity, bool>   _inside;

        #endregion


        #region Constructors

        public ScriptTrigger(Lua luaState, IZone zone, Script tringgerScript, IEnumerable<Entity> entities)
        {
            Zone                    = zone;
            luaState.DoString       ( tringgerScript.Text );
            _onInActionLua          = luaState["OnInAction"]  as LuaFunction;
            _onOutActionLua         = luaState["OnOutAction"] as LuaFunction;
            _entities               = entities;
            _inside                 = _entities.ToDictionary(x => x, zone.IsInside);

            ScriptingAPI.I.Core.Log.Info($"[Script] Loaded trigger script \"{tringgerScript.Path}.lua\".");
        }

        #endregion


        #region Methods

        /// <summary>
        /// Accepts the trigger to the entities if they came in the zone, or came out of the zone.
        /// </summary>
        public virtual void AcceptTrigger()
        {
            foreach (var e in _entities)
            {
                if (!_inside[e] && Zone.IsInside(e))
                {
                    OnInAction      (e);
                    _inside[e]      = true;
                }
                if (_inside[e] && !Zone.IsInside(e))
                {
                    OnOutAction     (e);
                    _inside[e]      = false;
                }
            }
        }

        #endregion
    }

 
    /// <summary>
    /// Script trigger only for the specified entity.
    /// </summary>
    /// <seealso cref="AshyScripting.ScriptTrigger" />
    public class PrivateScriptTrigger : ScriptTrigger
    {
        #region Constructors

        public PrivateScriptTrigger(Lua luaState, IZone zone, Script tringgerScript, Entity entity)
            : base( luaState, zone, tringgerScript, new []{ entity } )
        {
        }

        #endregion
    }
}