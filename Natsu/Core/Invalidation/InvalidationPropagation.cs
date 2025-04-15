#nullable disable

namespace Natsu.Core.Invalidation;

[Flags]
public enum InvalidationPropagation {
    None = 1 << 0,
    Parent = 1 << 1,
    Children = 1 << 2,
    All = Parent | Children
}
