using System;
using AshyCommon;
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

        public Device                   Device { get; internal set; }

        public void Tick(float dtime)
        {
            if (RenderTechnique.IsNotSet())
                return;
            
            if ( ! RenderTechnique.IsInitialized)
                return;

            RenderTechnique.Simulate    ( dtime );
            RenderTechnique.Render      ();
        }

    }
}
