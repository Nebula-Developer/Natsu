using System.Diagnostics;
using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;

namespace Natsu.Sandbox;

public class BouncyButton : InputElement {
    public BoxElement Background = new() {
        Color = Colors.Purple,
        RoundedCorners = new(10),
        Pivot = new(0.5f),
        RelativeSizeAxes = Axes.Both,
        IsAntialias = true
    };

    protected override void OnLoad() => Add(Background);

    protected override void OnPressDown(Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(0.6f, 2f, EaseType.ExpoOut);
    }

    protected override void OnPressUp(Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(1f, 0.8f, EaseType.ElasticOut);
    }

    protected override void OnPress(Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Color));
        Background.ColorTo(Colors.White).Then().ColorTo(Colors.Purple, 0.8f);
    }
}

public class MyApp : Application {
    private readonly Stopwatch _stopwatch = new();

    private ITransformSequence _loop = null!;

    public BouncyButton Button = new() {
        Size = new(200, 100),
        Pivot = new(0.5f)
    };

    protected override void OnLoad() {
        Add(Button);

        // loop = Button.MoveTo(new(0, 100), 0.5f).Then().MoveTo(new(0, 0), 0.5f).Loop(1).SetLoopPoint(1).RotateTo(360, 1f).Loop(-1, 1);
        _loop = new TransformSequence<BouncyButton>(Button).Then(5f).SetLoopPoint(3).MoveTo(new(0, 100), 5f, EaseType.ExpoOut).Loop(3, 3).Then(10f).Loop();

        Button.AddTransformSequence(_loop);

        _stopwatch.Start();
    }

    protected override void OnKeyDown(Key key, KeyMods mods) {
        _loop.Reset();
        _stopwatch.Restart();
    }

    protected override void OnRender() {
        Canvas.DrawText(_loop.Time.ToString("0.00"), new(10, 10), ResourceLoader.DefaultFont, new() {
            TextSize = 40
        });

        Canvas.DrawText(_stopwatch.Elapsed.TotalSeconds.ToString("0.00"), new(10, 50), ResourceLoader.DefaultFont, new() {
            TextSize = 40
        });
    }
}
