// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

namespace AshyCommon.Math
{
    /// <summary>
    /// Easy structure that calculates sine and cosine of the value and stores it.
    /// </summary>
    public struct SinCos
    {
        public float Cos { get; }
        public float Sin { get; }

        public SinCos(float x)
        {
            Cos = x.Cos();
            Sin = x.Sin();
        }
    }

    public static class Math
    {
        #region Constants

        public const float Epsilon3 = 1e-03f;
        public const float Epsilon5 = 1e-05f;
        public const float Epsilon6 = 1e-06f;
        public const float Epsilon7 = 1e-07f;

        public const float Pi = 3.1415926535897932384626433832795f;
        public const float PiMul2 = 6.2831853071795864769252867665590f;
        public const float PiMul4 = 12.566370614359172953850573533118f;
        public const float PiDiv2 = 1.5707963267948966192313216916398f;
        public const float PiDiv4 = 0.7853981633974483096156608458199f;

        #endregion


        #region Float extention methods

        /// <summary>
        /// Compares <paramref name="a"/> and <paramref name="b"/> with <paramref name="eps"/> precision.
        /// (a == b) -> 0; (a > b) -> 1; else -> -1
        /// </summary>
        public static int CompareTo(this float a, float b, float eps = Epsilon5)
        {
            return a.Eq(b, eps) ? 0 : System.Math.Sign(a - b);
        }

        /// <summary>
        /// Compares <paramref name="a"/> and <paramref name="b"/> for equality 
        /// with <paramref name="eps"/> precision.
        /// </summary>
        public static bool Eq(this float a, float b, float eps = Epsilon5)
        {
            return System.Math.Abs(a - b) < eps;
        }

        #region Inline bindigs

        public static float Sqrt(this float a)      => (float)System.Math.Sqrt(a);
        public static float Sqr(this float a)       => a * a;
        public static float Abs(this float a)       => System.Math.Abs(a);
        public static float Sin(this float a)       => (float)System.Math.Sin(a);
        public static float Cos(this float a)       => (float)System.Math.Cos(a);
        public static float Acos(this float a)      => (float)System.Math.Acos(a);
        public static float Asin(this float a)      => (float)System.Math.Asin(a);
        public static float Atan2(float a, float b) => (float)System.Math.Atan2(a, b);
        public static SinCos SinCos(this float a)   => new SinCos(a);
        public static float ToRadians(this float a) => a * (Pi / 180f);
        public static float ToDegrees(this float a) => a * (180f / Pi);
        public static float Lerp(this float a, float b, float t) => (b - a) * t + a;
        public static float RLerp(this float t, float a, float b) => a / (a - b);


        public static float Clamp(this float a, float down, float up) => a < down ? down : a > up ? up : a;

        #endregion

        #endregion
    }
}