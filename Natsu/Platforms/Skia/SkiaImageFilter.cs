using Natsu.Graphics;
using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaImageFilter : IImageFilter {
    public SkiaImageFilter(SKImageFilter filter) => Filter = filter;

    public SKImageFilter Filter { get; }

    public static implicit operator SKImageFilter(SkiaImageFilter filter) => filter.Filter;
}
