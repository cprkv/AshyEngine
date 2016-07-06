//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using AshyCore.Debug;
using AshyCore.EngineAPI.EngineCommands;
using AshyCore.EntitySystem;
using NLua;

namespace AshyScripting.ScriptingCommands
{
    #region Helper

    internal static class LevelCmdHelper
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
    }

    #endregion


    internal class AddEntity : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            var aec                     = (AshyCore.EngineAPI.EngineCommands.AddEntity) c;
            return                      ( LevelCmdHelper.InitEntity(aec.Entity) );
        }
    }

    internal class LoadLevel : IEngineCommandHandler
    {
        EngineCommandResult IEngineCommandHandler.Execute(IEngineCommand c)
        {
            var ll                      = (AshyCore.EngineAPI.EngineCommands.LoadLevel) c;
            var res                     = EngineCommandResult.Success;

            Engine.I.CreateWorld       ();

            foreach (var entity in ll.LoadingLevel.Entities)
            {
                res                     = LevelCmdHelper.InitEntity(entity).Worst(res);
            }

            return                      ( EngineCommandResult.Success );
        }
    }

    internal class DestroyLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            Engine.I.DestroyWorld       ();
            Memory.Collect              ( showLog: true );

            return                      ( EngineCommandResult.Success ); 
        }
    }

    internal static class ChangeLevel
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.ChangeLevel c)
        {
            Engine.I.DestroyWorld       ();
            Engine.I.CreateWorld        ();

            foreach (var entity in c.LoadingLevel.Entities)
            {
                LevelCmdHelper.InitEntity( entity );
            }

            return                      ( EngineCommandResult.Success );
        }
    }
}