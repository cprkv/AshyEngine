// 
// Created : 28.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

namespace AshyCommon.Math
{
    /// <summary>
    /// Quaternion structure.
    /// Describes quaternion q = W + X*i + Y*j + Z*k
    /// </summary>
    public struct Quat
    {
        #region Properties

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public float W { get; private set; }

        public Vec3 V
        {
            get { return new Vec3(X,Y,Z); }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public static Quat Zero => new Quat(0, 0, 0, 1);

        public float Len => X.Sqr() + Y.Sqr() + Z.Sqr() + W.Sqr();

        public Quat Conj => new Quat(W, -V);

        #endregion


        #region Constructors

        public Quat(Vec4 q)
        {
            X = q.X;
            Y = q.Y;
            Z = q.Z;
            W = q.W;
        }

        public Quat(float w, Vec3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        public Quat(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        #endregion


        #region Operators

        public static Quat operator *(Quat a, Quat b)
        {
            return new Quat(
                a.Y * b.Z - a.Z * b.Y + b.X * a.W + a.X * b.W,
                a.Z * b.X - a.X * b.Z + b.Y * a.W + a.Y * b.W,
                a.X * b.Y - a.Y * b.X + b.Z * a.W + a.Z * b.W,
                a.W * b.W - (a.X * b.X + a.Y * b.Y + a.Z * b.Z));
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Spherical linear interpolation between two quaternions
        /// Note: SLerp is not commutative
        /// </summary>
        public Quat SLerp(Quat q, float t)
        {
            var q1 = q;

            // Calculate cosine
            float cosTheta = X * q.X + Y * q.Y + Z * q.Z + W * q.W;

            // Use the shortest path
            if (cosTheta < 0)
            {
                cosTheta = -cosTheta;
                q1.X = -q.X; q1.Y = -q.Y;
                q1.Z = -q.Z; q1.W = -q.W;
            }

            // Initialize with linear interpolation
            float scale0 = 1 - t, scale1 = t;

            // Use spherical interpolation only if the quaternions are not very close
            if ((1 - cosTheta) > 0.001f)
            {
                // SLERP
                float theta = cosTheta.Acos();
                float sinTheta = theta.Sin();
                scale0 = Math.Sin((1 - t) * theta) / sinTheta;
                scale1 = Math.Sin(t * theta) / sinTheta;
            }

            // Calculate final quaternion
            return new Quat(X * scale0 + q1.X * scale1, Y * scale0 + q1.Y * scale1,
                            Z * scale0 + q1.Z * scale1, W * scale0 + q1.W * scale1);
        }

        /// <summary>
        /// Normalized linear quaternion interpolation
        /// Note: NLERP is faster than SLERP and commutative but does not yield constant velocity
        /// </summary>
        public Quat NLerp(Quat q, float t)
        {
            Quat qt;
            float cosTheta = X * q.X + Y * q.Y + Z * q.Z + W * q.W;

            // Use the shortest path and interpolate linearly
            if (cosTheta < 0)
            {
                qt = new Quat(X + (-q.X - X) * t, Y + (-q.Y - Y) * t,
                              Z + (-q.Z - Z) * t, W + (-q.W - W) * t);
            }
            else
            {
                qt = new Quat(X + (q.X - X) * t, Y + (q.Y - Y) * t,
                              Z + (q.Z - Z) * t, W + (q.W - W) * t);
            }

            // Return normalized quaternion
            float invLen = 1.0f / (qt.X * qt.X + qt.Y * qt.Y + qt.Z * qt.Z + qt.W * qt.W).Sqrt();

            return new Quat(qt.X * invLen, qt.Y * invLen, qt.Z * invLen, qt.W * invLen);
        }

        /// <summary>
        /// Inverts this quaternion.
        /// </summary>
        /// <returns>New quaternion.</returns>
        public Quat Invert()
        {
            var len = Len;
            if (len.Eq(0)) return this; // wat to do?

            float invLen = 1.0f / len;

            return new Quat(-X * invLen, -Y * invLen, -Z * invLen, W * invLen);
        }

        /// <summary>
        /// Scales the Quaternion to unit length.
        /// Normalize self.
        /// </summary>
        public Quat Normalize()
        {
            float scale = 1.0f / Len;
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
            return this;
        }

        #endregion


        #region Convert methods

        /// <summary>
        /// Converts quaternion to matrix.
        /// </summary>
        public Mat4 ToMat4()
        {
            // Calculate coefficients
            float x2 = X + X, y2 = Y + Y, z2 = Z + Z;
            float xx = X * x2, xy = X * y2, xz = X * z2;
            float yy = Y * y2, yz = Y * z2, zz = Z * z2;
            float wx = W * x2, wy = W * y2, wz = W * z2;

            var mat = new Mat4(new float[16])
            {
                [0, 0] = 1 - (yy + zz),
                [0, 1] = xy - wz,
                [0, 2] = xz + wy,
                [1, 0] = xy + wz,
                [1, 1] = 1 - (xx + zz),
                [1, 2] = yz - wx,
                [2, 0] = xz - wy,
                [2, 1] = yz + wx,
                [2, 2] = 1 - (xx + yy),
                [3, 3] = 1
            };
            return mat;
        }

        /// <summary>
        /// Convert this instance to an axis-angle representation.
        /// </summary>
        /// <returns>A Vector4 that is the axis-angle representation of this quaternion.</returns>
        /// <remarks>
        /// XYZ - axis
        /// W   - angle
        /// </remarks>
        public Vec4 ToAxisAngle()
        {
            Quat q = this;
            if (q.W.Abs() > 1.0f)
                q.Normalize();

            Vec4 result = new Vec4 { W = 2.0f * q.W.Acos() };

            // angle
            float den = (1.0f - q.W * q.W).Sqrt();
            if (den > 0.0001f)
            {
                result.X = q.X / den;
                result.Y = q.Y / den;
                result.Z = q.Z / den;
            }
            else
            {
                // This occurs when the angle is zero. 
                // Not a problem: just set an arbitrary normalized axis.
                result.X = 1f;
                result.Y = 0f;
                result.Z = 0f;
            }

            return result;
        }

        /// <summary>
        /// Returns euler angles representation of quaternion.
        /// </summary>
        public Vec3 ToEulerAngles()
        {
            return new Vec3(
                Math.Atan2(2 * (X * Y + Z * W), 1 - 2 * (Y.Sqr() + Z.Sqr())),
                Math.Asin(2 * (X * Z - W * Y)),
                Math.Atan2(2 * (X * W + Y * Z), 1 - 2 * (Z.Sqr() + W.Sqr()))
                );
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W})";
        }

        #endregion
    }
}