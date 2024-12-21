using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class MyApp : Application {
    #nullable disable
    public RectElement Child, Child2, Child3;
    #nullable enable

    protected override void OnLoad() {
        Child = new RectElement {
            ChildRelativeSizeAxes = Axes.Both,
            Color = Colors.Red,
            Scale = new Vector2(2f)
        };

        Child2 = new RectElement {
            ChildRelativeSizeAxes = Axes.Both,
            Color = Colors.Blue,
            Position = new Vector2(100, 100),
            Scale = new Vector2(0.5f)
        };

        Child3 = new RectElement {
            Color = Colors.Yellow,
            Size = new Vector2(50),
            Scale = 3
        };

        Child.Add(Child2);
        Child2.Add(Child3);
        Add(Child);
    }

    protected override void OnUpdate() => Child3.Position = MousePosition - 200;
}
