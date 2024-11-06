#nullable disable

using System.Diagnostics;
using Natsu.Mathematics;

namespace Natsu.Graphics;

public class FrameClock {
    public double DeltaTime { get; private set; }
    public double TotalTime { get; private set; }
    public int Frames { get; private set; }
    public int FPS { get; private set; }

    private readonly Stopwatch _stopwatch = new();

    public void Start() => _stopwatch.Start();
    public void Stop() => _stopwatch.Stop();

    public void Update() {
        DeltaTime = _stopwatch.Elapsed.TotalSeconds;
        TotalTime += DeltaTime;
        Frames++;
        FPS = (int)(1 / DeltaTime);
        _stopwatch.Restart();
    }
}

public class Application : IDisposable {
    public ResourceLoader ResourceLoader;
    public IRenderer Renderer;
    
    public ICanvas Canvas => Renderer.Canvas;
    public IOffscreenSurface CreateOffscreenSurface(int width, int height) => Renderer.CreateOffscreenSurface(width, height);

    public FrameClock FrameClock { get; } = new();
    
    protected virtual void OnLoad() { }
    protected virtual void OnUnload() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnRender() { }
    protected virtual void OnResize(int width, int height) { }

    public Application() => Root = new(this);

    public void Dispose() {
        OnUnload();
        ResourceLoader.Dispose();
        Renderer.Dispose();
    }

    public void Load() {
        OnLoad();
        FrameClock.Start();
    }

    public void Update() {
        FrameClock.Update();
        Root.Update();
        OnUpdate();
    }

    public void Render() {
        Renderer.Canvas.Clear(Colors.Black);
        Root.Render(Renderer.Canvas);
        Renderer.Canvas.ResetMatrix();
        OnRender();
        Renderer.Present();
    }

    public Vector2 Size {
        get => Root.Size;
        set => Resize((int)value.X, (int)value.Y);
    }

    public void Resize(int width, int height) {
        Renderer.Resize(width, height);
        OnResize(width, height);
        Root.Size = new(width / Root.Scale.X, height / Root.Scale.Y);
    }

    public RootElement Root { get; }
    public void Add(params Element[] elements) => Root.Add(elements);
    public void Remove(params Element[] elements) => Root.Remove(elements);

    protected List<Element> ConstructInverseTree(Element cur) {
        if (cur == null) return new();
        
        List<Element> elements = new();
        if (cur.Children.Count > 0) {
            lock (cur.Children)
                for (int i = cur.Children.Count - 1; i >= 0; i--) {
                    if (cur.Children[i] is CachedElement) continue;
                    elements.AddRange(ConstructInverseTree(cur.Children[i]));
                }
        }

        if (cur.HandlePositionalInput) elements.Add(cur);
        return elements;
    }

    protected List<Element> InputTree = new();
    public void ConstructInputTree() => InputTree = ConstructInverseTree(Root);
}
