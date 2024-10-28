using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public record SkiaFont(SKTypeface Typeface) : IFont;