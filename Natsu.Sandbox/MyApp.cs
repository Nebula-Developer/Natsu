using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class MyApp : Application {
#nullable disable
    public RectElement Child;
#nullable enable

    protected override void OnLoad() {
        Color[] colors = { Colors.Red, Colors.Green, Colors.Blue, Colors.White, Colors.Cyan, Colors.Magenta };

        RectElement? parent = null;

        for (int i = 0; i < 6; i++) {
            RectElement child = new() {
                Color = colors[i],
                ChildRelativeSizeAxes = Axes.Both,
                Name = $"Child {i}"
            };

            if (parent is not null) {
                child.Parent = parent;
                child.Position = new Vector2(10);
            } else Add(child);

            parent = child;
        }

        parent!.Add(new RectElement {
            RelativeSizeAxes = Axes.Both,
            Color = new Color(50, 100, 200)
        });

        Child = new RectElement {
            Color = Colors.Yellow,
            Size = new Vector2(50),
            Scale = 3,
            Name = "Main Child"
        };

        Child.Parent = parent;
    }

    protected override void OnUpdate() => Child.Position = MousePosition - 200;
}
