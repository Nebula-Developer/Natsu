using Natsu.Graphics;
using Natsu.Mathematics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaApplication : IApplication {
    public IRenderer Renderer { get; }
    public IResourceManager ResourceManager { get; }

    public void Dispose() {

    }

    public void Run() {
        // draw rect then save to img.png
        Renderer.Canvas.DrawRect(new Rect(0, 0, 100, 100), new Paint { Color = Colors.Red });
        Renderer.Present();



        SkiaRenderer renderer = (SkiaRenderer)Renderer;

        IFont font = ResourceManager.LoadFont("Arial");
        renderer.Canvas.DrawText("Hello, World!", new Vector2(0, 20), font, new Paint { Color = Colors.Black, FontSize = 30 });

        IOffscreenSurface offscreen = renderer.CreateOffscreenSurface(100, 100);
        offscreen.Canvas.DrawRect(new Rect(0, 0, 50, 100), new Paint { Color = Colors.Blue });

        renderer.Canvas.DrawOffscreenSurface(offscreen, new Vector2(0, 0));

        using (SKImage image = renderer.Surface.Snapshot()) {
            using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100)) {
                File.WriteAllBytes("img.png", data.ToArray());
            }
        }
    }

    private SKImageInfo _info;
    private readonly SKSurface _surface;

    public SkiaApplication() {
        _info = new SKImageInfo(800, 600);
        _surface = SKSurface.Create(_info);

        Renderer = new SkiaRenderer(_surface);
        ResourceManager = new SkiaResourceManager();
    }
}