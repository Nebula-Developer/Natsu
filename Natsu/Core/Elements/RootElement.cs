using Natsu.Mathematics;

namespace Natsu.Core.Elements;

public class RootElement : Element {
    public RootElement(Application app) {
        App = app;
        ScaleAffectsDrawSize = false;
    }

    protected override Vector2 ComputeAnchorPosition => AnchorPosition * (DrawSize * Scale);

    public override string Name => "Root";
}
