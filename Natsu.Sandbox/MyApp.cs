using System.Diagnostics;

using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Platforms.Skia;

using SkiaSharp;

namespace Natsu.Sandbox;

public class ButtonRect : InputElement {

    public Color BaseColor = Colors.Red;

    public bool Blocking;
    public Color HoverColor = Colors.Green;
    public RectElement Rect;
    public TextElement Text;

    public string Value = "Yeah!";

    public ButtonRect(IFont font) {
        RawChildren = [
            Rect = new RectElement {
                RelativeSizeAxes = Axes.Both,
                Paint = new Paint {
                    Color = BaseColor
                },
                RawChildren = [
                    Text = new TextElement("Yeah!", font) {
                        AnchorPosition = new Vector2(0.5f),
                        OffsetPosition = new Vector2(0.5f),
                        Paint = new Paint {
                            Color = Colors.White,
                            TextSize = 20,
                            IsAntialias = true
                        }
                    }
                ]
            }
        ];
    }

    public override bool OnMouseEnter(Vector2 position) {
        Rect.Paint.Color = HoverColor;
        Console.WriteLine("Mouse Enter " + Name);
        return Blocking;
    }

    public override void OnMouseLeave(Vector2 position) {
        Rect.Paint.Color = BaseColor;
        Console.WriteLine("Mouse Leave " + Name);
    }

    public override bool OnMouseDown(MouseButton button, Vector2 position) {
        Scale = new Vector2(0.9f);
        Console.WriteLine("Mouse Down " + Name);
        return Blocking;
    }

    public override void OnMouseUp(MouseButton button, Vector2 position) {
        Scale = new Vector2(1f);
        Console.WriteLine("Mouse Up " + Name);
    }

    public override void OnFocus() => Text.Paint.Color = Colors.Black;
    public override void OnBlur() => Text.Paint.Color = Colors.White;

    public string KeyString(Key key) => key switch {
        Key.A => "a",
        Key.B => "b",
        Key.C => "c",
        Key.D => "d",
        Key.E => "e",
        Key.F => "f",
        Key.G => "g",
        Key.H => "h",
        Key.I => "i",
        Key.J => "j",
        Key.K => "k",
        Key.L => "l",
        Key.M => "m",
        Key.N => "n",
        Key.O => "o",
        Key.P => "p",
        Key.Q => "q",
        Key.R => "r",
        Key.S => "s",
        Key.T => "t",
        Key.U => "u",
        Key.V => "v",
        Key.W => "w",
        Key.X => "x",
        Key.Y => "y",
        Key.Z => "z",
        Key.D0 => "0",
        Key.D1 => "1",
        Key.D2 => "2",
        Key.D3 => "3",
        Key.D4 => "4",
        Key.D5 => "5",
        Key.D6 => "6",
        Key.D7 => "7",
        Key.D8 => "8",
        Key.D9 => "9",
        Key.Space => " ",
        Key.Enter => "\n",
        _ => ""
    };

    public override bool OnKeyDown(Key key) {
        if (key == Key.Backspace) {
            if (Value.Length > 0)
                Value = Value.Substring(0, Value.Length - 1);
        } else
            Value += KeyString(key);

        Text.Text = Value;
        return Blocking;
    }

    public override bool OnKeyUp(Key key) {
        Value = Value.Substring(0, Value.Length - 1);
        Text.Text = Value;
        return Blocking;
    }
}

public class MyApp : Application {
    private readonly TextElement bouncyText;
    private readonly TextElement fpsText;

    private readonly Element testElm;

    public readonly Stopwatch Time = Stopwatch.StartNew();
    public float BaseScale = 1f;

    public CachedElement Cache;

    public float time;

