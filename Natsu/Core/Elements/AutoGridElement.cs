using Natsu.Core.InvalidationTemp;
using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     A grid element that will lay out its children so that they fit within the grid's bounds.
/// </summary>
public class AutoGridElement : LayoutElement {
    private GridOverflowDirection _overflowDirection = GridOverflowDirection.Horizontal;
    private Vector2 _spacing = 0f;

    /// <summary>
    ///     The spacing between the elements in the grid.
    /// </summary>
    public Vector2 Spacing {
        get => _spacing;
        set {
            _spacing = value;
            Invalidate(ElementInvalidation.Layout);
        }
    }

    /// <summary>
    ///     The direction the grid lays out its elements.
    /// </summary>
    public GridOverflowDirection OverflowDirection {
        get => _overflowDirection;
        set {
            _overflowDirection = value;
            Invalidate(ElementInvalidation.Layout);
        }
    }

    public override void ComputeLayout() {
        Vector2 size = DrawSize;

        float x = 0;
        float y = 0;
        float rowHeight = 0;
        float columnWidth = 0;

        for (int i = 0; i < Children.Count; i++) {
            Element? child = Children[i];
            Vector2 childSize = child.DrawSize;
            Vector2 realSize = childSize * child.Scale;

            if (OverflowDirection == GridOverflowDirection.Horizontal) {
                if (x + realSize.X > size.X) {
                    x = 0;
                    y += rowHeight + Spacing.Y;
                    rowHeight = 0;
                }
            } else {
                if (y + realSize.Y > size.Y) {
                    y = 0;
                    x += columnWidth + Spacing.X;
                    columnWidth = 0;
                }
            }

            child.Position = new(x, y);

            if (OverflowDirection == GridOverflowDirection.Horizontal) {
                x += realSize.X + Spacing.X;
                rowHeight = Math.Max(rowHeight, realSize.Y);
            } else {
                y += realSize.Y + Spacing.Y;
                columnWidth = Math.Max(columnWidth, realSize.X);
            }
        }

        Validate(ElementInvalidation.Layout);
    }

    protected override void OnDrawSizeChange(Vector2 size) => Invalidate(ElementInvalidation.Layout);
}
