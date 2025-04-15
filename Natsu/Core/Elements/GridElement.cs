using Natsu.Core.Invalidation;
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
///     A grid that will lay its children out in a grid pattern, with a fixed number of columns.
///     <br />
///     Will overflow based on the direction specified.
/// </summary>
// isnt based on the size of the grid, but just the columns. can overflow.
public class GridElement : LayoutElement {
    private int _columns;
    private GridOverflowDirection _overflowDirection = GridOverflowDirection.Horizontal;

    /// <summary>
    ///     The number of columns in the grid.
    ///     <br />
    ///     Acts as row count if the overflow direction is vertical.
    /// </summary>
    public int Columns {
        get => _columns;
        set {
            _columns = value;
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

        Validate(ElementInvalidation.Layout);
    }
}
