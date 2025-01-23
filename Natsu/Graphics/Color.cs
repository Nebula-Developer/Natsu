#nullable disable

using Natsu.Mathematics;
using SkiaSharp;

namespace Natsu.Graphics;

public class Color {
    public Color(byte r, byte g, byte b, byte a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

    public Color(float r, float g, float b) : this(r, g, b, 255) { }

    public Color(float r, float g, float b, float a) : this((byte)r, (byte)g, (byte)b, (byte)a) { }

    public Color() => (R, G, B, A) = (0, 0, 0, 255);

    public virtual byte R { get; set; }
    public virtual byte G { get; set; }
    public virtual byte B { get; set; }
    public virtual byte A { get; set; }

    public Color Value {
        get => this;
        set => Become(value);
    }

    public static Color operator +(Color a, Color b) => new((byte)Math.Clamp(a.R + b.R, 0, 255), (byte)Math.Clamp(a.G + b.G, 0, 255), (byte)Math.Clamp(a.B + b.B, 0, 255), (byte)Math.Clamp(a.A + b.A, 0, 255));

    public static Color operator -(Color a, Color b) => new((byte)Math.Clamp(a.R - b.R, 0, 255), (byte)Math.Clamp(a.G - b.G, 0, 255), (byte)Math.Clamp(a.B - b.B, 0, 255), (byte)Math.Clamp(a.A - b.A, 0, 255));

    public static Color operator *(Color a, Color b) => new((byte)Math.Clamp(a.R * b.R, 0, 255), (byte)Math.Clamp(a.G * b.G, 0, 255), (byte)Math.Clamp(a.B * b.B, 0, 255), (byte)Math.Clamp(a.A * b.A, 0, 255));

    public static Color operator /(Color a, Color b) => new((byte)Math.Clamp(a.R / b.R, 0, 255), (byte)Math.Clamp(a.G / b.G, 0, 255), (byte)Math.Clamp(a.B / b.B, 0, 255), (byte)Math.Clamp(a.A / b.A, 0, 255));

    public static Color operator *(Color a, float b) => new((byte)Math.Clamp(a.R * b, 0, 255), (byte)Math.Clamp(a.G * b, 0, 255), (byte)Math.Clamp(a.B * b, 0, 255), (byte)Math.Clamp(a.A * b, 0, 255));

    public static Color operator /(Color a, float b) => new((byte)Math.Clamp(a.R / b, 0, 255), (byte)Math.Clamp(a.G / b, 0, 255), (byte)Math.Clamp(a.B / b, 0, 255), (byte)Math.Clamp(a.A / b, 0, 255));

    public static bool operator ==(Color a, Color b) => a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;

    public static bool operator !=(Color a, Color b) => a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;

    public static Color Lerp(Color a, Color b, float t) => new((byte)EasingHelper.Lerp(a.R, b.R, t), (byte)EasingHelper.Lerp(a.G, b.G, t), (byte)EasingHelper.Lerp(a.B, b.B, t), (byte)EasingHelper.Lerp(a.A, b.A, t));

    public override bool Equals(object obj) => obj is Color other && this == other;

    public override int GetHashCode() => HashCode.Combine(R, G, B, A);

    public override string ToString() => $"rgba({R}, {G}, {B}, {A})";

    public static Color FromSKColor(SKColor color) => new(color.Red, color.Green, color.Blue, color.Alpha);

    public static SKColor ToSKColor(Color color) => new(color.R, color.G, color.B, color.A);

    public static implicit operator SKColor(Color color) => ToSKColor(color);

    public static implicit operator Color(SKColor color) => FromSKColor(color);

    public Color Become(Color b) {
        R = b.R;
        G = b.G;
        B = b.B;
        A = b.A;
        return this;
    }
}
