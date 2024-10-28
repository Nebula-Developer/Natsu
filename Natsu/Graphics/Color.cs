#nullable disable

using SkiaSharp;

namespace Natsu.Graphics;

public class Color {
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; }

    public Color(byte r, byte g, byte b, byte a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

    public Color() => (R, G, B, A) = (0, 0, 0, 255);

    public static Color operator +(Color a, Color b) => new Color((byte)(a.R + b.R), (byte)(a.G + b.G), (byte)(a.B + b.B), (byte)(a.A + b.A));
    public static Color operator -(Color a, Color b) => new Color((byte)(a.R - b.R), (byte)(a.G - b.G), (byte)(a.B - b.B), (byte)(a.A - b.A));
    public static Color operator *(Color a, float b) => new Color((byte)(a.R * b), (byte)(a.G * b), (byte)(a.B * b), (byte)(a.A * b));
    public static Color operator /(Color a, float b) => new Color((byte)(a.R / b), (byte)(a.G / b), (byte)(a.B / b), (byte)(a.A / b));

    public static bool operator ==(Color a, Color b) => a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
    public static bool operator !=(Color a, Color b) => a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;

    public static Color Lerp(Color a, Color b) => a + (b - a) / 2;

    public override bool Equals(object obj) => obj is Color other && this == other;
    public override int GetHashCode() => HashCode.Combine(R, G, B, A);

    public override string ToString() => $"rgba({R}, {G}, {B}, {A})";


    public static Color FromSKColor(SKColor color) => new Color(color.Red, color.Green, color.Blue, color.Alpha);
    public static SKColor ToSKColor(Color color) => new SKColor(color.R, color.G, color.B, color.A);

    public static implicit operator SKColor(Color color) => ToSKColor(color);
    public static implicit operator Color(SKColor color) => FromSKColor(color);
}
