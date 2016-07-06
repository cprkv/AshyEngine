// 
// Created : 18.03.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using AshyCommon;
using AshyCommon.Math;
using AshyCore;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using AshyUI;
using AshyUI.Graphics;
using System.Windows.Forms;
using AshyCore.EntitySystem;
using ShadingModel = OpenTK.Graphics.OpenGL.ShadingModel;
using Uniform = System.Collections.Generic.KeyValuePair<string, float[]>;

namespace AshyRenderGL
{
    // todo разделить на шаги и сказать что это рендерплатформ
    //public class Render
    //{
    //    #region Nested classes

    //    public class BuffersLayout
    //    {
    //        public int VertexArrayObjectId { get; set; }
    //        public int VertexBufferId { get; set; }
    //        public int IndexBufferId { get; set; }
    //    }

    //    #endregion


    //    #region Properties

    //    private readonly Vec3     _lightPosition = new Vec3(-5.0f, 5.0f, -5.0f);    

    //    public float              Framerate { get; set; }     
    //    private Point             _windowCenter;              
    //    public GameWindow         Window;                     
    //    private float             _mouseSense;                
    //    private bool              _captureMouse = true;       

    //    private Camera Camera => RenderAPI.Instance.Level.Player.Camera;

    //    private readonly Dictionary<Mesh, BuffersLayout>        _buffers        = new Dictionary<Mesh, BuffersLayout>();
    //    private readonly Dictionary<Texture, RenderTexture>     _textures       = new Dictionary<Texture, RenderTexture>();
    //    private readonly Dictionary<ShaderAlias, ShaderProgram> _shaderPrograms = new Dictionary<ShaderAlias, ShaderProgram>();

    //    private int               _lastcube = 1;
    //    private UITexture         _ui;
    //    private Skybox            _skybox;
    //    //private Particle        _particle;
    //    private Vec3              _brightness = new Vec3(0.2f, 0.2f, 0.4f);
    //    private Stopwatch         _timer;
    //    private float             _lastime;

    //    #endregion


    //    #region Constructors and destructors

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="Render"/> class.
    //    /// </summary>
    //    /// <param name="eh">The frame event handler which calls when frame rendering starded.</param>
    //    /// <param name="framerate">Needed framerate.</param>
    //    public Render(EventHandler<float> eh, float framerate = 60.0f) 
    //    {
    //        // TODO должно быть в обёртке окна
    //        Framerate = framerate;
    //        var gameInfo = RenderAPI.Instance.Resource.Get<ConfigTable>(
    //            "Config/GameInfo", AshyCore.Resource.ResourceTarget.Engine)["Game"];
    //        var userInfo = RenderAPI.Instance.UserConfig["User"];

    //        Window = new GameWindow(
    //            userInfo["WindowSizeX"]?.AsInt() ?? 800,
    //            userInfo["WindowSizeY"]?.AsInt() ?? 600,
    //            GraphicsMode.Default,
    //            gameInfo["Title"],
    //            (GameWindowFlags)(userInfo["FullScreen"]?.AsInt() ?? (int?)0).Value, 
    //            DisplayDevice.Default);

    //        UI.Instance.Init((uint)Window.Width, (uint)Window.Height);

    //        Window.Load         += Load;
    //        Window.RenderFrame  += DrawFrame;
    //        Window.Mouse.ButtonDown += Mouse_ButtonDown;
    //        Window.Mouse.ButtonUp += Mouse_ButtonUp;
    //        Window.RenderFrame  += (sender, e) => eh(sender, (float)e.Time);
    //        Window.Resize       += (sender, e) =>
    //        {
    //            _windowCenter = new Point(Window.Width / 2, Window.Height / 2);
    //            GL.Viewport(0, 0, Window.Width, Window.Height);
    //            UI.Instance.ReSize((uint)Window.Width, (uint)Window.Height);
    //        };
    //        Window.KeyDown      += (sender, e) =>
    //        {
    //            if (e.Key == Key.Escape) Window.Close();
    //            if (e.Key == Key.F1)
    //            {
    //                _captureMouse = !_captureMouse;
    //                Window.CursorVisible = !Window.CursorVisible;
    //            }
    //            if (e.Key == Key.F2)
    //            {
    //                var cube = RenderAPI.Instance.Level.GetEntity($"cube_000{_lastcube}");
    //                RenderAPI.Instance.Level.Spawn(
    //                    new Entity(cube, 
    //                               $"cube_000{++_lastcube}", 
    //                               cube.Position+new Vec3(0f,5f,0f), 
    //                               cube.Scale, 
    //                               cube.Rotation.ToEulerAngles()));
    //            }
    //            if (e.Key == Key.F3)
    //            {
    //                RenderAPI.Instance.Level.Entities.RemoveAll(
    //                    entity => entity.Name.Contains("cube") && !entity.Name.Contains("cube_0001"));
    //                _lastcube = 1;
    //            }
    //            RenderAPI.Instance.PressedKeys[(int)e.Key] = true;
    //        };
    //        Window.KeyUp += (sender, e) =>
    //        {
    //            RenderAPI.Instance.PressedKeys[(int)e.Key] = false;
    //        };

