using Natsu.Graphics.Shaders;
using Natsu.Mathematics;
using Natsu.Mathematics.Transforms;

namespace Natsu.Extensions;

public static class ShaderTransformExtensions {
    public static ITransformSequence<T> UniformTo<T, [DynamicProperty(DynamicProperties.Accessible)] V>(this T shader, string uniform, V to, double duration, Easing easing = Easing.Linear) where T : IShader where V : notnull {
        TransformSequence<T> sequence = new(shader) {
            Name = uniform
        };

        shader.AddTransformSequence(sequence);

        V? from;
        try {
            from = shader.GetUniform<V>(uniform);
        } catch {
            from = default;
        }

        Func<V, V, float, V> interpolation = EasingHelper.GetInterpolation<V>();

        sequence.Append(new Transform(t => { shader.SetUniform(uniform, interpolation(from!, to, t)); }) {
            Name = uniform,
            Easing = EasingHelper.FromEaseType(easing),
            Duration = (float)duration
        });

        return sequence;
    }
}
