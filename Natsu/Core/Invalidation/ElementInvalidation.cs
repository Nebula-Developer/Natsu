#nullable disable

namespace Natsu.Core.Invalidation;

[Flags]
public enum ElementInvalidation {
    None = 1 << 0,

    /// <summary>
    ///     Primarily used as invalidation for the <see cref="Element.Matrix" /> property.
    /// </summary>
    Geometry = 1 << 1,

    /// <summary>
    ///     Indicates the <see cref="Element.DrawSize" /> needs to be recalculated, without affecting the matrix.
    /// </summary>
    Size = 1 << 2,

    /// <summary>
    ///     Indicates the <see cref="Element.DrawSize" /> and <see cref="Element.Matrix" /> need to be recalculated.
    ///     <br />
    ///     This is the most common invalidation type for transform properties.
    /// </summary>
    DrawSize = Size | Geometry,

    /// <summary>
    ///     A custom invalidation for elements, used for layout purposes.
    /// </summary>
    Layout = 1 << 3,

    /// <summary>
    ///     Indicates the <see cref="Element.WorldOpacity" /> needs to be recalculated.
    /// </summary>
    Opacity = 1 << 4,

    /// <summary>
    ///     All possible invalidation types.
    /// </summary>
    All = DrawSize | Layout | Opacity
}