    public MyApp(SKSurface baseSurface) {
        LoadRenderer(baseSurface);
        ResourceLoader = new ResourceLoader(new SkiaResourceLoader());

        // Cache = new CachedElement {
        //     Size = new Vector2(1000),
        //     Parent = Root,
        //     AnchorPosition = new Vector2(0.5f),
        //     OffsetPosition = new Vector2(0.5f),
        //     Scale = new Vector2(BaseScale)
        // };

        // Root.OnSizeChange += v => {
        //     // set the cache scale to whatever the max size of root is so that it always covers
        //     float scale = Math.Max(v.X, v.Y) / 100f;
        //     Cache.Scale = new Vector2(scale * 1.5f);
        // };

        // for (int i = 0; i < 10000; i++) {
        //     float r = (float)(Math.Sin(i) * 0.2f + 0.2f);
        //     float g = (float)(Math.Cos(i) * 0.2f + 0.2f);
        //     float b = (float)(Math.Sin(i) * 0.2f + 0.2f);

        //     float a = (i % 25f + i / 25f) / 200f;

        //     Cache.Add(new RectElement {
        //         Size = new Vector2(10, 10),
        //         Paint = new Paint {
        //             // alpha makes cirlce patterns (cos and sin)
        //             Color = new Color(r, g, b, 0.9f)
        //             // Color = Colors.Red
        //         },
        //         // AnchorPosition = new(0.5f),
        //         // OffsetPosition = new(0.5f),
        //         Position = new Vector2(i % 100 * 10, i / 100 * 10),
        //         Name = $"Rect {i}"
        //     });
        // }

        // Root.OffsetPosition = new Vector2(1f);
        // Root.AnchorPosition = new Vector2(1f);

        // IImage image = ResourceLoader.LoadResourceImage("Resources/testimage.png");

        // ImageElement testImage = new(image) {
        //     Parent = Root,
        //     Size = new Vector2(200),
        //     Index = 5,
        //     AnchorPosition = new Vector2(0.5f),
        //     OffsetPosition = new Vector2(0.5f),
        //     Name = "Test Image",
        //     Clip = true
        // };
        // testImage.Paint.FilterQuality = FilterQuality.High;
        // testImage.Paint.IsAntialias = true;
        // ImageElement bg = new(image) {
        //     Parent = Root,
        //     RelativeSizeAxes = Axes.Both,
        //     Index = -99999,
        //     AnchorPosition = new Vector2(0.5f),
        //     OffsetPosition = new Vector2(0.5f),
        //     Name = "BG Image",
        // };

        // RectElement testImage2 = new() {
        //     Parent = testImage,
        //     Size = new Vector2(50),
        //     Paint = new Paint { Color = Colors.Cyan },
        //     Index = 5,
        //     AnchorPosition = new Vector2(0.5f),
        //     OffsetPosition = new Vector2(0.5f),
        //     Rotation = 30,
        //     Name = "Test Image Sub",
        // };
        // testElm = testImage;

        fpsText = new TextElement("FPS: 0", ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf")) {
            Parent = Root,
            Paint = new Paint {
                TextSize = 30,
                IsAntialias = true,
                FilterQuality = FilterQuality.Medium
            }
            // RawChildren = [
            //     new RectElement() {
            //         RelativeSizeAxes = Axes.Both,
            //         Paint = new() {
            //             Color = Colors.Red,
            //             StrokeWidth = 3,
            //             IsStroke = true
            //         }
            //     }
            // ]
        };

        // Add(new TextElement("Testing 123", ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf")) {
        //     AnchorPosition = new Vector2(0.5f),
        //     OffsetPosition = new Vector2(0.5f),
        //     Parent = Root,
        //     RawChildren = [
        //         new RectElement {
        //             RelativeSizeAxes = Axes.Both,
        //             Paint = new Paint {
        //                 Color = Colors.Red,
        //                 StrokeWidth = 3,
        //                 IsStroke = true
        //             }
        //         }
        //     ],
        //     Index = 9999999,
        //     Paint = new Paint { Color = Colors.Cyan, TextSize = 60 }
        // });

        // bouncyText = new TextElement("Hello 1234567890-!@#!@#", ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf")) {
        //     Parent = Root,
        //     // Position = new(0, 50),
        //     AnchorPosition = new Vector2(1),
        //     OffsetPosition = new Vector2(1),
        //     Paint = new Paint { Color = new Color(255, 255, 255, 50) }
        // };

        IFont font = ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf");

        ButtonRect rect = new(font) {
            Size = new Vector2(300),
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            Position = new Vector2(-150, 0),
            Parent = Root,
            Index = 9999998,
            Name = "Left"
        };

        ButtonRect rect2 = new(font) {
            Size = new Vector2(300),
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            Position = new Vector2(150, 0),
            Parent = Root,
            Blocking = false,
            Index = 9999999,
            Name = "Right"
        };
    } // FPS: 260

    public void LoadRenderer(SKSurface surface) {
        lock (this) Renderer = new SkiaRenderer(surface);
    }

    protected override void OnRender() {
        base.OnRender();
        float fps = 1f / (float)Time.Elapsed.TotalSeconds;
        // Cache.Rotation += 50f * (float)Time.Elapsed.TotalSeconds;
        time += (float)Time.Elapsed.TotalSeconds;

        // Cache.ForChildren(c => {
        //     c.Rotation += 50f * (float)Time.Elapsed.TotalSeconds;
        // });

        Time.Restart();

        fpsText.Text = $"FPS: {fps}";
        // bouncyText.Scale = new Vector2(5 + MathF.Sin(time) * 2f);

        float offset = 5f + MathF.Sin(time) * 1.5f;
        // Cache.Scale = new Vector2(offset);
    }

    // from the front to the back (reverse render order)


    public List<Element> GetElementsAt(Vector2 position, Element root, ref List<Element> elements) {
        List<Element> xy = new();
        foreach (Element x in PositionalInputList)
            if (x.PointInside(position)) {
                elements.Add(x);
                break;
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
