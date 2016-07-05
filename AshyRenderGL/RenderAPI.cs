using System;
using AshyCore.EngineAPI;
using AshyRenderGL.RenderCommands;
using AshyRenderGL.Techniques.GL4;

namespace AshyRenderGL
{
    public class RenderAPI : IEngineAPI
    {
        public static EngineProxy I { get; private set; }

        public EngineStatus Preinitialize(EngineProxy baseEngine)
        {
            I                           = baseEngine;
            Engine.I                    = baseEngine.Render as Engine;
            if (Engine.I == null) 
                return                  ( EngineStatus.CriticalFailed );

            Engine.I.CommandHandler     = new RenderCommandHandler();

            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
            Engine.I.GameWindow         = new Window();
            Engine.I.Device             = new Device();
            Engine.I.RenderTechnique    = new GL4Technique();

            I.Core.Log.Info             ( "AshyRender: Initialization successful" );
            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Free()
        {
            Engine.I                    = null;
            I                           = null;

            return                      ( EngineStatus.Free );
        }
    }
}
