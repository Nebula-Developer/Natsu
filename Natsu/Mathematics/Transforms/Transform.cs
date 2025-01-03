namespace Natsu.Mathematics.Transforms;

public class Transform {
    public Transform(Action<double> setter, double duration, double startTime = 0) {
        if (setter == null) throw new ArgumentNullException(nameof(setter));

        if (duration < 0) throw new ArgumentOutOfRangeException(nameof(duration), "Transform duration cannot be negative.");

        if (startTime < 0) throw new ArgumentOutOfRangeException(nameof(startTime), "Transform start time cannot be negative.");

        Setter = setter;
        StartTime = startTime;
        Duration = duration;
    }

    public double StartTime { get; set; }
    public double EndTime { get; set; }

    public double Duration {
        get => EndTime - StartTime;
        set => EndTime = StartTime + value;
    }

    public bool Performed { get; set; }

    public Easing Ease { get; set; } = Easings.Linear;

    public Action<double> Setter { get; set; }

    public void SetProgress(double progress, TransformSequence? sequence = null) {
        progress = Math.Clamp(progress, 0, 1);
        Setter(Ease(progress));
        OnSetProgress(progress, sequence);
    }

    public virtual void Reset(TransformSequence? sequence = null) => Performed = false;

    protected virtual void OnSetProgress(double progress, TransformSequence? sequence = null) { }
}
