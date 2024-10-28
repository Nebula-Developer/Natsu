namespace Natsu.Graphics;

public interface IResourceManager : IDisposable {
    public IImage LoadImage(string path);
    public IImage LoadImage(byte[] data);
    public IFont LoadFont(string path);
    public IFont LoadFontName(string path);
    public IFont LoadFont(byte[] data);
}