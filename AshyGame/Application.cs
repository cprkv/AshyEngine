using System.Diagnostics;
using System.Linq;
using AshyGame;
using AshyCore.EngineAPI;
using AshyCore.Debug;
using AshyCore.EngineAPI.EngineCommands;

namespace AshyGame
{
    internal class Application
    {
        private static readonly EngineAPIProxy Proxy = new EngineAPIProxy();

        private static EngineUser _user;

        internal static EngineStatus Preinitialize(string[] cmdArgs)
        {
            _user                       = new EngineUser(cmdArgs);
            EngineStatus[] status       = new EngineStatus[5];

            status[0] = Proxy.Core      .Preinitialize(_user);
            status[4] = Proxy.Game      .Preinitialize(_user); // todo: order??
            status[1] = Proxy.Physics   .Preinitialize(_user);
            status[2] = Proxy.Scripting .Preinitialize(_user);
            status[3] = Proxy.Render    .Preinitialize(_user);

            return                      status.All(s => s > EngineStatus.Failed) 
                                            ? ( EngineStatus.ReadyToLoad ) 
                                            : ( EngineStatus.CriticalFailed );
        }

        internal static EngineStatus Initialize()
        {
            EngineStatus[] status       = new EngineStatus[5];

            status[0] = Proxy.Core      .Initialize();
            status[4] = Proxy.Game      .Initialize();
            status[1] = Proxy.Physics   .Initialize();
            status[2] = Proxy.Scripting .Initialize(); // todo: order??
            status[3] = Proxy.Render    .Initialize();

            _user.Initialize            ();

            if (!status.Any(s => s == EngineStatus.CriticalFailed))
            {
                GameAPI.I.Core.Log.Info ("[Game] All systems were loaded.");
                return                  ( EngineStatus.ReadyToUse );
            }

            Critical.NoThrow(()         => _user.Core.Log.Error("Application Initialization failed!"));

            return                      ( EngineStatus.Failed );
        }

        internal static void Execute()
        {
            _user.CommandProcessor.AddCommand(new LoadLevel("TestL02"));

            Stopwatch loopTimer;
            while ( ! _user.Render.GameWindow.IsExiting)
            {
                _user.Tick              ( 60 );
            }
        }

        internal static void Free()
        {
            Critical.NoThrow(()         => _user.Core.Log.Info("--- Application shutting down ---"));
            EngineStatus[] status       = new EngineStatus[5];

            status[4] = Proxy.Game      .Free();
            status[1] = Proxy.Physics   .Free();
            status[2] = Proxy.Scripting .Free();
            status[3] = Proxy.Render    .Free();
            status[0] = Proxy.Core      .Free();

            Critical.NoThrow(()         => Memory.Collect(false));
        }
    }
}
