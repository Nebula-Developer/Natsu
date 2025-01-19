namespace Natsu.Mathematics.Transforms;

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
    public Dictionary<int, float> LoopPoints { get; }

    /// <summary>
    ///     Updates the sequence by the given time.
    /// </summary>
    /// <param name="time">The time to update the sequence by</param>
    public void Update(float time);

    /// <summary>
    ///     Resets the sequence to the given time.
    /// </summary>
    /// <param name="time">The time to reset to</param>
    /// <param name="index">Which index to prevent further resetting</param>
    public void ResetTo(float time, int index = -1);

    /// <summary>
    ///     Resets the sequence to the start.
    /// </summary>
    public void Reset();
}
