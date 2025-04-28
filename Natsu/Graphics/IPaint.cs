using Natsu.Graphics.Shaders;

namespace Natsu.Graphics;

/// <summary>
///     Represents a paint that can be used to draw shapes.
/// </summary>
public interface IPaint {
    /// <summary>
    ///     The RGBA color of the paint.
    /// </summary>
    Color Color { get; set; }

    /// <summary>
    ///     The width of the stroke.
    /// </summary>
    float StrokeWidth { get; set; }

    /// <summary>
    ///     Whether the paint is a stroke.
    /// </summary>
    bool IsStroke { get; set; }

    /// <summary>
    ///     Whether to use antialiasing.
    /// </summary>
    bool IsAntialias { get; set; }

    /// <summary>
    ///     The quality of the filter.
    ///     <br />
    ///     This is used to determine the quality of the filter when scaling.
    /// </summary>
    FilterQuality FilterQuality { get; set; }

    /// <summary>
    ///     The blend mode of the paint.
    /// </summary>
    BlendMode BlendMode { get; set; }

    /// <summary>
    ///     The size of the text.
    /// </summary>
    float TextSize { get; set; }

    /// <summary>
    ///     The cap of the stroke.
    /// </summary>
    StrokeCap StrokeCap { get; set; }

    /// <summary>
    ///     The join of the stroke.
    /// </summary>
    StrokeJoin StrokeJoin { get; set; }

    /// <summary>
    ///     The shader of the paint.
    ///     <br />
    ///     A shader will usually override other properties of the paint.
    /// </summary>
    IShader? Shader { get; set; }

    /// <summary>
    ///     The image filter of the paint.
    /// </summary>
    IImageFilter? ImageFilter { get; set; }
}