    //        UI.Instance.menuItemFileExit.MousePressedEvent += (sender) => Window.Close();
    //        UI.Instance.progressNSliderTestDialog.horizontalSBar.Slider.DragMovedEvent += BrightnessControl;

    //        Window.CursorVisible = false;
    //        _windowCenter = new Point(Window.Width / 2, Window.Height / 2);
    //        _mouseSense = userInfo["MouseSense"]?.AsFloat() ?? 0.005f;
    //    }

    //    ~Render()
    //    {
    //        Window.Dispose();
    //    }

    //    #endregion


    //    #region Public methods

    //    /// <summary>
    //    /// Runs main loop.
    //    /// </summary>
    //    public void Run()
    //    {
    //        Window.VSync = VSyncMode.Adaptive;
    //        Window.Run(Framerate);
            
    //        Release();
    //    }

    //    public void DrawFrame(object sender, FrameEventArgs args)
    //    {
    //        if (_captureMouse) ApplyMouseMove(_mouseSense);

    //        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

    //        GL.Viewport(0, 0, Window.Width, Window.Height);

    //        GL.Disable(EnableCap.DepthTest);
    //        GL.Disable(EnableCap.CullFace);
    //        _skybox.ShaderProgram.Use();
    //        _skybox.ShaderProgram.SetUniform(
    //            new Uniform("lightPos", _lightPosition.Values),
    //            new Uniform("viewProjectionMat", Camera.View.Transpose().ClipRotation().Values),
    //            new Uniform("modelMat", Camera.Proj.Values),
    //            new Uniform("ambientLight", _lightPosition.Values)
    //            );

    //        _skybox.BindAndRender();

    //        GL.Disable(EnableCap.CullFace);
    //        GL.Enable(EnableCap.DepthTest);

    //        foreach (var entity in RenderAPI.Instance.Level.GetEntities(ComponentType.Render))
    //        {
    //            var renderComponent = GetRenderComponent(entity);

    //            GL.BindVertexArray(_buffers[renderComponent.Mesh].VertexArrayObjectId);

    //            // set shader
    //            var shaderProgram = _shaderPrograms[renderComponent.Material.Shader];
    //            if (!shaderProgram.CanBeUsed) continue;
    //            shaderProgram.Use();
    //            shaderProgram.SetUniform(
    //                new Uniform("lightPos",           _lightPosition.Values),
    //                new Uniform("viewProjectionMat",  Camera.ViewProj.Values),
    //                new Uniform("modelMat",           entity.TransformMatrix.Values),
    //                new Uniform("ambientLight",       (renderComponent.Material.Color).Values)
    //                );

    //            // bind texture
    //            if (renderComponent.Material.HasDiffuse) _textures[renderComponent.Material.Diffuse].Bind(0);
    //            if (renderComponent.Material.HasNormal) _textures[renderComponent.Material.Normal].Bind(1);

    //            GL.DrawArrays(PrimitiveType.Triangles, 0, renderComponent.Mesh.VertIndices.Length);
    //        }

    //        float currentTime = (float) _timer.Elapsed.TotalSeconds;
    //        float delta = currentTime - _lastime;
    //        _lastime = currentTime;
    //        GL.BindVertexArray(0);

