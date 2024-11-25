using SkiaSharp;

namespace Natsu.Mathematics;

public struct Matrix {
    public float[,] Values;

    public Matrix() {
        Values = new float[3, 3];
        Reset();
    }

    public void Reset() => Values = new float[3, 3] {
        { 1, 0, 0 },
        { 0, 1, 0 },
        { 0, 0, 1 }
    };

    public void Translate(float x, float y) {
        float[,] translation = new float[3, 3] {
            { 1, 0, x },
            { 0, 1, y },
            { 0, 0, 1 }
        };
        Concat(translation);
    }

    public void Rotate(float degrees) {
        float radians = MathF.PI / 180 * degrees;
        float cos = MathF.Cos(radians);
        float sin = MathF.Sin(radians);

        float[,] rotation = new float[3, 3] {
            { cos, -sin, 0 },
            { sin, cos, 0 },
            { 0, 0, 1 }
        };
        Concat(rotation);
    }

    public void Rotate(float degrees, float px, float py) {
        Translate(-px, -py);
        Rotate(degrees);
        Translate(px, py);
    }

    public void Scale(float sx, float sy) {
        float[,] scale = new float[3, 3] {
            { sx, 0, 0 },
            { 0, sy, 0 },
            { 0, 0, 1 }
        };
        Concat(scale);
    }

    public void Scale(float sx, float sy, float px, float py) {
        Translate(-px, -py);
        Scale(sx, sy);
        Translate(px, py);
    }

    public void Skew(float kx, float ky) {
        float[,] skew = new float[3, 3] {
            { 1, kx, 0 },
            { ky, 1, 0 },
            { 0, 0, 1 }
        };
        Concat(skew);
    }

    public void PreTranslate(float x, float y) => PreConcat(CreateTranslation(x, y));
    public void PreRotate(float degrees) => PreConcat(CreateRotation(degrees));
    public void PreRotate(float degrees, float px, float py) => PreConcat(CreateRotation(degrees, px, py));
    public void PreScale(float sx, float sy) => PreConcat(CreateScale(sx, sy));
    public void PreScale(float sx, float sy, float px, float py) => PreConcat(CreateScale(sx, sy, px, py));
    public void PreSkew(float kx, float ky) => PreConcat(CreateSkew(kx, ky));

    public void Concat(Matrix matrix) => Concat(matrix.Values);

    public static implicit operator SKMatrix(Matrix matrix) => new() {
        ScaleX = matrix.Values[0, 0],
        SkewX = matrix.Values[0, 1],
        TransX = matrix.Values[0, 2],
        SkewY = matrix.Values[1, 0],
        ScaleY = matrix.Values[1, 1],
        TransY = matrix.Values[1, 2],
        Persp0 = matrix.Values[2, 0],
        Persp1 = matrix.Values[2, 1],
        Persp2 = matrix.Values[2, 2]
    };

    public static implicit operator Matrix(SKMatrix skMatrix) => new() {
        Values = new float[3, 3] {
            { skMatrix.ScaleX, skMatrix.SkewX, skMatrix.TransX },
            { skMatrix.SkewY, skMatrix.ScaleY, skMatrix.TransY },
            { skMatrix.Persp0, skMatrix.Persp1, skMatrix.Persp2 }
        }
    };

    public Vector2 MapPoint(Vector2 point) {
        float[] result = MultiplyVector(point, Values);
        return new Vector2(result[0], result[1]);
    }

    public Bounds MapBounds(Bounds bounds) {
        Vector2[] mappedPoints = new Vector2[4];
        Vector2[] points = bounds.Points;

        for (int i = 0; i < 4; i++) mappedPoints[i] = MapPoint(points[i]);

        return new Bounds(mappedPoints[0], mappedPoints[1], mappedPoints[2], mappedPoints[3]);
    }

    public Matrix Copy() {
        Matrix newMatrix = new();
        Array.Copy(Values, newMatrix.Values, Values.Length);
        return newMatrix;
    }

    public Matrix Invert() {
        float[,]? inverted = InvertMatrix(Values);
        if (inverted != null) return new Matrix { Values = inverted };

        throw new InvalidOperationException("Matrix cannot be inverted.");
    }

    private void Concat(float[,] other) => Values = MultiplyMatrix(other, Values);

    private void PreConcat(float[,] other) => Values = MultiplyMatrix(Values, other);

    private static float[,] CreateTranslation(float x, float y) => new float[3, 3] {
        { 1, 0, x },
        { 0, 1, y },
        { 0, 0, 1 }
    };

    private static float[,] CreateRotation(float degrees) {
        float radians = MathF.PI / 180 * degrees;
        float cos = MathF.Cos(radians);
        float sin = MathF.Sin(radians);

        return new float[3, 3] {
            { cos, -sin, 0 },
            { sin, cos, 0 },
            { 0, 0, 1 }
        };
    }

    private static float[,] CreateRotation(float degrees, float px, float py) {
        float[,] translationToOrigin = CreateTranslation(-px, -py);
        float[,] rotation = CreateRotation(degrees);
        float[,] translationBack = CreateTranslation(px, py);

        return MultiplyMatrix(translationBack, MultiplyMatrix(rotation, translationToOrigin));
    }

    private static float[,] CreateScale(float sx, float sy) => new float[3, 3] {
        { sx, 0, 0 },
        { 0, sy, 0 },
        { 0, 0, 1 }
    };

    private static float[,] CreateScale(float sx, float sy, float px, float py) {
        float[,] translationToOrigin = CreateTranslation(-px, -py);
        float[,] scale = CreateScale(sx, sy);
        float[,] translationBack = CreateTranslation(px, py);

        return MultiplyMatrix(translationBack, MultiplyMatrix(scale, translationToOrigin));
    }

    private static float[,] CreateSkew(float kx, float ky) => new float[3, 3] {
        { 1, kx, 0 },
        { ky, 1, 0 },
        { 0, 0, 1 }
    };

    private static float[,] MultiplyMatrix(float[,] a, float[,] b) {
        float[,] result = new float[3, 3];

        for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            result[i, j] = a[i, 0] * b[0, j] + a[i, 1] * b[1, j] + a[i, 2] * b[2, j];

        return result;
    }

    private static float[] MultiplyVector(Vector2 vector, float[,] matrix) => new[] {
        vector.X * matrix[0, 0] + vector.Y * matrix[0, 1] + matrix[0, 2],
        vector.X * matrix[1, 0] + vector.Y * matrix[1, 1] + matrix[1, 2],
        vector.X * matrix[2, 0] + vector.Y * matrix[2, 1] + matrix[2, 2]
    };

    private static float[,]? InvertMatrix(float[,] matrix) {
        float determinant = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                            matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

        if (MathF.Abs(determinant) < float.Epsilon) return null; // Singular matrix

        float invDet = 1f / determinant;

        float[,] result = new float[3, 3];
        // Calculate cofactors and transpose the matrix for adjugate
        result[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) * invDet;
        result[0, 1] = (matrix[0, 2] * matrix[2, 1] - matrix[0, 1] * matrix[2, 2]) * invDet;
        result[0, 2] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) * invDet;

        result[1, 0] = (matrix[1, 2] * matrix[2, 0] - matrix[1, 0] * matrix[2, 2]) * invDet;
        result[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) * invDet;
        result[1, 2] = (matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2]) * invDet;

        result[2, 0] = (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]) * invDet;
        result[2, 1] = (matrix[0, 1] * matrix[2, 0] - matrix[0, 0] * matrix[2, 1]) * invDet;
        result[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) * invDet;

        return result;
    }
}