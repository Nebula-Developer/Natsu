namespace Natsu.Mathematics.Transforms;

/// <summary>
///     Interface for marking a class as being viable for <see cref="TransformSequence{T}" /> extensinos
/// </summary>
public interface ITransformable {
    /// <summary>
    ///     Adds a transform sequence to the transformable.
    /// </summary>
    /// <param name="sequence">The sequence to add</param>
    public void AddTransformSequence(ITransformSequence sequence);

    /// <summary>
    ///     Stops all transform sequences.
    /// </summary>
    public void StopTransformSequences();

    /// <summary>
    ///     Stops transform sequences by their name.
    /// </summary>
    /// <param name="name">The name of the sequence(s) to stop</param>
    public void StopTransformSequences(params string[] name);

    /// <summary>
    ///     Stops a transform sequence.
    /// </summary>
    /// <param name="sequence">The sequence to stop</param>
    public void StopTransformSequence(ITransformSequence sequence);

    /// <summary>
    ///     Updates all transform sequences.
    /// </summary>
    /// <param name="time">The time since the last update</param>
    public void UpdateTransformSequences(double time);
}
