using SkiaSharp;

namespace Natsu.Mathematics;

public class RotationScaleMatrix {
    public RotationScaleMatrix(float rotation, float scale, Vector2 translation) {
        Cos = MathF.Cos(rotation) * scale;
        Sin = MathF.Sin(rotation) * scale;
        Tx = translation.X;
        Ty = translation.Y;
    }

    public RotationScaleMatrix(float cos, float sin, float tx, float ty) {
        Cos = cos;
        Sin = sin;
        Tx = tx;
        Ty = ty;
    }

    public float Cos { get; }
    public float Sin { get; }
    public float Tx { get; }
    public float Ty { get; }

    public static implicit operator SKRotationScaleMatrix(RotationScaleMatrix matrix) => new(matrix.Cos, matrix.Sin, matrix.Tx, matrix.Ty);
}
