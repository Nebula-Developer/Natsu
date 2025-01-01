namespace Natsu.Graphics;

public interface IOffscreenSurface : IDisposable {
    ICanvas Canvas { get; }
    int Width { get; }
    int Height { get; }
    void Flush();

    IImage Snapshot();
}
