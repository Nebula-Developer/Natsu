namespace Natsu.Mathematics.Transforms;

/// <summary>
///     A point in time that can be reset to.
///     <br />
///     Holds the time, and the index of the transform that it was created from.
/// </summary>
public struct LoopPoint {
    /// <summary>
    ///     The time of the loop point.
    /// </summary>
    public float Time { get; }

    /// <summary>
    ///     The index of the transform that the loop point was created from.
    /// </summary>
    public int Index { get; }

    /// <summary>
    ///     Creates a new loop point.
    /// </summary>
    /// <param name="time">The time of the loop point</param>
    /// <param name="index">The index of the transform that the loop point was created from</param>
    public LoopPoint(float time, int index) {
        Time = time;
        Index = index;
    }
}

/// <summary>
///     A sequence of transforms that are often used to perform animations on elements.
///     <br />
///     For targetting a spesific type, use <see cref="ITransformSequence{T}" />.
/// </summary>
public interface ITransformSequence {
    /// <summary>
    ///     The <see cref="ITransform" />s that are part of this sequence.
    /// </summary>
    public List<ITransform> Transforms { get; }

    /// <summary>
    ///     The name used to identify the sequence.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Whether every transform in the sequence has been completed.
    /// </summary>
    public bool IsCompleted { get; }

    /// <summary>
    ///     The base time of the sequence, which is used as a starting point for any new transforms.
    ///     <br />
    ///     This is often used to run sequences one after another.
    /// </summary>
    public float BaseTime { get; set; }

    /// <summary>
    ///     The current time of the sequence.
    /// </summary>
    public float Time { get; }

    /// <summary>
    ///     An index of points that can be reset to.
    /// </summary>
    public Dictionary<int, LoopPoint> LoopPoints { get; }

    /// <summary>
    ///     Updates the sequence by the given time.
    /// </summary>
    /// <param name="time">The time to update the sequence by</param>
    public void Update(float time);

    /// <summary>
    ///     Resets the sequence to the given time.
    /// </summary>
    /// <param name="time">The time to reset to</param>
    /// <param name="startIndex">The transform index to start resetting from</param>
    /// <param name="endIndex">The transform index to stop resetting at</param>
    public void ResetTo(float time, int startIndex = 0, int endIndex = -1);

    /// <summary>
    ///     Resets the sequence to the given loop point.
    /// </summary>
    /// <param name="loopPoint">The loop point to reset to</param>
    /// <param name="endIndex">The transform index to stop resetting at</param>
    public void ResetTo(LoopPoint loopPoint, int endIndex = -1) => ResetTo(loopPoint.Time, loopPoint.Index, endIndex);

    /// <summary>
    ///     Resets the sequence to the start.
    /// </summary>
    public void Reset() => ResetTo(0);
}
