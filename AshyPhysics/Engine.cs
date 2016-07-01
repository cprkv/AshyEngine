//   
// Created : 26.06.2016
// Author  : veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using AshyCore.EngineAPI;
using AshyCore.EntitySystem;
using System.Collections.Generic;
using System.Diagnostics;
using AshyCore;
using AshyCore.EngineAPI.EngineCommands;
using BulletSharp;
using BulletSharp.Math;

namespace AshyPhysics
{
    public class Engine : IPhysicsEngine
    {
        #region Internal Usage

        internal static Engine                      I { get; set; }

        internal DiscreteDynamicsWorld              World { get; set; }
        
        internal BroadphaseInterface                BroadPhase { get; set; }
        
        internal DefaultCollisionConfiguration      Collisions { get; set; }

        internal CollisionDispatcher                Dispatcher { get; set; }

        internal SequentialImpulseConstraintSolver  Solver { get; set; }

        internal List<CollisionShape>               CollisionShapes { get; set; }

        internal Dictionary<Entity, RigidBody>      Objects { get; set; }

        #endregion


        #region Implementation of IPhysicsEngine

        public EngineStatus                         Status { get; set; }

        public IEngineCommandHandler                CommandHandler { get; internal set; }

        public ICharacterPhysics RegisterCharacter(Entity e)
        {
            var character           = new CharacterPhysics(e);
            World.AddRigidBody      ( character.Body );
            Objects.Add             ( e, character.Body );

            return                  ( character );
        }

        public void Tick(float dtime)
        {
            World.StepSimulation    ( dtime, maxSubSteps: 30 );
        }

        internal void DestroyWorld()
        {
            CollisionShapes         = null;
            BroadPhase              = null;
            Collisions              = null;
            Dispatcher              = null;
            Solver                  = null;
            Objects                 = null;
            World                   = null;
        }

        internal void CreateWorld()
        {
            CollisionShapes         = new List<CollisionShape>();
            BroadPhase              = new DbvtBroadphase();
            Collisions              = new DefaultCollisionConfiguration();
            Dispatcher              = new CollisionDispatcher(Collisions);
            Solver                  = new SequentialImpulseConstraintSolver();
            Objects                 = new Dictionary<Entity, RigidBody>();

            World                   = new DiscreteDynamicsWorld(Dispatcher, 
                                                                BroadPhase, 
                                                                Solver, 
                                                                Collisions)
            {
                Gravity             = new Vector3(0.0f, -9.8f, 0.0f)
            };
        }

        #endregion
    }
}