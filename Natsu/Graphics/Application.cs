#nullable disable

using System.Diagnostics;

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
    public IResourceManager ResourceManager;
    public IRenderer Renderer;
    
    public ICanvas Canvas => Renderer.Canvas;
    public IOffscreenSurface CreateOffscreenSurface(int width, int height) => Renderer.CreateOffscreenSurface(width, height);

    public FrameClock FrameClock { get; } = new();
    
    protected virtual void OnLoad() { }
    protected virtual void OnUnload() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnRender() { }
    protected virtual void OnResize(int width, int height) { }

    public void Dispose() {
        OnUnload();
        ResourceManager.Dispose();
        Renderer.Dispose();
    }

    public void Load() {
        OnLoad();
        FrameClock.Start();
    }

    public void Update() {
        FrameClock.Update();
        OnUpdate();
    }

    public void Render() {
        OnRender();
        Renderer.Present();
    }
}