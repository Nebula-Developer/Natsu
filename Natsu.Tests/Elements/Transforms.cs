using Natsu.Core;
using Natsu.Mathematics;
using Xunit.Abstractions;

namespace Natsu.Tests.Mathematics;

public class Transforms {
    private readonly ITestOutputHelper _output;

    public Transforms(ITestOutputHelper output) => _output = output;

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
}
