using System;
using AshyCore.EngineAPI;

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


            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
            throw new NotImplementedException();
        }
        public EngineStatus Free()
        {
            Engine.I                    = null;
            I                           = null;

            return                      ( EngineStatus.Free );
        }
    }
}
