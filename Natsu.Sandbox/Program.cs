﻿
using System;
using System.Diagnostics;
using System.Numerics;

using Natsu.Graphics;
using Natsu.Platforms.Skia;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SkiaSharp;

namespace Natsu.Sandbox;

public class AppWindow() : GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default) {

    #nullable disable
    public SkiaApplication App;
    private GRContext _context;
    private GRGlInterface _interface;
    private GRBackendRenderTarget _target;
    private SKSurface _surface;
    #nullable restore

    public void CreateSurface(int width, int height) {
        lock (this) {
            _surface?.Dispose();
            _target = new GRBackendRenderTarget(width, height, 0, 8, new(0, (uint)SizedInternalFormat.Rgba8));
            _surface = SKSurface.Create(_context, _target, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);

            if (App == null) {
                App = new(_surface);
            } else {
                App.LoadRenderer(_surface);
            }
        }
    }

    protected override void OnLoad() {
        base.OnLoad();

        _interface = GRGlInterface.Create();
        _context = GRContext.CreateGl(_interface);
        CreateSurface(Size.X, Size.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);
        App.Render();
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e) {
        base.OnFramebufferResize(e);
        CreateSurface(e.Width, e.Height);
        App.Resize(e.Width, e.Height);
    }
}

public static unsafe class Program {
    public static void Main(string[] args) => new AppWindow().Run();
}