using System.Diagnostics;

using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Platforms.Skia;

using SkiaSharp;

namespace Natsu.Sandbox;

public class ButtonRect : InputElement {
    public RectElement Background, Border, Flash;

    public ButtonRect() {
        RawChildren = [
            Background = new RectElement {
                Parent = this,
                RelativeSizeAxes = Axes.Both,
                Paint = new Paint {
                    Color = new Color(130, 140, 150, 255),
                    IsAntialias = true,
                    FilterQuality = FilterQuality.Medium
                },
                RoundedCorners = new(10),
                RawChildren = [
                    Flash = new RectElement {
                        Parent = this,
                        RelativeSizeAxes = Axes.Both,
                        Paint = new Paint {
                            Color = new Color(255, 255, 255, 0),
                            IsAntialias = true,
                            FilterQuality = FilterQuality.Medium
                        },
                        RoundedCorners = new(10),
                    },
                    Border = new RectElement {
                        Parent = this,
                        RelativeSizeAxes = Axes.Both,
                        Paint = new Paint {
                            Color = new Color(150, 180, 200, 255),
                            IsAntialias = true,
                            FilterQuality = FilterQuality.Medium,
                            StrokeWidth = 3,
                            IsStroke = true
                        },
                        RoundedCorners = new(10 - 2.5f),
                        Margin = new(2.5f)
                    }
                ],
                AnchorPosition = new(0.5f),
                OffsetPosition = new(0.5f),
                Clip = true
            },
        ];
    }

    public override bool OnMouseDown(MouseButton button, Vector2 position) {
        if (button != MouseButton.Left) return false;
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(new Vector2(0.9f), 2f, Ease.ExponentialOut);
        return true;
    }

    public override void OnMouseUp(MouseButton button, Vector2 position) {
        if (button != MouseButton.Left) return;
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(new Vector2(1), 0.6f, Ease.ElasticOut);
    }

    public override void OnMousePress(MouseButton button, Vector2 position) {
        if (button != MouseButton.Left) return;
        Flash.StopTransformSequences();
        Flash.ColorTo(Colors.White).Then().ColorTo(Colors.WhiteTransparent, 0.5f);
    }

    public override bool OnMouseEnter(Vector2 position) => true;
    public override void OnMouseLeave(Vector2 position) { }

    public override void OnRenderChildren(ICanvas canvas) {
        base.OnRenderChildren(canvas);

        if (IsFocused) {
            canvas.DrawLine(new Vector2(0, 0), new Vector2(DrawSize.X, DrawSize.Y), new Paint { Color = Colors.Red, StrokeWidth = 2 });
            canvas.DrawLine(new Vector2(0, DrawSize.Y), new Vector2(DrawSize.X, 0), new Paint { Color = Colors.Red, StrokeWidth = 2 });
        }
    }
}

public class Slider : Element {
    public RectElement Background, Bar, Thumb;
    public Element ThumbContainer;
    public InputElement ThumbInput;

