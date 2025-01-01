using Natsu.Graphics;
using Natsu.Mathematics;
using SkiaSharp;

namespace Natsu.Platforms.Skia;

public record SkiaImage(SKImage Image) : IImage {
    public void Dispose() => Image.Dispose();

    public Vector2i Size => new(Image.Width, Image.Height);
}
