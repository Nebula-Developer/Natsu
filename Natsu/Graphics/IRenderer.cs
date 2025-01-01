namespace Natsu.Graphics;

public interface IRenderer : IDisposable {
    ICanvas Canvas { get; }
    void Flush();
    void Resize(int width, int height);

    IOffscreenSurface CreateOffscreenSurface(int width, int height);
}
