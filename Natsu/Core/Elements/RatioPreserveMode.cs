namespace Natsu.Core.Elements;

/// <summary>
///     The mode to preserve the ratio of the container.
/// </summary>
public enum RatioPreserveMode {
    /// <summary>
    ///     The content is fit to the width of the container.
    /// </summary>
    Width,

    /// <summary>
    ///     The content is fit to the height of the container.
    /// </summary>
    Height,

    /// <summary>
    ///     The content will fit inside the container.
    /// </summary>
    Fit,

    /// <summary>
    ///     The content will cover the container.
    /// </summary>
    Cover
}
