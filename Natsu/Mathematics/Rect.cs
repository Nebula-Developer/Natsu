#nullable disable

using SkiaSharp;

namespace Natsu.Mathematics;

public class Rect : IEquatable<Rect> {
    public float X { get; } // X-coordinate of the top-left corner
    public float Y { get; } // Y-coordinate of the top-left corner
    public float Width { get; }
    public float Height { get; }

    public Rect(float x, float y, float width, float height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public float Area => Width * Height;

    public float Perimeter => 2 * (Width + Height);

    public bool Contains(float x, float y) =>
        x >= X && x <= X + Width && y >= Y && y <= Y + Height;

    public bool Contains(Vector2 point) => Contains(point.X, point.Y);

    public bool Intersects(Rect other) =>
        X < other.X + other.Width && X + Width > other.X &&
        Y < other.Y + other.Height && Y + Height > other.Y;

    public Rect Intersection(Rect other) {
        if (!Intersects(other)) return null;

        float x = MathF.Max(X, other.X);
        float y = MathF.Max(Y, other.Y);
        float width = MathF.Min(X + Width, other.X + other.Width) - x;
        float height = MathF.Min(Y + Height, other.Y + other.Height) - y;

        return new Rect(x, y, width, height);
    }

    public override bool Equals(object obj) => obj is Rect other && this == other;

    public bool Equals(Rect other) =>
        other != null && X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;

    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    public override string ToString() => $"Rect(X: {X}, Y: {Y}, Width: {Width}, Height: {Height})";

    public static bool operator ==(Rect a, Rect b) =>
        a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;

    public static bool operator !=(Rect a, Rect b) => !(a == b);

    public static implicit operator (float, float, float, float)(Rect r) => (r.X, r.Y, r.Width, r.Height);
    public static implicit operator Rect((float, float, float, float) t) => new Rect(t.Item1, t.Item2, t.Item3, t.Item4);
    public static implicit operator SKRect(Rect r) => new SKRect(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
    public static implicit operator Rect(SKRect r) => new Rect(r.Left, r.Top, r.Width, r.Height);
}