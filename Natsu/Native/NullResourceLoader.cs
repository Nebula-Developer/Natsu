using System.Reflection;
using Natsu.Graphics;

namespace Natsu.Native;

public class NullResourceLoader : ResourceLoader {
    public override IFont DefaultFont => throw new NullResourceLoaderException();
    public override Assembly FrameworkAssembly => throw new NullResourceLoaderException();
    public override Assembly ProjectAssembly => throw new NullResourceLoaderException();
    public override IFont LoadFont(byte[] data, string name) => throw new NullResourceLoaderException();
    public override IImage LoadImage(byte[] data, string name) => throw new NullResourceLoaderException();
}

public class NullResourceLoaderException : Exception {
    public NullResourceLoaderException() : base("Resource loader is not set. Did you place loading methods in the wrong place?") { }
    public NullResourceLoaderException(string message) : base(message) { }
    public NullResourceLoaderException(string message, Exception innerException) : base(message, innerException) { }
}
