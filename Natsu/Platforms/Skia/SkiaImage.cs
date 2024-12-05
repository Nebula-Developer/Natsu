using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Native.Skia;

public record SkiaImage(SKImage Image) : IImage {
    public void Dispose() => Image.Dispose();
}
