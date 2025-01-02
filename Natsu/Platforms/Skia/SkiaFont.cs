using Natsu.Graphics;
using Natsu.Mathematics;
using SkiaSharp;

namespace Natsu.Platforms.Skia;

public record SkiaFont(SKTypeface Typeface) : IFont {
    public Vector2 MeasureText(string text, float size) => new(new SKFont(Typeface, size).MeasureText(text), size);
}
