using Natsu.Mathematics;

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
        AssertMatrix(new float[3, 3] {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 }
        }, matrix.Values);
    }

    [Fact]
    public void TestTranslate() {
        Matrix matrix = new();
        matrix.Translate(2, 3);
        AssertMatrix(new float[3, 3] {
            { 1, 0, 2 },
            { 0, 1, 3 },
            { 0, 0, 1 }
        }, matrix.Values);
    }

    [Fact]
    public void TestRotate() {
        Matrix matrix = new();
        matrix.Rotate(90);

        float radians = MathF.PI / 2;

        AssertMatrix(new float[3, 3] {
            { MathF.Cos(radians), -MathF.Sin(radians), 0 },
            { MathF.Sin(radians), MathF.Cos(radians), 0 },
            { 0, 0, 1 }
        }, matrix.Values);
    }

    [Fact]
    public void TestScale() {
        Matrix matrix = new();
        matrix.Scale(2, 3);
        AssertMatrix(new float[3, 3] {
            { 2, 0, 0 },
            { 0, 3, 0 },
            { 0, 0, 1 }
        }, matrix.Values);
    }

    [Fact]
    public void TestSkew() {
        Matrix matrix = new();
        matrix.Skew(1, 2);
        AssertMatrix(new float[3, 3] {
            { 1, 1, 0 },
            { 2, 1, 0 },
            { 0, 0, 1 }
        }, matrix.Values);
    }

    [Fact]
    public void TestMapPoint() {
        Matrix matrix = new();
        matrix.Translate(2, 3);
        Vector2 point = new(1, 1);
        Vector2 mappedPoint = matrix.MapPoint(point);
        Assert.Equal(new(3, 4), mappedPoint);
    }

    [Fact]
    public void TestInvert() {
        Matrix matrix = new();
        matrix.Scale(2, 3);
        Matrix invertedMatrix = matrix.Invert();
        AssertMatrix(new float[3, 3] {
            { 0.5f, 0, 0 },
            { 0, 0.33333334f, 0 },
            { 0, 0, 1 }
        }, invertedMatrix.Values);
    }

    [Fact]
    public void TestCopy() {
        Matrix matrix = new();
        matrix.Translate(2, 3);
        Matrix copyMatrix = matrix.Copy();
        AssertMatrix(matrix.Values, copyMatrix.Values);
    }
}
