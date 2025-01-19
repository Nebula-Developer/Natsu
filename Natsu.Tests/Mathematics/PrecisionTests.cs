using Natsu.Mathematics;

namespace Natsu.Tests.Mathematics;

public class PrecisionTests {
    [Fact]
    public void TestApproximatelyFloat() {
        Assert.True(Precision.Approximately(1.0000001f, 1.0000002f));
        Assert.False(Precision.Approximately(1.0001f, 1.0002f));
    }

    [Fact]
    public void TestApproximatelyDouble() {
        Assert.True(Precision.Approximately(1.0000000001, 1.0000000002));
        Assert.False(Precision.Approximately(1.0001, 1.0002));
    }

    [Fact]
    public void TestApproximatelyVector2() {
        Vector2 a = new(1.0000001f, 2.0000001f);
        Vector2 b = new(1.0000002f, 2.0000002f);
        Vector2 c = new(1.0001f, 2.0001f);
        Assert.True(Precision.Approximately(a, b));
        Assert.False(Precision.Approximately(a, c));
    }

    [Fact]
    public void TestApproximatelyVector3() {
        Vector3 a = new(1.0000001f, 2.0000001f, 3.0000001f);
        Vector3 b = new(1.0000002f, 2.0000002f, 3.0000002f);
        Vector3 c = new(1.0001f, 2.0001f, 3.0001f);
        Assert.True(Precision.Approximately(a, b));
        Assert.False(Precision.Approximately(a, c));
    }

    [Fact]
    public void TestApproximatelyVector4() {
        Vector4 a = new(1.0000001f, 2.0000001f, 3.0000001f, 4.0000001f);
        Vector4 b = new(1.0000002f, 2.0000002f, 3.0000002f, 4.0000002f);
        Vector4 c = new(1.0001f, 2.0001f, 3.0001f, 4.0001f);
        Assert.True(Precision.Approximately(a, b));
        Assert.False(Precision.Approximately(a, c));
    }
}
