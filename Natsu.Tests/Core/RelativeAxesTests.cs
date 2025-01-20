using Natsu.Core;
using Natsu.Mathematics;

namespace Natsu.Tests.Core;

public class RelativeAxesTests {
    [Fact]
    public void TestChildRelativeTransform() {
        Element parent = new() {
            ChildRelativeSizeAxes = Axes.Both
        };

        Element child1 = new() {
            Parent = parent,
            Size = new(100),
            Position = new(100)
        };

        Assert.Equal(new(200), parent.DrawSize);

        child1.Size = new(200);

        Assert.Equal(new(300), parent.DrawSize);

        _ = new Element {
            Parent = parent,
            Size = new(100),
            Position = new(300, 100)
        };

        Assert.Equal(new(400, 300), parent.DrawSize);
    }

    [Fact]
    public void TestChildRelativeScale() {
        Element parent = new() {
            ChildRelativeSizeAxes = Axes.Both,
            Scale = 2
        };

        Element child1 = new() {
            Parent = parent,
            Size = new(100),
            Position = new(100)
        };

        Assert.Equal(new(200, 200), parent.DrawSize);

        child1.Size = new(200);
        child1.Scale = 0.5f;

        Assert.Equal(new(200, 200), parent.DrawSize);
    }

    [Fact]
    public void TestNestedChildRelativity() {
        Element parent = new() {
            ChildRelativeSizeAxes = Axes.Both
        };

        Element child1 = new() {
            Parent = parent,
            ChildRelativeSizeAxes = Axes.Both,
            Position = new(100),
            Scale = 2
        };

        Element child2 = new() {
            Parent = child1,
            Position = new(100),
            Size = new(100),
            Scale = 2
        };

        // Explanation:
        // - child1's position is 100 (unaffected by its own scale).
        // - child2's position is affected by child1's 2x scale, making it 200.
        // - child2's size is scaled by both its own scale (2x) and child1's scale (2x), resulting in 400.
        // - The parent's size is calculated as:
        //   child1's position (100) + child2's scaled position (200) + child2's scaled size (400) = 700.

        Assert.Equal(new(700), parent.DrawSize);
    }

    [Fact]
    public void TestParentRelativity() {
        Element parent = new() {
            Size = new(100)
        };

        Element child1 = new() {
            Parent = parent,
            RelativeSizeAxes = Axes.Both
        };

        Assert.Equal(new(100), child1.DrawSize);
    }

    [Fact]
    public void TestNestedRelativity() {
        Element parent = new() {
            ChildRelativeSizeAxes = Axes.Both
        };

        Element child1 = new() {
            Parent = parent,
            RelativeSizeAxes = Axes.Both
        };

        _ = new Element {
            Parent = parent,
            Size = new(100)
        };

        _ = new Element {
            Parent = parent,
            Position = new(200, 0)
        };

        // Explanation:
        // - parent's size should be the biggest size of its children.
        // - child1's size should match parent's size, so it isn't included in the calculation.
        // - The first child's size is 100, so the parent's size should be 100.
        // - The second child's X-position is 200 (>100), so the parent's X-size now becomes 200.
        // - The parent's size should be (200, 100).

        Assert.Equal(new(200, 100), child1.DrawSize);
        Assert.Equal(new(200, 100), parent.DrawSize);
    }

    // TODO: Rotation affects ChildRelativeSizeAxes
}
