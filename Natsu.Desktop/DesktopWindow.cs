using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Native;
using Natsu.Platforms.Skia;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using MouseButton = Natsu.Input.MouseButton;

namespace Natsu.Platforms.Desktop;

public class DesktopWindow {
    private Vector2 _scale;

    public DesktopWindow(Application app, DesktopWindowSettings? settings = null) {
        if (settings == null) settings = new();

        App = app;
        Window = new(settings, this);
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

            Vector2 scale = Vector2.One;
            if (Window.TryGetCurrentMonitorScale(out float horiz, out float vert)) scale = new(horiz, vert);

            _scale = scale;

            int width = Window.FramebufferSize.X;
            int height = Window.FramebufferSize.Y;

            _target = new(width, height, 0, 8, new(0, (uint)SizedInternalFormat.Rgba8));
            _surface = SKSurface.Create(_context, _target, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);

            SkiaRenderer renderer = new(_surface);

            lock (App) {
                App.Renderer?.Dispose();
                App.Renderer = renderer;
            }

            App.Resize(new(width, height));
        }
    }

    public void Load() {
        App.Platform = Window;
        App.ResourceLoader = new SkiaResourceLoader();

        CreateSurface(Window.Size);

        App.Load();
    }

    public void Resize(int width, int height) => CreateSurface(new(width, height));

    public void Update() => App.Update();

    public void Render() {
        if (_surface == null) return;

        App.Render();
        _context.Flush();
        Window.SwapBuffers();
    }

    public void Dispose() => App.Dispose();

    public KeyMods TranslateKeyMods(KeyModifiers mods) {
        KeyMods keyMods = KeyMods.None;

        if ((mods & KeyModifiers.Shift) != 0) keyMods |= KeyMods.Shift;

        if ((mods & KeyModifiers.Control) != 0) keyMods |= KeyMods.Control;

        if ((mods & KeyModifiers.Alt) != 0) keyMods |= KeyMods.Alt;

        if ((mods & KeyModifiers.Super) != 0) keyMods |= KeyMods.Super;

        return keyMods;
    }

    public void KeyDown(KeyboardKeyEventArgs e) => App.KeyDown((Key)e.Key, TranslateKeyMods(e.Modifiers));

    public void KeyUp(KeyboardKeyEventArgs e) => App.KeyUp((Key)e.Key, TranslateKeyMods(e.Modifiers));

    public void TextInput(string change, int location, int replaced) => App.TextInput(change, location, replaced);

    public Vector2 MapPosition(Vector2 pos) => pos * _scale;

    public void MouseDown(MouseButtonEventArgs e) => App.MouseDown((MouseButton)e.Button, MapPosition(new(Window.MouseState.X, Window.MouseState.Y)));

    public void MouseUp(MouseButtonEventArgs e) => App.MouseUp((MouseButton)e.Button, MapPosition(new(Window.MouseState.X, Window.MouseState.Y)));

    public void MouseMove(MouseMoveEventArgs e) => App.MouseMove(MapPosition(new(e.X, e.Y)));

    public void MouseWheel(MouseWheelEventArgs e) => App.MouseWheel(new(e.Offset.X, e.Offset.Y));

    public void Run() => Window.Run();
#nullable disable
    private GRContext _context;
    private GRGlInterface _interface;
    private GRBackendRenderTarget _target;
    private SKSurface _surface;
}
