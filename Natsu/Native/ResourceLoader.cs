using System.Reflection;
using System.Text;

using Natsu.Graphics;
using Natsu.Utils;

namespace Natsu.Native;

public static class StringExtensions {
    public static string FormatResourcePath(this string path, Assembly asm) => $"{asm.GetName().Name}.{path.Replace('/', '.')}";
}

public abstract class ResourceLoader : IDisposable {
    public NamedStorage<byte[]> Data { get; } = new();
    public NamedStorage<IFont> FontData { get; } = new();
    public NamedStorage<IImage> ImageData { get; } = new();

    public virtual Assembly ProjectAssembly { get; } = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
    public virtual Assembly FrameworkAssembly { get; } = Assembly.GetExecutingAssembly();

    public virtual IFont DefaultFont => LoadFrameworkResourceFont("Resources/Fonts/Roboto/Roboto-Regular.ttf");

    public void Dispose() {
        OnDispose();
        Data.Dispose();
    }

    public abstract IImage LoadImage(byte[] data, string name);
    public abstract IFont LoadFont(byte[] data, string name);

    protected virtual void OnDispose() { }

    #region Project Assembly Resource Loading

    public byte[] LoadResource(string path, string? name = null) => LoadData(path, name, () => ProjectAssembly.GetManifestResourceStream(path.FormatResourcePath(ProjectAssembly)));

    public string LoadResourceString(string path, string? name = null) => Encoding.UTF8.GetString(LoadResource(path, name));

    public IImage LoadResourceImage(string path, string? name = null) => LoadImageData(path, name);

    public IFont LoadResourceFont(string path, string? name = null) => LoadFontData(path, name);

    #endregion

    #region Framework Assembly Resource Loading

    public byte[] LoadFrameworkResource(string path, string? name = null) => LoadData(path, name, () => FrameworkAssembly.GetManifestResourceStream(path.FormatResourcePath(FrameworkAssembly)));

    public string LoadFrameworkResourceString(string path, string? name = null) => Encoding.UTF8.GetString(LoadFrameworkResource(path, name));

    public IImage LoadFrameworkResourceImage(string path, string? name = null) => LoadFrameworkImageData(path, name);

    public IFont LoadFrameworkResourceFont(string path, string? name = null) => LoadFrameworkFontData(path, name);

    #endregion

    #region File Loading (from external files)

    public byte[] LoadFile(string path, string? name = null) => LoadData(path, name, () => new FileStream(path, FileMode.Open, FileAccess.Read));

    public string LoadFileString(string path, string? name = null) => Encoding.UTF8.GetString(LoadFile(path, name));

    public IImage LoadFileImage(string path, string? name = null) {
        if (ImageData[name ?? path] is IImage image) return image;

        byte[] data = LoadFile(path, name);
        image = LoadImage(data, name ?? path);
        ImageData[name ?? path] = image;
        return image;
    }

    public IFont LoadFileFont(string path, string? name = null) {
        if (FontData[name ?? path] is IFont font) return font;

        byte[] data = LoadFile(path, name);
        font = LoadFont(data, name ?? path);
        FontData[name ?? path] = font;
        return font;
    }

    #endregion

    #region Helper Methods

    private byte[] LoadData(string path, string? name, Func<Stream?> openStreamFunc) {
        if (Data[name ?? path] is byte[] data) return data;

        using Stream? stream = openStreamFunc();
        if (stream == null) throw new InvalidOperationException($"Resource '{path}' not found");

        data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        Data[name ?? path] = data;
        return data;
    }

    private IFont LoadFontData(string path, string? name) {
        if (FontData[name ?? path] is IFont font) return font;

        byte[] data = LoadResource(path, name);
        font = LoadFont(data, name ?? path);
        FontData[name ?? path] = font;
        return font;
    }

    private IImage LoadImageData(string path, string? name) {
        if (ImageData[name ?? path] is IImage image) return image;

        byte[] data = LoadResource(path, name);
        image = LoadImage(data, name ?? path);
        ImageData[name ?? path] = image;
        return image;
    }

    private IFont LoadFrameworkFontData(string path, string? name) {
        if (FontData[name ?? path] is IFont font) return font;

        byte[] data = LoadFrameworkResource(path, name);
        font = LoadFont(data, name ?? path);
        FontData[name ?? path] = font;
        return font;
    }

    private IImage LoadFrameworkImageData(string path, string? name) {
        if (ImageData[name ?? path] is IImage image) return image;

        byte[] data = LoadFrameworkResource(path, name);
        image = LoadImage(data, name ?? path);
        ImageData[name ?? path] = image;
        return image;
    }

    #endregion

}
