using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     An element that will draw an <see cref="IOffscreenSurface" /> to the screen.
/// </summary>
public class OffscreenSurfaceElement : Element {
    private IImage? _snapshot;

    private IOffscreenSurface? _surface;

    public OffscreenSurfaceElement(IOffscreenSurface? surface = null) => Surface = surface;

    public OffscreenSurfaceElement() { }

    /// <summary>
    ///     The offscreen surface to draw.
    /// </summary>
    public IOffscreenSurface? Surface {
        get => _surface;
        set {
            _surface = value;
            Invalidate();
        }
    }

    public bool Valid { get; private set; }

    public bool ImageScaling { get; set; } = false;
    public bool RenderSurface { get; set; } = true;

    public void Invalidate() {
        Valid = false;
        _snapshot?.Dispose();
    }

    public void Validate(IImage image) {
        _snapshot = image;
        Valid = true;
    }

    protected override void OnRender(ICanvas canvas) {
        base.OnRender(canvas);

        if (Surface == null) return;

        if (RenderSurface) {
            if (ImageScaling) throw new InvalidOperationException("Cannot use ImageScaling and RenderSurface at the same time, IOffscreenSurface cannot be scaled directly");

            canvas.DrawOffscreenSurface(Surface, Vector2.Zero);
        } else {
            if (!Valid) {
                Surface.Flush();
                Validate(Surface.Snapshot());
            }

            if (ImageScaling)
                canvas.DrawImage(_snapshot!, new Rect(0, 0, DrawSize.X, DrawSize.Y), Paint);
            else
                canvas.DrawOffscreenSurface(Surface, Vector2.Zero);
        }
    }
}
