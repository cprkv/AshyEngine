using System;
using System.Collections.Generic;

namespace AshyCore.EngineAPI.EngineCommands
{
    public class ProxyCommandProcessor
    {
        #region Properties

        private Queue<IEngineCommand>   ActiveCommands { get; }
                                        
        private EngineProxy             Proxy { get; }

        private List<IEngineCommandHandler> _handlers;

        #endregion


        #region Constructors

        public ProxyCommandProcessor(EngineProxy proxy)
        {
            Proxy                   = proxy;
            ActiveCommands          = new Queue<IEngineCommand>();
            _handlers               = new List<IEngineCommandHandler>()
            {
                Proxy.Game          .CommandHandler,        // todo: whats about order ???
                Proxy.Physics       .CommandHandler,
                Proxy.Script        .CommandHandler,
                Proxy.Render        .CommandHandler,
                Proxy.Core          .CommandHandler,
            };
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

            var res                 = EngineCommandResult.Success;


            foreach (var c in ActiveCommands)
            {
                Proxy.Core.Log.Info ( "[Proxy Command Processor] Starting command: " + c.Type );
                res                 = ProcessCommand(c);
                if (res == EngineCommandResult.CriticalFailed)
                    break;
            }

            ActiveCommands.Clear    ();

            return                  ( res );
        }

        private EngineCommandResult ProcessCommand(IEngineCommand c)
        {
            var res                 = EngineCommandResult.Success;

            foreach (var h in _handlers)
            {
                res                 = h.Execute(c);
                Proxy.Core.Log.Info ( "[Proxy Command Processor]-- "+ h.GetType().Name +" -- Command status: " + res );
                if (res == EngineCommandResult.CriticalFailed)
                    break;
            }

            return                  ( res );
        }

        #endregion
    }
}
