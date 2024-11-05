#nullable disable

namespace Natsu.Mathematics;

public struct Vector4 : IEquatable<Vector4> {
    public float X { get; }
    public float Y { get; }
    public float Z { get; }
    public float W { get; }

    public Vector4(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4(float value) : this(value, value, value, value) { }

    public Vector4() : this(0, 0, 0, 0) { }

    public static Vector4 operator +(Vector4 a, Vector4 b) => new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Vector4 operator -(Vector4 a, Vector4 b) => new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    public static Vector4 operator *(Vector4 a, float b) => new Vector4(a.X * b, a.Y * b, a.Z * b, a.W * b);
    public static Vector4 operator *(float a, Vector4 b) => new Vector4(b.X * a, b.Y * a, b.Z * a, b.W * a);
    public static Vector4 operator *(Vector4 a, Vector4 b) => new Vector4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
    public static Vector4 operator /(Vector4 a, float b) => new Vector4(a.X / b, a.Y / b, a.Z / b, a.W / b);

    public static bool operator ==(Vector4 a, Vector4 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
    public static bool operator !=(Vector4 a, Vector4 b) => !(a == b);

    public bool Equals(Vector4 other) => this == other;
    public override bool Equals(object obj) => obj is Vector4 other && this == other;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public override string ToString() => $"({X}, {Y}, {Z}, {W})";

    public static Vector4 Lerp(Vector4 a, Vector4 b, float t) => a + (b - a) * t;

    public static float Distance(Vector4 a, Vector4 b) => MathF.Sqrt(MathF.Pow(b.X - a.X, 2) + MathF.Pow(b.Y - a.Y, 2) + MathF.Pow(b.Z - a.Z, 2) + MathF.Pow(b.W - a.W, 2));

    public float Magnitude => MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);

    public Vector4 Normalize() {
        float length = Magnitude;
        return length > 0 ? new Vector4(X / length, Y / length, Z / length, W / length) : new Vector4(0, 0, 0, 0);
    }

    public static float Dot(Vector4 a, Vector4 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;

    public static implicit operator (float, float, float, float)(Vector4 v) => (v.X, v.Y, v.Z, v.W);
    public static implicit operator Vector4((float, float, float, float) t) => new Vector4(t.Item1, t.Item2, t.Item3, t.Item4);
    public static implicit operator Vector4(float f) => new Vector4(f, f, f, f);
}