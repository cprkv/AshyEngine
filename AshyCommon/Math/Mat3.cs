//  
// Created  : 23.05.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;

namespace AshyCommon.Math
{
    /// <summary>
    /// Matrix 3x3 transformation. Column-major.
    /// </summary>
    [Obsolete("Not used anymore", true)]
    public class Mat3
    {
        #region Fields

        public float[] Values;

        public static Mat3 Default => new Mat3(new float[9]);

        public static Mat3 Identity => new Mat3(Vec3.UnitX, Vec3.UnitY, Vec3.UnitZ);

        #endregion


        #region Properties

        public float this[int iRow, int iColumn]
        {
            get { return Values[3 * iRow + iColumn]; }
            set { Values[3 * iRow + iColumn] = value; }
        }

        public float this[int idx]
        {
            get { return Values[idx]; }
            set { Values[idx] = value; }
        }

        public Vec3 Row0 => new Vec3(this[0, 0], this[0, 1], this[0, 2]);
        public Vec3 Row1 => new Vec3(this[1, 0], this[1, 1], this[1, 2]);
        public Vec3 Row2 => new Vec3(this[2, 0], this[2, 1], this[2, 2]);

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Mat4" /> struct.
        /// </summary>
        /// <param name="values">The values array size should be 16.</param>
        public Mat3(float[] values)
        {
            Values = values;
        }

        /// <summary>
        /// Fills the matrix with rows <paramref name="i"/>, <paramref name="j"/>, <paramref name="k"/>, <paramref name="l"/>.
        /// </summary>
        public Mat3(Vec3 i, Vec3 j, Vec3 k)
        {
            Values = new float[16];
            for (int m = 0; m < 3; m++)
            {
                this[m, 0] = i[m];
                this[m, 1] = j[m];
                this[m, 2] = k[m];
            }
        }

        public Mat3 Invert()
        {
            int[] colIdx = { 0, 0, 0 };
            int[] rowIdx = { 0, 0, 0 };
            int[] pivotIdx = { -1, -1, -1 };

            // convert the matrix to an array for easy looping
            float[,] inverse =
            {
                {Row0.X, Row0.Y, Row0.Z},
                {Row1.X, Row1.Y, Row1.Z},
                {Row2.X, Row2.Y, Row2.Z},
            };
            int icol = 0;
            int irow = 0;
            for (int i = 0; i < 3; i++)
            {
                // Find the largest pivot value
                float maxPivot = 0.0f;
                for (int j = 0; j < 3; j++)
                {
                    if (pivotIdx[j] != 0)
                    {
                        for (int k = 0; k < 3; ++k)
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
                    for (int k = 0; k < 3; ++k)
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
                for (int k = 0; k < 3; ++k)
                    inverse[icol, k] *= oneOverPivot;

                // Do elimination of non-diagonal elements
                for (int j = 0; j < 3; ++j)
                {
                    // check this isn't on the diagonal
                    if (icol != j)
                    {
                        float f = inverse[j, icol];
                        inverse[j, icol] = 0.0f;
                        for (int k = 0; k < 3; ++k)
                            inverse[j, k] -= inverse[icol, k] * f;
                    }
                }
            }

            for (int j = 2; j >= 0; --j)
            {
                int ir = rowIdx[j];
                int ic = colIdx[j];
                for (int k = 0; k < 3; ++k)
                {
                    float f = inverse[k, ir];
                    inverse[k, ir] = inverse[k, ic];
                    inverse[k, ic] = f;
                }
            }

            return new Mat3(
                new Vec3(inverse[0, 0], inverse[0, 1], inverse[0, 2]),
                new Vec3(inverse[1, 0], inverse[1, 1], inverse[1, 2]),
                new Vec3(inverse[2, 0], inverse[2, 1], inverse[2, 2]));
        }

        #endregion 
    }
}