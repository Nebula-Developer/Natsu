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

    public Vector2 Size {
        get => _size;
        set => Resize((int)value.X, (int)value.Y);
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
    protected virtual void OnResize(int width, int height) { }

    public event Action DoLoad;
    public event Action DoDispose;
    public event Action DoUpdate;
    public event Action DoRender;
    public event Action<int, int> DoResize;


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

    public void Resize(int width, int height) {
        _size = new Vector2(width, height);
        Root.Size = new Vector2(width, height);

        Renderer.Resize(width, height);
        OnResize(width, height);
        DoResize?.Invoke(width, height);
    }

    public void Add(params Element[] elements) => Root.Add(elements);
    public void Remove(params Element[] elements) => Root.Remove(elements);

    public static implicit operator Element(Application app) => app.Root;
}
