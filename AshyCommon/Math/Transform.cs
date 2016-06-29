// 
// Created : 25.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;

namespace AshyCommon.Math
{
    /// <summary>
    /// Static class for create tranformations.
    /// </summary>
    public static class Transform
    {
        #region Matrix tranformation

        /// <summary>
        /// Build a look at matrix.
        /// </summary>
        /// <param name="eye">Position of viewer.</param>
        /// <param name="target">Position of viewer look to.</param>
        /// <param name="up">Up direction for (target-eye) vector</param>
        public static Mat4 LookAt(Vec3 eye, Vec3 target, Vec3 up)
        {
            Vec3 z = (eye - target).Norm();
            Vec3 x = Vec3.Cross(up, z).Norm();
            Vec3 y = Vec3.Cross(z, x).Norm();

            Mat4 result = Mat4.Identity;

            result[0, 0] = x.X;
            result[0, 1] = y.X;
            result[0, 2] = z.X;
            result[0, 3] = 0;
            result[1, 0] = x.Y;
            result[1, 1] = y.Y;
            result[1, 2] = z.Y;
            result[1, 3] = 0;
            result[2, 0] = x.Z;
            result[2, 1] = y.Z;
            result[2, 2] = z.Z;
            result[2, 3] = 0;
            result[3, 0] = -((x.X * eye.X) + (x.Y * eye.Y) + (x.Z * eye.Z));
            result[3, 1] = -((y.X * eye.X) + (y.Y * eye.Y) + (y.Z * eye.Z));
            result[3, 2] = -((z.X * eye.X) + (z.Y * eye.Y) + (z.Z * eye.Z));
            result[3, 3] = 1;

            return result;
        }

        public static Mat4 CreateOrthoProjection(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            float rl = right - left;
            float tb = top - bottom;
            float fn = zFar - zNear;

            return new Mat4(
                new Vec4(2.0f / rl, 0, 0, -(right + left) / rl),
                new Vec4(0, 2.0f / tb, 0, -(top + bottom) / tb),
                new Vec4(0, 0, -2.0f / fn, -(zFar + zNear) / fn),
                new Vec4(0, 0, 0, 1));
        }

        /// <summary>
        /// Creates a perspective projection matrix.
        /// </summary>
        /// <param name="fovy">Angle of the field of view in the y direction (in radians)</param>
        /// <param name="aspect">Aspect ratio of the view (width / height)</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        public static Mat4 PerspectiveFOV(float fovy, float aspect, float zNear, float zFar)
        {
#if DEBUG
            if (fovy <= 0 || fovy > Math.Pi)
                throw new ArgumentOutOfRangeException(nameof(fovy));
            if (aspect <= 0)
                throw new ArgumentOutOfRangeException(nameof(aspect));
            if (zNear <= 0)
                throw new ArgumentOutOfRangeException(nameof(zNear));
            if (zFar <= 0)
                throw new ArgumentOutOfRangeException(nameof(zFar));
#endif

            float yMax = zNear * (float)System.Math.Tan(0.5f * fovy);
            float yMin = -yMax;
            float xMin = yMin * aspect;
            float xMax = yMax * aspect;

            return PerspectiveOffCenter(xMin, xMax, yMin, yMax, zNear, zFar);
        }

        /// <summary>
        /// Creates an perspective projection matrix.
        /// </summary>
        /// <param name="left">Left edge of the view frustum</param>
        /// <param name="right">Right edge of the view frustum</param>
        /// <param name="bottom">Bottom edge of the view frustum</param>
        /// <param name="top">Top edge of the view frustum</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        public static Mat4 PerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Mat4 result = Mat4.Default;
#if DEBUG
            if (zNear <= 0)
                throw new ArgumentOutOfRangeException(nameof(zNear));
            if (zFar <= 0)
                throw new ArgumentOutOfRangeException(nameof(zFar));
            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException(nameof(zNear));
#endif

            float x = (2.0f * zNear) / (right - left);
            float y = (2.0f * zNear) / (top - bottom);
            float a = (right + left) / (right - left);
            float b = (top + bottom) / (top - bottom);
            float c = -(zFar + zNear) / (zFar - zNear);
            float d = -(2.0f * zFar * zNear) / (zFar - zNear);

            result[0, 0] = x;
            result[0, 1] = 0;
            result[0, 2] = 0;
            result[0, 3] = 0;
            result[1, 0] = 0;
            result[1, 1] = y;
            result[1, 2] = 0;
            result[1, 3] = 0;
            result[2, 0] = a;
            result[2, 1] = b;
            result[2, 2] = c;
            result[2, 3] = -1;
            result[3, 0] = 0;
            result[3, 1] = 0;
            result[3, 2] = d;
            result[3, 3] = 0;

