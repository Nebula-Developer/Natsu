using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

public class GraphElement : PaintableElement {
    public VectorPath? Path;

    public GraphElement() { }

    public GraphElement(VectorPath path) => Path = path;

    protected override void OnRender(ICanvas canvas) {
        if (Path == null) return;

        canvas.DrawPath(Path, Paint);
    }
}
