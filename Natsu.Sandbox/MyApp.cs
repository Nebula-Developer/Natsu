using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Input;
using Natsu.Mathematics;

public class MyApp : Application {
    private Vector2 _targetScale = Vector2.One;

    protected override void OnLoad() {
        BoxElement box = new() {
            RelativeSizeAxes = Axes.Both,
            Pivot = new(0.5f),
            Margin = new(10, 20, 30, 40)
        };
        Add(box);
    }

    protected override void OnKeyDown(Key key, KeyMods mods) {
        Vector2 orig = _targetScale;
        if (key == Key.Up)
            _targetScale *= 1.1f;
        else if (key == Key.Down) _targetScale /= 1.1f;

        Root.StopTransformSequence(nameof(Root.Scale));
        Root.ScaleTo(_targetScale, 0.5f, Easing.ExpoOut);
    }
}
