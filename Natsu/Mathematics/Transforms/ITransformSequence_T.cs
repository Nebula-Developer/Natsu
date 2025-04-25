namespace Natsu.Mathematics.Transforms;

/// <summary>
///     A sequence of transforms that are often used to perform animations on elements.
///     <br />
///     This is a type-specific version of <see cref="ITransformSequence" />.
/// </summary>
/// <typeparam name="T">The type of the target object that the transforms are applied to</typeparam>
public interface ITransformSequence<[DynamicProperty(DynamicProperties.Accessible)] T> : ITransformSequence {
    /// <summary>
    ///     The <typeparamref name="T" /> that the transforms are applied to.
    /// </summary>
    public T Target { get; }
}
