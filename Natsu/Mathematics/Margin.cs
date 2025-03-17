namespace Natsu.Mathematics;

public struct Margin {
    public float Top { get; set; }
    public float Bottom { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }

    public Vector2 TopLeft => new(Left, Top);
    public Vector2 TopRight => new(Right, Top);
    public Vector2 BottomLeft => new(Left, Bottom);
    public Vector2 BottomRight => new(Right, Bottom);

    public Vector2 Size => new(Left + Right, Top + Bottom);

    public Margin(float m) => Top = Bottom = Left = Right = m;

    public Margin(float tb, float lr) {
        Top = Bottom = tb;
        Left = Right = lr;
    }

    public Margin(float top, float bottom, float left, float right) {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public static implicit operator Vector4(Margin m) => new(m.Left, m.Top, m.Right, m.Bottom);
    public static implicit operator Margin(Vector2 v) => new(v.X, v.Y);
    public static implicit operator Margin(float f) => new(f);

    public static bool operator ==(Margin a, Margin b) => a.Top == b.Top && a.Bottom == b.Bottom && a.Left == b.Left && a.Right == b.Right;
    public static bool operator !=(Margin a, Margin b) => !(a == b);

    public override bool Equals(object? obj) => obj is Margin m && this == m;
    public override int GetHashCode() => Top.GetHashCode() ^ Bottom.GetHashCode() ^ Left.GetHashCode() ^ Right.GetHashCode();
    public override string ToString() => $"({Top}, {Bottom}, {Left}, {Right})";

    public static Margin Lerp(Margin a, Margin b, float t) => new(EasingHelper.LerpF(a.Top, b.Top, t), EasingHelper.LerpF(a.Bottom, b.Bottom, t), EasingHelper.LerpF(a.Left, b.Left, t), EasingHelper.LerpF(a.Right, b.Right, t));
}
