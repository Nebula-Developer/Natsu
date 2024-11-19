using SkiaSharp;

namespace Natsu.Mathematics;

public struct Bounds : IEquatable<Bounds> {
    public Bounds(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight - Vector2.One;
        BottomLeft = bottomLeft - Vector2.One;
        Points = new[] { TopLeft, TopRight, BottomRight, BottomLeft };

        Path = new SKPath();
        Path.MoveTo(TopLeft.X, TopLeft.Y - 1);
        Path.LineTo(TopRight.X, TopRight.Y - 1);
        Path.LineTo(BottomRight.X, BottomRight.Y);
        Path.LineTo(BottomLeft.X, BottomLeft.Y);
        Path.Close();
    }

    public Vector2 TopLeft { get; }
    public Vector2 TopRight { get; }
    public Vector2 BottomRight { get; }
    public Vector2 BottomLeft { get; }
    public SKPath Path { get; }

    public Vector2[] Points { get; }

    public float Width => Vector2.Distance(TopLeft, TopRight);
    public float Height => Vector2.Distance(TopLeft, BottomLeft);
    public float Area => Width * Height;
    public float Perimeter => 2 * (Width + Height);

    public static Bounds Empty { get; } = new(Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero);

    public bool Equals(Bounds other) => TopLeft == other.TopLeft && TopRight == other.TopRight && BottomRight == other.BottomRight && BottomLeft == other.BottomLeft;

    public bool Contains(Vector2 point) => Path.Contains(point.X, point.Y);

    public bool Contains(Bounds bounds) => Contains(bounds.TopLeft) && Contains(bounds.TopRight) && Contains(bounds.BottomRight) && Contains(bounds.BottomLeft);

    public bool Intersects(Bounds bounds) => Path.Op(bounds.Path, SKPathOp.Intersect, default);

    public Bounds Union(Bounds bounds) {
        var topLeft = new Vector2(MathF.Min(TopLeft.X, bounds.TopLeft.X), MathF.Min(TopLeft.Y, bounds.TopLeft.Y));
        var topRight = new Vector2(MathF.Max(TopRight.X, bounds.TopRight.X), MathF.Min(TopRight.Y, bounds.TopRight.Y));
        var bottomRight = new Vector2(MathF.Max(BottomRight.X, bounds.BottomRight.X), MathF.Max(BottomRight.Y, bounds.BottomRight.Y));
        var bottomLeft = new Vector2(MathF.Min(BottomLeft.X, bounds.BottomLeft.X), MathF.Max(BottomLeft.Y, bounds.BottomLeft.Y));

        return new Bounds(topLeft, topRight, bottomRight, bottomLeft);
    }

    public override bool Equals(object? obj) => obj is Bounds other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(TopLeft, TopRight, BottomRight, BottomLeft);

    public override string ToString() => $"Bounds(TopLeft: {TopLeft}, TopRight: {TopRight}, BottomRight: {BottomRight}, BottomLeft: {BottomLeft})";

    public static bool operator ==(Bounds a, Bounds b) => a.Equals(b);

    public static bool operator !=(Bounds a, Bounds b) => !(a == b);
}
