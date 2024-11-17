using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaOffscreenSurface : IOffscreenSurface {
    public SkiaOffscreenSurface(int width, int height) {
        Surface = SKSurface.Create(new SKImageInfo(width, height));
        Canvas = new SkiaCanvas(Surface.Canvas);
        Width = width;
        Height = height;
    }

    public SKSurface Surface { get; }
    public SKImage? Image { get; private set; }
    public bool UseSnapshot { get; set; } = true;
    public ICanvas Canvas { get; }
    public int Width { get; }
    public int Height { get; }

    public void Dispose() => Surface.Dispose();

    public void Flush() {
        Surface.Canvas.Flush();
        Image = Surface.Snapshot();
    }

    public IImage Snapshot() {
        if (Image == null) {
            throw new InvalidOperationException("SkiaOffscreenSurface not flushed before snapshot");
        }

        return new SkiaImage(Image);
    }
}