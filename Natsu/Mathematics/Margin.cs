namespace Natsu.Mathematics;

/// <summary>
///     A four-sided margin.
/// </summary>
public struct Margin {
    public float Top { get; set; }
    public float Bottom { get; set; }
    public float Left { get; set; }
    public float Right { get; set; }

    public Vector2 TopLeft => new(Left, Top);
    public Vector2 TopRight => new(Right, Top);
    public Vector2 BottomLeft => new(Left, Bottom);
    public Vector2 BottomRight => new(Right, Bottom);

    /// <summary>
    ///     The total size of the margin.
    ///     <br />
    ///     Equates to (Left + Right, Top + Bottom)
    /// </summary>
    public Vector2 Size => new(Left + Right, Top + Bottom);

    /// <summary>
    ///     Constructs a new Margin with all sides set to the same value.
    /// </summary>
    /// <param name="m">The value to set all sides to</param>
    public Margin(float m) => Top = Bottom = Left = Right = m;

    /// <summary>
    ///     Constructs a new Margin with the same value for left and right, top and bottom.
    /// </summary>
    /// <param name="lr">The value for left and right</param>
    /// <param name="tb">The value for top and bottom</param>
    public Margin(float lr, float tb) {
        Top = Bottom = tb;
        Left = Right = lr;
    }

    /// <summary>
    ///     Constructs a new Margin with the specified values for each side.
    /// </summary>
    /// <param name="top">The value for the top side</param>
    /// <param name="bottom">The value for the bottom side</param>
    /// <param name="left">The value for the left side</param>
    /// <param name="right">The value for the right side</param>
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
