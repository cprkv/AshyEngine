//   
// Created : 04.07.2016
// Author  : vadik
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
//  

using System;
using System.Collections.Generic;
using AshyCommon;
using AshyCommon.Math;
using AshyCore;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL
{
    public class BuffersLayout
    {
        public int VertexArrayObjectId { get; set; }
        public int VertexBufferId { get; set; }
        public int IndexBufferId { get; set; }
    }

    public class Device
    {
        public Dictionary<Mesh,         BuffersLayout> Buffers { get; private set; }
        public Dictionary<Texture,      RenderTexture> Textures { get; private set; }
        public Dictionary<ShaderAlias,  ShaderProgram> ShaderPrograms { get; private set; }

        public void Initialize()
        {
            Buffers                     = new Dictionary<Mesh, BuffersLayout>(32);
            Textures                    = new Dictionary<Texture, RenderTexture>(32);
            ShaderPrograms              = new Dictionary<ShaderAlias, ShaderProgram>(32);
        }

        public void Free()
        {
            
        }

        public BuffersLayout LoadMesh(Mesh mesh)
        {
            if ( ! Buffers.ContainsKey(mesh))
            {
                SetBufferAttributes     ( mesh );
                SetBuferData            ( mesh );
            }
            return                      ( Buffers[mesh] );
        } 


        #region Private Methods

        private void SetBufferAttributes(Mesh mesh)
        {
            var buf                     = Buffers.GetOrAdd(mesh, _ => new BuffersLayout());
            buf.VertexArrayObjectId     = GL.GenVertexArray();
            GL.BindVertexArray          ( buf.VertexArrayObjectId );

            var attribLength            = 4;

            for (int i = 0; i < attribLength; i++) // enable attributes for (position uvw normal tangent bitangent)
            {
                GL.EnableVertexAttribArray( i );
            }

            GL.BindBuffer               ( BufferTarget.ArrayBuffer, buf.VertexBufferId );

            for (int i = 0; i < attribLength; i++) // setting attributes sizes
            {
                GL.VertexAttribPointer  ( i, 3, VertexAttribPointerType.Float, false, Vec3.SizeInBytes * attribLength, Vec3.SizeInBytes * i );
            }

            GL.BindBuffer               ( BufferTarget.ElementArrayBuffer, buf.IndexBufferId );
        }

        private void SetBuferData(Mesh mesh)
        {
            Buffers.GetOrAdd    ( mesh, _ => new BuffersLayout() );
            var buf             = Buffers[mesh];
            var data            = mesh.GetBufferData();

            buf.VertexBufferId  = GL.GenBuffer();
            GL.BindBuffer       ( BufferTarget.ArrayBuffer, buf.VertexBufferId );
            GL.BufferData       ( BufferTarget.ArrayBuffer, data.SizeOfVertices, data.Data, BufferUsageHint.StaticDraw );

            buf.IndexBufferId   = GL.GenBuffer();
            GL.BindBuffer       ( BufferTarget.ElementArrayBuffer, buf.IndexBufferId );
            GL.BufferData       ( BufferTarget.ElementArrayBuffer, data.SizeOfIndices, data.Indecies, BufferUsageHint.StaticDraw );
        }

        #endregion
    }
}