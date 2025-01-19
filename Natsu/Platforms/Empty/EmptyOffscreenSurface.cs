using Natsu.Graphics;

namespace Natsu.Platforms.Empty;

public class EmptyOffscreenSurface : IOffscreenSurface {
    public EmptyOffscreenSurface(int width, int height) {
        Width = width;
        Height = height;
    }

    public bool UseSnapshot { get; set; } = true;
    public ICanvas Canvas { get; } = new EmptyCanvas();

    public int Width { get; }
    public int Height { get; }

    public void Dispose() { }

    public void Flush() { }

    public IImage Snapshot() => new EmptyImage();
}
