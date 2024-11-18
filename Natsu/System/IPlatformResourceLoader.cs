using Natsu.Graphics;

namespace Natsu.System;

// For loading things that are platform specific
public interface IPlatformResourceLoader : IDisposable {
    public IImage LoadImage(byte[] data, string name);
    public IFont LoadFont(byte[] data, string name);
}
