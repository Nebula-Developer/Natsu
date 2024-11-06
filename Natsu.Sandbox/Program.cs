
using System;
using System.Diagnostics;

using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Platforms.Skia;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

            TryGetCurrentMonitorScale(out float dpiX, out float dpiY);
            // App.Root.Scale = new(dpiX, dpiY);
            scale = new(dpiX, dpiY);
        }
    }

    Vector2 scale = new(1);


    protected override void OnLoad() {
        base.OnLoad();

        _interface = GRGlInterface.Create();
        _context = GRContext.CreateGl(_interface);
        CreateSurface(Size.X, Size.Y);
        App.Load();
        App.Resize(Size.X, Size.Y);
        test = SKSurface.Create(new SKImageInfo(100, 100));
        test.Canvas.Clear(SKColors.Red);
        App.Root.AnchorPosition = new(0.5f);
        App.Root.OffsetPosition = new(0.5f);
        x = new RectElement() {
            Paint = new() {
                Color = new(255, 0, 0, 255)
            },
            RawChildren = {
                new RectElement() {
                    RelativeSizeAxes = Axes.Both,
                    // Margin = 2.5f,
                    Paint = new() {
                        Color = new(0, 255, 0, 255)
                    }
                }
            },
            Index = 99999,
            Size = new(10, 10),
            Position = new Vector2(2f, 2f),
            OffsetPosition = new(0.5f)
        };

        App.Add(x);
        CursorState = CursorState.Hidden;
    }

    SKSurface test;
    Element x;

    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);
        x.Position = new(MousePosition.X, MousePosition.Y);
        
        App.Render();
        // SKCanvas canvas = _surface.Canvas;
        // canvas.Clear(Colors.White);
        // canvas.DrawSurface(test, new(0, 0));
        // canvas.Flush();
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e) {
        float clampWidth = Math.Max(e.Width, 1);
        float clampHeight = Math.Max(e.Height, 1);

        CreateSurface((int)clampWidth, (int)clampHeight);
        App.Resize((int)clampWidth, (int)clampHeight);
    }

    protected override void OnFocusedChanged(FocusedChangedEventArgs e) {
        base.OnFocusedChanged(e);
    }

    Vector2 startPos = 0, startSize = 0;
    bool dragging = false;

    protected override void OnMouseMove(MouseMoveEventArgs e) {
        base.OnMouseMove(e);
        if (dragging) {
            App.Root.Size = new(startSize.X + (e.X - startPos.X), startSize.Y + (e.Y - startPos.Y));
            // center
            App.Root.Position = new(Size.X / 2 - App.Root.Size.X / 2, Size.Y / 2 - App.Root.Size.Y / 2);
        }
    }
    protected override void OnMouseUp(MouseButtonEventArgs e) {
        base.OnMouseUp(e);
        dragging = false;
    }
    protected override void OnMouseDown(MouseButtonEventArgs e) {
        List<Element> elms = new();
        Stopwatch sw = Stopwatch.StartNew();
        App.GetElementsAt(new(MousePosition.X, MousePosition.Y), App.Root, ref elms);
        Console.WriteLine($"Took {sw.ElapsedMilliseconds}ms to get elements at position {MousePosition.X}, {MousePosition.Y}");
        foreach (Element elm in elms) {
            if (e.Button == MouseButton.Left)
                elm.Margin = new(elm.Margin.X + 10, elm.Margin.Y + 10);
            else if (e.Button == MouseButton.Right)
                elm.Margin = new(elm.Margin.X - 10, elm.Margin.Y - 10);
            Console.WriteLine(elm.Name);
        }
        Console.WriteLine();

        // if (elms.Count > 0 && elms[0].Name == "Root") {
            dragging = true;
            startPos = new(MousePosition.X, MousePosition.Y);
            startSize = App.Root.Size;
        // }

        // targetScale = targetScale == new Vector2(1) ? new(2f) : new(1);

        // UpdateFrequency = UpdateFrequency == 200 ? 60 : 200;
        // Console.WriteLine($"Update Frequency: {UpdateFrequency}");
    }

    Vector2 targetScale = new(1);
    public Vector2 Lerp(Vector2 a, Vector2 b, float t) => a + (b - a) * t;
    protected override void OnUpdateFrame(FrameEventArgs args) {
        base.OnUpdateFrame(args);
        Vector2 newSize = Lerp(App.Root.Scale, targetScale, (float)args.Time * 10f);
        if (Math.Abs(newSize.X - targetScale.X) > 0.001f) {
            App.Root.Scale = newSize;
            App.Root.Size = new(Size.X / App.Root.Scale.X, Size.Y / App.Root.Scale.Y);
        } else if (App.Root.Scale != targetScale) {
            App.Root.Scale = targetScale;
            App.Root.Size = new(Size.X / App.Root.Scale.X, Size.Y / App.Root.Scale.Y);
        }
    }
}

public static unsafe class Program {
    public static void Main(string[] args) => new AppWindow().Run();
}