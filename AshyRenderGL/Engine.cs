using System;
using AshyCore;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;
using AshyRenderGL.Techniques;

namespace AshyRenderGL
{
    public class Engine : IRenderEngine
    {
        internal static Engine I { get; set; }

        public IEngineCommandHandler    CommandHandler { get; internal set; }

        public EngineStatus             Status { get; internal set; }

        public IWindow                  GameWindow { get; internal set; }

        public RenderTechnique          RenderTechnique { get; internal set; }

        public void Tick(float dtime)
        {
            throw new NotImplementedException();
        }

    }
}
