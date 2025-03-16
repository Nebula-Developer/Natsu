using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Natsu.Mathematics.Transforms;

public class TransformSequence<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(T target) : ITransformSequence<T> {
    /// <summary>
    ///     Returns the end time of the sequence.
    /// </summary>
    public float EndTime => Transforms.Count > 0 ? Transforms.Max(t => t.StartTime + t.Duration) : 0;

    /// <summary>
    ///     Dictionary used to store additional data for future use.
    /// </summary>
    public Dictionary<string, object> FutureData { get; } = new();

    /// <summary>
    ///     The collective time of all the <see cref="Then" /> delays.
    ///     <br />
    ///     Used for calculating the <see cref="BaseTime" /> when there are no actual transforms.
    /// </summary>
    public float BaseDelayTime { get; set; }

    public List<ITransform> Transforms { get; } = new();

    public bool IsCompleted => Transforms.All(t => t.IsCompleted);

    public virtual float BaseTime { get; set; }

    public T Target { get; } = target;

    public float Time { get; private set; }

    public Dictionary<int, LoopPoint> LoopPoints { get; } = new() {
        [0] = new(0, 0)
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
                transform.Seek(1);
                transform.Complete(Time - endTime);
            }
        }
    }

    public void ResetTo(float toTime, int startIndex = 0, int endIndex = -1) {
        float fromTime = Time;
        Time = toTime;

        HashSet<string> valueResetNames = new();

        for (int i = startIndex; i < (endIndex == -1 ? Transforms.Count : endIndex); i++) {
            ITransform transform = Transforms[i];

            if (transform is LoopTransform loop && loop.StartTime > toTime && loop.StartTime <= fromTime) {
                loop.Reset();
                continue;
            }

            if (transform.StartTime >= toTime && transform.StartTime <= fromTime) {
                if (!valueResetNames.Contains(transform.Name)) {
                    transform.Reset();
                    valueResetNames.Add(transform.Name);
                } else {
                    transform.Reset(false);
                }

                transform.DoReset?.Invoke();
            } else if (transform.StartTime < toTime && transform.StartTime + transform.Duration >= toTime) {
                float progress = (toTime - transform.StartTime) / transform.Duration;
                transform.Reset(false);
                transform.Seek(progress);

                transform.DoReset?.Invoke();
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
    ///     Creates a basic transform.
    /// </summary>
    /// <param name="property">The name of the property to transform</param>
    /// <param name="to">The value to transform to</param>
    /// <param name="duration">The length of the transform</param>
    /// <param name="easing">The easing to apply to the time</param>
    /// <param name="name">The name of the transform for identification</param>
    /// <param name="interpolation">The interpolation function to use</param>
    /// <returns>The created sequence</returns>
    public TransformSequence<T> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] X>(string property, X to, float duration, Easing easing, string? name, Func<X, X, float, X> interpolation) {
        PropertyInfo? propertyInfo = typeof(T).GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (propertyInfo == null) throw new InvalidOperationException($"Property {property} does not exist in {typeof(T).Name}");

        X a = FutureData.TryGetValue(property, out object? value) ? (X)value : (X)propertyInfo.GetValue(Target)!;

        Transform transform = new(t => {
            X value = interpolation(a, to, t);
            propertyInfo.SetValue(Target, value);
        }) {
            Name = name ?? property,
            Duration = duration,
            Easing = EasingHelper.FromEaseType(easing)
        };

        FutureData[property] = to!;
        if (Name == string.Empty) Name = property;

        return Append(transform);
    }

    /// <summary>
    ///     Creates a basic transform, using a helper for determining the interpolation.
    /// </summary>
    /// <param name="property">The name of the property to transform</param>
    /// <param name="to">The value to transform to</param>
    /// <param name="duration">The length of the transform</param>
    /// <param name="easing">The easing to apply to the time</param>
    /// <param name="name">The name of the transform for identification</param>
    /// <returns>The created sequence</returns>
    public TransformSequence<T> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] X>(string property, X to, float duration, Easing easing = Easing.Linear, string? name = null) => Create(property, to, duration, easing, name, EasingHelper.GetInterpolation<X>());

    /// <summary>
    ///     Moves the <see cref="BaseTime" /> of the sequence to the end of the longest transform.
    /// </summary>
    /// <param name="delay">The delay to add to the end time</param>
    /// <returns>The sequence itself, for chaining</returns>
    public TransformSequence<T> Then(float delay = 0) {
        BaseTime = Math.Max(EndTime, BaseDelayTime) + delay;
        BaseDelayTime += delay;
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
            LoopPoint = LoopPoints[loopPoint]
        };

        loop.Sequence = this;
        loop.StartTime = BaseTime;
        loop.Duration = 0;

        Transforms.Add(loop);
        loop.Index = Transforms.Count - 1;

        return this;
    }

    /// <summary>
    ///     Sets a loop point at the current time.
    /// </summary>
    /// <param name="point">The loop point to set</param>
    /// <param name="withStartTime">Whether to set the start time of the loop point to the current time</param>
    /// <returns>The sequence itself, for chaining</returns>
    public TransformSequence<T> SetLoopPoint(int point, bool withStartTime = true) {
        LoopPoints[point] = new(BaseTime, withStartTime ? Transforms.Count + 1 : 0);
        return this;
    }

    /// <summary>
    ///     Performs an action in the sequence.
    /// </summary>
    /// <param name="action">The action to perform</param>
    /// <param name="reset">The action to perform when resetting the sequence</param>
    /// <param name="name">The name of the action for identification</param>
    /// <returns>The sequence itself, for chaining</returns>
    public TransformSequence<T> Do(Action action, Action? reset = null, string? name = null) {
        Transform transform = new(t => action()) {
            Name = name ?? "Action",
            Duration = 0,
            DoReset = reset
        };

        return Append(transform);
    }
}
