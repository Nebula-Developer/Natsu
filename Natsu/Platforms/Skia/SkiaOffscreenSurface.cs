using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Native.Skia;

public class SkiaOffscreenSurface : IOffscreenSurface {
    public SkiaOffscreenSurface(int width, int height) {
        if (width < 1 || height < 1) throw new ArgumentException("IOffscreenSurface width and height must be greater than 0");

        Surface = SKSurface.Create(new SKImageInfo(width, height));
        if (Surface == null) throw new InvalidOperationException("Failed to create SkiaSurface");

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
        if (Image == null) throw new InvalidOperationException("SkiaOffscreenSurface not flushed before snapshot");

        return new SkiaImage(Image);
    }
}
