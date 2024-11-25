namespace Natsu.Mathematics;

public struct Bounds : IEquatable<Bounds> {
    public Bounds(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;
        Points = new[] { TopLeft, TopRight, BottomRight, BottomLeft };
    }

    public Vector2 TopLeft { get; }
    public Vector2 TopRight { get; }
    public Vector2 BottomRight { get; }
    public Vector2 BottomLeft { get; }
    public Vector2[] Points { get; }

    public float Width => Vector2.Distance(TopLeft, TopRight);
    public float Height => Vector2.Distance(TopLeft, BottomLeft);
    public float Area => Width * Height;
    public float Perimeter => 2 * (Width + Height);

    public static Bounds Empty { get; } = new(Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero);

    public bool Equals(Bounds other) => TopLeft == other.TopLeft && TopRight == other.TopRight && BottomRight == other.BottomRight && BottomLeft == other.BottomLeft;

    public bool Contains(Vector2 point) => IsPointInTriangle(point, TopLeft, TopRight, BottomRight) || IsPointInTriangle(point, TopLeft, BottomRight, BottomLeft);

    public bool Contains(Bounds bounds) => Contains(bounds.TopLeft) && Contains(bounds.TopRight) && Contains(bounds.BottomRight) && Contains(bounds.BottomLeft);

    public bool Intersects(Bounds bounds) {
        foreach (Vector2 point in bounds.Points)
            if (Contains(point))
                return true;

        foreach (Vector2 point in Points)
            if (bounds.Contains(point))
                return true;

        return false;
    }

    public Bounds Union(Bounds bounds) {
        Vector2 topLeft = new(MathF.Min(TopLeft.X, bounds.TopLeft.X), MathF.Min(TopLeft.Y, bounds.TopLeft.Y));
        Vector2 topRight = new(MathF.Max(TopRight.X, bounds.TopRight.X), MathF.Min(TopRight.Y, bounds.TopRight.Y));
        Vector2 bottomRight = new(MathF.Max(BottomRight.X, bounds.BottomRight.X), MathF.Max(BottomRight.Y, bounds.BottomRight.Y));
        Vector2 bottomLeft = new(MathF.Min(BottomLeft.X, bounds.BottomLeft.X), MathF.Max(BottomLeft.Y, bounds.BottomLeft.Y));

        return new Bounds(topLeft, topRight, bottomRight, bottomLeft);
    }

    public override bool Equals(object? obj) => obj is Bounds other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(TopLeft, TopRight, BottomRight, BottomLeft);

    public override string ToString() => $"Bounds(TopLeft: {TopLeft}, TopRight: {TopRight}, BottomRight: {BottomRight}, BottomLeft: {BottomLeft})";

    public static bool operator ==(Bounds a, Bounds b) => a.Equals(b);

    public static bool operator !=(Bounds a, Bounds b) => !(a == b);

    private static bool IsPointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c) {
        // Barycentric coordinate check
        Vector2 v0 = c - a;
        Vector2 v1 = b - a;
        Vector2 v2 = p - a;

        float dot00 = Vector2.Dot(v0, v0);
        float dot01 = Vector2.Dot(v0, v1);
        float dot02 = Vector2.Dot(v0, v2);
        float dot11 = Vector2.Dot(v1, v1);
        float dot12 = Vector2.Dot(v1, v2);

        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return u >= 0 && v >= 0 && u + v <= 1;
    }
}