using System;
using AshyCore.Debug;
using AshyCore.EngineAPI;
using AshyGame.GameCommands;

namespace AshyGame
{
    internal class GameAPI : IEngineAPI
    {
        public static EngineProxy I { get; private set; }

        public EngineStatus Preinitialize(EngineProxy baseEngine)
        {
            I                       = baseEngine;
            Engine.I                = baseEngine.Game as Engine;
            if (Engine.I == null) 
                return              ( EngineStatus.CriticalFailed );

            Engine.I.CommandHandler = new GameCommandHandler();

            return                  ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
            throw new NotImplementedException();
        }

        public EngineStatus Free()
        {
            Engine.I                = null;
            I                       = null;

            return                  ( EngineStatus.Free );
        }
    }
}
