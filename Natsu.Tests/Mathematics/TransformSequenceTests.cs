using Xunit.Abstractions;

namespace Natsu.Mathematics.Transforms.Tests;

public class TransformSequenceTests {
    public TransformSequenceTests(ITestOutputHelper logger) => Logger = logger;
    private ITestOutputHelper Logger { get; }

    [Fact]
    public void TestEndTime() {
        TransformSequence<object>? sequence = new(new());
        sequence.Append(new MockTransform { StartTime = 0, Duration = 5 });
        sequence.Append(new MockTransform { StartTime = 3, Duration = 7 });

        Assert.Equal(10, sequence.EndTime);
    }

    [Fact]
    public void TestIsCompleted() {
        TransformSequence<object>? sequence = new(new());
        MockTransform? transform1 = new() { StartTime = 0, Duration = 5 };
        MockTransform? transform2 = new() { StartTime = 3, Duration = 7 };

        sequence.Append(transform1);
        sequence.Append(transform2);

        Assert.False(sequence.IsCompleted);

        transform1.Complete();
        transform2.Complete();

        Assert.True(sequence.IsCompleted);
    }

    [Fact]
    public void TestUpdate() {
        TransformSequence<object>? sequence = new(new());
        MockTransform? transform = new() { StartTime = 0, Duration = 5 };

        sequence.Append(transform);
        sequence.Update(3);

        Assert.False(transform.IsCompleted);

        sequence.Update(3);

        Assert.True(transform.IsCompleted);
    }

    [Fact]
    public void TestResetTo() {
        TransformSequence<object>? sequence = new(new());
        MockTransform? transform = new() { StartTime = 0, Duration = 5 };

        sequence.Append(transform);
        sequence.Update(5);
        sequence.ResetTo(3);

        Assert.False(transform.IsCompleted);
        Assert.Equal(3f / 5f, transform.Progress, .01f);
    }

    [Fact]
    public void TestAppend() {
        TransformSequence<object>? sequence = new(new());
        MockTransform? transform = new() { StartTime = 0, Duration = 5 };

        sequence.Append(transform);

        Assert.Single(sequence.Transforms);
        Assert.Equal(sequence, transform.Sequence);
    }

    [Fact]
    public void TestThen() {
        TransformSequence<object>? sequence = new(new());
        MockTransform? transform = new() { StartTime = 0, Duration = 5 };

        sequence.Append(transform);
        sequence.Then(2);

        Assert.Equal(7, sequence.BaseTime);
    }

    [Fact]
    public void TestLoop() {
        TransformSequence<object>? sequence = new(new());
        MockTransform? transform = new() { StartTime = 0, Duration = 1 };
        sequence.Append(transform);
        sequence.Loop(3);

        Assert.IsType<LoopTransform>(sequence.Transforms[1]);

        sequence.Update(1.1f); // First loop, 0.1s overtime
        Assert.Equal(0.1f, sequence.Time, .01f);
        sequence.Update(0.9f); // Second loop, 0s overtime
        Assert.Equal(0f, sequence.Time, .01f);
        sequence.Update(2f); // Third loop, 1s overtime
        Assert.Equal(1f, sequence.Time, .01f);

        sequence.Reset();
        sequence.Update(4f);
        Assert.Equal(1f, sequence.Time, .01f);
    }

    [Fact]
    public void TestSetLoopPoint() {
        TransformSequence<object>? sequence = new(new());

        sequence.Then(1);
        sequence.SetLoopPoint(3); // After 6 seconds, we will be here (1s).
        sequence.Then(5);
        sequence.Loop(3, 3);

        Assert.Equal(1, sequence.LoopPoints[3].Time);
        Assert.Equal(1, sequence.LoopPoints[3].Index);

        sequence.Update(6f);
        Assert.Equal(1f, sequence.Time, .01f);
    }

    public static IEnumerable<object[]> GetEaseValues() => Enum.GetValues(typeof(Easing)).Cast<Easing>().Select(e => new object[] { e });

    [Theory, MemberData(nameof(GetEaseValues))]
    public void TestEasing(Easing ease) {
        TransformSequence<object>? sequence = new(new());
        EaseFunction? easing = EasingHelper.FromEaseType(ease);

        float progress = 0;
        Transform? transform = new(t => { progress = t; }) { StartTime = 0, Duration = 10, Easing = easing, Name = "TestEasingsTransform" };

        sequence.Append(transform);

        for (int i = 1; i < 10; i++) {
            sequence.Update(1);
            Assert.Equal(easing(i / 10f), progress, .01f);
        }

        sequence.Update(10);
        Assert.Equal(1f, progress, .01f);
    }

    private class MockTransform : ITransform {
        public float Progress { get; private set; }
        public bool IsCompleted { get; private set; }
        public float StartTime { get; set; }
        public float Duration { get; set; }
        public string Name => "MockTransform";
        public int Index { get; set; }
        public ITransformSequence Sequence { get; set; } = null!;
        public EaseFunction Easing { get; set; } = EasingHelper.Linear;

        bool ITransform.IsCompleted {
            get => IsCompleted;
            set => IsCompleted = value;
        }

        public void Seek(float progress) => Progress = progress;

        public void Reset(bool resetValues = true) => IsCompleted = false;

        public void Complete() => IsCompleted = true;
    }
}
