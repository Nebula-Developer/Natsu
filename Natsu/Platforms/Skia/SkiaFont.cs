using Natsu.Graphics;
using Natsu.Mathematics;
using SkiaSharp;

namespace Natsu.Platforms.Skia;

public record SkiaFont(SKTypeface Typeface) : IFont {
    public Vector2 MeasureText(string text, float size) {
        SKPaint paint = new() {
            Typeface = Typeface,
            TextSize = size,
            IsAntialias = true
        };

        float w = paint.MeasureText(text);
        return new(w, size);
    }
}
