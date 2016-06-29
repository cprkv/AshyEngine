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

        public EngineStatus         Status { get; set; }

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

        #endregion
    }
}