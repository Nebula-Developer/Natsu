using Natsu.Graphics;

namespace Natsu.Platforms.Empty;

public class EmptyRenderer : IRenderer {
    public ICanvas Canvas { get; } = new EmptyCanvas();

    public void Dispose() { }

    public void Flush() { }

    public IOffscreenSurface CreateOffscreenSurface(int width, int height) => new EmptyOffscreenSurface(width, height);

    public void Resize(int width, int height) { }
}
