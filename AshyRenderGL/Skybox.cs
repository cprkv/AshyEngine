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
using AshyCommon;
using AshyCommon.Math;
using AshyCore;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace AshyRenderGL
{
    public class Skybox
    {
        #region Properties

        private Mesh                _cube;
        public ShaderProgram        ShaderProgram { get; private set; }
        public BuffersLayout        Buf { get; private set; }
        private int                 TextureId { get; }

        #endregion


        #region Constructors

        private Skybox(int textureId)
        {
            TextureId               = textureId;
        }

        #endregion


        #region Public methods

        public void BindAndRender()
        {
            if (_cube.IsNotSet())   return;

            GL.BindVertexArray      ( Buf.VertexArrayObjectId );
            GL.BindTexture          ( TextureTarget.TextureCubeMap, TextureId );
            GL.DrawArrays           ( PrimitiveType.Triangles, 0, _cube.VertIndices.Length );
            GL.BindVertexArray      ( 0 );
        }

        /// <summary>
        /// arguments: right left top bottom back front
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Skybox Load(params string[] path)
        {
            var textures            = path
                .Select             ( x => RenderAPI.I.Core.RM.Get<Texture>($"Textures/{x}", AshyCore.Resource.ResourceTarget.LoadedLevelPrivateRender) )
                .ToArray            ();

            return                  ( Load(textures) );
        }

        private static Skybox Load(params Texture[] textures)
        {
            Skybox result           = new Skybox(GL.GenTexture());


            GL.ActiveTexture        ( TextureUnit.Texture0 );
            GL.BindTexture          ( TextureTarget.TextureCubeMap, result.TextureId );

            Func<int, Action<BitmapData>> procFunc =
                iter => data => 
                    GL.TexImage2D(
                        TextureTarget.TextureCubeMapPositiveX + iter, 0, PixelInternalFormat.Rgba,
                        data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte,
                        data.Scan0
                    );

            for (int j = 0; j < textures.Length; j++)
            {
                textures[j].ProcessData(procFunc(j));
            }

            GL.TexParameter         ( TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear );
            GL.TexParameter         ( TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear );
                                      
            GL.TexParameter         ( TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureParameterName.ClampToEdge );
            GL.TexParameter         ( TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureParameterName.ClampToEdge );
            GL.TexParameter         ( TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureParameterName.ClampToEdge );

            GL.BindTexture          ( TextureTarget.TextureCubeMap, 0 ); 

            result.LoadVO           ();

            return                  ( result );
        }

        private void LoadVO()
        {
          //Setting buffers

            _cube                   = RenderAPI.I.Core.RM.Get<Mesh>("Meshes/cube", AshyCore.Resource.ResourceTarget.LoadedLevelPrivateRender);
            Buf                     = Engine.I.Device.LoadMesh(_cube);

          //Creating shader program 

            var uniformTypes        = new Dictionary<string, string>
            {
                { "viewProjectionMat",  "Mat4" },
                { "ambientLight",       "Vec3" },
                { "lightPos",           "Vec3" },
                { "modelMat",           "Mat4" }
            };
            var skyShader           = new ShaderAlias(@"skybox", @"skybox", uniformTypes);

            ShaderProgram           = ShaderProgram.Parse(skyShader).Compile();
        }

        #endregion 
    }
}