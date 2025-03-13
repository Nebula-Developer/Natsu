#nullable disable

namespace Natsu.Core;

[Flags]
public enum Invalidation {
    None = 1 << 0,
    Geometry = 1 << 1,
    Size = 1 << 2,
    DrawSize = Size | Geometry,
    Layout = 1 << 3,
    Opacity = 1 << 4,
    All = DrawSize | Layout | Opacity
}

[Flags]
public enum InvalidationPropagation {
    None = 1 << 0,
    Parent = 1 << 1,
    Children = 1 << 2,
    All = Parent | Children
}

public class InvalidationState {
    public InvalidationState(Invalidation invalidation = Invalidation.All) => State = invalidation;

    public Invalidation State { get; private set; }

    public void Invalidate(Invalidation invalidation) => State |= invalidation;

    public void Validate(Invalidation invalidation) => State &= ~invalidation;

    public void Reset() => State = Invalidation.None;
}
