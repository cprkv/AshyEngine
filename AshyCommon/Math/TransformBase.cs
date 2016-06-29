// 
// Created : 04.04.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;

namespace AshyCommon.Math
{
    public struct TransformBase : ITransformBase
    {
        #region Properties

        [Obsolete("Not used", true)]
        public Mat4 Matrix { get; set; }

        public Quat Rotation { get; set; }
        public Vec3 Scale { get; set; }
        public Vec3 Position { get; set; }

        public Mat4 ResultMatrix => Transform.Build(Position, Rotation, Scale);

        #endregion


        #region Constructors

        public TransformBase(Quat rotation, Vec3 scale, Vec3 position) : this()
        {
            Rotation = rotation;
            Scale = scale;
            Position = position;
        }

        public TransformBase(Vec3 rotation, Vec3 scale, Vec3 position) : this()
        {
            Rotation = Transform.FromEulerAnglesQuat(rotation);
            Scale = scale;
            Position = position;
        }

        #endregion
    }
}