namespace Natsu.Graphics;

public interface IRenderer : IDisposable {
    public ICanvas Canvas { get; }
    public void Present();

    public IOffscreenSurface CreateOffscreenSurface(int width, int height);
}