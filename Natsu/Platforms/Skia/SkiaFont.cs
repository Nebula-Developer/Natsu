
using Natsu.Graphics;
using Natsu.Mathematics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public record SkiaFont(SKTypeface Typeface) : IFont {
    public Vector2 MeasureText(string text, float size) {
        var paint = new SKPaint {
            Typeface = Typeface,
            TextSize = size
        };
        var bounds = new SKRect();
        paint.MeasureText(text, ref bounds);
        return new Vector2(bounds.Width, bounds.Height + 5);
    }
};