using System;
using AshyCore.EngineAPI;

namespace AshyRenderGL
{
    public class Engine : IRenderEngine
    {
        internal static Engine I { get; set; }

        public EngineStatus Status { get; set; }

        public void Tick(float dtime)
        {
            throw new NotImplementedException();
        }
    }
}
