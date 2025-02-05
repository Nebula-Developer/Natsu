using System.Diagnostics;
using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Utils.Logging;

namespace Natsu.Sandbox;

public class BouncyButton : InputElement {
    public BoxElement Background = new() {
        Color = Colors.Purple,
        RoundedCorners = new(10),
        Pivot = new(0.5f),
        RelativeSizeAxes = Axes.Both,
        IsAntialias = true
        // Clip = true
    };

    public BoxElement BottomRight = new() {
        Size = new(0.5f, 0.25f),
        RawRelativeSizeAxes = Axes.Both,
        Pivot = Vector2.One,
        RoundedCorners = new(30, 60),
        IsAntialias = true
    };

    protected override void OnLoad() {
        Add(Background);
        Background.Add(BottomRight);
    }

    protected override void OnPressDown(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(0.6f, 2f, EaseType.ExpoOut);
    }

    protected override void OnPressUp(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(1f, 0.8f, EaseType.ElasticOut);
    }

    protected override void OnPress(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Color));
        Background.ColorTo(Colors.White).Then().ColorTo(Colors.Purple, 0.8f);
    }
}

public class MyApp : Application {
    private readonly Stopwatch _stopwatch = new();

    public BouncyButton Button = new() {
        Size = new(400, 300),
        Pivot = new(0.5f)
    };

    protected override void OnLoad() {
        Add(Button);

        _stopwatch.Start();

        Button.Background.FadeOut(1).Then().FadeIn(1).Loop();

        Button.DoPressMove += (index, vec) => { Logging.Debug($"PressMove: {index} {vec}"); };
        Button.DoPress += (_, _) => {
            Button.UseLocalPositions = !Button.UseLocalPositions;
            Logging.Debug($"UseLocalPositions: {Button.UseLocalPositions}");
        };
    }
}
