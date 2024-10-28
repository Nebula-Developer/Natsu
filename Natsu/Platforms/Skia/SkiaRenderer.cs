using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaRenderer : IRenderer {
    public ICanvas Canvas { get; }
    public SKSurface Surface { get; }

    public SkiaRenderer(SKSurface surface) {
        Surface = surface;
        Canvas = new SkiaCanvas(surface.Canvas);
    }

    public void Dispose() => Surface.Dispose();

    public void Present() => Surface.Canvas.Flush();

    public IOffscreenSurface CreateOffscreenSurface(int width, int height) => new SkiaOffscreenSurface(width, height);
}