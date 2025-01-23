using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     An element that will draw an <see cref="IImage" /> to the screen.
/// </summary>
public class ImageElement : PaintableElement {
    public ImageElement(IImage image) {
        Image = image;
        Size = (Vector2)image.Size;
    }

    public IImage Image { get; }

    protected override void OnRender(ICanvas canvas) => canvas.DrawImage(Image, new Rect(0, 0, DrawSize.X, DrawSize.Y), Paint);
}
