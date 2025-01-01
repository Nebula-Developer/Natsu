#nullable disable

using Natsu.Core.Elements;
using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Native;

namespace Natsu.Core;

public partial class Application : IDisposable {
    private Vector2 _size;

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

    public Application() => Root = new(this);

    /// <summary>
    ///     The canvas that handles drawing operations directly to the <see cref="Renderer" />.
    /// </summary>
    public ICanvas Canvas => Renderer.Canvas;

    /// <summary>
    ///     A clock used for time keeping within the <see cref="Render" /> method.
    /// </summary>
    public FrameClock RenderTime { get; } = new();

    /// <summary>
    ///     A clock used for time keeping within the <see cref="Update" /> method.
    /// </summary>
    public FrameClock UpdateTime { get; } = new();

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

    public event Action DoLoad;
    public event Action DoDispose;
    public event Action DoUpdate;
    public event Action DoRender;
    public event Action<Vector2i> DoResize;

    /// <summary>
    ///     Loads the application.
    /// </summary>
    public void Load() {
        Root.Load();
        OnLoad();
        DoLoad?.Invoke();
    }

    /// <summary>
    ///     Updates the application.
    /// </summary>
    public void Update() {
        UpdateTime.Update();

        Root.Update();
        OnUpdate();
        DoUpdate?.Invoke();

        MouseMove();
    }

    /// <summary>
    ///     Renders the application.
    /// </summary>
    public void Render() {
        RenderTime.Update();

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
    ///     Adds new element(s) to the <see cref="Root" />.
    /// </summary>
    /// <param name="elements">The element(s) to add</param>
    public void Add(params Element[] elements) => Root.Add(elements);

    /// <summary>
    ///     Removes element(s) from the <see cref="Root" />.
    /// </summary>
    /// <param name="elements">The element(s) to remove</param>
    public void Remove(params Element[] elements) => Root.Remove(elements);

    public static implicit operator Element(Application app) => app.Root;
}
