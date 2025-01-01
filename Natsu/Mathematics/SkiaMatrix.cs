using SkiaSharp;

namespace Natsu.Mathematics;

public struct SkiaMatrix {
    public SKMatrix Matrix { get; set; } = SKMatrix.CreateIdentity();

    public void Reset() => Matrix = SKMatrix.CreateIdentity();

    public void Translate(float x, float y) => Matrix = Matrix.PostConcat(SKMatrix.CreateTranslation(x, y));

    public void Rotate(float degrees) => Matrix = Matrix.PostConcat(SKMatrix.CreateRotationDegrees(degrees));

    public void Rotate(float degrees, float px, float py) => Matrix = Matrix.PostConcat(SKMatrix.CreateRotationDegrees(degrees, px, py));

    public void Scale(float sx, float sy) => Matrix = Matrix.PostConcat(SKMatrix.CreateScale(sx, sy));

    public void Scale(float sx, float sy, float px, float py) => Matrix = Matrix.PostConcat(SKMatrix.CreateScale(sx, sy, px, py));

    public void Skew(float kx, float ky) => Matrix = Matrix.PostConcat(SKMatrix.CreateSkew(kx, ky));

    public void PreTranslate(float x, float y) => Matrix = Matrix.PreConcat(SKMatrix.CreateTranslation(x, y));

    public void PreRotate(float degrees) => Matrix = Matrix.PreConcat(SKMatrix.CreateRotationDegrees(degrees));

    public void PreRotate(float degrees, float px, float py) => Matrix = Matrix.PreConcat(SKMatrix.CreateRotationDegrees(degrees, px, py));

    public void PreScale(float sx, float sy) => Matrix = Matrix.PreConcat(SKMatrix.CreateScale(sx, sy));

    public void PreScale(float sx, float sy, float px, float py) => Matrix = Matrix.PreConcat(SKMatrix.CreateScale(sx, sy, px, py));

    public void PreSkew(float kx, float ky) => Matrix = Matrix.PreConcat(SKMatrix.CreateSkew(kx, ky));

    public void Concat(SkiaMatrix matrix) => Matrix = Matrix.PostConcat(matrix.Matrix);

    public static implicit operator SKMatrix(SkiaMatrix matrix) => matrix.Matrix;

    public static implicit operator SkiaMatrix(SKMatrix matrix) => new() { Matrix = matrix };

    public Bounds MapBounds(Bounds bounds) {
        SKPoint[]? points = Matrix.MapPoints(new SKPoint[] { bounds.TopLeft, bounds.TopRight, bounds.BottomRight, bounds.BottomLeft });

        return new(points[0], points[1], points[2], points[3]);
    }

    public Vector2 MapPoint(Vector2 point) => Matrix.MapPoint(point);

    public Rect MapRect(Rect rect) => Matrix.MapRect(rect);

    public SkiaMatrix Copy() => new() { Matrix = Matrix };

    public SkiaMatrix Invert() {
        Matrix.TryInvert(out SKMatrix inverted);
        return inverted;
    }

    public SkiaMatrix() { }
}
