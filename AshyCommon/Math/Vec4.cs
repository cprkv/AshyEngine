// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//

using System;

namespace AshyCommon.Math
{
    /// <summary>
    /// Vector 4 struct.
    /// </summary>
    public struct Vec4
    {
        #region Fields

        public float[] Values => new[] { X, Y, Z, W };

        /// <summary>
        /// Defines a unit-length Vec4 that points towards the X-axis.
        /// </summary>
        public static Vec4 UnitX => new Vec4(1, 0, 0, 0);

        /// <summary>
        /// Defines a unit-length Vec4 that points towards the Y-axis.
        /// </summary>
        public static Vec4 UnitY => new Vec4(0, 1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vec4 that points towards the Z-axis.
        /// </summary>
        public static Vec4 UnitZ => new Vec4(0, 0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vec4 that points towards the Z-axis.
        /// </summary>
        public static Vec4 UnitW => new Vec4(0, 0, 0, 1);

        /// <summary>
        /// Defines a zero-length Vec4.
        /// </summary>
        public static Vec4 Zero => new Vec4(0, 0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static Vec4 One => new Vec4(1, 1, 1, 1);

        #endregion


        #region Properties


        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float W { get; set; }

        /// <summary>Lenght squared.</summary>
        public float LenSqr => X.Sqr() + Y.Sqr() + Z.Sqr() + W.Sqr();

        /// <summary>Vector lenght.</summary>
        public float Len => LenSqr.Sqrt();

        public float this[int idx]
        {
            get
            {
                if (idx == 0) return X;
                if (idx == 1) return Y;
                if (idx == 2) return Z;
                if (idx == 3) return W;
                throw new ArgumentException(nameof(idx));
            }
            set
            {
                if (idx == 0) { X = value; return; }
                if (idx == 1) { Y = value; return; }
                if (idx == 2) { Z = value; return; }
                if (idx == 3) { W = value; return; }
                throw new ArgumentException(nameof(idx));
            }
        }


        #endregion


        #region Constructors

        public Vec4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vec4(Vec3 a, float w)
        {
            X = a.X;
            Y = a.Y;
            Z = a.Z;
            W = w;
        }
        
        #endregion


        #region Operators

        public static Vec4 operator +(Vec4 a, Vec4 b)
        {
            return new Vec4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }

        public static Vec4 operator -(Vec4 a, Vec4 b)
        {
            return new Vec4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        public static Vec4 operator -(Vec4 a)
        {
            return new Vec4(-a.X, -a.Y, -a.Z, -a.W);
        }

        public static Vec4 operator *(Vec4 a, Vec4 b)
        {
            return new Vec4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
        }

        public static Vec4 operator /(Vec4 a, Vec4 b)
        {
            return new Vec4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
        }

        public static Vec4 operator *(Vec4 a, float b)
        {
            return new Vec4(a.X * b, a.Y * b, a.Z * b, a.W * b);
        }

        public static Vec4 operator *(float a, Vec4 b)
        {
            return new Vec4(a * b.X, a * b.X, a * b.Z, a * b.W);
        }

        public static Vec4 operator /(Vec4 a, float b)
        {
            return new Vec4(a.X / b, a.Y / b, a.Z / b, a.W / b);
        }

        public static bool operator ==(Vec4 a, Vec4 b)
        {
            return a.X.Eq(b.X) && a.Y.Eq(b.Y) && a.Z.Eq(b.Z) && a.W.Eq(b.W);
        }

        public static bool operator !=(Vec4 a, Vec4 b)
        {
            return !(a == b);
        }

        #endregion


        #region Public Methods

        public Vec4 Add(Vec4 a)
        {
            X += a.X;
            Y += a.Y;
            Z += a.Z;
            W += a.W;
            return this;
        }

        public Vec4 Sub(Vec4 a)
        {
            X -= a.X;
            Y -= a.Y;
            Z -= a.Z;
            W -= a.W;
            return this;
        }

        public Vec4 Mul(Vec4 a)
        {
            X *= a.X;
            Y *= a.Y;
            Z *= a.Z;
            W *= a.W;
            return this;
        }

        public Vec4 Mul(Mat4 a)
        {
            this = this * a;
            return this;
        }

        public Vec4 Div(Vec4 a)
        {
            X /= a.X;
            Y /= a.Y;
            Z /= a.Z;
            W /= a.W;
            return this;
        }

        public float Dot(Vec4 a)
        {
            return Dot(this, a);
        }
        
        /// <summary>
        /// Vector normalization (self).
        /// </summary>
        public Vec4 NormSelf()
        {
            float temp = Len;
            X /= temp;
            Y /= temp;
            Z /= temp;
            W /= temp;
            return this;
        }

        /// <summary>
        /// Vector normalization. Produces new vector.
        /// </summary>
        public Vec4 Norm()
        {
            Vec4 temp = this;
            temp.NormSelf();
            return temp;
        }

        /// <summary>
        /// Clips vector with division all coordinates by W.
        /// </summary>
        public Vec3 ClipDivided()
        {
            return new Vec3(X/W, Y/W, Z/W);
        }

        /// <summary>
        /// Clips vector without division all coordinates by W.
        /// </summary>
        public Vec3 Clip()
        {
            return new Vec3(X, Y, Z);
        }

        public static float Dot(Vec4 a, Vec4 b)
        {
            return a.X*b.X + a.Y*b.Y + a.Z*b.Z + a.W*b.W;
        }

        public bool Equals(Vec4 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vec4 && this == (Vec4)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode*397) ^ Y.GetHashCode();
                hashCode = (hashCode*397) ^ Z.GetHashCode();
                hashCode = (hashCode*397) ^ W.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W})";
        }

        #endregion
    }
}