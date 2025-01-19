using Natsu.Core;
using Natsu.Mathematics;

namespace Natsu.Tests.Core;

public class CompositeTests {
    [Fact]
    public void CheckParent() {
        Element parent = new();
        Element parent2 = new();
        Element child = new();

        child.Parent = parent;
        Assert.Equal(parent, child.Parent);

        child.Parent = parent2;
        Assert.Equal(parent2, child.Parent);
    }

    [Fact]
    public void CheckContentParent() {
        Element parent = new();
        Element child = new();

        child.ContentParent = parent;
        Assert.Equal(parent.ContentContainer, child.Parent);
    }

    [Fact]
    public void CheckChildRelativeSizeAxes() {
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
        Assert.Equal(new(200, 200), child1.Size);
    }
}
