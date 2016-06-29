// 
// Created : 18.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using AshyCommon.Math;
using AshyCore.EntitySystem;

namespace AshyCore
{
    [Obsolete("Not used anymore", true)]
    public struct Vec3States
    {
        public Vec3 Old { get; set; }
        public Vec3 New { get; set; }

        public Vec3States(Vec3 @new, Vec3 old)
        {
            New = @new;
            Old = old;
        }
    }

    [Obsolete("Not used anymore", true)]
    public struct CameraPlane
    {
        public Vec3 Dir { get; }
        public Vec3 Right { get; }

        public CameraPlane(Vec3 dir, Vec3 right)
        {
            Dir = dir;
            Right = right;
        }
    }

    [Obsolete("Not used anymore", true)]
    public delegate CameraPlane DirectionSolver(Vec3States dir, Vec3States right);

    /// <summary>
    /// Represents camera pinhole for 3D viewer.
    /// </summary>
    public class Camera
    {
        #region Data

        public const float MovementScale = 1.0f;

        float Angle = 60.0f;
        float ZFar = 400.0f;
        float ZNear = 1.5f;
        private float _angleY = 0;
        private readonly float _maxAngle = 80f.ToRadians();

        public Vec3 Eye;
        public Vec3 Dir;
        public Vec3 Up;
        public Vec3 Right;
        public float Width;
        public float Height;

        float AspectRatio => Width / Height;
        float AngleInRadiance => Angle.ToRadians();
        public Mat4 View => Transform.LookAt(Eye, Eye + Dir, Up);
        public Mat4 Proj => Transform.PerspectiveFOV(AngleInRadiance, AspectRatio, ZNear, ZFar);
        public Mat4 ViewProj => View * Proj;
        public float InterpolationDeg;
        public BezierCurve BezierCurve;

        #endregion


        #region Constructors

        public Camera()
        {
            Eye = Vec3.Zero;
            Dir = new Vec3(0.0f, 0.0f, 1.0f);
            Up = new Vec3(0.0f, 1.0f, 0.0f);

            //Engine.Instance.AddFrameIteration((sender, f) =>
            //{
            //    Engine.Instance.Level.Player.Camera.InterpolationDeg += 0.001f;
            //    if (Engine.Instance.Level.Player.Camera.InterpolationDeg > 1)
            //        Engine.Instance.Level.Player.Camera.InterpolationDeg = 0;
            //    Engine.Instance.Level.Player.Camera.Eye = BezierCurve.Interpolate(Engine.Instance.Level.Player.Camera.InterpolationDeg);
            //});

            //Func<Vec3, Vec3> s = x =>
            //{
            //    var t = x.Z;
            //    x.Z = x.Y;
            //    x.Y = t;
            //    return x;
            //};

            //var nodes = new List<BezierCurve.Segment>()
            //{
            //    new BezierCurve.Segment(
            //        s(new Vec3(100.53f, 215.14f, 56.80f)),
            //        s(new Vec3(79.77f, 123.19f,  9.0534f)),
            //        s(new Vec3(98.59f, 184.501f, 41.549f)),
            //        s(new Vec3(98, 174, 17))),

            //    new BezierCurve.Segment(
            //        s(new Vec3(79.77f, 123.19f, 9.0534f)),
            //        s(new Vec3(10.86f, 75.62f, 3.919f)),
            //        s(new Vec3(68.5f, 92, 4.2f)),
            //        s(new Vec3(24.56f, 103.406f, 7.402f))),

            //    new BezierCurve.Segment(
            //        s(new Vec3(10.86f, 75.62f, 3.919f)),
            //        s(new Vec3(-4.76f, -15.6333f, 4.8115f)),
            //        s(new Vec3(-1.4198f, -50.72926f, 0.49342f)),
            //        s(new Vec3(-4.8484f, -4.20976f, 4.64704f))),
            //};

            //BezierCurve = new BezierCurve(nodes, 10);

            //DEBUG DRAW CURVE
            //var i = 4;

            //var entity = Engine.Instance.Level.GetEntity("sphere_0004");
            //foreach (var p in nodes.SelectMany(segment => segment.Defuse(1)))
            //{
            //    Engine.Instance.Level.Spawn(
            //        new Entity(entity,
            //            $"sphere_000{++i}",
            //            p,
            //            entity.Scale,
            //            entity.Rotation.ToEulerAngles()));
            //}
        }

        #endregion


        #region Public methods

        public void Move(Vec2 delta)
        {
            Eye += Right * delta.X * MovementScale;
            Eye += Dir * delta.Y * MovementScale;
        }

        public void Rotate(Vec2 delta)
        {
            Right = Dir.Cross(Up).Norm();

            _angleY = (_angleY + delta.Y * 0.4f).Clamp(0, 1.0f); // angle  right rotaton from 0 to 1

            Quat roty = Transform.FromAxisAngleQuat(Right, -_maxAngle).SLerp(
                Transform.FromAxisAngleQuat(Right, _maxAngle),
                _angleY
                );
            Quat rotx = Transform.FromAxisAngleQuat(Vec3.UnitY, delta.X);

            Dir.Y = 0;
            Dir = (rotx * roty * new Quat(0, Dir) * (rotx * roty).Conj).V.Norm();
        }

        #endregion
    }
}