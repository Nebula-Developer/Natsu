using Natsu.Core;
using Natsu.Mathematics;

namespace Natsu.Tests.Core;

public class Element_TransformsTests {
    [Fact]
    public void TestElementTransformPropagation() {
        Element parent = new() {
            Scale = 2,
            Position = new(10, 20),
            Size = new(1)
        };

        Element child = new() {
            Parent = parent,
            AnchorPosition = new(1),
            OffsetPosition = new(1),
            Position = new(1)
        };

        Assert.Equal(new(1), child.Position);
        Assert.Equal(new(14, 24), child.WorldPosition);
    }

    [Fact]
    public void TestElementChildRelativeAxes() {
        Element parent = new() {
            ChildRelativeSizeAxes = Axes.Both
        };

        Element child1 = new() {
            Parent = parent,
            Size = new(100),
            Position = new(100, 0)
        };

        Element child2 = new() {
            Parent = parent,
            Size = new(100),
            Position = new(0, 100)
        };

        Assert.Equal(new(200, 200), parent.DrawSize);

        child1.Size = new(200);
        Assert.Equal(new(300, 200), parent.DrawSize);

        child2.Size = new(200);
        Assert.Equal(new(300, 300), parent.DrawSize);
    }

    [Fact]
    public void TestElementRotation() {
        Element element = new() {
            Rotation = 45
        };

        Assert.Equal(45, element.Rotation);

        element.Rotation = 90;
        Assert.Equal(90, element.Rotation);
    }

    [Fact]
    public void TestElementScale() {
        Element element = new() {
            Scale = new(2, 3)
        };

        Assert.Equal(new(2, 3), element.Scale);

        element.Scale = new(1, 1);
        Assert.Equal(new(1, 1), element.Scale);
    }

    [Fact]
    public void TestElementMargin() {
        Element element = new() {
            Margin = new(10, 20)
        };

        Assert.Equal(new(10, 20), element.Margin);

        element.Margin = new(5, 5);
        Assert.Equal(new(5, 5), element.Margin);
    }

    [Fact]
    public void TestElementPosition() {
        Element element = new() {
            Position = new(10, 20)
        };

        Assert.Equal(new(10, 20), element.Position);

        element.Position = new(30, 40);
        Assert.Equal(new(30, 40), element.Position);
    }

    [Fact]
    public void TestElementAnchorPosition() {
        Element element = new() {
            AnchorPosition = new(0.5f, 0.5f)
        };

        Assert.Equal(new(0.5f, 0.5f), element.AnchorPosition);

        element.AnchorPosition = new(1, 1);
        Assert.Equal(new(1, 1), element.AnchorPosition);
    }

    [Fact]
    public void TestElementOffsetPosition() {
        Element element = new() {
            OffsetPosition = new(0.5f, 0.5f)
        };

        Assert.Equal(new(0.5f, 0.5f), element.OffsetPosition);

        element.OffsetPosition = new(1, 1);
        Assert.Equal(new(1, 1), element.OffsetPosition);
    }

    [Fact]
    public void TestElementPivot() {
        Element element = new() {
            Pivot = new(0.5f, 0.5f)
        };

        Assert.Equal(new(0.5f, 0.5f), element.AnchorPosition);
        Assert.Equal(new(0.5f, 0.5f), element.OffsetPosition);
    }

    [Fact]
    public void TestElementDrawSize() {
        Element element = new() {
            Size = new(100, 200)
        };

        Assert.Equal(new(100, 200), element.DrawSize);

        element.Size = new(300, 400);
        Assert.Equal(new(300, 400), element.DrawSize);
    }

    [Fact]
    public void TestElementRelativeSize() {
        Element parent = new() {
            Size = new(200, 200)
        };

        Element child = new() {
            Parent = parent,
            RelativeSizeAxes = Axes.Both
        };

        Assert.Equal(new(200, 200), child.RelativeSize);
    }

    [Fact]
    public void TestElementWorldScale() {
        Element element = new() {
            Scale = new(2, 3)
        };

        Assert.Equal(new(2, 3), element.WorldScale);
    }

    [Fact]
    public void TestElementToLocalSpace() {
        Element element = new() {
            Position = new(10, 20)
        };

        Vector2 screenSpace = new(15, 25);
        Vector2 localSpace = element.ToLocalSpace(screenSpace);

        Assert.Equal(new(5, 5), localSpace);
    }

    [Fact]
    public void TestElementToScreenSpace() {
        Element element = new() {
            Position = new(10, 20)
        };

        Vector2 localSpace = new(5, 5);
        Vector2 screenSpace = element.ToScreenSpace(localSpace);

        Assert.Equal(new(15, 25), screenSpace);
    }

    [Fact]
    public void TestElementPointInside() {
        Element element = new() {
            Size = new(100, 100)
        };

        Vector2 pointInside = new(50, 50);
        Vector2 pointOutside = new(150, 150);

        Assert.True(element.PointInside(pointInside));
        Assert.False(element.PointInside(pointOutside));
    }
}
