#nullable disable

namespace Natsu.Mathematics;

public struct Vector3 : IEquatable<Vector3> {
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    public Vector3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(float value) : this(value, value, value) { }

    public Vector3() : this(0, 0, 0) { }

    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector3 operator *(Vector3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);

    public static Vector3 operator *(float a, Vector3 b) => new(b.X * a, b.Y * a, b.Z * a);

    public static Vector3 operator *(Vector3 a, Vector3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    public static Vector3 operator /(Vector3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);

    public static bool operator ==(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

    public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);

    public bool Equals(Vector3 other) => this == other;

    public override bool Equals(object obj) => obj is Vector3 other && this == other;

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override string ToString() => $"({X}, {Y}, {Z})";

    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * t;

    public static float Distance(Vector3 a, Vector3 b) => MathF.Sqrt(MathF.Pow(b.X - a.X, 2) + MathF.Pow(b.Y - a.Y, 2) + MathF.Pow(b.Z - a.Z, 2));

    public float Magnitude => MathF.Sqrt(X * X + Y * Y + Z * Z);

    public Vector3 Normalize() {
        float length = Magnitude;
        return length > 0 ? new Vector3(X / length, Y / length, Z / length) : new Vector3(0, 0, 0);
    }

    public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static implicit operator (float, float, float)(Vector3 v) => (v.X, v.Y, v.Z);

    public static implicit operator Vector3((float, float, float) t) => new(t.Item1, t.Item2, t.Item3);

    public static implicit operator Vector3(float f) => new(f, f, f);
}