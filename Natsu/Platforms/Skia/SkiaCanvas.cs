using Natsu.Graphics;
using Natsu.Mathematics;
using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaCanvas(SKCanvas canvas) : ICanvas {
    public SKCanvas Canvas { get; } = canvas;

    public SKPaint Paint { get; } = new();

    public SKFont Font { get; } = new();

    public SKSamplingOptions Sampling { get; set; }

    public void Clear(Color color) => Canvas.Clear(color);

    public void DrawRect(Rect rect, Paint paint) => Canvas.DrawRect(rect, UsePaint(paint));

    public void DrawCircle(Vector2 center, float radius, Paint paint) => Canvas.DrawCircle(center, radius, UsePaint(paint));

    public void DrawLine(Vector2 start, Vector2 end, Paint paint) => Canvas.DrawLine(start, end, UsePaint(paint));

    public void DrawText(string text, Vector2 position, IFont font, Paint paint) => Canvas.DrawText(text, position.X, position.Y + paint.TextSize, SKTextAlign.Left, UseFont(paint, font), UsePaint(paint));

    public void DrawImage(IImage image, Vector2 position, Paint paint) => Canvas.DrawImage(TryImage(image), position, Sampling, UsePaint(paint));

    public void DrawImage(IImage image, Rect rect, Paint paint) => Canvas.DrawImage(TryImage(image), rect, Sampling, UsePaint(paint));

    public void DrawAtlas(IImage image, Rect[] regions, RotationScaleMatrix[] targets, Paint paint) => Canvas.DrawAtlas(TryImage(image), regions.Select(r => (SKRect)r).ToArray(), targets.Select(t => (SKRotationScaleMatrix)t).ToArray(), UsePaint(paint));

    public void DrawOval(Rect rect, Paint paint) => Canvas.DrawOval(rect, UsePaint(paint));

    public void DrawRoundRect(Rect rect, Vector2 radius, Paint paint) => Canvas.DrawRoundRect(rect, radius.X, radius.Y, UsePaint(paint));

    public void DrawPath(VectorPath path, Paint paint) => Canvas.DrawPath(path.SkiaPath, UsePaint(paint));

    public void DrawOffscreenSurface(IOffscreenSurface surface, Vector2 position) {
        if (surface is SkiaOffscreenSurface skiaSurface) {
            if (skiaSurface.UseSnapshot) {
                if (skiaSurface.Image == null) throw new ArgumentException("SkiaOffscreenSurface not flushed before rendering");

                Canvas.DrawImage(skiaSurface.Image, position);
            } else {
                Canvas.DrawSurface(skiaSurface.Surface, position);
            }
        } else {
            throw new ArgumentException("Non-SkiaOffscreenSurface provided to SkiaCanvas");
        }
    }

    public void ResetMatrix() => Canvas.ResetMatrix();

    public void SetMatrix(Matrix matrix) => Canvas.SetMatrix(matrix);

    public int Save() => Canvas.Save();

    public void Restore(int saveCount) => Canvas.RestoreToCount(saveCount);

    public void ClipRect(Rect rect, bool difference = false, bool antialias = false) => Canvas.ClipRect(rect, difference ? SKClipOperation.Difference : SKClipOperation.Intersect, antialias);

    public void ClipRoundRect(Rect rect, Vector2 radius, bool difference = false, bool antialias = false) => Canvas.ClipRoundRect(new(rect, radius.X, radius.Y), difference ? SKClipOperation.Difference : SKClipOperation.Intersect, antialias);

    public void ClipPath(VectorPath path, bool difference = false, bool antialias = false) => Canvas.ClipPath(path.SkiaPath, difference ? SKClipOperation.Difference : SKClipOperation.Intersect, antialias);

    public SKFont UseFont(Paint paint, IFont font) {
        if (font is not SkiaFont skiaFont) throw new ArgumentException("Non-SkiaFont provided to SkiaCanvas");

        Font.Typeface = skiaFont.Typeface;
        Font.Size = paint.TextSize;

        return Font;
    }

    public SKPaint UsePaint(Paint paint) {
        Paint.Color = paint.Color;
        Paint.IsStroke = paint.IsStroke;
        Paint.StrokeWidth = paint.StrokeWidth;
        Paint.IsAntialias = paint.IsAntialias;

        Sampling = paint.FilterQuality switch {
            FilterQuality.None => SKSamplingOptions.Default,
            FilterQuality.Low => new(new SKCubicResampler(0.1f, 0.1f)),
            FilterQuality.Medium => new(SKFilterMode.Linear),
            FilterQuality.High => new(SKCubicResampler.Mitchell),
            _ => SKSamplingOptions.Default
        };

        Paint.StrokeJoin = paint.StrokeJoin switch {
            StrokeJoin.Miter => SKStrokeJoin.Miter,
            StrokeJoin.Round => SKStrokeJoin.Round,
            StrokeJoin.Bevel => SKStrokeJoin.Bevel,
            _ => SKStrokeJoin.Miter
        };

        Paint.StrokeCap = paint.StrokeCap switch {
            StrokeCap.Butt => SKStrokeCap.Butt,
            StrokeCap.Round => SKStrokeCap.Round,
            StrokeCap.Square => SKStrokeCap.Square,
            _ => SKStrokeCap.Butt
        };

        return Paint;
    }

    public static SKImage TryImage(IImage image) => image is SkiaImage skiaImage ? skiaImage.Image : throw new ArgumentException("Non-SKImage provided to SkiaCanvas");
}
