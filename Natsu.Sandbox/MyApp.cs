using System.Diagnostics;
using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class BouncyButton : InputElement {
    public BoxElement Background = new() {
        Color = new(30, 40, 50),
        RoundedCorners = new(10),
        Pivot = new(0.5f),
        RelativeSizeAxes = Axes.Both,
        IsAntialias = true
        // Clip = true
    };

    protected override void OnLoad() => Add(Background);

    protected override void OnPressDown(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(0.6f, 2f, Easing.ExpoOut);
    }

    protected override void OnPressUp(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Scale));
        Background.ScaleTo(1f, 0.8f, Easing.ElasticOut);
    }

    protected override void OnPress(int index, Vector2 position) {
        Background.StopTransformSequences(nameof(Background.Color));
        Background.ColorTo(Colors.White).Then().ColorTo(new(30, 40, 50));
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

        Button.PivotTo(new(0)).PivotTo(new(1), 1, Easing.ExpoOut).Then().PivotTo(new(0), 1, Easing.ExpoOut).Loop();

        // _stopwatch.Start();

        // Button.Background.FadeOut(1).Then().FadeIn(1).Loop();

        // Button.DoPressMove += (index, vec) => { Logging.Debug($"PressMove: {index} {vec}"); };
        // Button.DoPress += (_, _) => {
        //     Button.UseLocalPositions = !Button.UseLocalPositions;
        //     Logging.Debug($"UseLocalPositions: {Button.UseLocalPositions}");
        // };
    }
}
