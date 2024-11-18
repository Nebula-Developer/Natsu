namespace Natsu.Graphics;

public interface IOffscreenSurface : IDisposable {
    public ICanvas Canvas { get; }
    public int Width { get; }
    public int Height { get; }
    public void Flush();

    public IImage Snapshot();
}
