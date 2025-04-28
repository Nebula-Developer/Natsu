using Natsu.Graphics;
using SkiaSharp;

namespace Natsu.Platforms.Skia;

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

    public IImageFilter CreateBlur(float sigmaX, float sigmaY) => new SkiaImageFilter(SKImageFilter.CreateBlur(sigmaX, sigmaY));

    public IImageFilter CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY, Color color) {
        SKColor skColor = new(color.R, color.G, color.B, color.A);
        return new SkiaImageFilter(SKImageFilter.CreateDropShadow(dx, dy, sigmaX, sigmaY, skColor));
    }
}
