//  
// Created  : 27.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Collections.Generic;
using System.Diagnostics;
using AshyCore;
using AshyCore.EntitySystem;
using BulletSharp;
using BulletSharp.Math;

namespace AshyPhysics
{
    // todo перенести в Engine
    //public class PhysicsModule : IPhysicsModule
    //{
    //    #region Properties

    //    public string Name => "Physics";

    //    public DiscreteDynamicsWorld World { get; private set; }
    //    public BroadphaseInterface BroadPhase { get; private set; }
    //    public DefaultCollisionConfiguration Collisions { get; private set; }
    //    public CollisionDispatcher Dispatcher { get; private set; }
    //    public SequentialImpulseConstraintSolver Solver { get; private set; }
    //    public List<CollisionShape> CollisionShapes { get; private set; }

    //    public Dictionary<Entity, RigidBody> Objects { get; } = new Dictionary<Entity, RigidBody>();

    //    #endregion


    //    #region Realisation IPhysicsModule

    //    public void Start()
    //    {
    //        var timer = new Stopwatch(); timer.Start();
    //        CollisionShapes = new List<CollisionShape>();
    //        BroadPhase = new DbvtBroadphase();
    //        Collisions = new DefaultCollisionConfiguration();
    //        Dispatcher = new CollisionDispatcher(Collisions);
    //        Solver = new SequentialImpulseConstraintSolver();
    //        World = new DiscreteDynamicsWorld(Dispatcher, BroadPhase, Solver, Collisions)
    //        {
    //            Gravity = new Vector3(0.0f, -9.8f, 0.0f)
    //        };
    //        PhysicsAPI.Instance.Log.Info($"[Physics] {Objects.Count} dynamic objects and " +
    //                                 $"{World.NumCollisionObjects - Objects.Count} static objects were added.");
    //        PhysicsAPI.Instance.Log.Info($"[Physics] Started in {timer.Elapsed.TotalSeconds} sec.");
    //    }

    //    public void Update(float dtime)
    //    {
    //        World.StepSimulation(dtime, 30);
    //    }

    //    public void LoadLevel(GameLevel gameLevel)
    //    {
    //        gameLevel.Entities.ForEach(RegisterEntity);
    //    }

    //    public void FreeLevelData()
    //    {
            
    //    }

    //    public void RegisterEntity(Entity entity)
    //    {
    //        var physicsComponent = entity.Get<PhysicsComponent>(ComponentType.Physics);
    //        if (physicsComponent == null) return;
    //        var isStatic = physicsComponent.Physics == PhysicsType.Static;

    //        var collisionShape = isStatic
    //            ? (CollisionShape) new BvhTriangleMeshShape(physicsComponent.Mesh.Convert(), false)
    //            : (CollisionShape) new ConvexTriangleMeshShape(physicsComponent.Mesh.Convert());

    //        CollisionShapes.Add(collisionShape);

    //        Vector3 inertia;
    //        collisionShape.CalculateLocalInertia(1.0f, out inertia);
    //        var motionState     = new EntityMotionState(entity, physicsComponent.Motion == MotionType.Kinematic);
    //        var rigidBodyInfo   = new RigidBodyConstructionInfo(isStatic ? 0.0f : 1.0f, motionState, collisionShape, inertia);
    //        var rigidBody       = new RigidBody(rigidBodyInfo)
    //        {
    //            Friction = 1.0f,
    //            Gravity = isStatic ? Vector3.Zero : new Vector3(0f, -9.8f, 0f),
    //            CollisionFlags = isStatic
    //                ? CollisionFlags.StaticObject
    //                : physicsComponent.Motion == MotionType.Dynamic
    //                    ? CollisionFlags.None
    //                    : CollisionFlags.KinematicObject,
    //            ActivationState = physicsComponent.Motion == MotionType.Dynamic
    //                ? ActivationState.ActiveTag
    //                : ActivationState.DisableDeactivation
    //        };

    //        World.AddRigidBody(rigidBody);

    //        if (!isStatic && !Objects.ContainsKey(entity)) Objects.Add(entity, rigidBody);
    //    }

    //    public void End()
    //    {
    //        World.Dispose();
    //    }

    //    public ICharacterPhysics RegisterCharacter(Entity e)
    //    {
    //        return new CharacterPhysics(e, this);
    //    }

    //    #endregion
    //}
}