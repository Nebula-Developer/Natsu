using Natsu.Core.Invalidation;

namespace Natsu.Core.Elements;

[Flags]
public enum LayoutAccessProperties {
    None = 0,
    Position = 1 << 0,
    Size = 1 << 1,
    Scale = 1 << 2,
    Pivot = 1 << 3
}

/// <summary>
///     An element that has a <see cref="ComputeLayout" /> method that will lay out its children when invalidated.
/// </summary>
public abstract class LayoutElement : Element {
    public virtual void ComputeLayout() { }

    protected override void OnUpdateChildren(double deltaTime) {
        base.OnUpdateChildren(deltaTime);
        if (Invalidated.HasFlag(ElementInvalidation.Layout)) ComputeLayout();
    }

    protected override void OnChildrenChange() {
        if (Loaded) Invalidate(ElementInvalidation.Layout);
    }
}
