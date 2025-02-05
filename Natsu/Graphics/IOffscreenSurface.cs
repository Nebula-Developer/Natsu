namespace Natsu.Graphics;

/// <summary>
///     Represents an offscreen surface that can be drawn to.
/// </summary>
public interface IOffscreenSurface : IDisposable {
    /// <summary>
    ///     The <see cref="ICanvas" /> that handles drawing operations.
    /// </summary>
    ICanvas Canvas { get; }

    /// <summary>
    ///     The width of the surface.
    /// </summary>
    int Width { get; }

    /// <summary>
    ///     The height of the surface.
    /// </summary>
    int Height { get; }

    /// <summary>
    ///     Flushes the <see cref="Canvas" />, and does any necessary operations to update the surface.
    /// </summary>
    void Flush();

    /// <summary>
    ///     Takes a snapshot of the surface.
    /// </summary>
    /// <returns>The snapshot of the surface</returns>
    IImage Snapshot();
}
