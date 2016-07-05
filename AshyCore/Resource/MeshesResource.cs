// 
// Created : 14.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AshyCommon.Math;

namespace AshyCore.Resource
{
    /// <summary>
    /// Represents Wavefront format (.obj) resource.
    /// <see cref="Resource.RC"/> returns <see cref="Mesh"/>.
    /// </summary>
    public class MeshesResource : Resource
    {
        #region Properties

        public static readonly string FileExtension = "obj";

        private static CultureInfo _parsingCulture = null;

        private static CultureInfo GetParsingCulture
        {
            get
            {
                if (_parsingCulture == null)
                {
                    _parsingCulture = (CultureInfo) CultureInfo.CurrentCulture.Clone();
                    _parsingCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                }
                return          ( _parsingCulture );
            }
        }

        #endregion


        #region Constructors

        public MeshesResource(string path, ResourceTarget target, VFS.IFileSystem fs)
            : base(path, target, fs)
        {
        }

        #endregion


        #region Methods

        public override object Load(string path, VFS.IFileSystem fs)
        {
            var obj             = fs.ReadLines($"{path}.{FileExtension}");
            var vert            = new List<Vec3>();
            var uvw             = new List<Vec3>();
            var normals         = new List<Vec3>();
            var vertexIndices   = new List<uint>();
            var normalIndices   = new List<uint>();
            var uvwIndices      = new List<uint>();

            var lines = obj
                .Select(s => s.Split(new[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(arr => arr.Length != 0);

            foreach (var str in lines)
            {
                if (str[0] == "v")
                {
                    vert.Add(new Vec3(float.Parse(str[1], NumberStyles.Any, GetParsingCulture),
                                      float.Parse(str[2], NumberStyles.Any, GetParsingCulture),
                                      float.Parse(str[3], NumberStyles.Any, GetParsingCulture))
                            );
                }
                if (str[0] == "vn")
                {
                    normals.Add(new Vec3(float.Parse(str[1], NumberStyles.Any, GetParsingCulture),
                                         float.Parse(str[2], NumberStyles.Any, GetParsingCulture),
                                         float.Parse(str[3], NumberStyles.Any, GetParsingCulture))
                                );
                }
                if (str[0] == "vt")
                {
                    uvw.Add(new Vec3(    float.Parse(str[1], NumberStyles.Any, GetParsingCulture),
                                     1 - float.Parse(str[2], NumberStyles.Any, GetParsingCulture),
                                     0.0f)                                  // todo: delete this zero!!
                            );            
                }
                if (str[0] == "f" && str.Length-1 == 6)                       // w/out texture coords
                {
                    vertexIndices   .Add(uint.Parse(str[1]) - 1);
                    normalIndices   .Add(uint.Parse(str[2]) - 1);
                    
                    vertexIndices   .Add(uint.Parse(str[3]) - 1);
                    normalIndices   .Add(uint.Parse(str[4]) - 1);

                    vertexIndices   .Add(uint.Parse(str[5]) - 1);
                    normalIndices   .Add(uint.Parse(str[6]) - 1);
                }
                if (str[0] == "f" && str.Length-1 == 9)                       // with texture coords
                {
                    vertexIndices   .Add(uint.Parse(str[1]) - 1);
                    uvwIndices      .Add(uint.Parse(str[2]) - 1);
                    normalIndices   .Add(uint.Parse(str[3]) - 1);

                    vertexIndices   .Add(uint.Parse(str[4]) - 1);
                    uvwIndices      .Add(uint.Parse(str[5]) - 1);
                    normalIndices   .Add(uint.Parse(str[6]) - 1);

                    vertexIndices   .Add(uint.Parse(str[7]) - 1);
                    uvwIndices      .Add(uint.Parse(str[8]) - 1);
                    normalIndices   .Add(uint.Parse(str[9]) - 1);
                }
            }

            if (uvw.Count == 0)
            {
                uvwIndices      = vertexIndices;
                uvw             = vert;
            }

            return new Mesh(
                vert            .ToArray(), 
                uvw             .ToArray(), 
                normals         .ToArray(),
                vertexIndices   .ToArray(), 
                uvwIndices      .ToArray(), 
                normalIndices   .ToArray()
                );
        }

        #endregion
    }
}