namespace Natsu.Mathematics.Transforms;

/// <summary>
///     Interface for marking a class as being viable for <see cref="TransformSequence{T}" /> extensinos
/// </summary>
public interface ITransformable {
    public void AddTransformSequence(ITransformSequence sequence);
}
