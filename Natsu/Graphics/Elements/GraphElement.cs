using Natsu.Graphics.Elements;

namespace Natsu.Graphics;

public class GraphElement : PaintableElement {
    public VectorPath? Path;

    public GraphElement() { }

    public GraphElement(VectorPath path) {
        Path = path;
    }

    public override void OnRender(ICanvas canvas) {
        if (Path == null) return;

        canvas.DrawPath(Path, Paint);
    }
}
