using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Graphics;

public class CachedElement : Element {
    public IOffscreenSurface? Surface { get; private set; }
    public bool Valid = false;

    public CachedElement() {
        OnSizeChange += (v) => {
            Valid = false;
        };
    }

    public void Rerender() {
        Valid = true;
        IOffscreenSurface nSurface = App.Renderer.CreateOffscreenSurface((int)DrawSize.X, (int)DrawSize.Y);
        nSurface.Canvas.Clear(Colors.Transparent);
        ForChildren(e => e.Render(nSurface.Canvas));
        nSurface.Flush();
        lock (Surface ?? new object()) {
            Surface?.Dispose();
            Surface = nSurface;
        }
    }

    public override void OnRender(ICanvas canvas) {
        if (Surface == null || !Valid) {
            Rerender();
        }

        if (Surface != null) {
            canvas.DrawOffscreenSurface(Surface, new(0, 0));
        }
    }

    public override void OnRenderChildren(ICanvas canvas) { }

    public override Matrix ChildAccessMatrix => new();
}
