

using Natsu.Mathematics;

namespace Natsu.Graphics;

public class RootElement : Element {
    protected override Vector2 ComputeAnchorPosition => AnchorPosition * (Size * Scale);

    public RootElement(Application app) => App = app;

    public override string Name {
        get => "Root";
    }
}
