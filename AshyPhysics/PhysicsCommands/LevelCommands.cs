//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using AshyCore;
using AshyCore.EngineAPI.EngineCommands;
using AshyCore.EntitySystem;
using BulletSharp;
using BulletSharp.Math;

namespace AshyPhysics.PhysicsCommands
{
    internal static class AddEntity
    {
        internal static EngineCommandResult InitEntity(Entity entity)
        {
            var physicsComponent        = entity.Get<PhysicsComponent>(ComponentType.Physics);
            if (physicsComponent == null)
                return                  ( EngineCommandResult.Success );

            var isStatic                = physicsComponent.Physics == PhysicsType.Static;

            var collisionShape = isStatic
                ? (CollisionShape)      new BvhTriangleMeshShape(physicsComponent.Mesh.Convert(), false)
                : (CollisionShape)      new ConvexTriangleMeshShape(physicsComponent.Mesh.Convert());

            Engine.I.CollisionShapes.Add(collisionShape);

            Vector3 inertia;
            collisionShape.CalculateLocalInertia(1.0f, out inertia);

            var motionState             = new EntityMotionState(entity, physicsComponent.Motion == MotionType.Kinematic);
            var rigidBodyInfo           = new RigidBodyConstructionInfo(isStatic ? 0.0f : 1.0f, motionState, collisionShape, inertia);
            var rigidBody               = new RigidBody(rigidBodyInfo)
            {
                Friction = 1.0f,
                Gravity  = isStatic 
                    ?                   Vector3.Zero 
                    :                   new Vector3(0.0f, -9.8f, 0.0f),
                CollisionFlags = isStatic
                    ?                   CollisionFlags.StaticObject
                    :                   physicsComponent.Motion == MotionType.Dynamic
                        ?                   CollisionFlags.None
                        :                   CollisionFlags.KinematicObject,
                ActivationState = physicsComponent.Motion == MotionType.Dynamic
                    ?                   ActivationState.ActiveTag
                    :                   ActivationState.DisableDeactivation
            };

            Engine.I.World.AddRigidBody ( rigidBody );

            if (!isStatic && !Engine.I.Objects.ContainsKey(entity))
            {
                Engine.I.Objects.Add    ( entity, rigidBody );
            }

            return                      ( EngineCommandResult.Success );
        }

        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.AddEntity c)
        {
            if ( ! PhysicsAPI.I.CheckAllInitialized || c == null)
                return                  ( EngineCommandResult.Failed );

            AddEntity.InitEntity        ( c.Entity );

            return                      ( EngineCommandResult.Success );
        }
    }

    internal static class LoadLevel
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.LoadLevel c)
        {
            if ( ! PhysicsAPI.I.CheckAllInitialized || c == null )
                return                  ( EngineCommandResult.Failed );

            foreach (var entity in c.LoadingLevel.Entities)
            {
                AddEntity.InitEntity    ( entity );
            }

            return                      ( EngineCommandResult.Success );
        }
    }

    internal static class DestroyLevel
    {
        internal static EngineCommandResult Process()
        {
            if ( ! CoreAPI.I.CheckAllInitialized )
                return                  ( EngineCommandResult.Failed );

            Engine.I.DestroyWorld       ();

            return                      ( EngineCommandResult.Success ); 
        }
    }

    internal static class ChangeLevel
    {
        internal static EngineCommandResult Process(AshyCore.EngineAPI.EngineCommands.ChangeLevel c)
        {
            if ( ! CoreAPI.I.CheckAllInitialized || c == null )
                return                  ( EngineCommandResult.Failed );

            Engine.I.DestroyWorld       ();
            Engine.I.CreateWorld        ();

            foreach (var entity in c.LoadingLevel.Entities)
            {
                AddEntity.InitEntity    ( entity );
            }

            return                      ( EngineCommandResult.Success );
        }
    }
}