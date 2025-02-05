using Natsu.Mathematics.Transforms;

namespace Natsu.Extensions;

public static class TransformableExtensions {
    public static TransformSequence<T> AppendToTransformable<T>(this TransformSequence<T> sequence, T elm) where T : ITransformable {
        elm.AddTransformSequence(sequence);
        return sequence;
    }

    /// <summary>
    ///     Starts a new <see cref="TransformSequence{T}" /> with the given name.
    /// </summary>
    /// <param name="transformable">The <see cref="ITransformable" /> to start the sequence on</param>
    /// <param name="name">The name of the sequence</param>
    /// <returns>The newly created <see cref="TransformSequence{T}" /></returns>
    public static TransformSequence<T> Begin<T>(this T transformable, string name) where T : ITransformable => new TransformSequence<T>(transformable) { Name = name }.AppendToTransformable(transformable);

    /// <summary>
    ///     Starts a new <see cref="TransformSequence{T}" /> with the given delay.
    /// </summary>
    /// <param name="transformable">The <see cref="ITransformable" /> to start the sequence on</param>
    /// <param name="delay">The delay before the sequence starts</param>
    /// <returns>The newly created <see cref="TransformSequence{T}" /></returns>
    public static TransformSequence<T> After<T>(this T transformable, float delay = 0, string name = "") where T : ITransformable => new TransformSequence<T>(transformable) { Name = name }.Then(delay).AppendToTransformable(transformable);
}
