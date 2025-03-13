#nullable disable

using Natsu.Mathematics;
using SkiaSharp;

namespace Natsu.Graphics;

public class Color {
    private byte _a;

    private byte _b;

    private byte _g;

    private byte _r;

    public Action DoChange;

    public Color(byte r, byte g, byte b, byte a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

    public Color() => (R, G, B, A) = (0, 0, 0, 255);

    public virtual byte R {
        get => _r;
        set {
            _r = value;
            DoChange?.Invoke();
        }
    }

    public virtual byte G {
        get => _g;
        set {
            _g = value;
            DoChange?.Invoke();
        }
    }

    public virtual byte B {
        get => _b;
        set {
            _b = value;
            DoChange?.Invoke();
        }
    }

    public virtual byte A {
        get => _a;
        set {
            _a = value;
            DoChange?.Invoke();
        }
    }

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

    public static Color FromHSV(float h, float s, float v) {
        int hi = (int)(h / 60) % 6;
        float f = h / 60 - hi;
        float p = v * (1 - s);
        float q = v * (1 - f * s);
        float t = v * (1 - (1 - f) * s);

        return hi switch {
            0 => new((byte)(v * 255), (byte)(t * 255), (byte)(p * 255)),
            1 => new((byte)(q * 255), (byte)(v * 255), (byte)(p * 255)),
            2 => new((byte)(p * 255), (byte)(v * 255), (byte)(t * 255)),
            3 => new((byte)(p * 255), (byte)(q * 255), (byte)(v * 255)),
            4 => new((byte)(t * 255), (byte)(p * 255), (byte)(v * 255)),
            _ => new((byte)(v * 255), (byte)(p * 255), (byte)(q * 255))
        };
    }

    public Color Become(Color b) {
        R = b.R;
        G = b.G;
        B = b.B;
        A = b.A;
        return this;
    }
}

public class ColorF : Color {
    public ColorF(float r, float g, float b, float a) : base((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), (byte)(a * 255)) { }

    public ColorF(float r, float g, float b) : this(r, g, b, 1f) { }

    public ColorF() : base(0, 0, 0, 255) { }
}
