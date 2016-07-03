// 
// Created : 04.04.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCommon.Math;
using AshyCore.EntitySystem;
using BulletSharp.Math;

namespace AshyPhysics
{
    public class EntityMotionState : BulletSharp.MotionState
    {
        #region Properties

        private Entity                  Entity { get; }

        public TransformBase            Transform => _transform;

        private TransformBase           _transform;

        #endregion


        #region Constructors

        public EntityMotionState(Entity entity, bool isKinematic)
        {
            Entity                      = entity;
            _transform                  = entity.Transform;
            if (isKinematic)
            {
                Entity.TransformEvent   += (o, tr) => _transform = tr;
            }
        }

        #endregion


        #region Methods

        public override void GetWorldTransform(out Matrix worldTrans)
        {
            worldTrans                  = Transform.ResultMatrix.Convert();
        }

        public override void SetWorldTransform(ref Matrix worldTrans)
        {
            Vector3     scale;
            Vector3     translation;
            Quaternion  rotation;
            worldTrans.Decompose        ( out scale, out rotation, out translation );
            _transform.Rotation         = rotation.Convert();
            _transform.Scale            = scale.Convert();
            _transform.Position         = translation.Convert();
            Entity.ForceSetTransform    ( Transform );
        }

        #endregion
    }
}