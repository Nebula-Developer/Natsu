using Natsu.Core;
using Xunit.Abstractions;

namespace Natsu.Tests.Mathematics;

public class Composite {
    private readonly ITestOutputHelper _output;

    public Composite(ITestOutputHelper output) => _output = output;

    [Fact]
    public void CheckParent() {
        Element parent = new();
        Element parent2 = new();
        Element child = new();

        parent.Add(child);
        Assert.Equal(parent, child.Parent);
        Assert.Single(parent.Children);

        parent2.Add(child);
        Assert.Equal(parent2, child.Parent);
        Assert.Empty(parent.Children);

        child.Parent = null;
        Assert.Null(child.Parent);
    }
}
