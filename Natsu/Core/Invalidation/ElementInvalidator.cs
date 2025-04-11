#nullable disable

namespace Natsu.Core.InvalidationTemp;

public class ElementInvalidator {
    public ElementInvalidator(ElementInvalidation invalidation = ElementInvalidation.All) => Element = invalidation;

    public void Reset() {
        Element = ElementInvalidation.None;
        Custom.Clear();
    }

    #region Element Invalidation
    public ElementInvalidation Element { get; private set; }

    public void Invalidate(ElementInvalidation invalidation) => Element |= invalidation;
    public void Validate(ElementInvalidation invalidation) => Element &= ~invalidation;
    #endregion

    #region Custom Invalidation
    public CustomInvalidation Custom { get; } = new();

    public void Invalidate(string invalidation) => Custom.Invalidate(invalidation);
    public void Validate(string invalidation) => Custom.Validate(invalidation);

    public bool HasFlag(string invalidation) => Custom.HasFlag(invalidation);
    public bool HasFlag(ElementInvalidation invalidation) => Element.HasFlag(invalidation);
    #endregion
}
