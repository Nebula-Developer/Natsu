using Natsu.Mathematics;

namespace Natsu.Core.Elements;

/// <summary>
///     The direction in which the grid overflows its elements.
/// </summary>
public enum GridOverflowDirection {
    Horizontal,
    Vertical
}

/// <summary>
///     An element that has a <see cref="ComputeLayout" /> method that will lay out its children when invalidated.
/// </summary>
public abstract class LayoutElement : Element {
    public virtual void ComputeLayout() { }

    protected override void OnUpdate() {
        if (Invalidated.HasFlag(Invalidation.Layout)) ComputeLayout();
    }

    protected override void OnChildrenChange() {
        if (Loaded) Invalidate(Invalidation.Layout);
    }
}

/// <summary>
///     A grid element that will lay out its children so that they fit within the grid's bounds.
/// </summary>
public class AutoGridElement : LayoutElement {
    private GridOverflowDirection _overflowDirection = GridOverflowDirection.Horizontal;
    private Vector2 _spacing = 0f;

    public Vector2 Spacing {
        get => _spacing;
        set {
            _spacing = value;
            Invalidate(Invalidation.Layout);
        }
    }

    public GridOverflowDirection OverflowDirection {
        get => _overflowDirection;
        set {
            _overflowDirection = value;
            Invalidate(Invalidation.Layout);
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

        Validate(Invalidation.Layout);
    }

    protected override void OnDrawSizeChange(Vector2 size) => Invalidate(Invalidation.Layout);
}

/// <summary>
///     A grid that will lay its children out in a grid pattern, with a fixed number of columns.
///     <br />
///     Will overflow based on the direction specified.
/// </summary>
// isnt based on the size of the grid, but just the columns. can overflow.
public class GridElement : LayoutElement {
    private int _columns;
    private GridOverflowDirection _overflowDirection = GridOverflowDirection.Horizontal;

    public int Columns {
        get => _columns;
        set {
            _columns = value;
            Invalidate(Invalidation.Layout);
        }
    }

    public GridOverflowDirection OverflowDirection {
        get => _overflowDirection;
        set {
            _overflowDirection = value;
            Invalidate(Invalidation.Layout);
        }
    }

    public override void ComputeLayout() {
        float x = 0;
        float y = 0;
        float rowHeight = 0;
        float columnWidth = 0;
        int column = 0;
        int row = 0;

        for (int i = 0; i < Children.Count; i++) {
            Element? child = Children[i];
            Vector2 childSize = child.DrawSize;
            Vector2 realSize = childSize * child.Scale;

            if (OverflowDirection == GridOverflowDirection.Vertical) {
                if (column >= Columns) {
                    column = 0;
                    row++;
                    x = 0;
                    y += rowHeight;
                    rowHeight = 0;
                }
            } else {
                if (row >= Columns) {
                    row = 0;
                    column++;
                    y = 0;
                    x += columnWidth;
                    columnWidth = 0;
                }
            }

            child.Position = new(x, y);

            if (OverflowDirection == GridOverflowDirection.Vertical) {
                x += realSize.X;
                columnWidth = Math.Max(columnWidth, realSize.X);
                rowHeight = Math.Max(rowHeight, realSize.Y);
                column++;
            } else {
                y += realSize.Y;
                columnWidth = Math.Max(columnWidth, realSize.X);
                rowHeight = Math.Max(rowHeight, realSize.Y);
                row++;
            }
        }

        Validate(Invalidation.Layout);
    }
}
