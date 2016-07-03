using System.Collections.Generic;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyCore.CoreCommands
{
    internal class CoreCommandHandler : EngineCommandHandlerBase
    {
        public CoreCommandHandler()
        {
            Executers = new Dictionary<EngineCommandType, IEngineCommandHandler>()
            {
                { EngineCommandType.LoadLevel,      new LoadLevel() },
                { EngineCommandType.DestroyLevel,   new DestroyLevel() },
            };
        }
    }
}
