using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Native.Skia;

public class SkiaRenderer : IRenderer {
    public SkiaRenderer(SKSurface surface) {
        Surface = surface;
        Canvas = new SkiaCanvas(surface.Canvas);
    }

    public SKSurface Surface { get; }
    public ICanvas Canvas { get; }

    public void Dispose() => Surface.Dispose();

    public void Flush() => Surface.Canvas.Flush();

    public IOffscreenSurface CreateOffscreenSurface(int width, int height) => new SkiaOffscreenSurface(width, height);

    public void Resize(int width, int height) { }
}
