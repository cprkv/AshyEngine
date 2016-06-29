// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace AshyCommon.Math
{
    /// <summary>
    /// Vector 3 struct.
    /// </summary>
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Fields

        public float[] Values => new[] { X, Y, Z };

        /// <summary>
        /// Defines a unit-length Vec3 that points towards the X-axis.
        /// </summary>
        public static Vec3 UnitX => new Vec3(1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vec3 that points towards the Y-axis.
        /// </summary>
        public static Vec3 UnitY => new Vec3(0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vec3 that points towards the Z-axis.
        /// </summary>
        public static Vec3 UnitZ => new Vec3(0, 0, 1);

        /// <summary>
        /// Defines a zero-length Vec3.
        /// </summary>
        public static Vec3 Zero => new Vec3(0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static Vec3 One => new Vec3(1, 1, 1);

        public static int SizeInBytes => Marshal.SizeOf(new Vec3());

        #endregion


        #region Properties

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        /// <summary>Lenght squared.</summary>
        public float LenSqr => X.Sqr() + Y.Sqr() + Z.Sqr();

        /// <summary>Vector lenght.</summary>
        public float Len => LenSqr.Sqrt();

        public float this[int idx]
        {
            get
            {
                if (idx == 0) return X;
                if (idx == 1) return Y;
                if (idx == 2) return Z;
                throw new ArgumentException(nameof(idx));
            }
            set
            {
                if (idx == 0) { X = value; return; }
                if (idx == 1) { Y = value; return; }
                if (idx == 2) { Z = value; return; }
                throw new ArgumentException(nameof(idx));
            }
        }


        #endregion


        #region Constructors

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3(Vec2 a, float z)
        {
            X = a.X;
            Y = a.Y;
            Z = z;
        }

        #endregion


        #region Operators

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator -(Vec3 a)
        {
            return new Vec3(-a.X, -a.Y, -a.Z);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        }

        public static Vec3 operator *(Vec3 a, float b)
        {
            return new Vec3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3 operator /(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public static Vec3 operator /(Vec3 a, float b)
        {
            return new Vec3(a.X / b, a.Y / b, a.Z / b);
        }

        public static bool operator ==(Vec3 a, Vec3 b)
        {
            return a.X.Eq(b.X) && a.Y.Eq(b.Y) && a.Z.Eq(b.Z);
        }

        public static bool operator !=(Vec3 a, Vec3 b)
        {
            return !(a == b);
        }

        #endregion


        #region Public Methods

        public Vec3 Add(Vec3 a)
        {
            X += a.X;
            Y += a.Y;
            Z += a.Z;
            return this;
        }

        public Vec3 Sub(Vec3 a)
        {
            X -= a.X;
            Y -= a.Y;
            Z -= a.Z;
            return this;
        }

        public Vec3 Mul(Vec3 a)
        {
            X *= a.X;
            Y *= a.Y;
            Z *= a.Z;
            return this;
        }

        public Vec3 Mul(float a)
        {
            X *= a;
            Y *= a;
            Z *= a;
            return this;
        }

        public Vec3 Div(Vec3 a)
        {
            X /= a.X;
            Y /= a.Y;
            Z /= a.Z;
            return this;
        }

        public float Dot(Vec3 a)
        {
            return Dot(this, a);
        }

        public Vec3 CrossSelf(Vec3 a)
        {
            return this = Cross(this, a);
        }

        public Vec3 Cross(Vec3 a)
        {
            return Cross(this, a);
        }

        /// <summary>
        /// Vector normalization (self).
        /// </summary>
        public Vec3 NormSelf()
        {
            float temp = Len;
            X /= temp;
            Y /= temp;
            Z /= temp;
            return this;
        }

        /// <summary>
        /// Vector normalization. Produces new vector.
        /// </summary>
        public Vec3 Norm()
        {
            Vec3 temp = this;
            temp.NormSelf();
            return temp;
        }

        /// <summary>
        /// Clips vector with division all coordinates by W.
        /// </summary>
        public Vec2 ClipDivided()
        {
            return new Vec2(X / Z, Y / Z);
        }

        /// <summary>
        /// Clips vector without division all coordinates by W.
        /// </summary>
        public Vec2 Clip()
        {
            return new Vec2(X, Y);
        }

        public Vec4 ToVec4(float w = 1.0f)
        {
            return new Vec4(X, Y, Z, w);
        }

        /// <summary>Transform a Vec3 by the given Matrix, and project the resulting Vec4 back to a Vec3</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vec3 TransformPerspective(Vec3 vec, Mat4 mat)
        {
            return vec.ToVec4().Mul(mat).ClipDivided();
        }

        public static float Dot(Vec3 a, Vec3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(a.Y * b.Z - a.Z * b.Y,
                            a.Z * b.X - a.X * b.Z,
                            a.X * b.Y - a.Y * b.X);
        }

        public bool Equals(Vec3 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vec3 && this == (Vec3)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode*397) ^ Y.GetHashCode();
                hashCode = (hashCode*397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Parses the specified string like "1.00 2.050 3.0000"
        /// </summary>
        public static Vec3 Parse(string str)
        {
            var strSplitted = str.Split();
            var cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            return new Vec3(float.Parse(strSplitted[0], NumberStyles.Any, cultureInfo),
                            float.Parse(strSplitted[1], NumberStyles.Any, cultureInfo),
                            float.Parse(strSplitted[2], NumberStyles.Any, cultureInfo));
        }

        public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            return (b - a)*t + a;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        #endregion
    }
}