using System.Reflection;

using Natsu.Graphics;
using Natsu.Native;
using Natsu.Utils;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaResourceLoader : ResourceLoader {
    public NamedStorage<IImage> Images { get; } = new();
    public NamedStorage<IFont> Fonts { get; } = new();

    public Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public override IImage LoadImage(byte[] data, string name) {
        if (Images[name] is { } cache) return cache;

        SKImage? image = SKImage.FromEncodedData(new SKMemoryStream(data));
        SkiaImage skiaImage = new(image);
        Images[name] = skiaImage;
        return skiaImage;
    }

    public override IFont LoadFont(byte[] data, string name) {
        if (Fonts[name] is IFont cache) return cache;

        SKTypeface? typeface = SKTypeface.FromData(SKData.CreateCopy(data));
        SkiaFont skiaFont = new(typeface);
        Fonts[name] = skiaFont;
        return skiaFont;
    }

    protected override void OnDispose() {
        Images.Dispose();
        Fonts.Dispose();
    }
}
