using Natsu.Graphics;
using Natsu.Graphics.Elements;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class MyApp : Application {
    private TextElement? fpsText;

    protected override void OnLoad() {
        Add(fpsText = new TextElement {
            ContentParent = Root,
            Text = "FPS: 0",
            Position = new Vector2(20, 20),
            Index = 10
        });

        Slider slider = new() {
            RelativeSizeAxes = Axes.X,
            Margin = new Vector2(20, 0),
            Size = new Vector2(0, 50),
            AnchorPosition = new Vector2(0.5f, 1),
            OffsetPosition = new Vector2(0.5f, 1f),
            ContentParent = Root,
            Index = 3,
            Position = new Vector2(0, -20)
        };

        TextBox test;

        Add(test = new TextBox {
            RelativeSizeAxes = Axes.X,
            Margin = new Vector2(30, 0),
            Size = new Vector2(100),
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            ContentParent = Root,
            Index = 3
        });

        Add(new TextBox {
            RelativeSizeAxes = Axes.X,
            Margin = new Vector2(30, 0),
            Size = new Vector2(100),
            AnchorPosition = new Vector2(0.5f),
            OffsetPosition = new Vector2(0.5f),
            ContentParent = Root,
            Position = new Vector2(0, 130),
            Index = 3
        });

        test.Preview.Font = ResourceLoader.LoadResourceFont("Resources/FiraCode-Regular.ttf");
    }

    protected override void OnRender() {
        base.OnRender();
        float fps = 1f / (float)RenderTime.DeltaTime;
        fpsText!.Text = $"FPS: {fps}";
    }
}
