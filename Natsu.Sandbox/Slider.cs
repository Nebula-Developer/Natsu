using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class Slider : Element {
    public RectElement Background, Bar, Thumb;

    public Element ThumbContainer;
    public InputElement ThumbInput;

    public Slider() {
        Children = [
            Background = new() {
                ContentParent = this,
                RelativeSizeAxes = Axes.Both,
                Color = new(100, 110, 130),
                IsAntialias = true,
                FilterQuality = FilterQuality.Medium,
                RoundedCorners = new(10),
                Content = [
                    Bar = new() {
                        ContentParent = this,
                        RelativeSizeAxes = Axes.Both,
                        Color = new(170, 190, 200),
                        IsAntialias = true,
                        FilterQuality = FilterQuality.Medium,
                        RoundedCorners = new(7.5f),
                        Margin = new(5),
                        Content = [
                            ThumbContainer = new() {
                                Margin = new(5, 0),
                                RelativeSizeAxes = Axes.Both,
                                Content = [
                                    Thumb = new() {
                                        ContentParent = this,
                                        Size = new(30),
                                        RelativeSizeAxes = Axes.Y,
                                        Margin = new(0, 5),
                                        Color = new(200, 230, 255),
                                        IsAntialias = true,
                                        FilterQuality = FilterQuality.Medium,
                                        RoundedCorners = new(3),
                                        AnchorPosition = new(1f, 0.5f),
                                        OffsetPosition = new(1f, 0.5f)
                                    }
                                ]
                            }
                        ]
                    }
                ],
                AnchorPosition = new(0.5f),
                OffsetPosition = new(0.5f)
            },
            ThumbInput = new() {
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

            Thumb.AnchorTo(new(value, 0.5f), 0.3f, Ease.ExponentialOut);
            Thumb.OffsetTo(new(value, 0.5f), 0.3f, Ease.ExponentialOut);

            updateRoot();
        }

        void updateRoot(bool force = false) {
            if (force) {
                App.Root.StopTransformSequences(nameof(App.Root.Scale));
                App.Root.Scale = new(1 + value * 5);
                return;
            }

            App.Root.StopTransformSequences(nameof(App.Root.Scale));
            App.Root.ScaleTo(new(1 + value * 5), 0.5f, Ease.ExponentialOut);
        }

        DoAppChange += old => { App.DoResize += _ => { updateRoot(true); }; };

        void press(Vector2 position) {
            pressed = true;
            updatePos(position);

            Thumb.StopTransformSequences(nameof(Thumb.Scale), nameof(Thumb.Paint.Color));
            Thumb.ScaleTo(new(0.9f), 0.2f, Ease.ExponentialOut);
            Thumb.ColorTo(Colors.White, 0.2f, Ease.ExponentialOut);

            Background.StopTransformSequences();
            Background.ScaleTo(new(1.02f), 0.2f, Ease.ExponentialOut);
            Background.ColorTo(Colors.White, 0.2f, Ease.ExponentialOut);

            Bar.StopTransformSequences();
            Bar.MarginTo(new(3), 0.2f, Ease.ExponentialOut);

            ThumbContainer.StopTransformSequences();
            ThumbContainer.MarginTo(new(10, 0), 0.2f, Ease.ExponentialOut);
        }

        void release(Vector2 position) {
            pressed = false;

            Thumb.StopTransformSequences(nameof(Thumb.Scale), nameof(Thumb.Paint.Color));
            Thumb.ScaleTo(new(1), 0.2f, Ease.ExponentialOut);
            Thumb.ColorTo(new(200, 230, 255), 0.2f, Ease.ExponentialOut);

            Background.StopTransformSequences();
            Background.ScaleTo(new(1), 0.2f, Ease.ExponentialOut);
            Background.ColorTo(new(100, 110, 130), 0.2f, Ease.ExponentialOut);

            Bar.StopTransformSequences();
            Bar.MarginTo(new(5), 0.2f, Ease.ExponentialOut);

            ThumbContainer.StopTransformSequences();
            ThumbContainer.MarginTo(new(5, 0), 0.2f, Ease.ExponentialOut);
        }

        ThumbInput.DoPress += position => press(position);
        ThumbInput.DoPressUp += position => release(position);

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
