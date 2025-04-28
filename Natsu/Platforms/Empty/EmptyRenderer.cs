using Natsu.Graphics;

namespace Natsu.Platforms.Empty;

public class EmptyRenderer : IRenderer {
    public ICanvas Canvas { get; } = new EmptyCanvas();

    public void Dispose() { }

    public void Flush() { }

    public IOffscreenSurface CreateOffscreenSurface(int width, int height) => new EmptyOffscreenSurface(width, height);

    public void Resize(int width, int height) { }

    public IImageFilter CreateBlur(float sigmaX, float sigmaY) => new EmptyImageFilter();
    public IImageFilter CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY, Color color) => new EmptyImageFilter();
}
