//  
// Created  : 03.06.2016
// Author   : Compiles
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using AshyCommon.Math;
using OpenTK.Graphics.OpenGL4;

namespace AshyRenderGL
{
    [Obsolete("Too slooooow", true)]
    public class Particle : IComparable
    {
        private Vec3 Pos { get; set; }
        private Vec3 Speed { get; set; }
        private Vec4 _color;
        private static int MaxParticles { get; } = 5555;
        private float Size { get; set; }
        private float Life { get; set; }
        private float Cameradistance { get; set; }
        private readonly Particle[] _particlesContainer = new Particle[MaxParticles];
        private readonly float[] _gParticlePositionSizeData = new float[MaxParticles * 4];
        private static readonly byte[] GParticleColorData = new byte[MaxParticles * 4];
        private int _particlesPositionBuffer;
        private int _billboardVertexBuffer;
        private int _particlesColorBuffer;
        private int _particlesCount;
        public ShaderProgram ShaderProgram { get; private set; }

        public void Load()
        { 
            int vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayId);

            for (int i = 0; i < MaxParticles; i++)
            {
                _particlesContainer[i] = new Particle
                {
                    Life = -1.0f,
                    Cameradistance = -1.0f
                };
            }

            var gVertexBufferData = new[]
            {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f,
                 0.5f,  0.5f, 0.0f
            };

            GL.GenBuffers(1, out _billboardVertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _billboardVertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, gVertexBufferData.Length * sizeof(float), gVertexBufferData, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out _particlesPositionBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _particlesPositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxParticles * 4 * sizeof(float), (IntPtr)null, BufferUsageHint.StreamDraw);

            GL.GenBuffers(1, out _particlesColorBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _particlesColorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxParticles * 4 * sizeof(byte), (IntPtr)null, BufferUsageHint.StreamDraw);

             var shaderArray = new[]
            {
                new Shader(@"Shaders\particle.vs", ShaderType.VertexShader),
                new Shader(@"Shaders\particle.fs", ShaderType.FragmentShader)
            };
            ShaderProgram = new ShaderProgram(new Dictionary<string, string>
            {
                { "CameraRight_worldspace", "Vec3" },
                { "CameraUp_worldspace", "Vec3" },
                { "VP", "Mat4" },

            }, shaderArray).Compile();
        }

        public void SortParticles()
        {
            Array.Sort(_particlesContainer);
        }

        public void Update()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _particlesPositionBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxParticles * 4 * sizeof(float), (IntPtr)null, BufferUsageHint.StreamDraw); 
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, _particlesCount * sizeof(float) * 4, _gParticlePositionSizeData);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _particlesColorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxParticles * 4 * sizeof(byte), (IntPtr)null, BufferUsageHint.StreamDraw);  
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, _particlesCount * sizeof(byte) * 4, GParticleColorData);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            RenderTexture.Load("particle").Bind(0);
           
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _billboardVertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, (IntPtr)0) ;

            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _particlesPositionBuffer);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, (IntPtr)0);

            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _particlesColorBuffer);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Byte, true, 0, (IntPtr)0);

            GL.VertexAttribDivisor(0, 0);
            GL.VertexAttribDivisor(1, 1);
            GL.VertexAttribDivisor(2, 1);

            GL.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, _particlesCount);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
        }

        public void GenParticles()
        {
            Random rand = new Random();
            _particlesContainer[0].Life = 2f;
            _particlesContainer[0].Pos = new Vec3(
                rand.Next(-50, 50),
                rand.Next(20, 40),
                rand.Next(-50, 50)
                );

            float spread = 20;
            var maindir = new Vec3(0.0f, -10f, 0.0f);

            var randomdir = new Vec3(
                (rand.Next()%2000 - 1000.0f)/1000.0f,
                (rand.Next()%2000 - 1000.0f)/1000.0f,
                (rand.Next()%2000 - 1000.0f)/1000.0f
                );

            _particlesContainer[0].Speed = maindir + randomdir*spread;

            _particlesContainer[0]._color.X = 100;
            _particlesContainer[0]._color.Y = 100;
            _particlesContainer[0]._color.Z = 100;
            _particlesContainer[0]._color.W = rand.Next(0, 125);

            _particlesContainer[0].Size = (rand.Next()%1000)/2000.0f + 0.1f;
        }

        public void SimulateParticles(float delta, Vec3 cameraPos)
        {
            _particlesCount = 0;
            for (int i = 0; i < MaxParticles; i++)
            {
                Particle p = _particlesContainer[i]; 
                if (p.Life > 0.0f)
                {
                    p.Life -= delta;
                    if (p.Life > 0.0f)
                    {
                        p.Speed += new Vec3(0.0f, -9.81f, 0.0f) *  delta * 0.5f;
                        p.Pos += p.Speed * (float)delta;
                        p.Cameradistance = (p.Pos - cameraPos).LenSqr;

                        _gParticlePositionSizeData[4 * _particlesCount + 0] = p.Pos.X;
                        _gParticlePositionSizeData[4 * _particlesCount + 1] = p.Pos.Y;
                        _gParticlePositionSizeData[4 * _particlesCount + 2] = p.Pos.Z;

                        _gParticlePositionSizeData[4 * _particlesCount + 3] = p.Size;

                        GParticleColorData[4 * _particlesCount + 0] = (byte)p._color.X;
                        GParticleColorData[4 * _particlesCount + 1] = (byte)p._color.Y;
                        GParticleColorData[4 * _particlesCount + 2] = (byte)p._color.Z;
                        GParticleColorData[4 * _particlesCount + 3] = (byte)p._color.W;

                    }
                    else
                        p.Cameradistance = -1.0f;
                    _particlesCount++;
                }
            }
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Particle))
                 throw  new Exception("cant compare");
            return Cameradistance.CompareTo(((Particle) obj).Cameradistance);
        }
    }
}