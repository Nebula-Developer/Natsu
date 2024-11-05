
using Natsu.Mathematics;

namespace Natsu.Graphics;

public class ImageElement : PaintableElement {
    public IImage Image { get; }
    public ImageElement(IImage image) => Image = image;

    public override void OnRender(ICanvas canvas) => canvas.DrawImage(Image, new Rect(0, 0, Size.X, Size.Y), Paint);
}
