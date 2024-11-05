using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaOffscreenSurface : IOffscreenSurface {
    public SKSurface Surface { get; }
    public SKImage? Image { get; private set; }
    public ICanvas Canvas { get; }
    public int Width { get; }
    public int Height { get; }
    public bool UseSnapshot { get; set; } = true;

    public SkiaOffscreenSurface(int width, int height) {
        Surface = SKSurface.Create(new SKImageInfo(width, height));
        Canvas = new SkiaCanvas(Surface.Canvas);
        Width = width;
        Height = height;
    }

    public void Dispose() => Surface.Dispose();
    public void Flush() {
        Surface.Canvas.Flush();
        Image = Surface.Snapshot();
    }
}