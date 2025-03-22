using Natsu.Core.InvalidationTemp;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A container that will prserve the scale of its content, relative to the target size.
/// </summary>
public class ScalePreserveContainer : Element {
    public readonly ScalePreserveContainerContent ContentWrapper;

    private RatioPreserveMode _ratioPreserveMode = RatioPreserveMode.Fit;

    public ScalePreserveContainer(Vector2 targetSize) {
        ContentWrapper = new(this) {
            Pivot = 0.5f,
            Clip = true
        };

        TargetSize = targetSize;

        RelativeSizeAxes = Axes.Both;
        Add(ContentWrapper);

        ContentWrapper.DoDrawSizeChange += _ => ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.Geometry);
    }

    /// <summary>
    ///     The target size to preserve the scale of the content.
    /// </summary>
    public Vector2 TargetSize {
        get => ContentWrapper.DrawSize;
        set => ContentWrapper.Size = value;
    }

    public float Ratio => TargetSize.X / TargetSize.Y;
    public override Element ContentContainer => ContentWrapper;

    /// <summary>
    ///     The mode to preserve the ratio of the container.
    /// </summary>
    public RatioPreserveMode Mode {
        get => _ratioPreserveMode;
        set {
            _ratioPreserveMode = value;
            ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.Geometry);
        }
    }

    public void UpdateScale() {
        float ratioX = DrawSize.X / TargetSize.X;
        float ratioY = DrawSize.Y / TargetSize.Y;

        switch (Mode) {
            case RatioPreserveMode.Fit:
                ContentWrapper.Scale = Math.Min(ratioX, ratioY);
                break;
            case RatioPreserveMode.Cover:
                ContentWrapper.Scale = Math.Max(ratioX, ratioY);
                break;
            case RatioPreserveMode.Width:
                ContentWrapper.Scale = ratioX;
                break;
            case RatioPreserveMode.Height:
                ContentWrapper.Scale = ratioY;
                break;
        }

        ContentWrapper.Validate(ElementInvalidation.Layout);
    }

    protected override void OnDrawSizeChange(Vector2 size) {
        base.OnDrawSizeChange(size);
        ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.Geometry);
    }
}

/// <summary>
///     The content of a <see cref="ScalePreserveContainer" />.
/// </summary>
/// <param name="container">The scale preserve container this content belongs to</param>
public class ScalePreserveContainerContent(ScalePreserveContainer container) : Element {
    public ScalePreserveContainer Container => container;

    public override Vector2 Scale {
        get {
            if (Invalidated.HasFlag(ElementInvalidation.Layout)) container.UpdateScale();
            return base.Scale;
        }
        set => base.Scale = value;
    }
}
