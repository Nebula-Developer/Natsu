using Natsu.Graphics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public record SkiaImage(SKImage Image) : IImage;