using Natsu.Mathematics;

namespace Natsu.Graphics;

public interface ICanvas {
    void Clear(Color color);

    void DrawRect(Rect rect, Paint paint);
    void DrawRoundRect(Rect rect, Vector2 radius, Paint paint);

    void DrawCircle(Vector2 center, float radius, Paint paint);
    void DrawOval(Rect rect, Paint paint);

    void DrawLine(Vector2 start, Vector2 end, Paint paint);
    void DrawText(string text, Vector2 position, IFont font, Paint paint);
    void DrawPath(VectorPath path, Paint paint);

    void DrawImage(IImage image, Vector2 position, Paint paint);
    void DrawImage(IImage image, Rect rect, Paint paint);
    void DrawAtlas(IImage image, Rect[] regions, RotationScaleMatrix[] targets, Paint paint);

    void DrawOffscreenSurface(IOffscreenSurface surface, Vector2 position);

    int Save();
    int Save(float opacity);
    void Restore(int saveCount);

    void ClipRect(Rect rect, bool difference = false, bool antialias = false);

    void ClipRoundRect(Rect rect, Vector2 radius, bool difference = false, bool antialias = false);

    void ClipPath(VectorPath path, bool difference = false, bool antialias = false);

    void ResetMatrix();
    void SetMatrix(Matrix matrix);
}
