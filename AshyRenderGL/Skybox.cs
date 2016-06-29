//  
// Created  : 17.05.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using AshyCommon.Math;
using AshyCore;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace AshyRenderGL
{
    public class Skybox
    {
        #region Properties

        private Mesh _cube;
        public ShaderProgram ShaderProgram { get; private set; }
        //public Render.BuffersLayout Buf { get; private set; }
        private int TextureId { get; }

        #endregion


        #region Constructors

        private Skybox(int textureId)
        {
            TextureId = textureId;
        }

        #endregion


        #region Public methods

        public void BindAndRender()
        {
            //GL.BindVertexArray(Buf.VertexArrayObjectId);
            GL.BindTexture(TextureTarget.TextureCubeMap, TextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _cube.VertIndices.Length);
            GL.BindVertexArray(0);
        }

        public static Skybox Load(params string[] path)
        {
            throw new NotImplementedException();
            //var textures = path.Select(x => RenderAPI.Instance.Resource.Get<Texture>(
            //    $"Textures/{x}", AshyCore.Resource.ResourceTarget.LoadedLevelPrivateRender))
            //    .ToArray();
            //return Load(textures);
        }

        private static Skybox Load(params Texture[] textures)
        {
            Skybox result = new Skybox(GL.GenTexture());

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, result.TextureId);

            Func<int, Action<BitmapData>> procFunc =
                iter => data => 
                    GL.TexImage2D(
                        TextureTarget.TextureCubeMapPositiveX + iter, 0, PixelInternalFormat.Rgba,
                        data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte,
                        data.Scan0
                    );

            for (   int j = 0; j < textures.Length; j++)
            {
                textures[j].ProcessData(procFunc(j));
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureParameterName.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureParameterName.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureParameterName.ClampToEdge);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0); 

            //result.LoadVO();

            return result;
        }

        //private void LoadVO()
        //{
        //    _cube = RenderAPI.Instance.Resource.Get<Mesh>("Meshes/cube", AshyCore.Resource.ResourceTarget.LoadedLevelPrivateRender);

        //    Buf = new Render.BuffersLayout();
        //    var data = _cube.GetBufferData();

        //    Buf.VertexBufferId = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, Buf.VertexBufferId);
        //    GL.BufferData(BufferTarget.ArrayBuffer, data.SizeOfVertices, data.Data, BufferUsageHint.StaticDraw);

        //    Buf.IndexBufferId = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, Buf.IndexBufferId);
        //    GL.BufferData(BufferTarget.ElementArrayBuffer, data.SizeOfIndices, data.Indecies, BufferUsageHint.StaticDraw);

        //    Buf.VertexArrayObjectId = GL.GenVertexArray();
        //    GL.BindVertexArray(Buf.VertexArrayObjectId);

        //    var attribLength = 4;

        //    for (int i = 0; i < attribLength; i++) // enable attributes for (position uvw normal tangent bitangent)
        //    {
        //        GL.EnableVertexAttribArray(i);
        //    }
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, Buf.VertexBufferId);

        //    for (int i = 0; i < attribLength; i++) // setting attributes sizes
        //    {
        //        GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, Vec3.SizeInBytes * attribLength, Vec3.SizeInBytes * i);
        //    }
        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, Buf.IndexBufferId);

        //    var shaderArray = new[]
        //    {
        //        new Shader(@"Shaders\skybox.vs", ShaderType.VertexShader),
        //        new Shader(@"Shaders\skybox.fs", ShaderType.FragmentShader)
        //    };
        //    ShaderProgram = new ShaderProgram(new Dictionary<string, string>
        //    {
        //        { "viewProjectionMat", "Mat4" },
        //        { "ambientLight", "Vec3" },
        //        { "lightPos", "Vec3" },
        //        { "modelMat", "Mat4" }
        //    }, shaderArray).Compile();
        //}

        #endregion 
    }
}