    public Slider() {
        RawChildren = [
            Background = new RectElement {
                Parent = this,
                RelativeSizeAxes = Axes.Both,
                Paint = new Paint {
                    Color = new Color(100, 110, 130),
                    IsAntialias = true,
                    FilterQuality = FilterQuality.Medium
                },
                RoundedCorners = new(10),
                RawChildren = [
                    Bar = new RectElement {
                        Parent = this,
                        RelativeSizeAxes = Axes.Both,
                        Paint = new Paint {
                            Color = new Color(170, 190, 200),
                            IsAntialias = true,
                            FilterQuality = FilterQuality.Medium
                        },
                        RoundedCorners = new(7.5f),
                        Margin = new(5),
                        RawChildren = [
                            ThumbContainer = new Element() {
                                Margin = new(5, 0),
                                RelativeSizeAxes = Axes.Both,
                                RawChildren = [
                                    Thumb = new RectElement {
                                        Parent = this,
                                        Size = new(30, 30),
                                        Paint = new Paint {
                                            Color = new Color(200, 230, 255),
                                            IsAntialias = true,
                                            FilterQuality = FilterQuality.Medium
                                        },
                                        RoundedCorners = new(3),
                                        AnchorPosition = new(1f, 0.5f),
                                        OffsetPosition = new(1f, 0.5f),
                                        // Position = new(25, 0)
                                    }
                                ]
                            }
                        ]
                    },
                ],
                AnchorPosition = new(0.5f),
                OffsetPosition = new(0.5f)
            },
            ThumbInput = new InputElement {
                Parent = this,
                RelativeSizeAxes = Axes.Both,
                GrabFallback = true
            }
        ];

        bool pressed = false;
        float value = 0;

        void updatePos(Vector2 position) {
            float coord = ThumbInput.ToLocalSpace(position).X;
            coord = Math.Clamp(coord, 25, DrawSize.X - Thumb.DrawSize.X + 5);
            Thumb.StopTransformSequences("Anchor", "Offset");
            // Thumb.MoveTo(new Vector2(coord, 0), 0.2f, Ease.ExponentialOut);

            value = (coord - 25) / (DrawSize.X - Thumb.DrawSize.X - 20);
            value = Math.Clamp(value, 0, 1);

            // Thumb.AnchorPosition = new Vector2(value, 0.5f);
            // Thumb.OffsetPosition = new Vector2(value, 0.5f);
            Thumb.AnchorTo(new Vector2(value, 0.5f), 0.2f, Ease.ExponentialOut);
            Thumb.OffsetTo(new Vector2(value, 0.5f), 0.2f, Ease.ExponentialOut);

            updateRoot();
        }

        void updateRoot(bool force = false) {
            if (force) {
                App.Root.StopTransformSequences(nameof(App.Root.Scale));
                App.Root.Scale = new Vector2(1 + value);
                return;
            }

            App.Root.StopTransformSequences(nameof(App.Root.Scale));
            App.Root.ScaleTo(new Vector2(1 + value), 0.5f, Ease.ExponentialOut);
        }

        AppChanged += (old) => {
            App.Resized += (w, h) => {
                updateRoot(true);
            };
        };

        ThumbInput.MouseDownEvent += (button, position) => {
            if (button != MouseButton.Left) return;
            pressed = true;
            updatePos(position);

            Thumb.StopTransformSequences(nameof(Thumb.Scale), nameof(Thumb.Paint.Color));
            Thumb.ScaleTo(new Vector2(0.9f), 0.2f, Ease.ExponentialOut);
            Thumb.ColorTo(Colors.White, 0.2f, Ease.ExponentialOut);

            Background.StopTransformSequences();
            Background.ScaleTo(new Vector2(1.02f), 0.2f, Ease.ExponentialOut);
            Background.ColorTo(Colors.White, 0.2f, Ease.ExponentialOut);

            Bar.StopTransformSequences();
            Bar.MarginTo(new Vector2(3), 0.2f, Ease.ExponentialOut);

            ThumbContainer.StopTransformSequences();
            ThumbContainer.MarginTo(new Vector2(10, 0), 0.2f, Ease.ExponentialOut);
        };

        ThumbInput.MouseUpEvent += (button, position) => {
            if (button != MouseButton.Left) return;
            pressed = false;

            Thumb.StopTransformSequences(nameof(Thumb.Scale), nameof(Thumb.Paint.Color));
            Thumb.ScaleTo(new Vector2(1), 0.2f, Ease.ExponentialOut);
            Thumb.ColorTo(new Color(200, 230, 255), 0.2f, Ease.ExponentialOut);

            Background.StopTransformSequences();
            Background.ScaleTo(new Vector2(1), 0.2f, Ease.ExponentialOut);
            Background.ColorTo(new Color(100, 110, 130), 0.2f, Ease.ExponentialOut);

            Bar.StopTransformSequences();
            Bar.MarginTo(new Vector2(5), 0.2f, Ease.ExponentialOut);

            ThumbContainer.StopTransformSequences();
            ThumbContainer.MarginTo(new Vector2(5, 0), 0.2f, Ease.ExponentialOut);
        };

        ThumbInput.MouseMoveEvent += (position) => {
            if (!pressed) return;
            updatePos(position);
        };
    }
}

public class MyApp : Application {
    private readonly TextElement fpsText;

