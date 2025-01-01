using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

public class ImageElement : PaintableElement {
    public ImageElement(IImage image) {
        Image = image;
        Size = (Vector2)image.Size;
    }

    public IImage Image { get; }

    protected override void OnRender(ICanvas canvas) => canvas.DrawImage(Image, new Rect(0, 0, DrawSize.X, DrawSize.Y), Paint);
}
