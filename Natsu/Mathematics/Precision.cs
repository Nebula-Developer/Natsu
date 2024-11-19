namespace Natsu.Mathematics;

public static class Precision {
    public const float Epsilon = 0.0001f;

    public static bool Approximately(float a, float b) => Math.Abs(a - b) < Epsilon;
    public static bool Approximately(double a, double b) => Math.Abs(a - b) < Epsilon;
    public static bool Approximately(Vector2 a, Vector2 b) => Approximately(a.X, b.X) && Approximately(a.Y, b.Y);
    public static bool Approximately(Vector3 a, Vector3 b) => Approximately(a.X, b.X) && Approximately(a.Y, b.Y) && Approximately(a.Z, b.Z);
    public static bool Approximately(Vector4 a, Vector4 b) => Approximately(a.X, b.X) && Approximately(a.Y, b.Y) && Approximately(a.Z, b.Z) && Approximately(a.W, b.W);
}