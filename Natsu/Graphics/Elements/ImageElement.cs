using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class ImageElement : PaintableElement {
    public ImageElement(IImage image) {
        Image = image;
    }

    public IImage Image { get; }

    public override void OnRender(ICanvas canvas) => canvas.DrawImage(Image, new Rect(0, 0, DrawSize.X, DrawSize.Y), Paint);
}