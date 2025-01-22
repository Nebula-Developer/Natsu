namespace Natsu.Graphics;

/// <summary>
/// A collection of basic colors for quick use.
/// </summary>
public static class Colors {
    public static ReadonlyColor White { get; } = new(255, 255, 255);
    public static ReadonlyColor Black { get; } = new(0, 0, 0);

    public static ReadonlyColor Red { get; } = new(255, 0, 0);
    public static ReadonlyColor Green { get; } = new(0, 255, 0);
    public static ReadonlyColor Blue { get; } = new(0, 0, 255);

    public static ReadonlyColor LightRed { get; } = new(255, 99, 71);
    public static ReadonlyColor LightGreen { get; } = new(144, 238, 144);
    public static ReadonlyColor LightBlue { get; } = new(173, 216, 230);

    public static ReadonlyColor DarkRed { get; } = new(139, 0, 0);
    public static ReadonlyColor DarkGreen { get; } = new(0, 100, 0);
    public static ReadonlyColor DarkBlue { get; } = new(0, 0, 139);

    public static ReadonlyColor Yellow { get; } = new(255, 255, 0);
    public static ReadonlyColor Cyan { get; } = new(0, 255, 255);
    public static ReadonlyColor Magenta { get; } = new(255, 0, 255);

    public static ReadonlyColor LightYellow { get; } = new(255, 255, 224);
    public static ReadonlyColor LightCyan { get; } = new(224, 255, 255);
    public static ReadonlyColor LightMagenta { get; } = new(255, 224, 255);

    public static ReadonlyColor DarkYellow { get; } = new(238, 238, 0);
    public static ReadonlyColor DarkCyan { get; } = new(0, 238, 238);
    public static ReadonlyColor DarkMagenta { get; } = new(238, 0, 238);

    public static ReadonlyColor Orange { get; } = new(255, 165, 0);
    public static ReadonlyColor Purple { get; } = new(128, 0, 128);
    public static ReadonlyColor Pink { get; } = new(255, 192, 203);

    public static ReadonlyColor LightOrange { get; } = new(255, 160, 122);
    public static ReadonlyColor LightPurple { get; } = new(147, 112, 219);
    public static ReadonlyColor LightPink { get; } = new(255, 182, 193);

    public static ReadonlyColor DarkOrange { get; } = new(255, 140, 0);
    public static ReadonlyColor DarkPurple { get; } = new(75, 0, 130);
    public static ReadonlyColor DarkPink { get; } = new(255, 20, 147);

    public static ReadonlyColor Aqua { get; } = new(0, 255, 255);
    public static ReadonlyColor Teal { get; } = new(0, 128, 128);
    public static ReadonlyColor Peach { get; } = new(255, 218, 185);

    public static ReadonlyColor LightAqua { get; } = new(224, 255, 255);
    public static ReadonlyColor LightTeal { get; } = new(0, 224, 224);
    public static ReadonlyColor LightPeach { get; } = new(255, 218, 185);

    public static ReadonlyColor DarkAqua { get; } = new(0, 206, 209);
    public static ReadonlyColor DarkTeal { get; } = new(0, 128, 128);
    public static ReadonlyColor DarkPeach { get; } = new(255, 218, 185);

    public static ReadonlyColor Brown { get; } = new(165, 42, 42);
    public static ReadonlyColor Maroon { get; } = new(128, 0, 0);
    public static ReadonlyColor Olive { get; } = new(128, 128, 0);

    public static ReadonlyColor LightBrown { get; } = new(205, 133, 63);
    public static ReadonlyColor LightMaroon { get; } = new(176, 48, 96);
    public static ReadonlyColor LightOlive { get; } = new(192, 192, 128);

    public static ReadonlyColor DarkBrown { get; } = new(139, 69, 19);
    public static ReadonlyColor DarkMaroon { get; } = new(139, 28, 98);
    public static ReadonlyColor DarkOlive { get; } = new(85, 107, 47);

    public static ReadonlyColor LightGray { get; } = new(211, 211, 211);
    public static ReadonlyColor Gray { get; } = new(128, 128, 128);
    public static ReadonlyColor DarkGray { get; } = new(64, 64, 64);

    public static ReadonlyColor Transparent { get; } = new(0, 0, 0, 0);
    public static ReadonlyColor WhiteTransparent { get; } = new(255, 255, 255, 0);
}
