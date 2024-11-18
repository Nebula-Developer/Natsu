namespace Natsu.Graphics;

public interface IRenderer : IDisposable {
    public ICanvas Canvas { get; }
    public void Flush();
    public void Resize(int width, int height);

    public IOffscreenSurface CreateOffscreenSurface(int width, int height);
}
