using AshyCore.EngineAPI;

namespace AshyGame
{
    internal class EngineAPIProxy
    {
        internal IEngineAPI Core         { get; }

        internal IEngineAPI Render       { get; }

        internal IEngineAPI Physics      { get; }

        internal IEngineAPI Scripting    { get; }

        internal IEngineAPI Game         { get; }

        internal EngineAPIProxy()
        {
            Core        = new AshyCore      .CoreAPI();
            Render      = new AshyRenderGL  .RenderAPI();
            Physics     = new AshyPhysics   .PhysicsAPI();
            Scripting   = new AshyScripting .ScriptingAPI();
            Game        = new AshyGame      .GameAPI();
        }
    }
}
