using System.Diagnostics;
using Natsu.Audio;
using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;

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
        Background.ColorTo(Colors.White).Then().ColorTo(new(30, 40, 50), 0.5f, Easing.QuadOut);
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

        byte[]? pluck = ResourceLoader.LoadResource("Resources/pluck.mp3");

        Button.DoPress += (_, _) => {
            IAudioStream? stream = AudioManager.CreateStream(pluck, true);

            stream.FrequencyTo(1, stream.Length / 4).Then(0.1f).VolumeTo(0, 1f).Then(0.1f).Append(new Transform(_ => { stream.Stop(); }) {
                Name = "stop"
            });
        };
    }
}
