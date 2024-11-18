using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class RectElement : Element {
    public Paint Paint { get; set; } = new() {
        Color = Colors.White,
        IsStroke = true,
        StrokeWidth = 2,
        IsAntialias = true,
        FilterQuality = FilterQuality.High
    };

    public override void OnRender(ICanvas canvas) => canvas.DrawRect(new Rect(0, 0, DrawSize.X, DrawSize.Y), Paint);
}
