using Natsu.Core;
using Natsu.Core.Elements;
using Natsu.Extensions;
using Natsu.Graphics;
using Natsu.Input;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;
using Natsu.Utils.Logging;

namespace Natsu.Sandbox;

public class MyApp : Application {
    public ImageElement Image = null!;

    protected override void OnLoad() {
        IImage image = ResourceLoader.LoadResourceImage("Resources/small-image.png");

        RatioPreserveContainer container = new(1) {
            RelativeSizeAxes = Axes.Both,
            Clip = false,
            Mode = RatioPreserveMode.Fit
        };

        Image = new(image) {
            RelativeSizeAxes = Axes.Both,
            ContentParent = container,
            AnchorPosition = new(0.5f),
            OffsetPosition = new(0.5f)
        };

        TransformSequence<ImageElement>? flash = Image.Begin("flash");
        flash.FadeOut(1).Then().FadeIn(1);
        flash.Loop(4); // Will flash 5 times total (it will have run once before the loop)

        TransformSequence<ImageElement>? spinny = Image.Begin("spinny-sizing");
        spinny.MoveTo(new(0, 200), 1, Ease.ExponentialOut).RotateTo(45, 1, Ease.ExponentialOut).Then();
        spinny.MoveTo(new(0, 0), 1, Ease.ExponentialOut).RotateTo(0, 1, Ease.ExponentialOut).Then();
        spinny.SetLoopPoint(1);
        spinny.MoveTo(new(0, -200), 1, Ease.ExponentialOut).ScaleTo(new(.75f), 1, Ease.ExponentialOut).Then();
        spinny.MoveTo(new(0, 0), 1, Ease.ExponentialOut).ScaleTo(new(1), 1, Ease.ExponentialOut);
        spinny.Loop(1, 1).Loop(2); // Go back to lp1 1 time, then go back to the start. 

        Logging.Fatal("This is a fatal message! AAAAAHHH!");
        Logging.Error("This is an error message!");
        Logging.Warn("This is a warning message!");
        Logging.Info("This is an info message!");

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
