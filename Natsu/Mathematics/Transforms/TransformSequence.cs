namespace Natsu.Mathematics.Transforms;

public class TransformSequence<T>(T target) : ITransformSequence<T> {
    /// <summary>
    ///     Returns the end time of the sequence.
    /// </summary>
    public float EndTime => Transforms.Max(t => t.StartTime + t.Duration);

    /// <summary>
    ///     Dictionary used to store additional data for future use.
    /// </summary>
    public Dictionary<string, object> FutureData { get; } = new();

    public List<ITransform> Transforms { get; } = new();

    public bool IsCompleted => Transforms.All(t => t.IsCompleted);

    public float BaseTime { get; set; }

    public T Target { get; } = target;

    public float Time { get; private set; }

    public Dictionary<int, float> LoopPoints { get; } = new() {
        [0] = 0
    };

    public string Name { get; set; } = "";

    public void Update(float time) {
        Time += time;
        foreach (ITransform transform in Transforms) {
            float endTime = transform.StartTime + transform.Duration;
            float startTime = transform.StartTime;

            if (Time < endTime && Time >= startTime) {
                float progress = (Time - startTime) / transform.Duration;
                transform.Seek(progress);
            } else if (Time >= endTime && !transform.IsCompleted) {
                transform.Complete();
            }
        }
    }

    public void ResetTo(float toTime, int index = -1) {
        float fromTime = Time;
        Time = toTime;

        HashSet<string> valueResetNames = new();

        for (int i = 0; i < (index == -1 ? Transforms.Count : index); i++) {
            ITransform transform = Transforms[i];
            if (transform.StartTime >= toTime && transform.StartTime < fromTime) {
                if (!valueResetNames.Contains(transform.Name)) {
                    transform.Reset();
                    valueResetNames.Add(transform.Name);
                } else {
                    transform.Reset(false);
                }
            } else if (transform.StartTime < toTime && transform.StartTime + transform.Duration >= toTime) {
                float progress = (toTime - transform.StartTime) / transform.Duration;
                transform.Reset(false);
                transform.Seek(progress);
            }
        }
    }

    public void Reset() => ResetTo(0);

    /// <summary>
    ///     Append a new transform to the sequence.
    /// </summary>
    /// <param name="transform">The transform to append</param>
    /// <param name="applyBaseTime">
    ///     Whether to apply this sequence's <see cref="BaseTime" /> to the transform's
    ///     <see cref="ITransform.StartTime" />
    /// </param>
    /// <returns>The sequence itself, for chaining</returns>
    public TransformSequence<T> Append(ITransform transform, bool applyBaseTime = true) {
        transform.Sequence = this;
        transform.StartTime += applyBaseTime ? BaseTime : 0;

        Transforms.Add(transform);
        transform.Index = Transforms.Count - 1;

        return this;
    }

    /// <summary>
    ///     Moves the <see cref="BaseTime" /> of the sequence to the end of the longest transform.
    /// </summary>
    /// <param name="delay">The delay to add to the end time</param>
    /// <returns>The sequence itself, for chaining</returns>
    public TransformSequence<T> Then(float delay = 0) {
        BaseTime = EndTime + delay;
        return this;
    }

    /// <summary>
    ///     Appends a <see cref="LoopTransform" /> to the sequence.
    /// </summary>
    /// <param name="loopCount">The amount of times the loop should be performed</param>
    /// <param name="loopPoint">The loop point to reset to</param>
    /// <returns>The sequence itself, for chaining</returns>
    public TransformSequence<T> Loop(int loopCount = -1, int loopPoint = 0) {
        if (!LoopPoints.ContainsKey(loopPoint)) throw new InvalidOperationException("Provided loop point does not exist. Set it using SetLoopPoint() first.");

        Then();

        LoopTransform loop = new() {
            LoopCount = loopCount,
            LoopTime = LoopPoints[loopPoint]
        };

        loop.Sequence = this;
        loop.StartTime = BaseTime;

        Transforms.Add(loop);
        loop.Index = Transforms.Count - 1;

        return this;
    }

    /// <summary>
    ///     Sets a loop point at the current time.
    /// </summary>
    /// <param name="point">The loop point to set</param>
    /// <returns>The sequence itself, for chaining</returns>
    public TransformSequence<T> SetLoopPoint(int point) {
        LoopPoints[point] = BaseTime;
        return this;
    }
}
