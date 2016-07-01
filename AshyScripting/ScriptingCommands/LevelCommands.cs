//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCore;
using AshyCore.EngineAPI.EngineCommands;
using AshyCore.EntitySystem;
using KeraLua;
using NLua;

namespace AshyScripting.ScriptingCommands
{
    internal static class AddEntity
    {
        internal static EngineCommandResult InitEntity(Entity entity)
        {
            if ( ! entity.HasComponent(ComponentType.Script) || Engine.I.UpdateFunctions.ContainsKey(entity))
                return                  ( EngineCommandResult.Success );

            var script                  = entity.Get<ScriptComponent>(ComponentType.Script).Script;

            Engine.I.LuaState.DoString  ( script.Text );
            Engine.I.UpdateFunctions.Add( entity, Engine.I.LuaState["Update"] as LuaFunction );

            ScriptingAPI.I.Core.Log.Info( $"[Script] Loaded script \"{script.Path}.lua\"." );

            return                      ( EngineCommandResult.Success );
        }

        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.AddEntity c)
        {
            if ( ! ScriptingAPI.I.CheckAllInitialized || c == null)
                return                  ( EngineCommandResult.Failed );

            AddEntity.InitEntity        ( c.Entity );

            return                      ( EngineCommandResult.Success );
        }
    }

    internal static class LoadLevel
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.LoadLevel c)
        {
            if ( ! ScriptingAPI.I.CheckAllInitialized || c == null )
                return                  ( EngineCommandResult.Failed );

            foreach (var entity in c.LoadingLevel.Entities)
            {
                AddEntity.InitEntity    ( entity );
            }

            return                      ( EngineCommandResult.Success );
        }
    }

    internal static class DestroyLevel
    {
        internal static EngineCommandResult Process()
        {
            if ( ! CoreAPI.I.CheckAllInitialized )
                return                  ( EngineCommandResult.Failed );

            Engine.I.DestroyWorld       ();

            return                      ( EngineCommandResult.Success ); 
        }
    }

    internal static class ChangeLevel
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.ChangeLevel c)
        {
            if ( ! CoreAPI.I.CheckAllInitialized || c == null )
                return                  ( EngineCommandResult.Failed );

            Engine.I.DestroyWorld       ();
            Engine.I.CreateWorld        ();

            foreach (var entity in c.LoadingLevel.Entities)
            {
                AddEntity.InitEntity    ( entity );
            }

            return                      ( EngineCommandResult.Success );
        }
    }
}