            return result;
        }

        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        public static Mat4 RotAxisMat(Vec3 axis, float angle)
        {
            Mat4 result = Mat4.Default;

            // normalize and create a local copy of the vector.
            axis.NormSelf();
            float axisX = axis.X, axisY = axis.Y, axisZ = axis.Z;

            // calculate angles
            float cos = (float)System.Math.Cos(-angle);
            float sin = (float)System.Math.Sin(-angle);
            float t = 1.0f - cos;

            // do the conversion math once
            float tXX = t * axisX * axisX,
                  tXY = t * axisX * axisY,
                  tXZ = t * axisX * axisZ,
                  tYY = t * axisY * axisY,
                  tYZ = t * axisY * axisZ,
                  tZZ = t * axisZ * axisZ;

            float sinX = sin * axisX,
                  sinY = sin * axisY,
                  sinZ = sin * axisZ;

            result[0, 0] = tXX + cos;
            result[0, 1] = tXY - sinZ;
            result[0, 2] = tXZ + sinY;
            result[0, 3] = 0;
            result[1, 0] = tXY + sinZ;
            result[1, 1] = tYY + cos;
            result[1, 2] = tYZ - sinX;
            result[1, 3] = 0;
            result[2, 0] = tXZ - sinY;
            result[2, 1] = tYZ + sinX;
            result[2, 2] = tZZ + cos;
            result[2, 3] = 0;
            result[3, 0] = 0;
            result[3, 1] = 0;
            result[3, 2] = 0;
            result[3, 3] = 1;

            return result;
        }

        /// <summary>
        /// Build a rotation matrix from the specified euler angles rotation.
        /// </summary>
        public static Mat4 RotEulerMat(float yaw, float pitch, float roll)
        {
            return RotAxisMat(Vec3.UnitX, yaw)*
                   RotAxisMat(Vec3.UnitY, pitch)*
                   RotAxisMat(Vec3.UnitZ, roll);
        }

        /// <summary>
        /// Build a rotation matrix from the specified euler angles rotation vector.
        /// </summary>
        public static Mat4 RotEulerMat(Vec3 rot)
        {
            return RotAxisMat(Vec3.UnitX, rot.X) *
                   RotAxisMat(Vec3.UnitY, rot.Y) *
                   RotAxisMat(Vec3.UnitZ, rot.Z);
        }

        /// <summary>
        /// Build a translation matrix from the specified x, y and z coordinates.
        /// </summary>
        public static Mat4 Translation(float x, float y, float z)
        {
            Mat4 result = Mat4.Identity;

            result[3, 0] = x;
            result[3, 1] = y;
            result[3, 2] = z;

            return result;
        }

        /// <summary>
        /// Build a translation matrix from the specified vector.
        /// </summary>
        public static Mat4 Translation(Vec3 translation)
        {
            Mat4 result = Mat4.Identity;

            result[3, 0] = translation.X;
            result[3, 1] = translation.Y;
            result[3, 2] = translation.Z;

            return result;
        }

        /// <summary>
        /// Build a scaling matrix from the specified x, y and z coordinates.
        /// </summary>
        public static Mat4 Scale(float x, float y, float z)
        {
            Mat4 result = Mat4.Identity;

            result[0, 0] = x;
            result[1, 1] = y;
            result[2, 2] = z;

            return result;
        }

        /// <summary>
        /// Build a scaling matrix from the specified vector.
        /// </summary>
        public static Mat4 Scale(Vec3 translation)
        {
            Mat4 result = Mat4.Identity;

            result[0, 0] = translation.X;
            result[1, 1] = translation.Y;
            result[2, 2] = translation.Z;

            return result;
        }

        /// <summary>
        /// Builds the transform matrix from position, rotation and scale.
        /// Applyes rotation, scale and then translation.
        /// </summary>
        public static Mat4 Build(Vec3 pos, Quat rot, Vec3 scale)
        {
            return rot.ToMat4() * Scale(scale) * Translation(pos);
        }

        /// <summary>
        /// Builds the transform matrix from position, rotation and scale.
        /// Applyes rotation, scale and then translation;
        /// </summary>
        public static Mat4 Build(Vec3 pos, Vec3 rot, Vec3 scale)
        {
            return RotEulerMat(rot) * Scale(scale) * Translation(pos);
        }

        #endregion


        #region Quaternion transformation

        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        public static Quat FromAxisAngleQuat(Vec3 axis, float angle)
        {
            var t = (angle * 0.5f).SinCos();
            axis *= t.Sin;
            return new Quat(axis.X, axis.Y, axis.Z, t.Cos);
        }

        /// <summary>
        /// Build a rotation matrix from the specified euler angles rotation.
        /// </summary>
        public static Quat FromEulerAnglesQuat(float yaw, float pitch, float roll)
        {
            var x = (yaw   * 0.5f).SinCos();
            var y = (pitch * 0.5f).SinCos();
            var z = (roll  * 0.5f).SinCos();
           
            return new Quat(
                  z.Sin * y.Sin * x.Cos - z.Cos * y.Cos * x.Sin,
                -(z.Cos * y.Sin * x.Cos + z.Sin * y.Cos * x.Sin),
                  z.Cos * y.Sin * x.Sin - z.Sin * y.Cos * x.Cos,
                  x.Cos * y.Cos * z.Cos + x.Sin * y.Sin * z.Sin);
        }

        /// <summary>
        /// Build a rotation matrix from the specified euler angles rotation.
        /// </summary>
        public static Quat FromEulerAnglesQuat(Vec3 rot)
        {
            var x = (rot.X * 0.5f).SinCos();
            var y = (rot.Y * 0.5f).SinCos();
            var z = (rot.Z * 0.5f).SinCos();
           
            return new Quat(
                  z.Sin * y.Sin * x.Cos - z.Cos * y.Cos * x.Sin,
                -(z.Cos * y.Sin * x.Cos + z.Sin * y.Cos * x.Sin),
                  z.Cos * y.Sin * x.Sin - z.Sin * y.Cos * x.Cos,
                  x.Cos * y.Cos * z.Cos + x.Sin * y.Sin * z.Sin);
        }

        #endregion
    }
}