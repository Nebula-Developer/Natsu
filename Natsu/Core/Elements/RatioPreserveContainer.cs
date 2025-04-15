using Natsu.Core.Invalidation;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A container that will preserve the ratio of its content.
/// </summary>
public class RatioPreserveContainer : LayoutElement {
    public readonly Element RatioContent;

    private RatioPreserveMode _mode = RatioPreserveMode.Fit;
    private float _ratio = 16f / 9f;

    public RatioPreserveContainer(float ratio, RatioPreserveMode mode = RatioPreserveMode.Fit) {
        RatioContent = new() {
            Pivot = 0.5f,
            Clip = true
        };
        Add(RatioContent);

        _ratio = ratio;
        _mode = mode;

        RatioContent.Invalidate(ElementInvalidation.Layout);
    }

    public override Element ContentContainer => RatioContent;

    /// <summary>
    ///     Controls the pivot of the content.
    /// </summary>
    public Vector2 ContentPivot {
        get => RatioContent.Pivot;
        set => RatioContent.Pivot = value;
    }

    /// <summary>
    ///     The ratio for the container to preserve.
    /// </summary>
    public float Ratio {
        get => _ratio;
        set {
            _ratio = value;
            Invalidate(ElementInvalidation.Layout);
        }
    }

    /// <summary>
    ///     The mode to preserve the ratio of the container.
    /// </summary>
    public RatioPreserveMode Mode {
        get => _mode;
        set {
            _mode = value;
            Invalidate(ElementInvalidation.Layout);
        }
    }

    public void UpdateSize() {
        Vector2 size = DrawSize;
        float ratio = size.X / size.Y;

        switch (Mode) {
            case RatioPreserveMode.Width:
                RatioContent.Size = new(size.X, size.X / Ratio);
                break;
            case RatioPreserveMode.Height:
                RatioContent.Size = new(size.Y * Ratio, size.Y);
                break;
            case RatioPreserveMode.Fit:
                if (ratio > Ratio)
                    RatioContent.Size = new(size.Y * Ratio, size.Y);
                else
                    RatioContent.Size = new(size.X, size.X / Ratio);
                break;
            case RatioPreserveMode.Cover:
                if (ratio > Ratio)
                    RatioContent.Size = new(size.X, size.X / Ratio);
                else
                    RatioContent.Size = new(size.Y * Ratio, size.Y);
                break;
        }

        Validate(ElementInvalidation.Layout);
    }

    protected override void OnLoad() => RatioContent.Invalidate(ElementInvalidation.Layout | ElementInvalidation.DrawSize);

    protected override void OnDrawSizeChange(Vector2 size) => RatioContent.Invalidate(ElementInvalidation.Layout | ElementInvalidation.DrawSize);

    public override void ComputeLayout() {
        UpdateSize();
        base.ComputeLayout();
    }
}
