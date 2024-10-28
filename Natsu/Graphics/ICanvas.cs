using Natsu.Mathematics;

using SkiaSharp;

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

    public void ResetMatrix();
    public void SetMatrix(Matrix matrix);
}

public class Matrix {
    public SKMatrix SkiaMatrix { get; set; } = new SKMatrix();

    public void Reset() => SkiaMatrix = SKMatrix.CreateIdentity();
    public void Translate(float x, float y) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateTranslation(x, y));
    
    public void Rotate(float degrees) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateRotationDegrees(degrees));
    public void Rotate(float degrees, float px, float py) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateRotationDegrees(degrees, px, py));

    public void Scale(float sx, float sy) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateScale(sx, sy));
    public void Scale(float sx, float sy, float px, float py) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateScale(sx, sy, px, py));

    public void Skew(float kx, float ky) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateSkew(kx, ky));

    public void PreTranslate(float x, float y) => SkiaMatrix = SKMatrix.CreateTranslation(x, y).PostConcat(SkiaMatrix);

    public void PreRotate(float degrees) => SkiaMatrix = SKMatrix.CreateRotationDegrees(degrees).PostConcat(SkiaMatrix);
    public void PreRotate(float degrees, float px, float py) => SKMatrix.CreateRotationDegrees(degrees, px, py).PostConcat(SkiaMatrix);

    public void PreScale(float sx, float sy) => SKMatrix.CreateScale(sx, sy).PostConcat(SkiaMatrix);
    public void PreScale(float sx, float sy, float px, float py) => SKMatrix.CreateScale(sx, sy, px, py).PostConcat(SkiaMatrix);

    public void PreSkew(float kx, float ky) => SKMatrix.CreateSkew(kx, ky).PostConcat(SkiaMatrix);
    
    public void Concat(Matrix matrix) => SkiaMatrix = SkiaMatrix.PostConcat(matrix.SkiaMatrix);


    public static implicit operator SKMatrix(Matrix matrix) => matrix.SkiaMatrix;
    public static implicit operator Matrix(SKMatrix matrix) => new Matrix { SkiaMatrix = matrix };
}