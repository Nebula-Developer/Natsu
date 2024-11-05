using System.Reflection;
using System.Text;

using Natsu.Utils;

namespace Natsu.Graphics;

public class ResourceLoader : IDisposable {
    public NamedStorage<byte[]> Data { get; } = new();
    
    public IPlatformResourceLoader PlatformLoader { get; }

    public ResourceLoader(IPlatformResourceLoader platformLoader) =>
        PlatformLoader = platformLoader;

    public Assembly Assembly { get; } = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

    public string CreateResourcePath(string path) => $"{Assembly.GetName().Name}.{path.Replace('/', '.')}";

    public byte[] LoadResource(string path, string? name = null) {
        if (Data[name ?? path] is byte[] data) {
            return data;
        }

        using var stream = Assembly.GetManifestResourceStream(CreateResourcePath(path));
        if (stream == null)
            throw new InvalidOperationException($"Resource '{path}' not found");

        data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        Data[name ?? path] = data;
        return data;
    }

    public byte[] LoadFile(string path, string? name = null) {
        if (Data[name ?? path] is byte[] data) {
            return data;
        }

        data = File.ReadAllBytes(path);
        Data[name ?? path] = data;
        return data;
    }

    public string LoadResourceString(string path, string? name = null) => Encoding.UTF8.GetString(LoadResource(path, name));
    public string LoadFileString(string path, string? name = null) => Encoding.UTF8.GetString(LoadFile(path, name));

    public IImage LoadImageBytes(byte[] data, string name) => PlatformLoader.LoadImage(data, name);
    public IFont LoadFontBytes(byte[] data, string name) => PlatformLoader.LoadFont(data, name);

    public IImage LoadResourceImage(string path, string? name = null) => PlatformLoader.LoadImage(LoadResource(path, name), name ?? path);

    public IFont LoadResourceFont(string path, string? name = null) => PlatformLoader.LoadFont(LoadResource(path, name), name ?? path);

    public IImage LoadFileImage(string path, string? name = null) => PlatformLoader.LoadImage(LoadFile(path, name), name ?? path);
    public IFont LoadFileFont(string path, string? name = null) => PlatformLoader.LoadFont(LoadFile(path, name), name ?? path);

    public void Dispose() => PlatformLoader.Dispose(); // We know that Data has a non disposable value type
}

// For loading things that are platform specific
public interface IPlatformResourceLoader : IDisposable {
    public IImage LoadImage(byte[] data, string name);
    public IFont LoadFont(byte[] data, string name);
}