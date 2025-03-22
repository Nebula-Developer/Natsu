using Natsu.Graphics;
using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Mathematics;
using Natsu.Extensions;

namespace NatsuApp;

public class MyApp : Application {
    public readonly BoxElement Box = new() {
        Color = Colors.Orange,
        Size = new(100),
        Pivot = new(0.5f), // Center the box to the screen.
        RoundedCorners = new(20),
        IsAntialias = true
    };

    // An input element inside the box that we can click.
    // Sometimes you'd want this the other way around, but it's just an example.
    public InputElement BoxInputArea = new() {
        RelativeSizeAxes = Axes.Both // Makes it the same size as its parent.
    };

    public BoxElement CoverBox = new() {
        RelativeSizeAxes = Axes.Both,
        Margin = 30,
        Color = new(100, 150, 200, 100),
        Pivot = new(0.5f),
        RoundedCorners = new(20),
        IsAntialias = true
    };

    public readonly TextElement Text = new() {
        Text = "Click the box! It's fun!",
        Color = Colors.White,
        Pivot = new(0.5f),
        IsAntialias = true,
        TextSize = 20
    };


    protected override void OnLoad() {
        Add(Box, Text, CoverBox);
        Box.Add(BoxInputArea);

        Root.Scale = new(2f); // This makes the whole scene 2x bigger!

        BoxInputArea.DoPress += (_, _) => {
            Box.StopTransformSequences(nameof(Box.Color), nameof(Box.Rotation));
            Box.ColorTo(Colors.Green).ColorTo(Colors.Orange, 2f, Easing.ExpoOut);
            Box.RotateTo(0).RotateTo(90, 0.5f, Easing.ExpoOut);
        };


        BoxInputArea.DoPressDown += (_, _) => {
            Box.StopTransformSequences(nameof(Box.Scale));
            Box.ScaleTo(0.5f, 3f, Easing.ExpoOut);
        };

        BoxInputArea.DoPressUp += (_, _) => {
            Box.StopTransformSequences(nameof(Box.Scale));
            Box.ScaleTo(1f, 0.8f, Easing.ElasticOut);
        };
    }
}
