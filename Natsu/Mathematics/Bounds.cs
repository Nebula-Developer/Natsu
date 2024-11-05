using System;
using System.Numerics;
using System.Security.Cryptography;

using SkiaSharp;

namespace Natsu.Mathematics {
    public class Bounds : IEquatable<Bounds> {
        public Vector2 TopLeft { get; }
        public Vector2 TopRight { get; }
        public Vector2 BottomRight { get; }
        public Vector2 BottomLeft { get; }
        public SKPath Path { get; }

        public Bounds(Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft) {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;

            Path = new SKPath();
            Path.MoveTo(TopLeft.X, TopLeft.Y);
            Path.LineTo(TopRight.X, TopRight.Y);
            Path.LineTo(BottomRight.X, BottomRight.Y);
            Path.LineTo(BottomLeft.X, BottomLeft.Y);
            Path.Close();
        }

        public bool Contains(Vector2 point) => Path.Contains(point.X, point.Y);
        public bool Contains(Bounds bounds) => Contains(bounds.TopLeft) && Contains(bounds.TopRight) && Contains(bounds.BottomRight) && Contains(bounds.BottomLeft);
        public bool Intersects(Bounds bounds) => Path.Op(bounds.Path, SKPathOp.Intersect, default);

        public float Width => Vector2.Distance(TopLeft, TopRight);
        public float Height => Vector2.Distance(TopLeft, BottomLeft);
        public float Area => Width * Height;
        public float Perimeter => 2 * (Width + Height);

        public override bool Equals(object? obj) => obj is Bounds other && Equals(other);

        public bool Equals(Bounds? other) =>
            TopLeft == other?.TopLeft &&
            TopRight == other.TopRight &&
            BottomRight == other.BottomRight &&
            BottomLeft == other.BottomLeft;

        public override int GetHashCode() =>
            HashCode.Combine(TopLeft, TopRight, BottomRight, BottomLeft);

        public override string ToString() =>
            $"Bounds(TopLeft: {TopLeft}, TopRight: {TopRight}, BottomRight: {BottomRight}, BottomLeft: {BottomLeft})";

        public static bool operator ==(Bounds a, Bounds b) =>
            a.Equals(b);

        public static bool operator !=(Bounds a, Bounds b) =>
            !(a == b);

        public static Bounds Empty { get; } = new Bounds(Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero);
    }
}
