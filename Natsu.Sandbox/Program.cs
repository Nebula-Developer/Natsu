
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

public class AppWindow() : GameWindow(new GameWindowSettings() {
    UpdateFrequency = 244
}, NativeWindowSettings.Default) {

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
        App.Load();
        App.Resize(Size.X, Size.Y);
        test = SKSurface.Create(new SKImageInfo(100, 100));
        test.Canvas.Clear(SKColors.Red);
    }

    SKSurface test;

    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);
        App.Render();
        // SKCanvas canvas = _surface.Canvas;
        // canvas.Clear(Colors.White);
        // canvas.DrawSurface(test, new(0, 0));
        // canvas.Flush();
        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e) {
        base.OnFramebufferResize(e);
        float clampWidth = Math.Max(e.Width, 1);
        float clampHeight = Math.Max(e.Height, 1);
        
        CreateSurface((int)clampWidth, (int)clampHeight);
        App.Resize((int)clampWidth, (int)clampHeight);
    }
}

public static unsafe class Program {
    public static void Main(string[] args) => new AppWindow().Run();
}