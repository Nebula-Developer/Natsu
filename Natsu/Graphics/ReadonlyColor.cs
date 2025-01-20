namespace Natsu.Graphics;

/// <summary>
///     An immutable color struct.
///     <br />
///     Used for colors that should be copied rather than referenced.
/// </summary>
public class ReadonlyColor {
    public ReadonlyColor(byte r, byte g, byte b, byte a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public ReadonlyColor(byte r, byte g, byte b) : this(r, g, b, 255) { }
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }
    public byte A { get; }

    public static implicit operator Color(ReadonlyColor color) => new(color.R, color.G, color.B, color.A);
    public static explicit operator ReadonlyColor(Color color) => new(color.R, color.G, color.B, color.A);
}
