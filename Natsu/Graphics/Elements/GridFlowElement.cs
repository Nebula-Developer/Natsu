using Natsu.Mathematics;
using Natsu.Utils;

namespace Natsu.Graphics.Elements;

public enum GridOverflowDirection {
    Horizontal,
    Vertical
}

public class GridElement : Element {
    private int _columns = 1;
    private int _rows = 1;
    private Vector2 _spacing = 0f;

    public Dirty Layout = new();


    public Vector2 Spacing {
        get => _spacing;
        set {
            _spacing = value;
            Layout.Invalidate();
        }
    }

    public int Columns {
        get => _columns;
        set {
            _columns = value;
            Layout.Invalidate();
        }
    }

    public int Rows {
        get => _rows;
        set {
            _rows = value;
            Layout.Invalidate();
        }
    }

    public void ComputeLayout() {
        Vector2 size = DrawSize;
        Vector2 cellSize = new((size.X - Spacing.X * (Columns - 1)) / Columns, (size.Y - Spacing.Y * (Rows - 1)) / Rows);

        for (int i = 0; i < Children.Count; i++) {
            Element child = Children[i];
            int x = i % Columns;
            int y = i / Columns;

            child.Position = new Vector2(x * (cellSize.X + Spacing.X), y * (cellSize.Y + Spacing.Y));

            child.Size = cellSize;
        }

        Layout.Validate();
    }

    protected override void OnChildrenChange() {
        if (Loaded)
            Layout.Invalidate();
    }

    protected override void OnLoad() => Layout.Invalidate();

    protected override void OnDrawSizeChange(Vector2 size) => Layout.Invalidate();

    protected override void OnUpdate() {
        if (Layout.IsDirty)
            ComputeLayout();
    }
}

public class GridFlowElement : Element {
    private GridOverflowDirection _overflowDirection = GridOverflowDirection.Horizontal;
    private Vector2 _spacing = 0f;

    public Dirty Layout = new();

    public Vector2 Spacing {
        get => _spacing;
        set {
            _spacing = value;
            Layout.Invalidate();
        }
    }

    public GridOverflowDirection OverflowDirection {
        get => _overflowDirection;
        set {
            _overflowDirection = value;
            Layout.Invalidate();
        }
    }

    public void ComputeLayout() {
        Vector2 size = DrawSize;

        float x = 0;
        float y = 0;
        float rowHeight = 0;
        float columnWidth = 0;

        for (int i = 0; i < Children.Count; i++) {
            Element child = Children[i];
            Vector2 childSize = child.DrawSize;

            if (OverflowDirection == GridOverflowDirection.Horizontal) {
                if (x + childSize.X > size.X) {
                    x = 0;
                    y += rowHeight + Spacing.Y;
                    rowHeight = 0;
                }
            } else {
                if (y + childSize.Y > size.Y) {
                    y = 0;
                    x += columnWidth + Spacing.X;
                    columnWidth = 0;
                }
            }

            child.Position = new Vector2(x, y);

            if (OverflowDirection == GridOverflowDirection.Horizontal) {
                x += childSize.X + Spacing.X;
                rowHeight = Math.Max(rowHeight, childSize.Y);
            } else {
                y += childSize.Y + Spacing.Y;
                columnWidth = Math.Max(columnWidth, childSize.X);
            }
        }

        Layout.Validate();
    }

    protected override void OnChildrenChange() {
        if (Loaded)
            Layout.Invalidate();
    }

    protected override void OnLoad() => Layout.Invalidate();

    protected override void OnDrawSizeChange(Vector2 size) => Layout.Invalidate();

    protected override void OnUpdate() {
        if (Layout.IsDirty)
            ComputeLayout();
    }
}
