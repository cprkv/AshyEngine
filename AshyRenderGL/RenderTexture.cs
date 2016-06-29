// 
// Created : 29.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using AshyCore;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL
{
    public class RenderTexture
    {
        #region Properties

        private int TextureId { get; }

        #endregion
        
        
        #region Constructors

        private RenderTexture(int textureId)
        {
            TextureId = textureId;
        }

        #endregion


        #region Public methods

        public void Bind(int unit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
        }

        public static RenderTexture Load(string path)
        {
            RenderTexture result = new RenderTexture(GL.GenTexture());

            GL.BindTexture(TextureTarget.Texture2D, result.TextureId);

            RenderAPI.I.Core.RM.Get<Texture>($"Textures/{path}", AshyCore.Resource.ResourceTarget.LoadedLevelPrivateRender)
                .ProcessData(data =>
                GL.TexImage2D(
                    TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte,
                    data.Scan0
                    )
                );

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return result;
        }

        public static RenderTexture Load(Texture texture)
        {
            RenderTexture result = new RenderTexture(GL.GenTexture());

            GL.BindTexture(TextureTarget.Texture2D, result.TextureId);

            texture.ProcessData(data =>
                GL.TexImage2D(
                    TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte,
                    data.Scan0
                    )
                );

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return result;
        }

        #endregion
    }
}