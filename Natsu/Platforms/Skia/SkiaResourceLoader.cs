using System.Reflection;
using System.Text;

using Natsu.Graphics;
using Natsu.Utils;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaResourceLoader : IPlatformResourceLoader {
    public NamedStorage<IImage> Images { get; } = new();
    public NamedStorage<IFont> Fonts { get; } = new();

    public Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public IImage LoadImage(byte[] data, string name) {
        if (Images[name] is IImage cache)
            return cache;
        var image = SKImage.FromEncodedData(new SKMemoryStream(data));
        SkiaImage skiaImage = new(image);
        Images[name] = skiaImage;
        return skiaImage;
    }

    public IFont LoadFont(byte[] data, string name) {
        if (Fonts[name] is IFont cache)
            return cache;
        var typeface = SKTypeface.FromData(SKData.CreateCopy(data));
        SkiaFont skiaFont = new(typeface);
        Fonts[name] = skiaFont;
        return skiaFont;
    }

    public void Dispose() {
        Images.Dispose();
        Fonts.Dispose();
    }
}