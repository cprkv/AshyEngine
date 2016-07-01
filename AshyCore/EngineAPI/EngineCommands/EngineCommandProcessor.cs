using System.Collections.Generic;

namespace AshyCore.EngineAPI.EngineCommands
{
    public class ProxyCommandProcessor
    {
        #region Properties

        private Queue<IEngineCommand>   ActiveCommands { get; }

        private EngineProxy             Proxy { get; }

        #endregion


        #region Constructors

        public ProxyCommandProcessor(EngineProxy proxy)
        {
            Proxy                   = proxy;
            ActiveCommands          = new Queue<IEngineCommand>();
        }

        #endregion


        #region Methods

        public void AddCommand(IEngineCommand c)
        {
            ActiveCommands.Enqueue  ( c );
        }

        public EngineCommandResult ProcessAllCommands()
        {
            if (ActiveCommands.Count == 0)
                return              ( EngineCommandResult.Success );

            if (!Proxy.CheckAllInitialized)
                return              ( EngineCommandResult.Failed );

            EngineCommandResult res = EngineCommandResult.Success;

            foreach (var c in ActiveCommands)
            {
                res                 = Proxy.Core.CommandHandler.Execute(c).Worst(res);
                if (res == EngineCommandResult.CriticalFailed)
                    break;

                res                 = Proxy.Game.CommandHandler.Execute(c).Worst(res);
                if (res == EngineCommandResult.CriticalFailed)
                    break;

                res                 = Proxy.Physics.CommandHandler.Execute(c).Worst(res);
                if (res == EngineCommandResult.CriticalFailed)
                    break;

                res                 = Proxy.Script.CommandHandler.Execute(c).Worst(res);
                if (res == EngineCommandResult.CriticalFailed)
                    break;

                res                 = Proxy.Render.CommandHandler.Execute(c).Worst(res);
                if (res == EngineCommandResult.CriticalFailed)
                    break;
            }
            ActiveCommands.Clear    ();

            return                  ( res );
        }

        #endregion
    }
}
