using AshyCommon.Math;
using AshyCore;
using AshyCore.EntitySystem;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using AshyRenderGL.Light;

namespace AshyRenderGL
{
    public class DepthMap
    {
        private int depthMapBufferId;
        public int DepthMapId { get; private set; }
        public int DepthCubeMapId { get; private set; }
        private RenderingScene _scene;

        public DepthMap(RenderingScene scene)
        {
            _scene = scene;
            Init();
        }

        private void Init()
        {
            DepthMapId = GL.GenTexture();
            depthMapBufferId = GL.GenFramebuffer();
            DepthCubeMapId = GL.GenTexture();

            #region 2D Texture for depth map 
            GL.BindTexture(TextureTarget.Texture2D, DepthMapId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent,
                Engine.I.GameWindow.Width, Engine.I.GameWindow.Height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new [] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.BindTexture(TextureTarget.Texture2D, 0);
            #endregion

            #region cubemap texture for depth map
            GL.BindTexture(TextureTarget.TextureCubeMap, DepthCubeMapId);
            for (int i = 0; i < 6; ++i)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.DepthComponent,
                    Engine.I.GameWindow.Width, Engine.I.GameWindow.Height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToBorder);
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            #endregion
        }

        public static ShaderProgram LoadSpecificShader(string name, string geometryShaderName = null)
        {
            var shaderArray = new[]
            {
                new Shader($@"Shaders\{name}.vs", ShaderType.VertexShader),
                new Shader($@"Shaders\{name}.fs", ShaderType.FragmentShader)
            };
            if (geometryShaderName != null)
            {
                shaderArray = new[]
                {
                    shaderArray[0], shaderArray[1],
                    new Shader($@"Shaders\{geometryShaderName}.gs", ShaderType.GeometryShader)
                };
            }
            var shaderProgram = new ShaderProgram(new Dictionary<string, string>(), shaderArray).Compile();
            GL.DeleteShader(shaderArray[0].Id);
            GL.DeleteShader(shaderArray[1].Id);
            if (geometryShaderName != null)
            {
                GL.DeleteShader(shaderArray[2].Id);
            }
            return shaderProgram;
        }

        public void FillDepthCubeMap(PointLightCaster light)
        {
            GL.BindTexture(TextureTarget.ProxyTextureCubeMap, DepthCubeMapId);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapBufferId);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.TextureCubeMap, DepthMapId, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            RenderAPI.I.Core.Log.Info($"[Render] Depth buffer cubemap status: {GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer)}");
            //set shader
            var shaderProgram = LoadSpecificShader("depthCubeMap", "depthCubeMap");
            if (!shaderProgram.CanBeUsed) throw new Exception($"FillDepthCubeMap: shader can't be used");
            shaderProgram.Use();
            SetShadowTransforms(light, shaderProgram);

            #region fill depthmap

            foreach (var entity in _scene.RenderableEntities)
            {
                var renderComponent = entity.Value;
                GL.BindVertexArray(Engine.I.Device.Buffers[renderComponent.Mesh.Id].VertexArrayObjectId);

                GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "model"), 1, false, entity.Key.TransformMatrix.Values);

                if (renderComponent.Material.HasDiffuse) Engine.I.Device.Textures[renderComponent.Material.Diffuse].Bind(0);
                if (renderComponent.Material.HasNormal) Engine.I.Device.Textures[renderComponent.Material.Normal].Bind(1);

