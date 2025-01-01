using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Mathematics;

namespace NatsuApp;

public class MyApp : Application {
    public RectElement Spinner = new() {
        Size = new Vector2(100),
        RoundedCorners = new Vector2(10),
        Color = Colors.Red,
        IsAntialias = true,
        OffsetPosition = new Vector2(0.5f),
        AnchorPosition = new Vector2(0.5f)
    };

    protected override void OnLoad() {
        Add(Spinner);
        Spinner.RotateTo(360, 2, Ease.QuinticOut).Loop();
    }
}
