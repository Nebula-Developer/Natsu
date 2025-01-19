using Natsu.Core;

namespace Natsu.Tests.Core;

public class Element_CompositeTests {
    [Fact]
    public void TestContentParent() {
        Element parent = new();
        Element child = new();
        child.ContentParent = parent;
        Assert.Equal(parent.ContentContainer, child.Parent);
    }

    [Fact]
    public void TestParent() {
        Element parent = new();
        Element child = new();
        child.Parent = parent;
        Assert.Equal(parent, child.Parent);
    }

    [Fact]
    public void TestContent() {
        Element parent = new();
        Element child1 = new();
        Element child2 = new();
        parent.Content = new List<Element> { child1, child2 };
        Assert.Contains(child1, parent.Content);
        Assert.Contains(child2, parent.Content);
    }

    [Fact]
    public void TestChildren() {
        Element parent = new();
        Element child1 = new();
        Element child2 = new();
        parent.Children = new List<Element> { child1, child2 };
        Assert.Contains(child1, parent.Children);
        Assert.Contains(child2, parent.Children);
    }

    [Fact]
    public void TestAdd() {
        Element parent = new();
        Element child = new();
        parent.Add(child);
        Assert.Contains(child, parent.Children);
    }

    [Fact]
    public void TestRemove() {
        Element parent = new();
        Element child = new();
        parent.Add(child);
        parent.Remove(child);
        Assert.DoesNotContain(child, parent.Children);
    }

    [Fact]
    public void TestAddContent() {
        Element parent = new();
        Element child = new();
        parent.AddContent(child);
        Assert.Contains(child, parent.Content);
    }

    [Fact]
    public void TestRemoveContent() {
        Element parent = new();
        Element child = new();
        parent.AddContent(child);
        parent.RemoveContent(child);
        Assert.DoesNotContain(child, parent.Content);
    }

    [Fact]
    public void TestClear() {
        Element parent = new();
        Element child = new();
        parent.Add(child);
        parent.Clear();
        Assert.Empty(parent.Children);
    }

    [Fact]
    public void TestClearContent() {
        Element parent = new();
        Element child = new();
        parent.AddContent(child);
        parent.ClearContent();
        Assert.Empty(parent.Content);
    }

    [Fact]
    public void TestForChildren() {
        Element parent = new();
        Element child1 = new();
        Element child2 = new();
        parent.Add(child1, child2);
        int count = 0;
        parent.ForChildren(child => count++);
        Assert.Equal(2, count);
    }

    [Fact]
    public void TestSortChild() {
        Element parent = new();
        Element child1 = new() { Index = 2 };
        Element child2 = new() { Index = 1 };
        parent.Add(child1, child2);
        parent.SortChild(child1);
        Assert.Equal(child2, parent.Children[0]);
        Assert.Equal(child1, parent.Children[1]);
    }

    [Fact]
    public void TestHasChild() {
        Element parent = new();
        Element child = new();
        parent.Add(child);
        Assert.True(parent.HasChild(child));
    }

    [Fact]
    public void TestChildrenChanged() {
        Element parent = new();
        bool eventTriggered = false;
        parent.DoChildrenChange += () => eventTriggered = true;
        parent.Add(new Element());
        Assert.True(eventTriggered);
    }
}
