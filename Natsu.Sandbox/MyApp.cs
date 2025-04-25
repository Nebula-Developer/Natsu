using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;

public class MyApp : Application {
    protected override void OnLoad() {
        Vector2 pivot = 0.5f;

        // Root.Pivot = 0.5f;
        // Root.Rotation = 45;
        Root.MoveTo(10f, 1f).Loop();

        BoxElement parent = new() {
            RelativeSizeAxes = Axes.Both,
            Color = Colors.Green,
            Opacity = 1f
        };

        float opacity = 1f;

        DoKeyDown += (key, mods) => {
            switch (key) {
                case Key.Escape:
                    Platform.Exit();
                    break;

                case Key.Up:
                    if (opacity >= 1f) {
                        opacity = 1f;
                        break;
                    }

                    parent.OpacityTo(opacity += 0.25f, 0.25f);
                    break;

                case Key.Down:
                    if (opacity <= 0f) {
                        opacity = 0f;
                        break;
                    }

                    parent.OpacityTo(opacity -= 0.25f, 0.25f);
                    break;
            }

            Console.WriteLine("Key: " + key + " Mods: " + mods);
        };

        BoxElement child = new() {
            Parent = parent,
            Color = new(255, 0, 255, 100),
            RelativeSizeAxes = Axes.Both,
            Size = 0.5f,
            // Pivot = 0.5f,
            Opacity = 2f,
            Children = [
                new BoxElement {
                    Size = 100f,
                    Pivot = 0.5f
                }
            ]
        };

        child.Opacity -= 0.01f;

        Console.WriteLine(child.WorldOpacity);

        Add(parent);

        Console.WriteLine(child.WorldOpacity);

        // Root.Opacity = 0f;
        // Root.OpacityTo(1f, 1f).Then().OpacityTo(0f, 1f).Loop();
    }
}
