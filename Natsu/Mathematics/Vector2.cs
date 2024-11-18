#nullable disable

using SkiaSharp;

namespace Natsu.Mathematics;

public struct Vector2 : IEquatable<Vector2> {
    public float X { get; }
    public float Y { get; }

    public Vector2(float x, float y) {
        X = x;
        Y = y;
    }

    public Vector2(float value) : this(value, value) { }

    public Vector2() : this(0, 0) { }

    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);

    public static Vector2 operator *(Vector2 a, float b) => new(a.X * b, a.Y * b);

    public static Vector2 operator *(float a, Vector2 b) => new(b.X * a, b.Y * a);

    public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);

    public static Vector2 operator /(Vector2 a, float b) => new(a.X / b, a.Y / b);

    public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);

    public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;

    public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

    public static Vector2 operator -(Vector2 a) => new(-a.X, -a.Y);

    public bool Equals(Vector2 other) => this == other;

    public override bool Equals(object obj) => obj is Vector2 other && this == other;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"({X}, {Y})";

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => a + (b - a) * t;

    public Vector2 Lerp(Vector2 other, float t) => Lerp(this, other, t);

    public static float Distance(Vector2 a, Vector2 b) => MathF.Sqrt(MathF.Pow(b.X - a.X, 2) + MathF.Pow(b.Y - a.Y, 2));

    public float Magnitude => MathF.Sqrt(X * X + Y * Y);

    public Vector2 Normalize() {
        float length = Magnitude;
        return length > 0 ? new Vector2(X / length, Y / length) : new Vector2(0, 0);
    }

    public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;

    public static implicit operator (float, float)(Vector2 v) => (v.X, v.Y);

    public static implicit operator Vector2((float, float) t) => new(t.Item1, t.Item2);

    public static implicit operator SKPoint(Vector2 v) => new(v.X, v.Y);

    public static implicit operator Vector2(SKPoint p) => new(p.X, p.Y);

    public static implicit operator Vector2(float f) => new(f, f);

    public static Vector2 Zero => new(0, 0);
    public static Vector2 One => new(1, 1);
}
