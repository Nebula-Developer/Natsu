using Natsu;
using Natsu.Graphics;
using Natsu.Graphics.Elements;

namespace NatsuApp;

public class MyApp : Application {
    public RectElement Spinner = new() {
        Size = new(100),
        RoundedCorners = new(10),
        Color = Colors.Red,
        OffsetPosition = new(0.5f),
        AnchorPosition = new(0.5f)
    };

    protected override void OnLoad() {
        Add(Spinner);
        Console.WriteLine("Loaded!");
    }

    protected override void OnUpdate() => Spinner.Rotation += (float)UpdateTime.DeltaTime * 100f;
}
