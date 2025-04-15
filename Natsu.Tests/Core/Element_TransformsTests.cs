using Natsu.Core;
using Natsu.Mathematics;

namespace Natsu.Tests.Core;

public class Element_GeometryTests {
    [Fact]
    public void WorldPosition_WithPivot_IsCorrect() {
        Element root = new() {
            Position = 100,
            Size = 200
        };

        Element child = new() {
            Parent = root,
            Size = 50,
            Pivot = 0.5f,
            Position = 0
        };

        // 100 + (200 / 2) - (50 / 2)
        // 100 + 100 - 25

        Assert.Equal(175, child.WorldPosition);
    }

    [Fact]
    public void DrawSize_WithRelativeSizeAxes_IsCorrect() {
        Element parent = new() {
            Size = new(500, 400)
        };

        Element child = new() {
            Parent = parent,
            RelativeSizeAxes = Axes.Both
        };

        Assert.Equal(new(500, 400), child.DrawSize);

        parent.Size = new(300, 600);
        Assert.Equal(new(300, 600), child.DrawSize);
    }

    [Fact]
    public void DrawSize_WithMargins_IsCorrect() {
        Element element = new() {
            Size = 100,
            Margin = new(10, 20)
        };

        Assert.Equal(new(80, 60), element.DrawSize);
    }

    [Fact]
    public void WorldScale_AccumulatesCorrectly() {
        Element parent = new() {
            Scale = new(2, 3)
        };

        Element child = new() {
            Parent = parent,
            Scale = new(0.5f, 2f)
        };

        Assert.Equal(new(1, 6), child.WorldScale);
    }

    [Fact]
    public void ToLocalSpace_RoundTrip() {
        Element element = new() {
            Position = new(30, 40)
        };

        Vector2 original = new(50, 60);
        Vector2 local = element.ToLocalSpace(original);
        Vector2 back = element.ToScreenSpace(local);

        Assert.Equal(original, back);
    }

    [Fact]
    public void PointInside_RespectsSizeAndMargin() {
        Element element = new() {
            Size = 100,
            Margin = 10
        };

        // 100 - 10 - 10
        Assert.True(element.PointInside(80));
        Assert.False(element.PointInside(81));
    }

    [Fact]
    public void ChangingAnchorAffectsWorldPosition() {
        Element parent = new() {
            Position = 0,
            Size = 300
        };

        Element child = new() {
            Parent = parent,
            AnchorPosition = 1,
            Size = 50
        };

        Assert.Equal(300, child.WorldPosition);
    }

    [Fact]
    public void NestedScaleAndRotationAppliesCorrectly() {
        Element root = new() {
            Position = 0,
            Scale = 2,
            Rotation = 90
        };

        Element mid = new() {
            Parent = root,
            Position = 10,
            Rotation = 90,
            Scale = 0.5f
        };

        Element leaf = new() {
            Parent = mid,
            Position = 5,
            Scale = 0.5f
        };

        Assert.Equal(-180, leaf.WorldRotation, 0.01f);
        Assert.Equal(0.5f, leaf.WorldScale);
    }

    [Fact]
    public void RelativeAxes_OnlyAffectSpecifiedAxes() {
        Element parent = new() {
            Size = new(300, 400)
        };

        Element child = new() {
            Parent = parent,
            RelativeSizeAxes = Axes.X
        };

        Assert.Equal(new(300, 0), child.RelativeSize);
    }

    [Fact]
    public void DrawSizeWithParentChildMargin() {
        Element parent = new() {
            Margin = 10,
            ChildRelativeSizeAxes = Axes.Both
        };

        Element child = new() {
            Parent = parent,
            Size = 50
        };

        Assert.Equal(30, parent.DrawSize);
    }

    [Fact]
    public void PivotResetsPositionAsExpected() {
        Element element = new() {
            Size = 200,
            Pivot = 0.5f,
            Position = 50
        };

        Assert.Equal(50, element.Position);
        // 50 - (200 / 2)
        Assert.Equal(-50, element.WorldPosition);
    }

    [Fact]
    public void PaddingAffectsRelativeSize() {
        Element root = new() {
            Size = 100,
            Padding = new(10, 20)
        };

        Element child = new() {
            Parent = root,
            RelativeSizeAxes = Axes.Both
        };

        Assert.Equal(new(80, 60), child.DrawSize);
    }

    [Fact]
    public void PositionStackCorrectly() {
        Element root = new() {
            Position = 50
        };

        Element child = new() {
            Parent = root,
            Position = new(20, 30)
        };

        Assert.Equal(new(70, 80), child.WorldPosition);
    }

    [Fact]
    public void NestedPaddingAndMargins_AccumulateCorrectly() {
        Element parent = new() {
            Size = new(500, 400),
            Padding = new(20, 30)
        };

        Element child1 = new() {
            Parent = parent,
            Size = 100,
            Margin = 10
        };

        Element child2 = new() {
            Parent = parent,
            Size = 150,
            Margin = 15
        };

        Assert.Equal(80, child1.DrawSize);
        Assert.Equal(120, child2.DrawSize);
    }

    [Fact]
    public void ChildRelativeAxes_ParentSizeBasedOnChildren() {
        Element child1 = new() {
            Size = 100
        };

        Element child2 = new() {
            Size = 150
        };

        Element parent = new() {
            Parent = null,
            ChildRelativeSizeAxes = Axes.Both
        };

        parent.Size = new(child1.Size.X + child2.Size.X, child1.Size.Y + child2.Size.Y);

        Assert.Equal(250, parent.Size);
    }

    [Fact]
    public void PaddingAndMargin_RespectRelativeSizeAxes() {
        Element root = new() {
            Size = 500,
            Padding = 20
        };

        Element child = new() {
            Parent = root,
            RelativeSizeAxes = Axes.Both,
            Margin = 10
        };

        Assert.Equal(440, child.DrawSize.X);
    }
}
