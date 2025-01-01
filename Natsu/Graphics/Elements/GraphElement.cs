using Natsu.Graphics.Elements;
using Natsu.Mathematics;

namespace Natsu.Graphics;

public class GraphElement : PaintableElement {
    public VectorPath? Path;

    public GraphElement() { }

    public GraphElement(VectorPath path) => Path = path;

    protected override void OnRender(ICanvas canvas) {
        if (Path == null) return;

        canvas.DrawPath(Path, Paint);
    }
}
