namespace Natsu.Graphics;

public interface IApplication : IDisposable {
    public IResourceManager ResourceManager { get; }
    public IRenderer Renderer { get; }
    public void Run();
}