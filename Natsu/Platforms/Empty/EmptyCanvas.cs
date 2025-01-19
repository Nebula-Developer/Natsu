using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Platforms.Empty;

public class EmptyCanvas : ICanvas {
    public void Clear(Color color) { }

    public void DrawRect(Rect rect, Paint paint) { }

    public void DrawCircle(Vector2 center, float radius, Paint paint) { }

    public void DrawLine(Vector2 start, Vector2 end, Paint paint) { }

    public void DrawText(string text, Vector2 position, IFont font, Paint paint) { }

    public void DrawImage(IImage image, Vector2 position, Paint paint) { }

    public void DrawImage(IImage image, Rect rect, Paint paint) { }

    public void DrawAtlas(IImage image, Rect[] regions, RotationScaleMatrix[] targets, Paint paint) { }

    public void DrawOval(Rect rect, Paint paint) { }

    public void DrawRoundRect(Rect rect, Vector2 radius, Paint paint) { }

    public void DrawPath(VectorPath path, Paint paint) { }

    public void DrawOffscreenSurface(IOffscreenSurface surface, Vector2 position) { }

    public void ResetMatrix() { }

    public void SetMatrix(Matrix matrix) { }

    public int Save() => -1;

    public void Restore(int saveCount) { }

    public void ClipRect(Rect rect, bool difference = false, bool antialias = false) { }

    public void ClipRoundRect(Rect rect, Vector2 radius, bool difference = false, bool antialias = false) { }

    public void ClipPath(VectorPath path, bool difference = false, bool antialias = false) { }
}