                GL.DrawArrays(PrimitiveType.Triangles, 0, renderComponent.Mesh.IndexLength);
            }

            #endregion

            GL.BindTexture(TextureTarget.ProxyTextureCubeMap, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
        }

        private void SetShadowTransforms(PointLightCaster light, ShaderProgram shaderProgram)
        {
            Func<Vec3, Vec3, Mat4> getView = (direction, up) => Transform.LookAt(light.Position, light.Position + direction, up);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "shadowMatrices[0]"), 1, false,
                (getView(new Vec3(1.0f, 0.0f, 0.0f), new Vec3(0.0f, -1.0f, 0.0f)) * light.Proj).Values);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "shadowMatrices[1]"), 1, false,
                (getView(new Vec3(-1.0f, 0.0f, 0.0f), new Vec3(0.0f, -1.0f, 0.0f)) * light.Proj).Values);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "shadowMatrices[2]"), 1, false,
                (getView(new Vec3(0.0f, 1.0f, 0.0f), new Vec3(0.0f, 0.0f, 1.0f)) * light.Proj).Values);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "shadowMatrices[3]"), 1, false,
                (getView(new Vec3(0.0f, -1.0f, 0.0f), new Vec3(0.0f, 0.0f, -1.0f)) * light.Proj).Values);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "shadowMatrices[4]"), 1, false,
                (getView(new Vec3(0.0f, 0.0f, 1.0f), new Vec3(0.0f, -1.0f, 0.0f)) * light.Proj).Values);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "shadowMatrices[5]"), 1, false,
                (getView(new Vec3(0.0f, 0.0f, -1.0f), new Vec3(0.0f, -1.0f, 0.0f)) * light.Proj).Values);
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ProgramId, "lightPos"), 1, light.Position.Values);
            GL.Uniform1(GL.GetUniformLocation(shaderProgram.ProgramId, "far_plane"), 1, new[] { light.zFar });

        }

        /// <summary>
        /// Fill depth map by framebuffer (depthMapBufferId) to  depthMap 2d texture(DepthMapId)
        /// </summary>
        /// <param name="lightSpaceMatrix">View from which map filled</param>
        public void FillDepthMap(Mat4 lightSpaceMatrix)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.BindTexture(TextureTarget.Texture2D, DepthMapId);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapBufferId);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthMapId, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            RenderAPI.I.Core.Log.Info($"[Render] Depth buffer status: {GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer)}");
            //set shader
            var shaderProgram = LoadSpecificShader("depthMapShader");
            if (!shaderProgram.CanBeUsed) throw new Exception("FillDepthMap: shader can't be used");
            shaderProgram.Use();

            #region fill depthmap

            foreach (var entity in _scene.RenderableEntities)
            {
                var renderComponent = entity.Value;
                GL.BindVertexArray(Engine.I.Device.Buffers[renderComponent.Mesh.Id].VertexArrayObjectId);

                GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "model"), 1, false, entity.Key.TransformMatrix.Values);
                GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ProgramId, "lightSpaceMatrix"), 1, false, lightSpaceMatrix.Values);

                if (renderComponent.Material.HasDiffuse) Engine.I.Device.Textures[renderComponent.Material.Diffuse].Bind(0);
                if (renderComponent.Material.HasNormal) Engine.I.Device.Textures[renderComponent.Material.Normal].Bind(1);

                GL.DrawArrays(PrimitiveType.Triangles, 0, renderComponent.Mesh.IndexLength);
            }

            #endregion

            shaderProgram.Free();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
        }


        /// <summary>
        /// Draw depthmap on rectangle
        /// </summary>
        public void DrawDepthMap()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, DepthMapId);
            var sp = LoadSpecificShader("DepthMapD");
            sp.Use();
            DrawRectangle();
            sp.Free();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        ///!! need properly shaders!!!!
        /// </summary>
        public void DrawDepthCubeMap()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindTexture(TextureTarget.ProxyTextureCubeMap, DepthCubeMapId);
            var sp = LoadSpecificShader("DepthMapD");
            sp.Use();
            DrawRectangle();
            sp.Free();
            GL.BindTexture(TextureTarget.ProxyTextureCubeMap, 0);
        }


        /// <summary>
        /// Draw rectangle on all screen (used for depthmap drawing)
        /// !!SPECEFIE Framebuffer, shaders, texture before invoking!!
        /// </summary>
        private void DrawRectangle()
        {
            GL.ClearColor(Color.Wheat);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            float[] vertices =  
                {
                     // Positions        // Texture Coords
                     1.0f,  1.0f, 0.0f,  1.0f, 1.0f,   // Top Right
                     1.0f, -1.0f, 0.0f,  1.0f, 0.0f,   // Bottom Right
                    -1.0f, -1.0f, 0.0f,  0.0f, 0.0f,   // Bottom Left
                    -1.0f,  1.0f, 0.0f,  0.0f, 1.0f    // Top Left 
                };
            uint[] indexs = { 2, 1, 0, 0, 3, 2 };
            var vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * 5 * 4, vertices, BufferUsageHint.StaticDraw);
            var indexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 6 * sizeof(uint), indexs, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * 4, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * 4, 3 * 4);
            GL.EnableVertexAttribArray(1);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(indexBufferId);
        }

        internal void Release()
        {
            GL.DeleteFramebuffer(depthMapBufferId);
            GL.DeleteTexture(DepthMapId);
            GL.DeleteTexture(DepthCubeMapId);
        }
    }
}