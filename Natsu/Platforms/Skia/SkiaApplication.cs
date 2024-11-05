using System.Diagnostics;

using Natsu.Graphics;
using Natsu.Mathematics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaApplication : Application {
    public void LoadRenderer(SKSurface surface) {
        lock (this)
            Renderer = new SkiaRenderer(surface);
    }

    public SkiaApplication(SKSurface baseSurface) {
        LoadRenderer(baseSurface);
        ResourceManager = new SkiaResourceManager();

        RectElement a = new() {
            RelativeSizeAxes = Axes.Both,
            Name = "A"
        };
        RectElement b = new() {
            OffsetPosition = new(0.5f),
            AnchorPosition = new(0.5f),
            Parent = a,
            RelativeSizeAxes = Axes.Both,
            Name = "B"
        };


        b.Paint.Color = new Color(255, 255, 255, 50);
        b.Paint.IsStroke = false;

        a.Paint.Color = Colors.Yellow;

        Add(a);
        
        c = new() {
            Parent = b,
            Size = new(25),
            // Rotation = 20,
            AnchorPosition = new(1),
            OffsetPosition = new(1),
            Name = "C"
        };

        c.Paint.Color = Colors.Red;
        c.Paint.IsStroke = true;
        c.Paint.StrokeWidth = 6;

        Resize(800, 600);
        Root.Scale = new(1f);
        Root.OffsetPosition = new(0.5f);
        Root.AnchorPosition = new(0.5f);
    }
    RectElement c;

    protected override void OnResize(int width, int height) {
        base.OnResize(width, height);
        Console.WriteLine(c.Parent!.Size);
    }

    protected override void OnRender() {
        base.OnRender();
        float time = (float)this.time.Elapsed.TotalSeconds;
        Root.Rotation = time * 100;
    }

    Stopwatch time = Stopwatch.StartNew();
}