    //        //_particle.GenParticles();
    //        //_particle.SimulateParticles(delta, Camera.Eye);
    //        //_particle.SortParticles();
    //        //_particle.ShaderProgram?.Use();
    //        //_particle.ShaderProgram?.SetUniform(
    //        //    new Uniform("CameraRight_worldspace", Camera.Right.Values),
    //        //    new Uniform("CameraUp_worldspace", Camera.Up.Values),
    //        //    new Uniform("VP", Camera.ViewProj.Values)
    //        //    );
    //        //_particle.Update();

    //        #region UI

    //        GL.Viewport(0, 0, 857 / 2, 190 / 2);
    //        GL.Disable(EnableCap.DepthTest);
    //        //GL.Disable(EnableCap.CullFace);
    //        GL.Enable(EnableCap.Blend);
    //        GL.BindVertexArray(_buffers[_ui.Mesh].VertexArrayObjectId);
    //        var shaderProgram1 = _shaderPrograms[_ui.Shader];
    //        if (shaderProgram1.CanBeUsed)
    //        {
    //            shaderProgram1.Use();
    //            _textures[_ui.Texture].Bind(0);
    //            GL.DrawArrays(PrimitiveType.Triangles, 0, _ui.Mesh.VertIndices.Length);
    //        }
    //        GL.Disable(EnableCap.Blend);
    //        GL.Enable(EnableCap.DepthTest);
            
    //        #endregion

    //        GL.BindVertexArray(0);
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    //        GL.UseProgram(0);
    //        UI.Instance.ImportMouseMotion(Window.Mouse.X, Window.Mouse.Y);
    //        UI.Instance.BeginPaint();
    //        UI.Instance.EndPaint();
            
    //        Window.SwapBuffers();
    //    }

    //    public void Release()
    //    {
    //        GL.UseProgram(0);
    //        _shaderPrograms.ForEach(p => p.Value.Free());
    //        TextureManager.Singleton.UnloadAll();
    //    }

    //    public void Register(Entity entity)
    //    {
    //        if (!entity.HasComponent(ComponentType.Render)) return;

    //        var renderComponent = GetRenderComponent(entity);

    //        // send buffers data to opengl
    //        if (!_buffers.ContainsKey(renderComponent.Mesh))
    //        {
    //            SendBuffers(renderComponent.Mesh);
    //            BindBuffers(renderComponent.Mesh);
    //        }

    //        // compiling shader programs and add to dic<shaderAlias, shaderProgram>
    //        if (renderComponent.Material.Shader != null)
    //            _shaderPrograms.GetOrAdd(renderComponent.Material.Shader, s => ShaderProgram.Parse(s).Compile());

    //        // loading textures
    //        if (renderComponent.Material.Diffuse != null)
    //            _textures.GetOrAdd(renderComponent.Material.Diffuse, RenderTexture.Load);
    //        if (renderComponent.Material.Normal != null)
    //            _textures.GetOrAdd(renderComponent.Material.Normal, RenderTexture.Load);
    //    }

    //    #endregion

        
    //    #region Private Methods

    //    private RenderComponent GetRenderComponent(Entity e) => e.Get<RenderComponent>(ComponentType.Render);

    //    private void ApplyMouseMove(float sense = 0.003f)
    //    {
    //        var mouseDelta = new Vec2(Window.Mouse.X - _windowCenter.X, Window.Mouse.Y - _windowCenter.Y);
    //        Camera.Rotate(mouseDelta * -sense);
    //        Cursor.Position = Window.PointToScreen(_windowCenter);
    //    }

    //    private void Load(object sender, EventArgs e)
    //    {
    //        _timer = new Stopwatch(); _timer.Start();
    //        SetupLevel();

    //        Window.Resize += (sender4, args) =>
    //        {
    //            Camera.Width = Window.Width;
    //            Camera.Height = Window.Height;
    //        };

    //        GL.Enable(EnableCap.DepthTest);
    //        //GL.Enable(EnableCap.CullFace);
    //        GL.Disable(EnableCap.CullFace);
    //        GL.Enable(EnableCap.Texture2D);
    //        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
    //        GL.Enable(EnableCap.Blend);
    //        GL.Enable(IndexedEnableCap.Blend, 0);

    //        SendDataToOpenGL();
    //        //_particle = new Particle();
    //        //_particle.Load();

    //        #region UI

