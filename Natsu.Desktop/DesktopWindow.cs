using Natsu.Core;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Native;
using Natsu.Platforms.Audio.ManagedBassAudio;
using Natsu.Platforms.Skia;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using MouseButton = Natsu.Input.MouseButton;

namespace Natsu.Platforms.Desktop;

/// <summary>
///     A cross-platform desktop window.
///     <br />
///     Primarily targets Windows, macOS, and Linux.
/// </summary>
public class DesktopWindow {
    private Vector2 _scale;

    /// <summary>
    ///     Constructs a new <see cref="DesktopWindow" />.
    /// </summary>
    /// <param name="app">The <see cref="Application" /> to host</param>
    /// <param name="settings">The settings to use for the window</param>
    public DesktopWindow(Application app, DesktopWindowSettings? settings = null) {
        if (settings == null) settings = new();

        App = app;
        Window = new(settings, this);
    }

    /// <summary>
    ///     The <see cref="Application" /> hosted by the window.
    /// </summary>
    public Application App { get; }

    internal NativeWindow Window { get; }

    /// <summary>
    ///     The <see cref="INativePlatform" /> that handles agnostic platform operations.
    /// </summary>
    public INativePlatform Platform => Window;

    /// <summary>
    ///     Creates a new surface for rendering.
    ///     <br />
    ///     This should be called whenever the window is resized.
    /// </summary>
    public void CreateSurface() {
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

    internal void Load() {
        App.Platform = Window;
        App.ResourceLoader = new SkiaResourceLoader();
        App.AudioManager = new BassAudioManager();

        CreateSurface();

        App.Load();
    }

    internal void Resize(int width, int height) => CreateSurface();

    internal void Update(double time) => App.Update(time);

    internal void Render(double time) {
        if (_surface == null) return;

        App.Render(time);
        _context.Flush();
        Window.SwapBuffers();
    }

    internal void Dispose() => App.Dispose();

    internal KeyMods TranslateKeyMods(KeyModifiers mods) {
        KeyMods keyMods = KeyMods.None;

        if ((mods & KeyModifiers.Shift) != 0) keyMods |= KeyMods.Shift;

        if ((mods & KeyModifiers.Control) != 0) keyMods |= KeyMods.Control;

        if ((mods & KeyModifiers.Alt) != 0) keyMods |= KeyMods.Alt;

        if ((mods & KeyModifiers.Super) != 0) keyMods |= KeyMods.Super;

        return keyMods;
    }

    internal void KeyDown(KeyboardKeyEventArgs e) => App.KeyDown((Key)e.Key, TranslateKeyMods(e.Modifiers));

    internal void KeyUp(KeyboardKeyEventArgs e) => App.KeyUp((Key)e.Key, TranslateKeyMods(e.Modifiers));

    internal void TextInput(string change, int location, int replaced) => App.TextInput(change, location, replaced);

    /// <summary>
    ///     Maps a position from window space to application space.
    /// </summary>
    /// <param name="pos">The position to map</param>
    /// <returns>The mapped position</returns>
    public Vector2 MapPosition(Vector2 pos) => pos * _scale;

    internal void MouseDown(MouseButtonEventArgs e) => App.MouseDown((MouseButton)e.Button, MapPosition(new(Window.MouseState.X, Window.MouseState.Y)));

    internal void MouseUp(MouseButtonEventArgs e) => App.MouseUp((MouseButton)e.Button, MapPosition(new(Window.MouseState.X, Window.MouseState.Y)));

    internal void MouseMove(MouseMoveEventArgs e) => App.MouseMove(MapPosition(new(e.X, e.Y)));

    internal void MouseWheel(MouseWheelEventArgs e) => App.MouseWheel(new(e.Offset.X, e.Offset.Y));

    internal void Focus() => App.Focus();

    internal void Blur() => App.Blur();

    /// <summary>
    ///     Runs the window.
    /// </summary>
    public void Run() => Window.Run();
#nullable disable
    private GRContext _context;
    private GRGlInterface _interface;
    private GRBackendRenderTarget _target;
    private SKSurface _surface;
}
