using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;

namespace Natsu.Sandbox;

public class MyApp : Application {
    public ImageElement Image = null!;

    protected override void OnLoad() {
        IImage image = ResourceLoader.LoadResourceImage("Resources/small-image.png");

        RatioPreserveContainer container = new(1) {
            RelativeSizeAxes = Axes.Both,
            Clip = false
        };

        Image = new(image) {
            RelativeSizeAxes = Axes.Both,
            ContentParent = container,
            AnchorPosition = new(0.5f),
            OffsetPosition = new(0.5f)
        };

        Add(container);
    }

    protected override void OnKeyDown(Key key, KeyMods mods) {
        switch (key) {
            case Key.D1:
                Image.FilterQuality = FilterQuality.None;
                break;
            case Key.D2:
                Image.FilterQuality = FilterQuality.Low;
                break;
            case Key.D3:
                Image.FilterQuality = FilterQuality.Medium;
                break;
            case Key.D4:
                Image.FilterQuality = FilterQuality.High;
                break;
        }
    }
}
