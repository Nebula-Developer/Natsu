#nullable disable

using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Graphics;

public partial class Application : IDisposable {
    public IRenderer Renderer;
    public ResourceLoader ResourceLoader;

    public Application() => Root = new RootElement(this);

    public ICanvas Canvas => Renderer.Canvas;

    public FrameClock RenderTime { get; } = new();
    public FrameClock UpdateTime { get; } = new();

    public RootElement Root { get; }

    public Vector2 Size {
        get => Root.Size;
        set => Resize((int)value.X, (int)value.Y);
    }

    protected virtual void OnLoad() { }
    protected virtual void OnDispose() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnRender() { }
    protected virtual void OnResize(int width, int height) { }

    public event Action Loaded;
    public event Action Disposed;
    public event Action Updated;
    public event Action Rendered;
    public event Action<int, int> Resized;

    public void Dispose() {
        Root.Dispose();
        OnDispose();
        Disposed?.Invoke();

        ResourceLoader.Dispose();
        Renderer.Dispose();
    }


    public void Load() {
        Root.Load();
        OnLoad();
        Loaded?.Invoke();
    }

    public void Update() {
        UpdateTime.Update();

        Root.Update();
        OnUpdate();
        Updated?.Invoke();
    }

    public void Render() {
        RenderTime.Update();

        Canvas.Clear(Colors.Black);
        Root.Render(Canvas);
        OnRender();
        Rendered?.Invoke();
        Canvas.ResetMatrix();

        Renderer.Flush();
    }

    public void Resize(int width, int height) {
        Renderer.Resize(width, height);
        OnResize(width, height);
        Resized?.Invoke(width, height);

        Root.Size = new Vector2(width / Root.Scale.X, height / Root.Scale.Y);
    }

    public void Add(params Element[] elements) => Root.Add(elements);
    public void Remove(params Element[] elements) => Root.Remove(elements);

    public static implicit operator Element(Application app) => app.Root;
}