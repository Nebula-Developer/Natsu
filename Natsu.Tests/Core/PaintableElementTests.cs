using Natsu.Core.Elements;
using Natsu.Graphics;

namespace Natsu.Tests.Core;

public class PaintableElementTests {
    [Fact]
    public void TestColorProperty() {
        PaintableElement? element = new();
        Color? color = new() { R = 255, G = 0, B = 255, A = 0 };

        element.Color = color;

        color.R = 0;
        color.G = 255;
        color.B = 0;
        color.A = 255;

        Assert.Equal(color.R, element.Color.R);
        Assert.Equal(color.G, element.Color.G);
        Assert.Equal(color.B, element.Color.B);
        Assert.Equal(color.A, element.Color.A);
    }

    [Fact]
    public void TestDoPaintValueChangeEvent() {
        PaintableElement? element = new();
        bool eventTriggered = false;

        element.DoPaintValueChange += () => eventTriggered = true;
        element.Paint.Color = new() { R = 255, G = 0, B = 0, A = 128 };

        Assert.True(eventTriggered);
    }

    [Fact]
    public void TestOpacityValueChangeEvent() {
        PaintableElement? element = new();
        bool eventTriggered = false;

        element.DoPaintValueChange += () => eventTriggered = true;
        element.Opacity = 0.5f;

        Assert.True(eventTriggered);

        eventTriggered = false;
        element.Color.R = 255;

        Assert.True(eventTriggered);
    }
}
