using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Mathematics;

public class MyApp : Application {
    protected override void OnLoad() {
        Vector2 pivot = 0.5f;

        BoxElement wrappper = new() {
            Pivot = 0.5f,
            RawRelativeSizeAxes = Axes.Both,
            Size = 0.5f,
            Scale = 1.5f,
            Parent = this,
            Color = Colors.DarkGray
        };

        new BoxElement {
            Color = Colors.Orange,
            Size = new(120, 110),
            Pivot = pivot,
            Parent = wrappper,
            Opacity = 0.5f,
            Padding = 10,
            Margin = 10,
            IsStroke = true,
            StrokeWidth = 5
        };

        new BoxElement {
            Color = Colors.Cyan,
            Size = 100f,
            Pivot = pivot,
            Parent = wrappper,
            Opacity = 0.5f,
            Padding = new(5, 10),
            Margin = 10,
            IsStroke = true,
            StrokeWidth = 5
        };

        BoxElement x = new() {
            Color = Colors.Green,
            Size = new(50f, 50f),
            Scale = new(2f, 2f),
            Pivot = pivot,
            Parent = wrappper,
            Opacity = 0.5f,
            Padding = 10,
            Margin = 10,
            IsStroke = true,
            StrokeWidth = 5
        };
        x.ScaleTo(new(20), 5f).Loop();

        new BoxElement {
            Color = Colors.Blue,
            Size = new(50f, 200f),
            Scale = new(2f, 0.5f),
            Pivot = pivot,
            Parent = wrappper,
            Opacity = 0.5f,
            Padding = 10,
            Margin = 10,
            IsStroke = true,
            StrokeWidth = 5
        };
    }
}
