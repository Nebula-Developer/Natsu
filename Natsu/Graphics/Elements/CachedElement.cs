using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public class CachedElement : Element {
    public bool Valid;

    public CachedElement() {
        DoSizeChange += _ => Valid = false;
    }

    public IOffscreenSurface? Surface { get; private set; }

    public override Matrix ChildAccessMatrix => new();

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
        if (Surface == null || !Valid) Rerender();

        if (Surface != null) canvas.DrawOffscreenSurface(Surface, new Vector2(0, 0));
    }

    public override void OnRenderChildren(ICanvas canvas) { }
}