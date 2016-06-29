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
    /// Matrix 4x4 struct. Column-major.
    /// </summary>
    public struct Mat4
    {
        #region Fields

        public float[] Values;

        public static Mat4 Default => new Mat4(new float[16]);

        public static Mat4 Identity => new Mat4(Vec4.UnitX, Vec4.UnitY, Vec4.UnitZ, Vec4.UnitW);

        #endregion


        #region Properties

        public float this[int iRow, int iColumn]
        {
            get { return Values[4 * iRow + iColumn]; }
            set { Values[4 * iRow + iColumn] = value; }
        }

        public float this[int idx]
        {
            get { return Values[idx]; }
            set { Values[idx] = value; }
        }

        public Vec4 Row0 => new Vec4(this[0, 0], this[0, 1], this[0, 2], this[0, 3]);
        public Vec4 Row1 => new Vec4(this[1, 0], this[1, 1], this[1, 2], this[1, 3]);
        public Vec4 Row2 => new Vec4(this[2, 0], this[2, 1], this[2, 2], this[2, 3]);
        public Vec4 Row3 => new Vec4(this[3, 0], this[3, 1], this[3, 2], this[3, 3]);

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Mat4" /> struct.
        /// </summary>
        /// <param name="values">The values array size should be 16.</param>
        public Mat4(float[] values)
        {
            Values = values;
        }

        /// <summary>
        /// Fills the matrix with rows <paramref name="i"/>, <paramref name="j"/>, <paramref name="k"/>, <paramref name="l"/>.
        /// </summary>
        public Mat4(Vec4 i, Vec4 j, Vec4 k, Vec4 l)
        {
            Values = new float[16];
            for (int m = 0; m < 4; m++)
            {
                this[m, 0] = i[m];
                this[m, 1] = j[m];
                this[m, 2] = k[m];
                this[m, 3] = l[m];
            }
        }

        #endregion
        
        
        #region Operators

        public static Mat4 operator +(Mat4 a, Mat4 b)
        {
            return new Mat4(a.Values).Add(b);
        }

        public static Mat4 operator -(Mat4 a, Mat4 b)
        {
            return new Mat4(a.Values).Sub(b);
        }

        public static Mat4 operator *(Mat4 a, Mat4 b)
        {
            Mat4 temp = Default;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp[i, j] = a[i, 0] * b[0, j] +
                                 a[i, 1] * b[1, j] +
                                 a[i, 2] * b[2, j] +
                                 a[i, 3] * b[3, j];
                }
            }
            return temp;
        }

        public static Vec4 operator *(Vec4 a, Mat4 b)
        {
            Vec4 temp = Vec4.Zero;
            for (int j = 0; j < 4; j++)
            {
                temp[j] = a[0] * b[0, j] +
                          a[1] * b[1, j] +
                          a[2] * b[2, j] +
                          a[3] * b[3, j];
            }
            return temp;
        }

        public static bool operator ==(Mat4 a, Mat4 b)
        {
            for (int i = 0; i < 16; i++)
            {
                if (!a.Values[i].Eq(b.Values[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(Mat4 a, Mat4 b)
        {
            return !(a == b);
        }

        #endregion


        #region Methods

        public Mat4 Add(Mat4 b)
        {
            for (int i = 0; i < 16; i++)
            {
                this[i] += b[i];
            }
            return this;
        }

        public Mat4 Sub(Mat4 b)
        {
            for (int i = 0; i < 16; i++)
            {
                this[i] -= b[i];
            }
            return this;
        }

        public Mat4 Mul(Mat4 b)
        {
            this = this * b;
            return this;
        }

        public Mat4 Transpose()
        {
            Mat4 transposed = Default;
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    transposed[i, j] = this[j, i];
                }
            }
            return transposed;
        }

        public Vec3 GetAngles()
        {
            return new Vec3(Math.Atan2( this[2, 1], this[2, 2]),
                            Math.Atan2(-this[0, 2], new Vec2(this[1, 2], this[2, 2]).Len), 
                            Math.Atan2( this[0, 1], this[0, 0]));
        }

        /// <summary>
        /// There is no any normalization.
        /// </summary>
        public Quat ToQuat()
        {
            var trace = this[0, 0] + this[1, 1] + this[2, 2];
            Vec4 result = new Vec4();

            if (trace > 0)
            {
                var s = (trace + 1).Sqrt() * 2;
                var invS = 1f / s;

                result.W = s * 0.25f;
                result.X = (this[2,1] - this[1,2]) * invS;
                result.Y = (this[0,2] - this[2,0]) * invS;
                result.Z = (this[1,0] - this[0,1]) * invS;
            }
            else
            {
                float m00 = this[0,0], m11 = this[1,1], m22 = this[2,2];

                if (m00 > m11 && m00 > m22)
                {
                    var s = (1 + m00 - m11 - m22).Sqrt() * 2;
                    var invS = 1f / s;

                    result.W = (this[2,1] - this[1,2]) * invS;
                    result.X = s * 0.25f;
                    result.Y = (this[0,1] + this[1,0]) * invS;
                    result.Z = (this[0,2] + this[2,0]) * invS;
                }
                else if (m11 > m22)
                {
                    var s = (1 + m11 - m00 - m22).Sqrt() * 2;
                    var invS = 1f / s;

                    result.W = (this[0,2] - this[2,0]) * invS;
                    result.X = (this[0,1] + this[1,0]) * invS;
                    result.Y = s * 0.25f;
                    result.Z = (this[1,2] + this[2,1]) * invS;
                }
                else
                {
                    var s = (1 + m22 - m00 - m11).Sqrt() * 2;
                    var invS = 1f / s;

                    result.W = (this[1,0] - this[0,1]) * invS;
                    result.X = (this[0,2] + this[2,0]) * invS;
                    result.Y = (this[1,2] + this[2,1]) * invS;
                    result.Z = s * 0.25f;
                }
            }
            return new Quat(result);
        }

        public Vec3 ExtractPosition()
        {
            return new Vec3(this[3, 0], this[3, 1], this[3, 2]);
        }

        /// <summary>
        /// Returns the rotation component of this instance. Quite slow.
        /// </summary>
        /// <param name="rowNormalise">
        /// Whether the method should row-normalise (i.e. remove scale from) the Matrix. 
        /// Pass false if you know it's already normalised.
        /// </param>
        public Quat ExtractRotation(bool rowNormalise = true)
        {
            var row0 = Row0.Clip();
            var row1 = Row0.Clip();
            var row2 = Row0.Clip();

            if (rowNormalise)
            {
                row0.NormSelf();
                row1.NormSelf();
                row2.NormSelf();
            }

            Vec4 q = new Vec4();
            float trace = 0.25f * (row0[0] + row1[1] + row2[2] + 1.0f);

            if (trace > 0)
            {
                float sq = trace.Sqrt();

                q.W = sq;
                sq = 1.0f / (4.0f * sq);
                q.X = (row1[2] - row2[1]) * sq;
                q.Y = (row2[0] - row0[2]) * sq;
                q.Z = (row0[1] - row1[0]) * sq;
            }
            else if (row0[0] > row1[1] && row0[0] > row2[2])
            {
                float sq = 2.0f * (1.0f + row0[0] - row1[1] - row2[2]).Sqrt();

                q.X = (float)(0.25 * sq);
                sq = 1.0f / sq;
                q.W = (row2[1] - row1[2]) * sq;
                q.Y = (row1[0] + row0[1]) * sq;
                q.Z = (row2[0] + row0[2]) * sq;
            }
            else if (row1[1] > row2[2])
            {
                float sq = 2.0f * (1.0f + row1[1] - row0[0] - row2[2]).Sqrt();

                q.Y = 0.25f * sq;
                sq = 1.0f / sq;
                q.W = (row2[0] - row0[2]) * sq;
                q.X = (row1[0] + row0[1]) * sq;
                q.Z = (row2[1] + row1[2]) * sq;
            }
            else
            {
                float sq = 2.0f * (1.0f + row2[2] - row0[0] - row1[1]).Sqrt();

                q.Z = 0.25f * sq;
                sq = 1.0f / sq;
                q.W = (row1[0] - row0[1]) * sq;
                q.X = (row2[0] + row0[2]) * sq;
                q.Y = (row2[1] + row1[2]) * sq;
            }

            return new Quat(q).Normalize();
        }

        public Vec3 ExtractScale()
        {
            return new Vec3(Row0.Clip().Len, Row1.Clip().Len, Row2.Clip().Len);
        }

        public Mat4 ClipRotation()
        {
            return new Mat4(
                Row0.Clip().ToVec4(0),
                Row1.Clip().ToVec4(0),
                Row2.Clip().ToVec4(0),
                Row3);
        }

        [Obsolete("Not used anymore", true)]
        public Mat3 ToMat3()
        {
            return new Mat3(Row0.Clip(), Row1.Clip(), Row2.Clip());
        }

        public Mat4 Invert()
        {
            int[] colIdx = { 0, 0, 0, 0 };
            int[] rowIdx = { 0, 0, 0, 0 };
            int[] pivotIdx = { -1, -1, -1, -1 };

            // convert the matrix to an array for easy looping
            float[,] inverse =
            {
                {Row0.X, Row0.Y, Row0.Z, Row0.W},
                {Row1.X, Row1.Y, Row1.Z, Row1.W},
                {Row2.X, Row2.Y, Row2.Z, Row2.W},
                {Row3.X, Row3.Y, Row3.Z, Row3.W}
            };
            int icol = 0;
            int irow = 0;
            for (int i = 0; i < 4; i++)
            {
                // Find the largest pivot value
                float maxPivot = 0.0f;
                for (int j = 0; j < 4; j++)
                {
                    if (pivotIdx[j] != 0)
                    {
                        for (int k = 0; k < 4; ++k)
                        {
                            if (pivotIdx[k] == -1)
                            {
                                float absVal = System.Math.Abs(inverse[j, k]);
                                if (absVal > maxPivot)
                                {
                                    maxPivot = absVal;
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (pivotIdx[k] > 0)
                            {
                                return this;
                            }
                        }
                    }
                }

                ++(pivotIdx[icol]);

                // Swap rows over so pivot is on diagonal
                if (irow != icol)
                {
                    for (int k = 0; k < 4; ++k)
                    {
                        float f = inverse[irow, k];
                        inverse[irow, k] = inverse[icol, k];
                        inverse[icol, k] = f;
                    }
                }

                rowIdx[i] = irow;
                colIdx[i] = icol;

                float pivot = inverse[icol, icol];

                // Scale row so it has a unit diagonal
                float oneOverPivot = 1.0f / pivot;
                inverse[icol, icol] = 1.0f;
                for (int k = 0; k < 4; ++k)
                    inverse[icol, k] *= oneOverPivot;

                // Do elimination of non-diagonal elements
                for (int j = 0; j < 4; ++j)
                {
                    // check this isn't on the diagonal
                    if (icol != j)
                    {
                        float f = inverse[j, icol];
                        inverse[j, icol] = 0.0f;
                        for (int k = 0; k < 4; ++k)
                            inverse[j, k] -= inverse[icol, k] * f;
                    }
                }
            }

            for (int j = 3; j >= 0; --j)
            {
                int ir = rowIdx[j];
                int ic = colIdx[j];
                for (int k = 0; k < 4; ++k)
                {
                    float f = inverse[k, ir];
                    inverse[k, ir] = inverse[k, ic];
                    inverse[k, ic] = f;
                }
            }

            return new Mat4(
                new Vec4(inverse[0, 0], inverse[0, 1], inverse[0, 2], inverse[0, 3]),
                new Vec4(inverse[1, 0], inverse[1, 1], inverse[1, 2], inverse[1, 3]),
                new Vec4(inverse[2, 0], inverse[2, 1], inverse[2, 2], inverse[2, 3]),
                new Vec4(inverse[3, 0], inverse[3, 1], inverse[3, 2], inverse[3, 3]));
        }

        #endregion


        #region Equality methods

        public bool Equals(Mat4 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Mat4 && Equals((Mat4)obj);
        }

        public override int GetHashCode()
        {
            return Values?.GetHashCode() ?? 0;
        }
        
        #endregion
    }
}