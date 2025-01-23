using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Natsu.Mathematics.Transforms;
using Natsu.Utils.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Natsu.Mathematics.Transforms.Tests;

public class TransformSequenceTests {
    ITestOutputHelper Logger { get; }

    public TransformSequenceTests(ITestOutputHelper logger) {
        Logger = logger;
    }

    private class MockTransform : ITransform {
        public float StartTime { get; set; }
        public float Duration { get; set; }
        public float Progress { get; private set; }
        public bool IsCompleted { get; private set; }
        public string Name => "MockTransform";
        public int Index { get; set; }
        public ITransformSequence Sequence { get; set; } = null!;
        public EaseFunction Easing { get; set; } = EasingHelper.Linear;
        bool ITransform.IsCompleted { get => IsCompleted; set => IsCompleted = value; }

        public void Seek(float progress) => Progress = progress;

        public void Complete() {
            IsCompleted = true;
        }

        public void Reset(bool resetValues = true) {
            IsCompleted = false;
        }
    }

    [Fact]
    public void TestEndTime() {
        var sequence = new TransformSequence<object>(new object());
        sequence.Append(new MockTransform { StartTime = 0, Duration = 5 });
        sequence.Append(new MockTransform { StartTime = 3, Duration = 7 });

        Assert.Equal(10, sequence.EndTime);
    }

    [Fact]
    public void TestIsCompleted() {
        var sequence = new TransformSequence<object>(new object());
        var transform1 = new MockTransform { StartTime = 0, Duration = 5 };
        var transform2 = new MockTransform { StartTime = 3, Duration = 7 };

        sequence.Append(transform1);
        sequence.Append(transform2);

        Assert.False(sequence.IsCompleted);

        transform1.Complete();
        transform2.Complete();

        Assert.True(sequence.IsCompleted);
    }

    [Fact]
    public void TestUpdate() {
        var sequence = new TransformSequence<object>(new object());
        var transform = new MockTransform { StartTime = 0, Duration = 5 };

        sequence.Append(transform);
        sequence.Update(3);

        Assert.False(transform.IsCompleted);

        sequence.Update(3);

        Assert.True(transform.IsCompleted);
    }

    [Fact]
    public void TestResetTo() {
        var sequence = new TransformSequence<object>(new object());
        var transform = new MockTransform { StartTime = 0, Duration = 5 };

        sequence.Append(transform);
        sequence.Update(5);
        sequence.ResetTo(3);

        Assert.False(transform.IsCompleted);
        Assert.Equal(3f/5f, transform.Progress, .01f);
    }

    [Fact]
    public void TestAppend() {
        var sequence = new TransformSequence<object>(new object());
        var transform = new MockTransform { StartTime = 0, Duration = 5 };

        sequence.Append(transform);

        Assert.Single(sequence.Transforms);
        Assert.Equal(sequence, transform.Sequence);
    }

    [Fact]
    public void TestThen() {
        var sequence = new TransformSequence<object>(new object());
        var transform = new MockTransform { StartTime = 0, Duration = 5 };

        sequence.Append(transform);
        sequence.Then(2);

        Assert.Equal(7, sequence.BaseTime);
    }

    [Fact]
    public void TestLoop() {
        var sequence = new TransformSequence<object>(new object());
        var transform = new MockTransform { StartTime = 0, Duration = 1 };
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
        var sequence = new TransformSequence<object>(new object());
        
        sequence.Then(1);
        sequence.SetLoopPoint(3); // After 6 seconds, we will be here (1s).
        sequence.Then(5);
        sequence.Loop(3, 3);

        Assert.Equal(1, sequence.LoopPoints[3].Time);
        Assert.Equal(1, sequence.LoopPoints[3].Index);

        sequence.Update(6f);
        Assert.Equal(1f, sequence.Time, .01f);
    }

    public static IEnumerable<object[]> GetEaseValues() =>
        Enum.GetValues(typeof(EaseType))
            .Cast<EaseType>()
            .Select(e => new object[] { e });

    [Theory]
    [MemberData(nameof(GetEaseValues))]
    public void TestEasing(EaseType ease) {
        var sequence = new TransformSequence<object>(new object());
        var easing = EasingHelper.FromEaseType(ease);

        float progress = 0;
        var transform = new Transform((t) => {
            progress = t;
        }) { StartTime = 0, Duration = 10, Easing = easing, Name = "TestEasingsTransform" };

        sequence.Append(transform);

        for (int i = 1; i < 10; i++) {
            sequence.Update(1);
            Assert.Equal(easing(i / 10f), progress, .01f);
        }

        sequence.Update(10);
        Assert.Equal(1f, progress, .01f);
    }
}