    public MyApp(SKSurface baseSurface) {
        LoadRenderer(baseSurface);
        ResourceLoader = new ResourceLoader(new SkiaResourceLoader());

        IFont font = ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf");

        fpsText = new TextElement("FPS: 0", font) {
            Parent = Root,
            Paint = new Paint {
                TextSize = 30,
                IsAntialias = true,
                FilterQuality = FilterQuality.Medium
            }
        };

        ButtonRect rect = new() {
            Size = new Vector2(300, 100),
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            Position = new Vector2(-150, 0),
            Parent = Root,
            Index = 2,
            Name = "Left"
        };

        ButtonRect rect2 = new() {
            Size = new Vector2(300, 100),
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            Position = new Vector2(150, 0),
            Parent = Root,
            Index = 1,
            Name = "Right"
        };

        ImageElement test = new(ResourceLoader.LoadResourceImage("Resources/testimage.png")) {
            RelativeSizeAxes = Axes.Both,
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            Parent = rect.Flash
        };

        rect.MousePressEvent += (button, position) => {
            if (button != MouseButton.Left) return;

            rect.StopTransformSequences(nameof(rect.Position));
            rect.Position = new Vector2(-150, 0);
            rect.MoveTo(new(-150, 50), 0.5f, Ease.CubicOut).Then().MoveTo(new(-150, 0), 0.5f, Ease.CubicIn).Then().Loop(0, 1).Then()
                .SetLoopPoint(1).MoveTo(new(-150, -50), 0.5f, Ease.CubicOut).Then().MoveTo(new(-150, 0), 0.5f, Ease.CubicIn).Then().Loop(0, int.MaxValue, 0);
        };

        rect2.MousePressEvent += (button, position) => {
            if (button != MouseButton.Left) return;
            rect2.StopTransformSequences(nameof(rect2.Rotation));
            float rot = MathF.Round(rect2.Rotation / 90) * 90 + 90;
            rect2.RotateTo(rot, 1f, Ease.ElasticOut);
        };

        ButtonRect scaleButton = new() {
            Size = new(100, 50),
            AnchorPosition = new(1, 0),
            OffsetPosition = new(1, 0),
            Parent = Root,
            Index = 555
        };

        scaleButton.Background.Add(new TextElement("Scale", font) {
            AnchorPosition = new(0.5f),
            OffsetPosition = new(0.5f),
            Paint = new Paint {
                TextSize = 20
            }
        });

        bool t = false;
        scaleButton.MousePressEvent += (button, position) => {
            if (button != MouseButton.Left) return;

            Root.StopTransformSequences(nameof(Root.Scale));
            Root.ScaleTo(new Vector2(t ? 1 : 1.5f), 0.5f, Ease.ElasticOut);

            t = !t;
        };

        Slider slider = new() {
            RelativeSizeAxes = Axes.X,
            Margin = new(30, 30),
            Size = new(0, 110),
            AnchorPosition = new(0.5f, 1),
            OffsetPosition = new(0.5f, 0.5f),
            Parent = Root,
            Index = 3,
            Position = new(0, -55)
        };

        GlobalInputElement globalElm = new() {
            GrabFallback = false
        };
        Add(globalElm);

        bool ctrl = false;
        bool toggle = false;

        globalElm.KeyDownEvent += (k) => {
            if (k == Key.LeftControl || k == Key.RightControl) {
                ctrl = true;
                return;
            }

            if (ctrl && k == Key.D) {
                if (toggle) {
                    slider.MoveTo(new Vector2(0, -55), 0.3f, Ease.ExponentialOut);
                } else {
                    slider.MoveTo(new Vector2(0, 30), 0.3f, Ease.ExponentialOut);
                }

                toggle = !toggle;
            }
        };

        globalElm.KeyUpEvent += (k) => {
            if (k == Key.LeftControl || k == Key.RightControl) {
                ctrl = false;
            }
        };
    }

    public void LoadRenderer(SKSurface surface) {
        lock (this) Renderer = new SkiaRenderer(surface);
    }

    protected override void OnRender() {
        base.OnRender();
        float fps = 1f / (float)RenderTime.DeltaTime;
        fpsText.Text = $"FPS: {fps}";
    }
}
