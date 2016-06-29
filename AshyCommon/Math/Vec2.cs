// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

using System;
using System.Runtime.InteropServices;

namespace AshyCommon.Math
{
    /// <summary>
    /// Vector 2 struct.
    /// </summary>
    public struct Vec2
    {
        #region Properties

        public float[] Values => new[] { X, Y };

        public static Vec2 Zero => new Vec2(0.0f, 0.0f);

        public static int SizeInBytes => Marshal.SizeOf(new Vec2());

        public float X { get; set; }

        public float Y { get; set; }

        /// <summary>Lenght squared.</summary>
        public float LenSqr => X.Sqr() + Y.Sqr();

        /// <summary>Vector lenght.</summary>
        public float Len => LenSqr.Sqrt();

        public float this[int idx]
        {
            get
            {
                if (idx == 0) return X;
                if (idx == 1) return Y;
                throw new ArgumentException(nameof(idx));
            }
            set
            {
                if (idx == 0) { X = value; return; }
                if (idx == 1) { Y = value; return; }
                throw new ArgumentException(nameof(idx));
            }
        }

        #endregion


        #region Constructors

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        #endregion


        #region Operators

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2 operator -(Vec2 a)
        {
            return new Vec2(-a.X, -a.Y);
        }

        public static Vec2 operator *(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X * b.X, a.Y * b.Y);
        }

        public static Vec2 operator /(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X / b.X, a.Y / b.Y);
        }

        public static Vec2 operator *(Vec2 a, float b)
        {
            return new Vec2(a.X * b, a.Y * b);
        }

        public static Vec2 operator *(float a, Vec2 b)
        {
            return new Vec2(a * b.X, a * b.X);
        }

        public static Vec2 operator /(Vec2 a, float b)
        {
            return new Vec2(a.X / b, a.Y / b);
        }

        public static bool operator ==(Vec2 a, Vec2 b)
        {
            return a.X.Eq(b.X) && a.Y.Eq(b.Y);
        }

        public static bool operator !=(Vec2 a, Vec2 b)
        {
            return !(a == b);
        }

        #endregion


        #region Public Methods

        public Vec2 Add(Vec2 a)
        {
            X += a.X;
            Y += a.Y;
            return this;
        }

        public Vec2 Sub(Vec2 a)
        {
            X -= a.X;
            Y -= a.Y;
            return this;
        }

        public Vec2 Mul(Vec2 a)
        {
            X *= a.X;
            Y *= a.Y;
            return this;
        }

        public Vec2 Div(Vec2 a)
        {
            X /= a.X;
            Y /= a.Y;
            return this;
        }

        public static float Dot(Vec2 a, Vec2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public float Dot(Vec2 a)
        {
            return Dot(this, a);
        }

        /// <summary>
        /// Vector normalization (self).
        /// </summary>
        public Vec2 NormSelf()
        {
            float temp = Len;
            X /= temp;
            Y /= temp;
            return this;
        }

        /// <summary>
        /// Vector normalization. Produces new vector.
        /// </summary>
        public Vec2 Norm()
        {
            Vec2 temp = this;
            temp.NormSelf();
            return temp;
        }

        public bool Equals(Vec2 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vec2 && this == (Vec2)obj;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        #endregion
    }
}