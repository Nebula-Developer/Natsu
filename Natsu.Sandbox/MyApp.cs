using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;

public class MyApp : Application {
    private BoxElement? _box;
    private Vector2 _targetScale = Vector2.One;

    protected override void OnLoad() {
        new BoxElement {
            RelativeSizeAxes = Axes.Both,
            Color = Colors.Gray,
            Parent = this
        };

        BoxElement box = new() {
            Size = (170, 170),
            Color = Colors.Black,
            ImageFilter = Renderer.CreateBlur(10, 10),
            IsStroke = true,
            StrokeWidth = 10
        };

        BoxElement box2 = new() {
            Size = (170, 170),
            Color = Colors.White,
            Pivot = new(1),
            ImageFilter = Renderer.CreateBlur(10, 10),
            IsStroke = true,
            StrokeWidth = 10
        };

        box.DoRender += () => { };

        TextElement text = new("FPS: 0") {
            TextSize = 32,
            Pivot = 1f,
            Position = -20f,
            Parent = this
        };

        text.DoUpdate += time => { text.Text = $"FPS: {Time.TPS:F2}"; };

        BoxElement boxP = new() {
            Size = new(100),
            Pivot = new(0.5f),
            Clip = true,
            ClipAntiAlias = true,
            RoundedCorners = 10f,
            Color = Colors.Red
        };

        BoxElement boxShadow = new() {
            Size = new(100),
            Pivot = new(0.5f),
            RoundedCorners = 10f,
            ImageFilter = Renderer.CreateDropShadow(0, 30, 30, 30, Colors.Black),
            Parent = this
        };

        box.Parent = boxP;
        box2.Parent = boxP;
        boxP.Parent = boxShadow;

        BlurredBackgroundElement blur = new() {
            Children = [
                _box = new() {
                    Size = 30f,
                    OffsetPosition = new(0.5f),
                    RoundedCorners = 30f
                }
            ],
            Parent = this
        };
    }

    protected override void OnMouseMove(Vector2 position) {
        if (_box == null) return;
        _box.StopTransformSequence(nameof(_box.Position));
        _box.MoveTo(position / _box.WorldScale, 0.3f, Easing.ExpoOut);
    }

    protected override void OnKeyDown(Key key, KeyMods mods) {
        if (key == Key.Up)
            _targetScale *= 2f;
        else if (key == Key.Down) _targetScale /= 2f;

        Root.StopTransformSequence(nameof(Root.Scale));
        Root.ScaleTo(_targetScale, 0.5f, Easing.ExpoOut);
    }
}

public class BlurredBackgroundElement : Element {
    protected override void OnRenderChildren(ICanvas canvas) {
        int restore = canvas.CreateBackdropFilter(Renderer.CreateBlur(10, 10));
        base.OnRenderChildren(canvas);
        canvas.Restore(restore);
    }
}
