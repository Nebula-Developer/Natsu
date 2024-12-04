using Natsu.Mathematics;
using Natsu.Utils;

namespace Natsu.Graphics.Elements;

public class CachedElement : Element {

    private readonly Matrix _identityMatrix = new();
    private bool _passThrough;

    public Dirty DirtySurface = new();
    public IOffscreenSurface? Surface;

    public CachedElement() {
        DoSizeChange += _ => DirtySurface.Invalidate();
    }

    public override bool BlockPositionalInput => true;

    public bool PassThrough {
        get => _passThrough;
        set {
            if (_passThrough == value) return;

            _passThrough = value;
            ForChildren(e => e.Invalidate(Invalidation.DrawSize));
        }
    }
    public override Matrix ChildAccessMatrix => PassThrough ? Matrix : _identityMatrix;

    public void Rerender() {
        lock (DirtySurface) {
            DirtySurface.Validate();

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

            Surface?.Dispose();
        }
    }

    protected override void OnRender(ICanvas canvas) {
        if (PassThrough) return;

        if (Surface == null || DirtySurface.IsDirty) Rerender();
        if (Surface != null) canvas.DrawOffscreenSurface(Surface, new Vector2(0, 0));
    }

    protected override void OnRenderChildren(ICanvas canvas) {
        if (PassThrough) base.OnRenderChildren(canvas);
    }
}
