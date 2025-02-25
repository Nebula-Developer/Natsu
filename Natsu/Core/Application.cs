#nullable disable

using Natsu.Audio;
using Natsu.Core.Elements;
using Natsu.Graphics;
using Natsu.Graphics.Shaders;
using Natsu.Mathematics;
using Natsu.Native;
using Natsu.Utils.Logging;

namespace Natsu.Core;

public partial class Application : IDisposable {
    private Vector2 _size;

    public Logger AppLogger = new() {
        Outputs = {
            new FileLoggerOutput(Path.Join(AppContext.BaseDirectory, "log.txt")),
            new ConsoleLoggerOutput()
        },
        Prefix = "[App] "
    };

    /// <summary>
    ///     The <see cref="IAudioManager" /> that handles audio operations.
    /// </summary>
    public IAudioManager AudioManager;

    /// <summary>
    ///     The <see cref="INativePlatform" /> that handles agnostic platform operations.
    /// </summary>
    public INativePlatform Platform;

    /// <summary>
    ///     The <see cref="IRenderer" /> that handles rendering operations on a <see cref="ICanvas" />.
    /// </summary>
    public IRenderer Renderer;

    /// <summary>
    ///     The <see cref="Native.ResourceLoader" /> that handles loading resources.
    /// </summary>
    public ResourceLoader ResourceLoader;

    /// <summary>
    ///     The <see cref="IShaderManager" /> that handles shader operations.
    /// </summary>
    public IShaderManager ShaderManager;

    public Application() => Root = new(this);

    /// <summary>
    ///     The canvas that handles drawing operations directly to the <see cref="Renderer" />.
    /// </summary>
    public ICanvas Canvas => Renderer.Canvas;

    /// <summary>
    ///     A clock used for time keeping within the <see cref="Render" /> method.
    /// </summary>
    [Obsolete("For application runtime, use Application.Time instead.")]
    public Clock RenderTime { get; } = new();

    /// <summary>
    ///     A clock used for time keeping within the <see cref="Update" /> method.
    /// </summary>
    public Clock Time { get; } = new();

    /// <summary>
    ///     A scheduler used for scheduling tasks.
    /// </summary>
    public Scheduler Scheduler { get; } = new();

    /// <summary>
    ///     The root element of the application.
    /// </summary>
    public RootElement Root { get; }

    /// <summary>
    ///     The integer size of the application.
    ///     </br>
    ///     In a screen context, this is normally the window's framebuffer size.
    /// </summary>
    public Vector2i Size {
        get => _size;
        set => Resize(value);
    }

    /// <summary>
    ///     Disposes the application and all of its resources/elements.
    /// </summary>
    public void Dispose() {
        AppLogger.Debug($"Disposing {GetType().Name}...");

        Root.Dispose();
        OnDispose();
        DoDispose?.Invoke();

        ResourceLoader.Dispose();
        Renderer.Dispose();
    }

    protected virtual void OnLoad() { }

    protected virtual void OnDispose() { }

    protected virtual void OnUpdate() { }

    protected virtual void OnRender() { }

    protected virtual void OnResize(Vector2i size) { }

    protected virtual void OnFocus() { }

    protected virtual void OnBlur() { }

    public event Action DoLoad;
    public event Action DoDispose;
    public event Action DoUpdate;
    public event Action DoRender;
    public event Action<Vector2i> DoResize;
    public event Action DoFocus;
    public event Action DoBlur;

    /// <summary>
    ///     Loads the application.
    /// </summary>
    public void Load() {
        AppLogger.Debug($"Loading {GetType().Name} on {Platform?.GetType().Name ?? "no platform"}...");
        AppLogger.Debug($"Renderer: {Renderer?.GetType().Name ?? "none"}");
        AppLogger.Debug($"ResourceLoader: {ResourceLoader?.GetType().Name ?? "none"}");

        AudioManager.Load();
        AppLogger.Debug($"AudioManager: {AudioManager?.GetType().Name ?? "none"}");

        // TODO: Find a better way to handle this
        // This only works if the Application class is in the same assembly
        // as the resources, which covers most cases
        ResourceLoader.ProjectAssembly = GetType().Assembly;

        Root.Load();
        OnLoad();
        DoLoad?.Invoke();
    }

    /// <summary>
    ///     Updates the application.
    /// </summary>
    /// <param name="time">The time since the last update</param>
    public void Update(double time) {
        Time.Update(time);
        Scheduler.Update(time);
        AudioManager.Update(time);

        Root.Update();
        OnUpdate();
        DoUpdate?.Invoke();

        MouseMove(null, true);
    }

    /// <summary>
    ///     Renders the application.
    /// </summary>
    /// <param name="time">The time since the last render</param>
    public void Render(double time) {
        Canvas.Clear(Colors.Black);
        Root.Render(Canvas);
        OnRender();
        DoRender?.Invoke();
        Canvas.ResetMatrix();

        Renderer.Flush();
    }

    /// <summary>
    ///     Resizes the application.
    /// </summary>
    /// <param name="size">The new size of the application</param>
    public void Resize(Vector2i size) {
        _size = (Vector2)size;
        Root.Size = (Vector2)size;

        Renderer.Resize(size.X, size.Y);
        OnResize(size);
        DoResize?.Invoke(size);
    }

    /// <summary>
    ///     Triggers the focus event.
    /// </summary>
    public void Focus() {
        OnFocus();
        DoFocus?.Invoke();
    }

    /// <summary>
    ///     Triggers the blur event.
    /// </summary>
    public void Blur() {
        OnBlur();
        DoBlur?.Invoke();
    }

    /// <summary>
    ///     Adds new element(s) to the <see cref="Root" />.
    /// </summary>
    /// <param name="elements">The element(s) to add</param>
    public void Add(params Element[] elements) => Root.Add(elements);

    /// <summary>
    ///     Removes element(s) from the <see cref="Root" />.
    /// </summary>
    /// <param name="elements">The element(s) to remove</param>
    public void Remove(params Element[] elements) => Root.Remove(elements);

    /// <summary>
    ///     Schedules a task to be executed after a certain amount of time.
    /// </summary>
    /// <param name="time">The time to wait before executing the task</param>
    /// <param name="task">The task to execute</param>
    /// <returns>The scheduled task</returns>
    public ScheduledTask Schedule(double time, Action task) => Scheduler.Schedule(time, task);

    public static implicit operator Element(Application app) => app.Root;
}
