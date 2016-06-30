using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshyCore.EngineAPI.EngineCommands
{
    public class LoadLevel : IEngineCommand
    {
        public EngineCommandType Type => EngineCommandType.LoadLevel;

        public GameLevel LoadingLevel { get; } 

        public LoadLevel(string levelName)
        {
            // LoadingLevel = ... parse or load from world ...
        }
    }

    public class DestroyLevel : IEngineCommand
    {
        public EngineCommandType Type => EngineCommandType.DestroyLevel;

        public GameLevel DestroyingLevel { get; }

        public DestroyLevel()
        {
            DestroyingLevel = CoreAPI.I.Game.Level;
        }
    }

    public class ChangeLevel : IEngineCommand
    {
        public EngineCommandType Type => EngineCommandType.ChangeLevel;

        public GameLevel LoadingLevel { get; }

        public GameLevel DestroyingLevel { get; }

        public ChangeLevel(string levelName)
        {
            DestroyingLevel = CoreAPI.I.Game.Level;
            // LoadingLevel = ... parse or load from world ...
        }
    }
}
