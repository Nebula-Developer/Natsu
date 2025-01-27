using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     The mode to preserve the ratio of the container.
/// </summary>
public enum RatioPreserveMode {
    /// <summary>
    ///     The content is fit to the width of the container.
    /// </summary>
    Width,

    /// <summary>
    ///     The content is fit to the height of the container.
    /// </summary>
    Height,

    /// <summary>
    ///     The content will fit inside the container.
    /// </summary>
    Fit,

    /// <summary>
    ///     The content will cover the container.
    /// </summary>
    Cover
}

/// <summary>
///     A container that will preserve the ratio of its content.
/// </summary>
public class RatioPreserveContainer : Element {
    public readonly Element ContentWrapper = new() {
        AnchorPosition = new(0.5f),
        OffsetPosition = new(0.5f),
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

    public new bool Clip {
        get => ContentWrapper.Clip;
        set => ContentWrapper.Clip = value;
    }

    public override Element ContentContainer => ContentWrapper;

    public float Ratio {
        get => _ratio;
        set {
            _ratio = value;
            Fit();
        }
    }

    /// <summary>
    ///     The mode to preserve the ratio of the container.
    /// </summary>
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
                ContentWrapper.Size = new(size.X, size.X / Ratio);
                break;
            case RatioPreserveMode.Height:
                ContentWrapper.Size = new(size.Y * Ratio, size.Y);
                break;
            case RatioPreserveMode.Fit:
                if (ratio > Ratio)
                    ContentWrapper.Size = new(size.Y * Ratio, size.Y);
                else
                    ContentWrapper.Size = new(size.X, size.X / Ratio);

                break;
            case RatioPreserveMode.Cover:
                if (ratio > Ratio)
                    ContentWrapper.Size = new(size.X, size.X / Ratio);
                else
                    ContentWrapper.Size = new(size.Y * Ratio, size.Y);

                break;
        }
    }

    protected override void OnLoad() => Fit();

    protected override void OnDrawSizeChange(Vector2 size) => Fit();
}
