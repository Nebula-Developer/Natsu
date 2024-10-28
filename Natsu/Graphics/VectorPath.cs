using Natsu.Mathematics;

using SkiaSharp;

namespace Natsu.Graphics;

public class VectorPath {
    public SKPath SkiaPath { get; } = new SKPath();

    public void MoveTo(Vector2 point) => SkiaPath.MoveTo(point.X, point.Y);
    public void LineTo(Vector2 point) => SkiaPath.LineTo(point.X, point.Y);
    public void QuadTo(Vector2 control, Vector2 end) => SkiaPath.QuadTo(control.X, control.Y, end.X, end.Y);
    public void CubicTo(Vector2 control1, Vector2 control2, Vector2 end) => SkiaPath.CubicTo(control1.X, control1.Y, control2.X, control2.Y, end.X, end.Y);
    public void Close() => SkiaPath.Close();

    public void AddRect(Rect rect) => SkiaPath.AddRect(rect);
    public void AddOval(Rect rect) => SkiaPath.AddOval(rect);
    public void AddRoundRect(Rect rect, Vector2 radius) => SkiaPath.AddRoundRect(rect, radius.X, radius.Y);
    public void AddCircle(Vector2 center, float radius) => SkiaPath.AddCircle(center.X, center.Y, radius);
    public void AddArc(Rect rect, float startAngle, float sweepAngle) => SkiaPath.AddArc(rect, startAngle, sweepAngle);
    public void AddPath(VectorPath path) => SkiaPath.AddPath(path.SkiaPath);
    public void AddPoly(Vector2[] points, bool close) => SkiaPath.AddPoly(points.Select(p => new SKPoint(p.X, p.Y)).ToArray(), close);
}