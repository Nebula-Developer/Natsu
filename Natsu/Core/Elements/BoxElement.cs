using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A simple box that is drawn to the screen.
/// </summary>
public class BoxElement : PaintableElement {
    /// <summary>
    ///     The rounded corners of the box.
    ///     <br />
    ///     Execute the <see cref="ICanvas.DrawRoundRect" /> method if not equal to zero.
    /// </summary>
    public Vector2 RoundedCorners { get; set; }

    protected override void OnRender(ICanvas canvas) {
        if (RoundedCorners != Vector2.Zero)
            canvas.DrawRoundRect(new(0, 0, DrawSize.X, DrawSize.Y), RoundedCorners, Paint);
        else
            canvas.DrawRect(new(0, 0, DrawSize.X, DrawSize.Y), Paint);
    }

    public override void ClipCanvas(ICanvas canvas) {
        if (RoundedCorners != Vector2.Zero)
            canvas.ClipRoundRect(new(0, 0, DrawSize.X, DrawSize.Y), RoundedCorners);
        else
            canvas.ClipRect(new(0, 0, DrawSize.X, DrawSize.Y));
    }
}
