using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaResourceManager : IResourceManager {
    public IImage LoadImage(string path) => new SkiaImage(SKImage.FromEncodedData(File.ReadAllBytes(path)));
    public IImage LoadImage(byte[] data) => new SkiaImage(SKImage.FromEncodedData(data));
    public IFont LoadFontName(string name) => new SkiaFont(SKTypeface.FromFamilyName(name));
    public IFont LoadFont(string path) => new SkiaFont(SKTypeface.FromFile(path));
    public IFont LoadFont(byte[] data) => new SkiaFont(SKTypeface.FromData(SKData.CreateCopy(data)));
}