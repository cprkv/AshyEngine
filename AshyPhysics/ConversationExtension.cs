//  
// Created  : 27.03.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCommon.Math;
using AshyCore;
using BulletSharp;
using BulletSharp.Math;

namespace AshyPhysics
{
    public static class ConversationExtension
    {
        #region Vectors

        public static Vector3 Convert(this Vec3 vec) => new Vector3 ( vec.X, vec.Y, vec.Z );
                                                                                          
        public static Vec3 Convert(this Vector3 vec) => new Vec3    ( vec.X, vec.Y, vec.Z );
                                                                      
        public static Vec4 Convert(this Vector4 vec) => new Vec4    ( vec.X, vec.Y, vec.Z, vec.W );

        #endregion


        #region Matrices

        public static Matrix Convert(this Mat4 mat4)
        {
            return new Matrix(mat4[0, 0], mat4[0, 1], mat4[0, 2], mat4[0, 3],
                              mat4[1, 0], mat4[1, 1], mat4[1, 2], mat4[1, 3],
                              mat4[2, 0], mat4[2, 1], mat4[2, 2], mat4[2, 3],
                              mat4[3, 0], mat4[3, 1], mat4[3, 2], mat4[3, 3]);
        }

        public static Mat4 Convert(this Matrix mat4)
        {
            return new Mat4(mat4.Column1.Convert(), 
                            mat4.Column2.Convert(),
                            mat4.Column3.Convert(), 
                            mat4.Column4.Convert());
        }

        #endregion


        #region Quaternions

        public static Quaternion Convert(this Quat quat) => new Quaternion(quat.X, quat.Y, quat.Z, quat.W);

        public static Quat Convert(this Quaternion quat) => new Quat(quat.X, quat.Y, quat.Z, quat.W);

        #endregion


        #region Meshes

        public static TriangleMesh Convert(this Mesh mesh)
        {
            var result = new TriangleMesh();
            for (var i = 0; i < mesh.VertIndices.Length; i += 3)
            {
                result.AddTriangle(
                    mesh.Vertices[mesh.VertIndices[i]]      .Convert(),
                    mesh.Vertices[mesh.VertIndices[i + 1]]  .Convert(),
                    mesh.Vertices[mesh.VertIndices[i + 2]]  .Convert());
            }
            return result;
        }

        #endregion
    }
}