using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using Natsu.Graphics;
using Natsu.Mathematics;

using SkiaSharp;

namespace Natsu.Platforms.Skia;

public class SkiaApplication : Application {
    public void LoadRenderer(SKSurface surface) {
        lock (this)
            Renderer = new SkiaRenderer(surface);
    }

    public CachedElement Cache;
    public float BaseScale = 1f;

    public SkiaApplication(SKSurface baseSurface) {
        LoadRenderer(baseSurface);
        ResourceLoader = new ResourceLoader(new SkiaResourceLoader());
        
        Cache = new() {
            Size = new(1000),
            Parent = Root,
            AnchorPosition = new(0.5f),
            OffsetPosition = new(0.5f),
            Scale = new(BaseScale)
        };

        Root.OnSizeChange += (v) => {
            // set the cache scale to whatever the max size of root is so that it always covers
            float scale = Math.Max(v.X, v.Y) / 1000f;
            BaseScale = scale * 1.5f;
        };

        for (int i = 0; i < 10000; i++) {
            float r = (float)(i % 100) / 100f;
            float g = (float)(i / 100) / 100f;
            float b = (float)(i % 100 + i / 100) / 200f;

            Cache.Add(new RectElement() {
                Size = new(10, 10),
                Paint = new() {
                    Color = new(r, g, b, 0.7f)
                },
                Position = new(i % 100 * 10, i / 100 * 10)
            });
        }

        Root.Scale = new(0.25f);
        Root.OffsetPosition = new(1f);
        Root.AnchorPosition = new(1f);

        foreach (string s in Assembly.GetCallingAssembly().GetManifestResourceNames()) {
            Console.WriteLine(s);
        }

        IImage image = ResourceLoader.LoadResourceImage("Resources/testimage.png");


        ImageElement testImage = new(image) {
            Parent = Root,
            Size = new(200),
            Index = 5,
            AnchorPosition = new(1),
            OffsetPosition = new(1),
            Scale = new(10)
        };
        RectElement testImage2 = new() {
            Parent = testImage,
            Size = new(50),
            Paint = new() {
                Color = Colors.Cyan
            },
            Index = 5,
            AnchorPosition = new(1f),
            OffsetPosition = new(1f),
            Rotation = 30
        };
        testElm = testImage;
    } // FPS: 260

    Element testElm;

    protected override void OnRender() {
        base.OnRender();
        float fps = 1f / (float)Time.Elapsed.TotalSeconds;
        Cache.Rotation += 50f * (float)Time.Elapsed.TotalSeconds;
        time += (float)Time.Elapsed.TotalSeconds;
        // testElm.Rotation += 50f * (float)Time.Elapsed.TotalSeconds;
        Time.Restart();

        Renderer.Canvas.DrawText($"FPS: {fps}", new Vector2(10, 10), ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf"), new Paint() {
            Color = Colors.White,
            FontSize = 20
        });

        Renderer.Canvas.DrawText($"Hello 1234567890-!@#!@#: {fps}", new Vector2(10, 30), ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf"), new Paint() {
            Color = Colors.White,
            FontSize = MathF.Sin(time) * 10 + 30
        });

        float offset = 30f + MathF.Sin(time) * 30f;
        Cache.Scale = new(BaseScale + offset);
    }

    public float time;

    public readonly Stopwatch Time = Stopwatch.StartNew();
}