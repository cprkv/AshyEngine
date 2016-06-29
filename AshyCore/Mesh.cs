// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Linq;
using AshyCommon.Math;

namespace AshyCore
{
    /// <summary>
    /// Prepared data to be stored in GPU buffers.
    /// </summary>
    public struct BufferDataDesctiption
    {
        public Vec3[] Data { get; }
        public uint[] Indecies { get; }

        public BufferDataDesctiption(Vec3[] data, uint[] indecies)
        {
            Data = data;
            Indecies = indecies;
        }

        /// <summary>
        /// Size of vertices in bytes.
        /// </summary>
        public int SizeOfVertices => Data.Length * Vec3.SizeInBytes;

        /// <summary>
        /// Size of vertIndices in bytes.
        /// </summary>
        public int SizeOfIndices => Indecies.Length * sizeof(uint);
    }


    /// <summary>
    /// Class for 3D objects.
    /// </summary>
    /// TODO: make inherited classes which will adds Bitangents, Tangents, UVWs ... etc
    public class Mesh : IEquatable<Mesh>
    {
        #region Properties

        private static long _globalMeshId = 0;
        private long _id;
        public Vec3[] Vertices { get; protected set; }
        public Vec3[] UVW { get; protected set; }
        public Vec3[] Normals { get; protected set; }
        public uint[] VertIndices { get; protected set; }
        public uint[] NormIndices { get; protected set; }
        public uint[] UVWIndices { get; protected set; }


        #endregion


        #region Constructors

        public Mesh(Vec3[] vertices, Vec3[] uvw, Vec3[] normals, uint[] vertIndices, uint[] uvwIndices, uint[] normIndices)
        {
            Vertices = vertices;
            UVW = uvw;
            Normals = normals;
            VertIndices = vertIndices;
            UVWIndices = uvwIndices;
            NormIndices = normIndices;
            _id = _globalMeshId++;
        }

        #endregion


        #region Public methods

        /// <returns>
        /// Tuple (PlainData, PlainIndecies).
        /// </returns>
        public BufferDataDesctiption GetBufferData() 
        {
            List<Vec3> result = new List<Vec3>();

            Action<int> addVnUnN = i =>
            {
                result.Add(Vertices[VertIndices[i]]);
                result.Add(UVW[UVWIndices[i]]);
                result.Add(Normals[NormIndices[i]]);
            };

            for (int i = 0; i < VertIndices.Length; i += 3)
            {
                // calculates tangent and bitangent
                var v0 = Vertices[VertIndices[i + 0]];
                var v1 = Vertices[VertIndices[i + 1]];
                var v2 = Vertices[VertIndices[i + 2]];
                var u0 = UVW[UVWIndices[i + 0]];
                var u1 = UVW[UVWIndices[i + 1]];
                var u2 = UVW[UVWIndices[i + 2]];
                Vec3 dpos1 = v1 - v0;
                Vec3 dpos2 = v2 - v0;
                Vec3 duv1 = u1 - u0;
                Vec3 duv2 = u2 - u0;
                float r = 1.0f / (duv1.X * duv2.Y - duv1.Y * duv2.X);
                Vec3 tangent = (dpos1 * duv2.Y - dpos2 * duv1.Y) * r;

                addVnUnN(i);
                result.Add(tangent);

                addVnUnN(i + 1);
                result.Add(tangent);

                addVnUnN(i + 2);
                result.Add(tangent);
            }

            var plainIndecies = Enumerable.Range(0, VertIndices.Length)
                .Select(x => (uint)x)
                .ToArray();
            return new BufferDataDesctiption(result.ToArray(), plainIndecies);
        }

        public bool Equals(Mesh other)
        {
            //return Vertices.SequenceEqual(other.Vertices)
            //    && UVW.SequenceEqual(other.UVW)
            //    && Normals.SequenceEqual(other.Normals)
            //    && NormIndices.SequenceEqual(other.NormIndices)
            //    && VertIndices.SequenceEqual(other.VertIndices)
            //    && UVWIndices.SequenceEqual(other.UVWIndices);
            return _id == other._id;
        }

        #endregion
    }
}