    //        _ui = new UITexture(RenderAPI.Instance.Resource.Get<Texture>("Textures/hud", AshyCore.Resource.ResourceTarget.PrivateUI));
    //        _textures.Add(_ui.Texture, RenderTexture.Load(_ui.Texture));
    //        SendBuffers(_ui.Mesh);
    //        BindBuffers(_ui.Mesh);
    //        _shaderPrograms.Add(_ui.Shader, ShaderProgram.Parse(_ui.Shader).Compile());

    //        #endregion

    //        // right left top bottom back front
    //        _skybox = Skybox.Load(
    //            "sky_rt", "sky_lf", "sky_up",
    //            "sky_dn", "sky_bk", "sky_ft");

    //        GL.ClearColor(Color.FromArgb(87, 104, 100));
    //        OpenTK.Graphics.OpenGL.GL.ShadeModel(ShadingModel.Smooth);
    //        GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

    //        RenderAPI.Instance.Log.Info($"[Render] Started in {_timer.Elapsed.TotalSeconds} sec.");
    //        _timer.Restart();

    //        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
    //        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
    //        GC.WaitForPendingFinalizers();
    //        Process currentProc = Process.GetCurrentProcess();

    //        RenderAPI.Instance.Log.Info($"[Engine] Сollecting memory. End status: {currentProc.PrivateMemorySize64 / 1024 / 1024} Mb");
    //        GCSettings.LatencyMode = GCLatencyMode.LowLatency;
    //    }

    //    void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
    //    {
    //        UI.Instance.ImportMousePress(e.Button, e.X, e.Y);
    //    }

    //    void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
    //    {
    //        UI.Instance.ImportMouseRelease(e.Button, e.X, e.Y);
    //    }

    //    void BrightnessControl(object sender, int x, int y)
    //    {
    //        var val = UI.Instance.progressNSliderTestDialog.horizontalSBar.Value;
    //        _brightness = new Vec3(val / 50f + 0.5f, val / 50f + 0.5f, val / 50f + 0.5f);
    //    }

    //    private void SetupLevel()
    //    {
    //        var cam = Camera;
    //        cam.Width = Window.Width;
    //        cam.Height = Window.Height;
    //        cam.Dir = new Vec3(-0.051445f, -0.566747f, 0.822310f);
    //        cam.Eye = new Vec3(-3.910404f, 6.532519f, -8.444711f);
    //        cam.Right = new Vec3(-0.821857f, 0.000000f, -0.051417f);
    //    }

    //    private void SendDataToOpenGL()
    //    {
    //        RenderAPI.Instance.Level
    //            .GetEntities(ComponentType.Render)
    //            .ForEach(Register);
    //    }

    //    private void BindBuffers(Mesh mesh)
    //    {
    //        var buf = _buffers[mesh];

    //        buf.VertexArrayObjectId = GL.GenVertexArray();
    //        GL.BindVertexArray(buf.VertexArrayObjectId);

    //        var attribLength = 4;

    //        for (int i = 0; i < attribLength; i++) // enable attributes for (position uvw normal tangent bitangent)
    //        {
    //            GL.EnableVertexAttribArray(i);
    //        }
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, buf.VertexBufferId);

    //        for (int i = 0; i < attribLength; i++) // setting attributes sizes
    //        {
    //            GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, Vec3.SizeInBytes * attribLength, Vec3.SizeInBytes * i);
    //        }
    //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, buf.IndexBufferId);
    //    }

    //    private void SendBuffers(Mesh mesh)
    //    {
    //        _buffers.GetOrAdd(mesh, _=>new BuffersLayout());
    //        var buf = _buffers[mesh];
    //        var data = mesh.GetBufferData();

    //        buf.VertexBufferId = GL.GenBuffer();
    //        GL.BindBuffer(BufferTarget.ArrayBuffer, buf.VertexBufferId);
    //        GL.BufferData(BufferTarget.ArrayBuffer, data.SizeOfVertices, data.Data, BufferUsageHint.StaticDraw);

    //        buf.IndexBufferId = GL.GenBuffer();
    //        GL.BindBuffer(BufferTarget.ElementArrayBuffer, buf.IndexBufferId);
    //        GL.BufferData(BufferTarget.ElementArrayBuffer, data.SizeOfIndices, data.Indecies, BufferUsageHint.StaticDraw);
    //    }

    //    #endregion
    //}
}