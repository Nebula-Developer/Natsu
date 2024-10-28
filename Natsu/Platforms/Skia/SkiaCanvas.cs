using Natsu.Graphics;
using Natsu.Mathematics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaCanvas(SKCanvas canvas) : ICanvas {
    public SKCanvas Canvas { get; } = canvas;

    public SKPaint Paint { get; } = new SKPaint();

    public SKPaint UsePaint(Paint paint, IFont? font = null) {
        Paint.Color = paint.Color;
        Paint.IsStroke = paint.IsStroke;
        Paint.StrokeWidth = paint.StrokeWidth;
        Paint.IsAntialias = paint.IsAntialias;
        Paint.FilterQuality = paint.FilterQuality switch {
            FilterQuality.None => SKFilterQuality.None,
            FilterQuality.Low => SKFilterQuality.Low,
            FilterQuality.Medium => SKFilterQuality.Medium,
            FilterQuality.High => SKFilterQuality.High,
            _ => SKFilterQuality.None
        };
        Paint.TextSize = paint.FontSize;

        if (font is SkiaFont skiaFont)
            Paint.Typeface = skiaFont.Typeface;

        return Paint;
    }

    public static SKImage TryImage(IImage image) => image is SkiaImage skiaImage ? skiaImage.Image : throw new ArgumentException("Non-SKImage provided to SkiaCanvas");

    public void Clear(Color color) => Canvas.Clear(color);
    public void DrawRect(Rect rect, Paint paint) => Canvas.DrawRect(rect, UsePaint(paint));
    public void DrawCircle(Vector2 center, float radius, Paint paint) => Canvas.DrawCircle(center, radius, UsePaint(paint));
    public void DrawLine(Vector2 start, Vector2 end, Paint paint) => Canvas.DrawLine(start, end, UsePaint(paint));
    public void DrawText(string text, Vector2 position, IFont font, Paint paint) => Canvas.DrawText(text, position.X, position.Y + paint.FontSize, UsePaint(paint));
    public void DrawImage(IImage image, Vector2 position, Paint paint) => Canvas.DrawImage(TryImage(image), position, UsePaint(paint));
    public void DrawImage(IImage image, Rect rect, Paint paint) => Canvas.DrawImage(TryImage(image), rect, UsePaint(paint));
    public void DrawOval(Rect rect, Paint paint) => Canvas.DrawOval(rect, UsePaint(paint));
    public void DrawRoundRect(Rect rect, Vector2 radius, Paint paint) => Canvas.DrawRoundRect(rect, radius.X, radius.Y, UsePaint(paint));
    public void DrawPath(VectorPath path, Paint paint) => Canvas.DrawPath(path.SkiaPath, UsePaint(paint));
    public void DrawOffscreenSurface(IOffscreenSurface surface, Vector2 position) {
        if (surface is SkiaOffscreenSurface skiaSurface) {
            Canvas.DrawSurface(skiaSurface.Surface, position);
        } else {
            throw new ArgumentException("Non-SkiaOffscreenSurface provided to SkiaCanvas");
        }
    }

    public void ResetMatrix() => Canvas.ResetMatrix();
    public void SetMatrix(Matrix matrix) => Canvas.SetMatrix(matrix);
}