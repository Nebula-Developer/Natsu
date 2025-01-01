namespace Natsu.Graphics;

public interface IPaint {
    Color Color { get; set; }
    float StrokeWidth { get; set; }
    bool IsStroke { get; set; }
    bool IsAntialias { get; set; }
    FilterQuality FilterQuality { get; set; }
    float TextSize { get; set; }
    StrokeCap StrokeCap { get; set; }
    StrokeJoin StrokeJoin { get; set; }
}
