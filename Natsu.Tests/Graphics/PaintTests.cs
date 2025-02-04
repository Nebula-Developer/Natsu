using Natsu.Graphics;

namespace Natsu.Tests.Graphics;

public class PaintTests {
    [Fact]
    public void TestColorProperty() {
        Paint? paint = new();
        bool eventTriggered = false;
        paint.DoChange += () => eventTriggered = true;

        Color newColor = Colors.Red;
        paint.Color = newColor;

        Assert.Equal(newColor, paint.Color);
        Assert.True(eventTriggered);
    }

    [Fact]
    public void TestPropertyChangeEvent() {
        Paint? paint = new();

        int eventCount = 0;
        paint.DoChange += () => eventCount++;

        paint.Color = Colors.Red;
        paint.StrokeWidth = 2;
        paint.IsStroke = true;
        paint.IsAntialias = true;

        Assert.Equal(4, eventCount);

        paint.FilterQuality = FilterQuality.High;
        paint.TextSize = 24;
        paint.StrokeCap = StrokeCap.Round;
        paint.StrokeJoin = StrokeJoin.Round;

        Assert.Equal(8, eventCount);
    }

    [Fact]
    public void TestColorImmutability() {
        Paint? paint = new();
        paint.Color = Colors.White;
        Color originalColor = paint.Color;

        paint.Color.R = 255;
        paint.Color.G = 0;
        paint.Color.B = 0;

        Assert.Equal(originalColor, paint.Color);
        Assert.Equal(Colors.Red, paint.Color);

        originalColor.R = 0;
        originalColor.G = 255;
        originalColor.B = 0;

        Assert.Equal(originalColor, paint.Color);
        Assert.Equal(Colors.Green, paint.Color);

        paint.Color.Become(Colors.Green);

        Assert.Equal(Colors.Green, originalColor);

        paint.Color = Colors.Blue;

        Assert.Equal(Colors.Blue, paint.Color);
        Assert.Equal(Colors.Green, originalColor);
    }
}
