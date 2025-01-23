using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     The primary element in an <see cref="Application" />.
///     <br />
///     This element should always be used as the root of the element tree in an application hierarchy.
/// </summary>
public class RootElement : Element {
    public RootElement(Application app) {
        App = app;
        ScaleAffectsDrawSize = false;
    }

    protected override Vector2 ComputeAnchorPosition => AnchorPosition * (DrawSize * Scale);

    public override string Name => "Root";
}
