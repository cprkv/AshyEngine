//   
// Created : 01.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using AshyCore;
using AshyCore.Debug;
using AshyCore.EngineAPI;
using AshyCore.EngineAPI.EngineCommands;
using AshyCore.EntitySystem;
using BulletSharp;
using BulletSharp.Math;

namespace AshyPhysics.PhysicsCommands
{
    #region Helper

    internal static class LevelCmdHelper
    {
        internal static EngineCommandResult InitEntity(Entity entity)
        {
            try
            {
                var physicsComponent    = entity.Get<PhysicsComponent>(ComponentType.Physics);
                if (physicsComponent == null)
                    return              ( EngineCommandResult.Success );

                var isStatic            = physicsComponent.Physics == PhysicsType.Static;

                var collisionShape = isStatic
                    ? (CollisionShape)  new BvhTriangleMeshShape(physicsComponent.Mesh.Convert(), false)
                    : (CollisionShape)  new ConvexTriangleMeshShape(physicsComponent.Mesh.Convert());

                Engine.I.CollisionShapes.Add(collisionShape);

                Vector3 inertia;
                collisionShape.CalculateLocalInertia(1.0f, out inertia);

                var motionState         = new EntityMotionState(entity, physicsComponent.Motion == MotionType.Kinematic);
                var rigidBodyInfo       = new RigidBodyConstructionInfo(isStatic ? 0.0f : 1.0f, motionState, collisionShape, inertia);
                var rigidBody           = new RigidBody(rigidBodyInfo)
                {
                    Friction = 1.0f,
                    Gravity  = isStatic 
                        ?               Vector3.Zero 
                        :               new Vector3(0.0f, -9.8f, 0.0f),
                    CollisionFlags = isStatic
                        ?               CollisionFlags.StaticObject
                        :               physicsComponent.Motion == MotionType.Dynamic
                            ?               CollisionFlags.None
                            :               CollisionFlags.KinematicObject,
                    ActivationState = physicsComponent.Motion == MotionType.Dynamic
                        ?               ActivationState.ActiveTag
                        :               ActivationState.DisableDeactivation
                };

                Engine.I.World.AddRigidBody( rigidBody );

                if (!isStatic && !Engine.I.Objects.ContainsKey(entity))
                {
                    Engine.I.Objects.Add( entity, rigidBody );
                }
            }
            catch (Exception e)
            {
                return                  ( EngineCommandResult.Failed );
            }

            return                      ( EngineCommandResult.Success );
        }
    }

    #endregion


    internal class AddEntity : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            var aec                     = (AshyCore.EngineAPI.EngineCommands.AddEntity) c;
            return                      ( LevelCmdHelper.InitEntity(aec.Entity) );
        }
    }

    internal class LoadLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            var ll                      = (AshyCore.EngineAPI.EngineCommands.LoadLevel) c;
            var res                     = EngineCommandResult.Success;

            Engine.I.CreateWorld        ();

            foreach (var entity in ll.LoadingLevel.Entities)
            {
                res                     = LevelCmdHelper.InitEntity(entity).Worst(res);
            }

            return                      ( res );
        }
    }

    internal class DestroyLevel : IEngineCommandHandler
    {
        public EngineCommandResult Execute(IEngineCommand c)
        {
            Engine.I.DestroyWorld       ();
            Memory.Collect              ( showLog: true );

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
            Memory.Collect              ( showLog: true );
            Engine.I.CreateWorld        ();

            foreach (var entity in c.LoadingLevel.Entities)
            {
                LevelCmdHelper.InitEntity    ( entity );
            }

            return                      ( EngineCommandResult.Success );
        }
    }
}