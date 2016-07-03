using System;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyRenderGL
{
    public class Engine : IRenderEngine
    {
        internal static Engine I { get; set; }

        public IEngineCommandHandler    CommandHandler { get; internal set; }

        public EngineStatus             Status { get; internal set; }

        public void Tick(float dtime)
        {
            throw new NotImplementedException();
        }
    }
}
