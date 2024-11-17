using SkiaSharp;

namespace Natsu.Mathematics;

public class Matrix {
    public SKMatrix SkiaMatrix { get; set; } = SKMatrix.CreateIdentity();

    public void Reset() => SkiaMatrix = SKMatrix.CreateIdentity();

    public void Translate(float x, float y) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateTranslation(x, y));

    public void Rotate(float degrees) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateRotationDegrees(degrees));

    public void Rotate(float degrees, float px, float py) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateRotationDegrees(degrees, px, py));

    public void Scale(float sx, float sy) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateScale(sx, sy));

    public void Scale(float sx, float sy, float px, float py) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateScale(sx, sy, px, py));

    public void Skew(float kx, float ky) => SkiaMatrix = SkiaMatrix.PostConcat(SKMatrix.CreateSkew(kx, ky));

    public void PreTranslate(float x, float y) => SkiaMatrix = SkiaMatrix.PreConcat(SKMatrix.CreateTranslation(x, y));

    public void PreRotate(float degrees) => SkiaMatrix = SkiaMatrix.PreConcat(SKMatrix.CreateRotationDegrees(degrees));

    public void PreRotate(float degrees, float px, float py) => SkiaMatrix = SkiaMatrix.PreConcat(SKMatrix.CreateRotationDegrees(degrees, px, py));

    public void PreScale(float sx, float sy) => SkiaMatrix = SkiaMatrix.PreConcat(SKMatrix.CreateScale(sx, sy));

    public void PreScale(float sx, float sy, float px, float py) => SkiaMatrix = SkiaMatrix.PreConcat(SKMatrix.CreateScale(sx, sy, px, py));

    public void PreSkew(float kx, float ky) => SkiaMatrix = SkiaMatrix.PreConcat(SKMatrix.CreateSkew(kx, ky));

    public void Concat(Matrix matrix) => SkiaMatrix = SkiaMatrix.PostConcat(matrix.SkiaMatrix);

    public static implicit operator SKMatrix(Matrix matrix) => matrix.SkiaMatrix;

    public static implicit operator Matrix(SKMatrix matrix) => new() { SkiaMatrix = matrix };

    public Bounds MapBounds(Bounds bounds) {
        SKPoint[] points = SkiaMatrix.MapPoints(new SKPoint[] { bounds.TopLeft, bounds.TopRight, bounds.BottomRight, bounds.BottomLeft });

        return new Bounds(points[0], points[1], points[2], points[3]);
    }

    public Vector2 MapPoint(Vector2 point) => SkiaMatrix.MapPoint(point);

    public Rect MapRect(Rect rect) => SkiaMatrix.MapRect(rect);

    public Matrix Copy() => new() { SkiaMatrix = SkiaMatrix };

    public Matrix Invert() {
        SkiaMatrix.TryInvert(out SKMatrix inverted);
        return inverted;
    }
}