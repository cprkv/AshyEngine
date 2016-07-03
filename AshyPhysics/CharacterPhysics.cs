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
    /// <summary>
    /// Represents movable character uses sphere for simulation.
    /// </summary>
    public class CharacterPhysics : ICharacterPhysics
    {
        #region Properties

        /// <summary>
        /// Center of character physical shape.
        /// </summary>
        public Vec3                 Center => MotionState.Transform.Position;

        public EntityMotionState    MotionState { get; }

        public RigidBody            Body { get; }

        public CollisionShape       CollisionShape { get; }

        /// <summary>
        /// <code>true</code>, if character not moving.
        /// </summary>
        private bool                _realised = true;

        #endregion


        #region Contructor

        /// <summary>
        /// Creates <see cref="CharacterPhysics"/>.
        /// </summary>
        /// <param name="c">Entity of character. No restrictions.</param>
        public CharacterPhysics(Entity c)
        {
            MotionState             = new EntityMotionState(c, false);
            CollisionShape          = new SphereShape(2.0f);
            Vector3 inertia;
            CollisionShape          .CalculateLocalInertia(50.0f, out inertia);

            Body = new RigidBody(new RigidBodyConstructionInfo(50.0f, MotionState, CollisionShape, inertia))
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

        public void SetForce(Vec3 force, int movingSpeed = 10)
        {
            var dir                 = force.Convert();
            dir.Y                   = 0;
            dir.Normalize           ();
            Body.LinearVelocity     = dir * movingSpeed;
        }

        public void StopForce()
        {
            if (_realised)          return;
            Body.LinearVelocity     = Vector3.Zero;
            _realised               = false;
        } 

        #endregion
    }
}