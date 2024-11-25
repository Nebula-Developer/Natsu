using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class RectElement : PaintableElement {
    public Vector2 RoundedCorners { get; set; }

    public override void OnRender(ICanvas canvas) {
        if (RoundedCorners != Vector2.Zero)
            canvas.DrawRoundRect(new Rect(0, 0, DrawSize.X, DrawSize.Y), RoundedCorners, Paint);
        else
            canvas.DrawRect(new Rect(0, 0, DrawSize.X, DrawSize.Y), Paint);
    }

    public override void ClipCanvas(ICanvas canvas) {
        if (RoundedCorners != Vector2.Zero)
            canvas.ClipRoundRect(new Rect(0, 0, DrawSize.X, DrawSize.Y), RoundedCorners);
        else
            canvas.ClipRect(new Rect(0, 0, DrawSize.X, DrawSize.Y));
    }
}