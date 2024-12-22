#nullable disable

using Natsu.Graphics.Elements;
using Natsu.Mathematics;
using Natsu.Native;

namespace Natsu.Graphics;

public partial class Application : IDisposable {
    private Vector2 _size;
    public INativePlatform Platform;

    public IRenderer Renderer;
    public ResourceLoader ResourceLoader;

    public Application() {
        Root = new RootElement(this);
    }

    public ICanvas Canvas => Renderer.Canvas;

    public FrameClock RenderTime { get; } = new();
    public FrameClock UpdateTime { get; } = new();

    public RootElement Root { get; }

    public Vector2i Size {
        get => _size;
        set => Resize(value);
    }

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


    public void Load() {
        Root.Load();
        OnLoad();
        DoLoad?.Invoke();
    }

    public void Update() {
        UpdateTime.Update();

        Root.Update();
        OnUpdate();
        DoUpdate?.Invoke();
        
        MouseMove();
    }

    public void Render() {
        RenderTime.Update();

        Canvas.Clear(Colors.Black);
        Root.Render(Canvas);
        OnRender();
        DoRender?.Invoke();
        Canvas.ResetMatrix();

        Renderer.Flush();
    }

    public void Resize(Vector2i size) {
        _size = (Vector2)size;
        Root.Size = (Vector2)size;

        Renderer.Resize(size.X, size.Y);
        OnResize(size);
        DoResize?.Invoke(size);
    }

    public void Add(params Element[] elements) => Root.Add(elements);
    public void Remove(params Element[] elements) => Root.Remove(elements);

    public static implicit operator Element(Application app) => app.Root;
}
