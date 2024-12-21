#nullable disable

namespace Natsu.Mathematics;

public struct Vector2i : IEquatable<Vector2i> {
    public int X { get; }
    public int Y { get; }

    public Vector2i(int x, int y) {
        X = x;
        Y = y;
    }

    public Vector2i(int value) : this(value, value) { }

    public Vector2i() : this(0, 0) { }

    public static Vector2i operator +(Vector2i a, Vector2i b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2i operator -(Vector2i a, Vector2i b) => new(a.X - b.X, a.Y - b.Y);

    public static Vector2i operator *(Vector2i a, int b) => new(a.X * b, a.Y * b);

    public static Vector2i operator *(int a, Vector2i b) => new(b.X * a, b.Y * a);

    public static Vector2i operator *(Vector2i a, Vector2i b) => new(a.X * b.X, a.Y * b.Y);

    public static Vector2i operator /(Vector2i a, int b) => new(a.X / b, a.Y / b);

    public static Vector2i operator /(Vector2i a, Vector2i b) => new(a.X / b.X, a.Y / b.Y);

    public static bool operator ==(Vector2i a, Vector2i b) => a.X == b.X && a.Y == b.Y;

    public static bool operator !=(Vector2i a, Vector2i b) => !(a == b);

    public static Vector2i operator -(Vector2i a) => new(-a.X, -a.Y);

    public bool Equals(Vector2i other) => this == other;

    public override bool Equals(object obj) => obj is Vector2i other && this == other;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"({X}, {Y})";

    public static Vector2i Lerp(Vector2i a, Vector2i b, float t) {
        int x = (int)(a.X + (b.X - a.X) * t);
        int y = (int)(a.Y + (b.Y - a.Y) * t);
        return new Vector2i(x, y);
    }

    public Vector2i Lerp(Vector2i other, float t) => Lerp(this, other, t);

    public Vector2i Max(Vector2i other) => new(Math.Max(X, other.X), Math.Max(Y, other.Y));
    public Vector2i Min(Vector2i other) => new(Math.Min(X, other.X), Math.Min(Y, other.Y));

    public static Vector2i Max(Vector2i a, Vector2i b) => a.Max(b);
    public static Vector2i Min(Vector2i a, Vector2i b) => a.Min(b);

    public static int Distance(Vector2i a, Vector2i b) => (int)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));

    public int Magnitude => (int)Math.Sqrt(X * X + Y * Y);

    public Vector2i Normalize() {
        int length = Magnitude;
        return length > 0 ? new Vector2i(X / length, Y / length) : new Vector2i(0, 0);
    }

    public static int Dot(Vector2i a, Vector2i b) => a.X * b.X + a.Y * b.Y;

    public static implicit operator (int, int)(Vector2i v) => (v.X, v.Y);

    public static implicit operator Vector2i((int, int) t) => new(t.Item1, t.Item2);

    public static implicit operator Vector2i(Vector2 v) => new((int)v.X, (int)v.Y);

    public static explicit operator Vector2(Vector2i v) => new(v.X, v.Y);

    public static Vector2i Zero => new(0, 0);
    public static Vector2i One => new(1, 1);
}
