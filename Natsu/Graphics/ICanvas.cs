using Natsu.Mathematics;

namespace Natsu.Graphics;

public interface ICanvas {
    public void Clear(Color color);
    public void DrawRect(Rect rect, Paint paint);
    public void DrawCircle(Vector2 center, float radius, Paint paint);
    public void DrawLine(Vector2 start, Vector2 end, Paint paint);
    public void DrawText(string text, Vector2 position, IFont font, Paint paint);
    public void DrawImage(IImage image, Vector2 position, Paint paint);
    public void DrawImage(IImage image, Rect rect, Paint paint);
    public void DrawOval(Rect rect, Paint paint);
    public void DrawRoundRect(Rect rect, Vector2 radius, Paint paint);
    public void DrawPath(VectorPath path, Paint paint);
    public void DrawOffscreenSurface(IOffscreenSurface surface, Vector2 position);
}