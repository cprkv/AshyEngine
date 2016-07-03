using AshyCore.EngineAPI;
using BulletSharp;
using BulletSharp.Math;
using System.Collections.Generic;
using AshyPhysics.PhysicsCommands;

namespace AshyPhysics
{
    public class PhysicsAPI : IEngineAPI
    {
        public static EngineProxy I { get; private set; }

        public EngineStatus Preinitialize(EngineProxy baseEngine)
        {
            I                           = baseEngine;
            Engine.I                    = baseEngine.Physics as Engine;
            if (Engine.I == null) 
                return                  ( EngineStatus.CriticalFailed );

            Engine.I.CommandHandler     = new PhysicsCommandHandler();

            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
            return                      ( EngineStatus.ReadyToUse );
        }

        public EngineStatus Free()
        {
            Engine.I.DestroyWorld       ();
            I                           = null;
            Engine.I                    = null;
            return                      ( EngineStatus.Free );
        }
    }
}
