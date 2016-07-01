using AshyCore.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AshyCore.EntitySystem;

namespace AshyCore.EngineAPI.EngineCommands
{
    #region LoadLevel

    public class LoadLevel : IEngineCommand
    {
        public EngineCommandType Type => EngineCommandType.LoadLevel;

        public GameLevel LoadingLevel { get; }

        public LoadLevel(string levelName)
        {
            var confLevel = Engine.I.RM.Get<ConfigTable>($"Config/Levels/{levelName}", ResourceTarget.World);
            LoadingLevel = new GameLevel(confLevel);
        }
    }

    #endregion


    #region DestroyLevel

    public class DestroyLevel : IEngineCommand
    {
        public EngineCommandType Type => EngineCommandType.DestroyLevel;

        public DestroyLevel()
        {
        }
    }

    #endregion


    #region ChangeLevel

    public class ChangeLevel : IEngineCommand
    {
        public EngineCommandType Type => EngineCommandType.ChangeLevel;

        public GameLevel LoadingLevel { get; }

        public GameLevel DestroyingLevel { get; }

        public ChangeLevel(string levelName)
        {
            DestroyingLevel = CoreAPI.I.Game.Level;
            var confLevel = Engine.I.RM.Get<ConfigTable>($"Config/Levels/{levelName}", ResourceTarget.World);
            LoadingLevel = new GameLevel(confLevel);
        }
    }

    #endregion


    #region AddEntity

    public class AddEntity : IEngineCommand
    {
        public EngineCommandType Type => EngineCommandType.AddEntity;

        public Entity Entity { get; }

        public AddEntity(Entity entity)
        {
            Entity = entity;
        }
    }

    #endregion
}
