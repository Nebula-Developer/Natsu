using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class RootElement : Element {
    public RootElement(Application app) {
        App = app;
    }

    protected override Vector2 ComputeAnchorPosition => AnchorPosition * (Size * Scale);

    public override string Name => "Root";
}
