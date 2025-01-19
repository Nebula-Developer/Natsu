using Natsu.Mathematics;
using SkiaSharp;

namespace Natsu.Tests.Mathematics;

public class MatrixTests {
    internal static void AssertMatrix(float[,] a, float[,] b) {
        for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            Assert.True(MathF.Abs(a[i, j] - b[i, j]) < Precision.Epsilon, $"Expected: {a[i, j]}, Actual: {b[i, j]} at position [{i},{j}]");
    }

    [Fact]
    public void TestReset() {
        Matrix matrix = new();
        matrix.Reset();

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Reset();

        AssertMatrix(skiaMatrix.Values, matrix.Values);
    }

    [Fact]
    public void TestTranslate() {
        Matrix matrix = new();
        matrix.Translate(2, 3);

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Translate(2, 3);

        AssertMatrix(skiaMatrix.Values, matrix.Values);
    }

    [Fact]
    public void TestRotate() {
        Matrix matrix = new();
        matrix.Rotate(90);

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Rotate(90);

        AssertMatrix(skiaMatrix.Values, matrix.Values);
    }

    [Fact]
    public void TestScale() {
        Matrix matrix = new();
        matrix.Scale(2, 3);

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Scale(2, 3);

        AssertMatrix(skiaMatrix.Values, matrix.Values);
    }

    [Fact]
    public void TestSkew() {
        Matrix matrix = new();
        matrix.Skew(1, 2);

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Skew(1, 2);

        AssertMatrix(skiaMatrix.Values, matrix.Values);
    }

    [Fact]
    public void TestMapPoint() {
        Matrix matrix = new();
        matrix.Translate(2, 3);
        Vector2 point = new(1, 1);
        Vector2 mappedPoint = matrix.MapPoint(point);

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Translate(2, 3);
        SKPoint skPoint = new(1, 1);
        SKPoint mappedSkPoint = skiaMatrix.MapPoint(skPoint);

        Assert.True(MathF.Abs(mappedSkPoint.X - mappedPoint.X) < Precision.Epsilon, $"Expected: {mappedSkPoint.X}, Actual: {mappedPoint.X}");
    }

    [Fact]
    public void TestInvert() {
        Matrix matrix = new();
        matrix.Scale(2, 3);
        Matrix invertedMatrix = matrix.Invert();

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Scale(2, 3);
        SkiaMatrix invertedSkiaMatrix = skiaMatrix.Invert();

        AssertMatrix(invertedSkiaMatrix.Values, invertedMatrix.Values);
    }

    [Fact]
    public void TestCopy() {
        Matrix matrix = new();
        matrix.Translate(2, 3);
        Matrix copyMatrix = matrix.Copy();

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Translate(2, 3);
        SkiaMatrix copySkiaMatrix = skiaMatrix.Copy();

        AssertMatrix(copySkiaMatrix.Values, copyMatrix.Values);
    }

    [Fact]
    public void TestMultiplePost() {
        Matrix matrix = new();
        matrix.Translate(2, 3);
        matrix.Scale(2, 3);
        matrix.Rotate(90);
        matrix.Scale(4, 4, 10, 10);

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.Translate(2, 3);
        skiaMatrix.Scale(2, 3);
        skiaMatrix.Rotate(90);
        skiaMatrix.Scale(4, 4, 10, 10);

        AssertMatrix(skiaMatrix.Values, matrix.Values);
    }

    [Fact]
    public void TestMultiplePre() {
        Matrix matrix = new();
        matrix.PreTranslate(2, 3);
        matrix.PreScale(2, 3);
        matrix.PreRotate(90);
        matrix.PreScale(4, 4, 10, 10);

        SkiaMatrix skiaMatrix = new();
        skiaMatrix.PreTranslate(2, 3);
        skiaMatrix.PreScale(2, 3);
        skiaMatrix.PreRotate(90);
        skiaMatrix.PreScale(4, 4, 10, 10);

        AssertMatrix(skiaMatrix.Values, matrix.Values);
    }
}
