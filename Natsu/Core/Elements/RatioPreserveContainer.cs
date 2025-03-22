using Natsu.Core.InvalidationTemp;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A container that will preserve the ratio of its content.
/// </summary>
public class RatioPreserveContainer : Element {
    public readonly RatioPreserveContainerContent ContentWrapper;

    private RatioPreserveMode _mode = RatioPreserveMode.Fit;
    private float _ratio = 16f / 9f;

    public RatioPreserveContainer(float ratio, RatioPreserveMode mode = RatioPreserveMode.Fit) {
        ContentWrapper = new(this) {
            Pivot = 0.5f,
            Clip = true
        };
        Add(ContentWrapper);

        _ratio = ratio;
        _mode = mode;

        ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.DrawSize);
    }

    public override Element ContentContainer => ContentWrapper;

    /// <summary>
    ///     The ratio for the container to preserve.
    /// </summary>
    public float Ratio {
        get => _ratio;
        set {
            _ratio = value;
            ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.DrawSize);
        }
    }

    /// <summary>
    ///     The mode to preserve the ratio of the container.
    /// </summary>
    public RatioPreserveMode Mode {
        get => _mode;
        set {
            _mode = value;
            ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.DrawSize);
        }
    }

    public void UpdateSize() {
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

        ContentWrapper.Validate(ElementInvalidation.Layout);
    }

    protected override void OnLoad() => ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.DrawSize);

    protected override void OnDrawSizeChange(Vector2 size) => ContentWrapper.Invalidate(ElementInvalidation.Layout | ElementInvalidation.DrawSize);
}

/// <summary>
///     The content of a <see cref="RatioPreserveContainer" />.
/// </summary>
/// <param name="container">The ratio preserve container this content belongs to</param>
public class RatioPreserveContainerContent(RatioPreserveContainer container) : Element {
    public RatioPreserveContainer Container => container;

    public override Vector2 Size {
        get {
            if (Invalidated.HasFlag(ElementInvalidation.Layout)) container.UpdateSize();
            return base.Size;
        }
        set => base.Size = value;
    }
}
