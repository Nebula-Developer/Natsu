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
            float scale = Math.Max(v.X, v.Y) / 100f;
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
                    // Color = Colors.Red
                },
                // AnchorPosition = new(0.5f),
                // OffsetPosition = new(0.5f),
                Position = new(i % 100 * 10, i / 100 * 10),
                Name = $"Rect {i}",
                HandlePositionalInput = true
            });
        }

        Root.OffsetPosition = new(1f);
        Root.AnchorPosition = new(1f);

        IImage image = ResourceLoader.LoadResourceImage("Resources/testimage.png");


        ImageElement testImage = new(image) {
            Parent = Root,
            Size = new(200),
            Index = 5,
            AnchorPosition = new(0.5f),
            OffsetPosition = new(0.5f),
            Name = "Test Image",
            HandlePositionalInput = true
        };
        RectElement testImage2 = new() {
            Parent = testImage,
            Size = new(50),
            Paint = new() {
                Color = Colors.Cyan
            },
            Index = 5,
            AnchorPosition = new(0.5f),
            OffsetPosition = new(0.5f),
            Rotation = 30,
            Name = "Test Image Sub",
            HandlePositionalInput = true
        };
        testElm = testImage;

        fpsText = new("FPS: 0", ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf")) {
            Parent = Root,
        };

        bouncyText = new($"Hello 1234567890-!@#!@#", ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf")) {
            Parent = Root,
            // Position = new(0, 50),
            AnchorPosition = new(1),
            OffsetPosition = new(1),
            HandlePositionalInput = true,
            Paint = new() {
                Color = new Color(255, 255, 255, 50)
            }
        };
    } // FPS: 260

    Element testElm;
    TextElement fpsText, bouncyText;

    protected override void OnRender() {
        base.OnRender();
        float fps = 1f / (float)Time.Elapsed.TotalSeconds;
        Cache.Rotation += 50f * (float)Time.Elapsed.TotalSeconds;
        time += (float)Time.Elapsed.TotalSeconds;
        // testElm.Rotation += 50f * (float)Time.Elapsed.TotalSeconds;
        Time.Restart();

        // Renderer.Canvas.DrawText($"FPS: {fps}", new Vector2(10, 10), ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf"), new Paint() {
        //     Color = Colors.White,
        //     FontSize = 20
        // });
        fpsText.Text = $"FPS: {fps}";

        // bouncyText.Paint.TextSize = MathF.Ceiling((MathF.Sin(time) * 10 + 30) * 100) / 100;
        // 2 to 5
        bouncyText.Scale = new(5 + MathF.Sin(time) * 2f);

        float offset = 5f + MathF.Sin(time) * 1.5f;
        Cache.Scale = new(offset);
    }

    public float time;

    public readonly Stopwatch Time = Stopwatch.StartNew();

    // from the front to the back (reverse render order)
    

    public List<Element> GetElementsAt(Vector2 position, Element root, ref List<Element> elements) {
        var xy = new List<Element>();
        foreach (Element x in InputTree) {
            if (x.HandlePositionalInput && x.PointInside(position)) {
                elements.Add(x);
                break;
            }
        }

        return elements;
        // if (elements == null) elements = new();
        // if (root == null) return elements;

        // if (root.Children.Count > 0) {
        //     foreach (Element child in root.Children) {
        //         if (elements.Contains(child))
        //             continue;

        //         if (child.HandlePositionalInput && child.PointInside(position))
        //             elements.Add(child);
                
        //         GetElementsAt(position, child, ref elements);
        //     }
        // }

        // return elements;
    }
}