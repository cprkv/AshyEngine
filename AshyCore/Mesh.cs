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
        public static readonly List<WeakReference> HoldingData = new List<WeakReference>();
        public Vec3[]               Data        { get; }
        public uint[]               Indecies    { get; }

        public BufferDataDesctiption(Vec3[] data, uint[] indecies)
        {
            Data                    = data;
            Indecies                = indecies;
            HoldingData.AddRange(new List<WeakReference>
            {
                new WeakReference( Data    , false ),
                new WeakReference( Indecies, false ),
            });
        }

        /// <summary>
        /// Size of vertices in bytes.
        /// </summary>
        public int SizeOfVertices   => Data.Length * Vec3.SizeInBytes;

        /// <summary>
        /// Size of vertIndices in bytes.
        /// </summary>
        public int SizeOfIndices    => Indecies.Length * sizeof(uint);
    }


    /// <summary>
    /// Class for 3D objects.
    /// </summary>
    /// TODO: make inherited classes which will adds Bitangents, Tangents, UVWs ... etc
    public class Mesh : IEquatable<Mesh>
    {
        #region Properties

        public static readonly List<WeakReference> HoldingData = new List<WeakReference>();
        private static long         _globalMeshId   = 0;
        public readonly long        Id;
        public Vec3[]               Vertices        { get; protected set; }
        public Vec3[]               UVW             { get; protected set; }
        public Vec3[]               Normals         { get; protected set; }
        public uint[]               VertIndices     { get; protected set; }
        public int                  IndexLength     { get; }
        public uint[]               NormIndices     { get; protected set; }
        public uint[]               UVWIndices      { get; protected set; }


        #endregion


        #region Constructors

        public Mesh(Vec3[] vertices, Vec3[] uvw, Vec3[] normals, uint[] vertIndices, uint[] uvwIndices, uint[] normIndices)
        {
            Vertices                = vertices;
            UVW                     = uvw;
            Normals                 = normals;
            VertIndices             = vertIndices;
            IndexLength             = VertIndices.Length;
            UVWIndices              = uvwIndices;
            NormIndices             = normIndices;
            Id                      = _globalMeshId++;
            HoldingData.AddRange(new List<WeakReference>
            {
                new WeakReference( Vertices   , false ),
                new WeakReference( UVW        , false ),
                new WeakReference( Normals    , false ),
                new WeakReference( VertIndices, false ),
                new WeakReference( UVWIndices , false ),
                new WeakReference( NormIndices, false ),
            });
        }

        #endregion


        #region Public methods

        /// <returns>
        /// Tuple (PlainData, PlainIndecies).
        /// </returns>
        public BufferDataDesctiption GetBufferData() 
        {
            var result              = new List<Vec3>();

            Action<int> addVnUnN    = i =>
            {
                result.Add          ( Vertices[VertIndices[i]] );
                result.Add          ( UVW[UVWIndices[i]] );
                result.Add          ( Normals[NormIndices[i]] );
            };

            for (int i = 0; i < VertIndices.Length; i += 3)
            {
                // calculates tangent and bitangent
                var v0              = Vertices[VertIndices[i + 0]];
                var v1              = Vertices[VertIndices[i + 1]];
                var v2              = Vertices[VertIndices[i + 2]];
                var u0              = UVW[UVWIndices[i + 0]];
                var u1              = UVW[UVWIndices[i + 1]];
                var u2              = UVW[UVWIndices[i + 2]];
                Vec3 dpos1          = v1 - v0;
                Vec3 dpos2          = v2 - v0;
                Vec3 duv1           = u1 - u0;
                Vec3 duv2           = u2 - u0;
                float r             = 1.0f / (duv1.X * duv2.Y - duv1.Y * duv2.X);
                Vec3 tangent        = (dpos1 * duv2.Y - dpos2 * duv1.Y) * r;

                addVnUnN            ( i );
                result.Add          ( tangent );
                                      
                addVnUnN            ( i + 1 );
                result.Add          ( tangent );
                                      
                addVnUnN            ( i + 2 );
                result.Add          ( tangent );
            }

            var plainIndecies       = Enumerable
                .Range              ( 0, VertIndices.Length )
                .Select             ( x => (uint)x )
                .ToArray            ();

            return                  ( new BufferDataDesctiption(result.ToArray(), plainIndecies) );
        }

        public bool Equals(Mesh other)
        {
            return                  ( Id == other.Id );
        }

        public void Free()
        {
            Vertices                = null;
            UVW                     = null;
            Normals                 = null;
            VertIndices             = null;
            UVWIndices              = null;
            NormIndices             = null;
        }

        #endregion
    }
}