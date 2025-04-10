namespace Natsu.Mathematics.Transforms;

/// <summary>
///     A transform that executes a setter function.
/// </summary>
public class Transform : ITransform {
    public Transform(Action<float> setter) => Setter = setter;

    public Action<float> Setter { get; }
    public Action? DoReset { get; set; }

    public ITransformSequence Sequence { get; set; } = null!;

    public required string Name { get; set; }

    public float StartTime { get; set; }

    public float Duration { get; set; }

    public int Index { get; set; }

    public EaseFunction Easing { get; set; } = EasingHelper.Linear;

    public bool IsCompleted { get; set; }

    public void Seek(float time) {
        float progress = (float)Easing(time);
        Setter(progress);
    }
}
