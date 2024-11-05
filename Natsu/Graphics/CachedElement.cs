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
        lock (Surface ?? new object()) {
            IOffscreenSurface nSurface = App.Renderer.CreateOffscreenSurface((int)Size.X, (int)Size.Y);
            nSurface.Canvas.Clear(Colors.Transparent);
            ForChildren(e => e.Render(nSurface.Canvas));
            nSurface.Flush();
            Surface?.Dispose();
            Surface = nSurface;
            Valid = true;
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
