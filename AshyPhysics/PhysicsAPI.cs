using AshyCore.EngineAPI;
using BulletSharp;
using BulletSharp.Math;
using System.Collections.Generic;

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

            Engine.I.CollisionShapes    = new List<CollisionShape>();
            Engine.I.BroadPhase         = new DbvtBroadphase();
            Engine.I.Collisions         = new DefaultCollisionConfiguration();
            Engine.I.Dispatcher         = new CollisionDispatcher(Engine.I.Collisions);
            Engine.I.Solver             = new SequentialImpulseConstraintSolver();
            Engine.I.Objects            = new Dictionary<AshyCore.EntitySystem.Entity, RigidBody>();

            return                      ( EngineStatus.ReadyToLoad );
        }

        public EngineStatus Initialize()
        {
            Engine.I.World              = new DiscreteDynamicsWorld(Engine.I.Dispatcher, 
                                                                    Engine.I.BroadPhase, 
                                                                    Engine.I.Solver, 
                                                                    Engine.I.Collisions)
            {
                Gravity                 = new Vector3(0.0f, -9.8f, 0.0f)
            };

            return                      ( EngineStatus.ReadyToUse );
        }

        public EngineStatus Free()
        {
            I                           = null;

            Engine.I                    = null;
            return                      ( EngineStatus.Free );
        }
    }
}
