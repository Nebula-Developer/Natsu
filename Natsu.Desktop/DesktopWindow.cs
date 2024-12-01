using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Platforms.Skia;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

using SkiaSharp;

namespace Natsu.Platforms.Desktop;

public class DesktopWindow {

    public DesktopWindow(Application app, DesktopWindowSettings? settings = null) {
        if (settings == null) settings = new DesktopWindowSettings();
        App = app;
        Window = new NativeWindow(settings, this);
    }

    public Application App { get; }
    public NativeWindow Window { get; }
    public INativePlatform Platform => Window;

    public void CreateSurface(Vector2 size) {
        size = size.Max(Vector2.One);
        lock (this) {
            _surface?.Dispose();
            _target?.Dispose();

            if (_interface == null || _context == null) {
                _interface?.Dispose();
                _context?.Dispose();

                _interface = GRGlInterface.Create();
                _context = GRContext.CreateGl(_interface);
            }

            _target = new GRBackendRenderTarget((int)size.X, (int)size.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            _surface = SKSurface.Create(_context, _target, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);

            SkiaRenderer renderer = new(_surface);

            lock (App) {
                App.Renderer?.Dispose();
                App.Renderer = renderer;
            }

            App.Resize((int)size.X, (int)size.Y);
        }
    }

    public void Load() {
        App.Platform = Window;
        App.ResourceLoader = new SkiaResourceLoader();

        CreateSurface(Window.Size);

        App.Load();
    }

    public void Resize(int width, int height) => CreateSurface(new Vector2(width, height));
    public void Update() => App.Update();

    public void Render() {
        if (_surface == null) return;

        App.Render();
        _context.Flush();
        Window.SwapBuffers();
    }

    public void Dispose() => App.Dispose();

    public void KeyDown(KeyboardKeyEventArgs e) => App.KeyDown((Key)e.Key);
    public void KeyUp(KeyboardKeyEventArgs e) => App.KeyUp((Key)e.Key);

    public void MouseDown(MouseButtonEventArgs e) => App.MouseDown((MouseButton)e.Button, new Vector2(Window.MouseState.X, Window.MouseState.Y));
    public void MouseUp(MouseButtonEventArgs e) => App.MouseUp((MouseButton)e.Button, new Vector2(Window.MouseState.X, Window.MouseState.Y));
    public void MouseMove(MouseMoveEventArgs e) => App.MouseMove(new Vector2(e.X, e.Y));
    public void MouseWheel(MouseWheelEventArgs e) => App.MouseWheel(new Vector2(e.Offset.X, e.Offset.Y));

    public void Run() => Window.Run();
#nullable disable
    private GRContext _context;
    private GRGlInterface _interface;
    private GRBackendRenderTarget _target;
    private SKSurface _surface;
}