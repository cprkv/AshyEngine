//  
// Created  : 26.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Collections.Generic;
using System.Diagnostics;
using AshyCommon;
using AshyCore;
using AshyCore.EntityManagement;
using AshyCore.EntitySystem;
using NLua;
using Lua = NLua.Lua;

namespace AshyScripting
{
    // todo move to engine
    //public class ScriptModule : IScriptingModule
    //{
    //    #region Properties

    //    public string Name => "Scripting";
    //    private Lua LuaState { get; } = new Lua();
    //    private Dictionary<Entity, LuaFunction> UpdateFunctions { get; } = new Dictionary<Entity, LuaFunction>();
    //    private readonly List<ScriptTrigger> _triggers = new List<ScriptTrigger>();

    //    #endregion


    //    #region Public methods

    //    public void Start()
    //    {
    //        var timer = new Stopwatch(); timer.Start();
    //        LuaState.LoadCLRPackage();
    //        LuaState.DoString(@" import ('AshyCommon', 'AshyCommon.Math')
    //                             import ('AshyCore', 'AshyCore')
    //                             import ('AshyCore', 'AshyCore.Entity')");

    //        ScriptingAPI.Instance.Log.Info($"[Script] Started in {timer.Elapsed.TotalSeconds} sec.");
    //    }

    //    public void Update(float dtime)
    //    {
    //        _triggers.ForEach(x => x.AcceptTrigger());
    //        UpdateFunctions.ForEach(f => f.Value.Call(f.Key, dtime));
    //    }

    //    public void LoadLevel(GameLevel gameLevel)
    //    {
    //        gameLevel.Entities.ForEach(RegisterEntity);
    //    }

    //    public void FreeLevelData()
    //    {

    //    }

    //    public void RegisterEntity(Entity entity)
    //    {
    //        if (!entity.HasComponent(ComponentType.Script) || UpdateFunctions.ContainsKey(entity))
    //        {
    //            return;
    //        }
    //        var script = entity.Get<ScriptComponent>(ComponentType.Script).Script;
    //        LuaState.DoString(script.Text);
    //        UpdateFunctions.Add(entity, LuaState["Update"] as LuaFunction);
    //        ScriptingAPI.Instance.Log.Info($"[Script] Loaded script \"{script.Path}.lua\".");
    //    }

    //    public void End()
    //    {
    //        LuaState.Dispose();
    //        ScriptingAPI.Instance.Log.Info("[Script] Ended.");
    //    }

    //    public ITrigger AttachTrigger(IZone zone, Script trigger, IEnumerable<Entity> entities)
    //    {
    //        var scriptTrigger = new ScriptTrigger(LuaState, zone, trigger, entities);
    //        _triggers.Add(scriptTrigger);
    //        return scriptTrigger;
    //    }

    //    public ITrigger AttachTrigger(IZone zone, Script trigger, Entity e)
    //    {
    //        var scriptTrigger = new PrivateScriptTrigger(LuaState, zone, trigger, e);
    //        _triggers.Add(scriptTrigger);
    //        return scriptTrigger;
    //    }

    //    #endregion
    //}
}