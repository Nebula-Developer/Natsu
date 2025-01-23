using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A graph element that will draw a <see cref="VectorPath" /> to the screen.
/// </summary>
public class GraphElement : PaintableElement {
    /// <summary>
    ///     The path to draw.
    /// </summary>
    public VectorPath? Path;

    public GraphElement() { }

    public GraphElement(VectorPath path) => Path = path;

    protected override void OnRender(ICanvas canvas) {
        if (Path == null) return;

        canvas.DrawPath(Path, Paint);
    }
}
