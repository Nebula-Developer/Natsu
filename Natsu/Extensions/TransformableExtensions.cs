using Natsu.Core;
using Natsu.Mathematics.Transforms;

namespace Natsu.Extensions;

public static class TransformableExtensions {
    public static TransformSequence<T> AppendToTransformable<T>(this TransformSequence<T> sequence, Element elm) where T : ITransformable {
        elm.AddTransformSequence(sequence);
        return sequence;
    }

    public static TransformSequence<T> Begin<T>(this T transformable, string name) where T : ITransformable => new(transformable) { Name = name };

    public static TransformSequence<T> After<T>(this T transformable, float delay = 0) where T : ITransformable => new TransformSequence<T>(transformable).Then(delay);
}
