using Natsu.Mathematics;

namespace Natsu.Graphics.Elements;

public enum RatioPreserveMode {
    Width,
    Height,
    Fit,
    Cover
}

public class RatioPreserveContainer : Element {

    public readonly Element ContentWrapper = new() {
        AnchorPosition = new Vector2(0.5f),
        OffsetPosition = new Vector2(0.5f),
        Clip = true
    };

    private RatioPreserveMode _mode = RatioPreserveMode.Fit;
    private float _ratio = 16f / 9f;

    public RatioPreserveContainer(float ratio, RatioPreserveMode mode = RatioPreserveMode.Fit) {
        Add(ContentWrapper);

        _ratio = ratio;
        _mode = mode;

        Fit();
    }

    public override Element ContentContainer => ContentWrapper;

    public float Ratio {
        get => _ratio;
        set {
            _ratio = value;
            Fit();
        }
    }

    public RatioPreserveMode Mode {
        get => _mode;
        set {
            _mode = value;
            Fit();
        }
    }

    public void Fit() {
        Vector2 size = DrawSize;
        float ratio = size.X / size.Y;

        switch (Mode) {
            case RatioPreserveMode.Width:
                ContentWrapper.Size = new Vector2(size.X, size.X / Ratio);
                break;
            case RatioPreserveMode.Height:
                ContentWrapper.Size = new Vector2(size.Y * Ratio, size.Y);
                break;
            case RatioPreserveMode.Fit:
                if (ratio > Ratio)
                    ContentWrapper.Size = new Vector2(size.Y * Ratio, size.Y);
                else
                    ContentWrapper.Size = new Vector2(size.X, size.X / Ratio);
                break;
            case RatioPreserveMode.Cover:
                if (ratio > Ratio)
                    ContentWrapper.Size = new Vector2(size.X, size.X / Ratio);
                else
                    ContentWrapper.Size = new Vector2(size.Y * Ratio, size.Y);
                break;
        }
    }

    protected override void OnLoad() => Fit();

    protected override void OnDrawSizeChange(Vector2 size) => Fit();
}
