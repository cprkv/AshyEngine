// 
// Created : 04.07.2016
// Author  : Veyroter
// Copyright (C) AshyCat 2016
// This product are licensed under MICROSOFT REFERENCE SOURCE LICENSE(MS-RSL).
// 

using System.Drawing;

namespace AshyCore
{
    #region Handlers

    /// <param name="dtime">In seconds.</param>
    public delegate void        DrawFrameFunc(float dtime);

    /// <param name="coords">Screen coordinates.</param>
    public delegate void        MouseDownFunc(Input.MouseButton button, Point coords);

    /// <param name="coords">Screen coordinates.</param>
    public delegate void        MouseUpFunc(Input.MouseButton button, Point coords);

    public delegate void        KeyDownFunc(Input.Key button);

    public delegate void        KeyUpFunc(Input.Key button);

    public delegate void        ResizeFunc(Size size);

    /// <param name="coords">Screen coordinates.</param>
    public delegate void        MouseMoveFunc(Point delta, Point coords);

    #endregion

    public interface IWindow
    {
        event DrawFrameFunc     RenderFrame;
        event MouseDownFunc     MouseButtonDown;
        event MouseUpFunc       MouseButtonUp;
        event MouseMoveFunc     MouseMove;
        event KeyDownFunc       KeyDown;
        event KeyUpFunc         KeyUp;
        event ResizeFunc        Resize;

        int                     Width { get; }
        int                     Height { get; }
        bool                    CaptureMouse { get; set; } 
        float                   Framerate { get; set; }

        bool[]                  PressedKeys { get; }

        /// <summary>
        /// Runs main loop.
        /// </summary>
        void                    Run();
    }
}
