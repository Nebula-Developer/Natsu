using Natsu.Core.Invalidation;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A container that will prserve the scale of its content, relative to the target size.
/// </summary>
public class ScalePreserveContainer : LayoutElement {
    public readonly Element ScaledContent;

    private RatioPreserveMode _ratioPreserveMode = RatioPreserveMode.Fit;

    public ScalePreserveContainer(Vector2 targetSize) {
        ScaledContent = new() {
            Pivot = 0.5f,
            Clip = true
        };

        TargetSize = targetSize;

        RelativeSizeAxes = Axes.Both;
        Add(ScaledContent);
    }

    /// <summary>
    ///     Controls the pivot of the content.
    /// </summary>
    public Vector2 ContentPivot {
        get => ScaledContent.Pivot;
        set => ScaledContent.Pivot = value;
    }

    /// <summary>
    ///     The target size to preserve the scale of the content.
    /// </summary>
    public Vector2 TargetSize {
        get => ScaledContent.DrawSize;
        set => ScaledContent.Size = value;
    }

    public float Ratio => TargetSize.X / TargetSize.Y;
    public override Element ContentContainer => ScaledContent;

    /// <summary>
    ///     The mode to preserve the ratio of the container.
    /// </summary>
    public RatioPreserveMode Mode {
        get => _ratioPreserveMode;
        set {
            _ratioPreserveMode = value;
            Invalidate(ElementInvalidation.Layout);
        }
    }

    public void UpdateScale() {
        float ratioX = DrawSize.X / TargetSize.X;
        float ratioY = DrawSize.Y / TargetSize.Y;

        switch (Mode) {
            case RatioPreserveMode.Fit:
                ScaledContent.Scale = Math.Min(ratioX, ratioY);
                break;
            case RatioPreserveMode.Cover:
                ScaledContent.Scale = Math.Max(ratioX, ratioY);
                break;
            case RatioPreserveMode.Width:
                ScaledContent.Scale = ratioX;
                break;
            case RatioPreserveMode.Height:
                ScaledContent.Scale = ratioY;
                break;
        }
    }

    protected override void OnDrawSizeChange(Vector2 size) {
        base.OnDrawSizeChange(size);
        Invalidate(ElementInvalidation.Layout);
    }

    public override void ComputeLayout() {
        UpdateScale();
        base.ComputeLayout();
    }
}
