using Natsu.Graphics;
using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Mathematics;
using Natsu.Extensions;

namespace NatsuApp;

public class MyApp : Application {
    // A 100x100 rounded box, in the center of the screen.
    // We add it to the hirearchy in the OnLoad method.
    public readonly BoxElement Box = new() {
        Color = Colors.Orange,
        Size = new(100),
        Pivot = new(0.5f),
        RoundedCorners = new(20),
        IsAntialias = true
    };

    // An input element inside the box that we can click.
    // Sometimes you'd want this the other way around, but it's just an example.
    public InputElement BoxInputArea = new() {
        RelativeSizeAxes = Axes.Both
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
        // Adding the box, text and cover box to the root element.
        Add(Box, Text, CoverBox);

        // Adding the input area to the box.
        Box.Add(BoxInputArea);

        // This makes everything visible 2x bigger.
        Root.Scale = new(2f);

        // Below we add some handlers to the input area.
        // The press argumemnts are (index, pos), where
        // index is the mouse/finger index.

        BoxInputArea.DoPress += (_, _) => {
            Box.ColorTo(Colors.Green).ColorTo(Colors.Orange, 2f, Easing.ExpoOut);
            Box.RotateTo(0).RotateTo(90, 0.5f, Easing.ExpoOut);
        };

        BoxInputArea.DoPressDown += (_, _) => Box.ScaleTo(0.5f, 3f, Easing.ExpoOut);
        BoxInputArea.DoPressUp += (_, _) => Box.ScaleTo(1f, 0.8f, Easing.ElasticOut);
    }
}
