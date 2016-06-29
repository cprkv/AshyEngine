// 
// Created : 03.06.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCommon.Math;
using AshyCore.EntitySystem;
using BulletSharp;
using BulletSharp.Math;

namespace AshyPhysics
{
    public class CharacterPhysics : ICharacterPhysics
    {
        #region Properties

        public Vec3                 Center => MotionState.Transform.Position;

        public EntityMotionState    MotionState { get; }

        public RigidBody            Body { get; }

        public CollisionShape       CollisionShape { get; }

        private bool                _realised = true;

        #endregion


        #region Contructor

        /// <summary>
        /// Creates <see cref="CharacterPhysics"/>.
        /// Binds all physics to <paramref name="m"/>.
        /// </summary>
        /// <param name="c">Entity of character. Should contain <see cref="GeomComponent"/>.</param>
        /// <param name="m">The m.</param> todo сделать нужные поля внутри physics engine
        public CharacterPhysics(Entity c)
        {
            MotionState             = new EntityMotionState(c, false);
            CollisionShape          = new SphereShape(2.0f);
            Vector3 inertia;
            CollisionShape          .CalculateLocalInertia(50.0f, out inertia);

            Body = new RigidBody    (new RigidBodyConstructionInfo(50.0f, MotionState, CollisionShape, inertia))
            {
                Friction            = 2.5f,
                Gravity             = new Vector3(0f, -9.8f, 0f),
                CollisionFlags      = CollisionFlags.None,
                ActivationState     = ActivationState.ActiveTag,
                AngularFactor       = new Vector3(0, 0.0f, 0),
            };
            Body                    .SetSleepingThresholds(0, 0);
        }

        #endregion


        #region Methods

        public void SetForce(Vec3 a)
        {
            var v = (a.Norm() * 10).Convert(); v.Y = 0;
            Body.LinearVelocity = v;
        }

        public void StopForce()
        {
            if (_realised) return;
            Body.LinearVelocity = Vector3.Zero;
            _realised = false;
        } 

        #endregion
    }
}