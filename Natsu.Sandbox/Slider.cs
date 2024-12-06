using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class Slider : Element {
    public RectElement Background, Bar, Thumb;
    public Element ThumbContainer;
    public InputElement ThumbInput;

    public Slider() {
        Children = [
            Background = new RectElement {
                ContentParent = this,
                RelativeSizeAxes = Axes.Both,
                Paint = new Paint {
                    Color = new Color(100, 110, 130),
                    IsAntialias = true,
                    FilterQuality = FilterQuality.Medium
                },
                RoundedCorners = new Vector2(10),
                Content = [
                    Bar = new RectElement {
                        ContentParent = this,
                        RelativeSizeAxes = Axes.Both,
                        Paint = new Paint {
                            Color = new Color(170, 190, 200),
                            IsAntialias = true,
                            FilterQuality = FilterQuality.Medium
                        },
                        RoundedCorners = new Vector2(7.5f),
                        Margin = new Vector2(5),
                        Content = [
                            ThumbContainer = new Element {
                                Margin = new Vector2(5, 0),
                                RelativeSizeAxes = Axes.Both,
                                Content = [
                                    Thumb = new RectElement {
                                        ContentParent = this,
                                        Size = new Vector2(30),
                                        RelativeSizeAxes = Axes.Y,
                                        Margin = new Vector2(0, 5),
                                        Paint = new Paint {
                                            Color = new Color(200, 230, 255),
                                            IsAntialias = true,
                                            FilterQuality = FilterQuality.Medium
                                        },
                                        RoundedCorners = new Vector2(3),
                                        AnchorPosition = new Vector2(1f, 0.5f),
                                        OffsetPosition = new Vector2(1f, 0.5f)
                                    }
                                ]
                            }
                        ]
                    }
                ],
                AnchorPosition = new Vector2(0.5f),
                OffsetPosition = new Vector2(0.5f)
            },
            ThumbInput = new InputElement {
                ContentParent = this,
                RelativeSizeAxes = Axes.Both,
                GrabFallback = true,
                Cursor = CursorStyle.ResizeHorizontal
            }
        ];

        bool pressed = false;
        float value = 0;

        void updatePos(Vector2 position) {
            float coord = ThumbInput.ToLocalSpace(position).X;
            coord = Math.Clamp(coord, 25, DrawSize.X - Thumb.DrawSize.X + 5);
            Thumb.StopTransformSequences("Anchor", "Offset");

            value = (coord - 25) / (DrawSize.X - Thumb.DrawSize.X - 20);
            value = Math.Clamp(value, 0, 1);

            Thumb.AnchorTo(new Vector2(value, 0.5f), 0.3f, Ease.ExponentialOut);
            Thumb.OffsetTo(new Vector2(value, 0.5f), 0.3f, Ease.ExponentialOut);

            updateRoot();
        }

        void updateRoot(bool force = false) {
            if (force) {
                App.Root.StopTransformSequences(nameof(App.Root.Scale));
                App.Root.Scale = new Vector2(1 + value * 5);
                return;
            }

            App.Root.StopTransformSequences(nameof(App.Root.Scale));
            App.Root.ScaleTo(new Vector2(1 + value * 5), 0.5f, Ease.ExponentialOut);
        }

        DoAppChange += old => {
            App.DoResize += (w, h) => {
                updateRoot(true);
            };
        };

        void press(Vector2 position) {
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
        }

        void release(Vector2 position) {
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
        }

        ThumbInput.DoMouseDown += (button, position) => {
            if (button != MouseButton.Left) return;

            press(position);
        };

        ThumbInput.DoMouseUp += (button, position) => {
            if (button != MouseButton.Left) return;

            release(position);
        };

        ThumbInput.DoTouchDown += (id, position) => {
            if (id != 0) return;

            press(position);
        };

        ThumbInput.DoTouchUp += (id, position) => {
            if (id != 0) return;

            release(position);
        };

        ThumbInput.DoMouseMove += position => {
            if (!pressed) return;

            updatePos(position);
        };

        ThumbInput.DoTouchMove += (id, position) => {
            if (id != 0 || !pressed) return;

            updatePos(position);
        };
    }
}
