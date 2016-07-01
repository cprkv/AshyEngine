using System;
using AshyCore;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyGame
{
    internal class Engine : IGameEngine
    {
        internal static Engine          I { get; set; }

        public GameLevel                Level { get; internal set; }

        public EngineStatus             Status { get; internal set; }

        //public World                    World { get; internal set; }

        public IEngineCommandHandler    CommandHandler { get; internal set; }

        public void Tick(float dtime)
        {
            throw new NotImplementedException();
        }
    }
}
