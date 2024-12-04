using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class CachedElement : Element {
    private bool _passThrough;

    public bool Valid { get; set; }
    public bool PassThrough {
        get => _passThrough;
        set {
            if (_passThrough == value) return;

            _passThrough = value;
            ForChildren(e => e.Invalidate(Invalidation.DrawSize));
        }
    }

    public CachedElement() {
        DoSizeChange += _ => Valid = false;
    }

    public IOffscreenSurface? Surface { get; private set; }

    private Matrix _identityMatrix = new();
    public override Matrix ChildAccessMatrix => PassThrough ? Matrix : _identityMatrix;

    public void Rerender() {
        Valid = true;

        // To prevent clipping content while preserving Matrix we create
        // a first surface with the size of DrawSize + WorldPosition
        // to account for clipping, but this means there may be a giant
        // memory allocation if the position is far away.

        // Otherwise, we can just set the ChildAccessMatrix to
        // an empty Matrix, meaning each element will position
        // from (0,0), which is relative to the surface coordinates
        // but not the world coordinates.

        IOffscreenSurface nSurface = App.Renderer.CreateOffscreenSurface((int)DrawSize.X, (int)DrawSize.Y);

        nSurface.Canvas.Clear(Colors.Transparent);
        ForChildren(e => e.Render(nSurface.Canvas));
        nSurface.Flush();

        lock (Surface ?? new object()) {
            Surface?.Dispose();
            Surface = nSurface;
        }
    }

    protected override void OnRender(ICanvas canvas) {
        if (PassThrough) return;

        if (Surface == null || !Valid) Rerender();
        if (Surface != null) canvas.DrawOffscreenSurface(Surface, new Vector2(0, 0));
    }

    protected override void OnRenderChildren(ICanvas canvas) {
        if (PassThrough) base.OnRenderChildren(canvas);
    }
}
