namespace Natsu.Mathematics.Transforms;

/// <summary>
///     A transform that can be appended to a <see cref="ITransformSequence" />.
/// </summary>
public interface ITransform {
    /// <summary>
    ///     The <see cref="ITransformSequence" /> that is handling this transform.
    /// </summary>
    public ITransformSequence Sequence { get; set; }

    /// <summary>
    ///     The index of the transform in the sequence.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    ///     The name used to identify the transform.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The time at which the transform starts.
    /// </summary>
    public float StartTime { get; set; }

    /// <summary>
    ///     The duration of the transform.
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    ///     The easing function of the transform.
    /// </summary>
    public EaseFunction Easing { get; set; }

    /// <summary>
    ///     Whether the transform has already been performed.
    ///     <br />
    ///     Used to tell an <see cref="ITransformSequence" /> whether it has already finished in the current progression.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    ///     Called when the transform has been reset.
    /// </summary>
    public Action? DoReset { get; set; }

    /// <summary>
    ///     Seeks the transform to a specific time.
    /// </summary>
    /// <param name="time">The time to seek to</param>
    public void Seek(float time);

    /// <summary>
    ///     Resets the transform's perform state, and optionally also seeks to the start of the transform.
    /// </summary>
    /// <param name="seek">Whether to seek to the start of the transform</param>
    public void Reset(bool seek = true) {
        if (seek) Seek(0);
        IsCompleted = false;
    }

    /// <summary>
    ///     Called when the transform has been completed.
    /// </summary>
    /// <param name="overtime">The amount of time the transform has been completed overtime</param>
    public void Complete(float overtime = 0) => IsCompleted = true;
}
