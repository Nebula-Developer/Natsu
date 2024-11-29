#nullable disable

namespace Natsu.Graphics;

[Flags]
public enum Invalidation {
    None = 1 << 0,
    Geometry = 1 << 1,
    DrawSize = 1 << 2 | Geometry,
    All = Geometry | DrawSize
}