using Natsu.Core.Invalidation;

namespace Natsu.Core.Elements;

/// <summary>
///     An element that has a <see cref="ComputeLayout" /> method that will lay out its children when invalidated.
/// </summary>
public abstract class LayoutElement : Element {
    public virtual void ComputeLayout() { }

    protected override void OnUpdate(double _) {
        if (Invalidated.HasFlag(ElementInvalidation.Layout)) ComputeLayout();
    }

    protected override void OnChildrenChange() {
        if (Loaded) Invalidate(ElementInvalidation.Layout);
    }
}
