namespace Natsu.Graphics;

public static class Colors {
    public static ReadonlyColor White { get; } = new(255, 255, 255);
    public static ReadonlyColor Black { get; } = new(0, 0, 0);

    public static ReadonlyColor Red { get; } = new(255, 0, 0);
    public static ReadonlyColor Green { get; } = new(0, 255, 0);
    public static ReadonlyColor Blue { get; } = new(0, 0, 255);

    public static ReadonlyColor Yellow { get; } = new(255, 255, 0);
    public static ReadonlyColor Cyan { get; } = new(0, 255, 255);
    public static ReadonlyColor Magenta { get; } = new(255, 0, 255);

    public static ReadonlyColor Orange { get; } = new(255, 165, 0);
    public static ReadonlyColor Purple { get; } = new(128, 0, 128);
    public static ReadonlyColor Pink { get; } = new(255, 192, 203);

    public static ReadonlyColor Aqua { get; } = new(0, 255, 255);
    public static ReadonlyColor Teal { get; } = new(0, 128, 128);
    public static ReadonlyColor Peach { get; } = new(255, 218, 185);

    public static ReadonlyColor Brown { get; } = new(165, 42, 42);
    public static ReadonlyColor Maroon { get; } = new(128, 0, 0);
    public static ReadonlyColor Olive { get; } = new(128, 128, 0);

    public static ReadonlyColor LightGray { get; } = new(211, 211, 211);
    public static ReadonlyColor Gray { get; } = new(128, 128, 128);
    public static ReadonlyColor DarkGray { get; } = new(64, 64, 64);

    public static ReadonlyColor Transparent { get; } = new(0, 0, 0, 0);
    public static ReadonlyColor WhiteTransparent { get; } = new(255, 255, 255, 0);
}
