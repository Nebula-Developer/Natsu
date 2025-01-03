namespace Natsu.Mathematics.Transforms;

public class TransformSequence {
    private readonly List<Transform> _affectedElements = new();

    public TransformSequence(string name) => Name = name;

    public List<Transform> Sequence { get; } = new();
    public double BaseTime { get; set; }
    public double EndTime => Math.Max(BaseTime, Sequence.Max(t => t.EndTime));
    public double Time { get; protected set; }
    public double DeltaTime { get; protected set; }

    public string Name { get; set; } = "";

    public bool IsComplete => Time >= EndTime;

    public bool Stopped { get; set; }

    public Dictionary<int, double> LoopPoints { get; } = new() { { 0, 0 } };

    public void Update(double dt) {
        Time += dt;
        DeltaTime = dt;
        if (Time > EndTime) Time = EndTime;

        Update();
    }

    public void Stop() => Stopped = true;

    public void Update() {
        foreach (Transform? transform in Sequence) {
            if (transform.Duration == 0 && Time >= transform.StartTime) {
                if (transform.Performed) continue;

                transform.Performed = true;
                transform.SetProgress(1, this);
                continue;
            }

            if (_affectedElements.Contains(transform) && Time > transform.EndTime) {
                _affectedElements.Remove(transform);
                transform.Performed = true;
                transform.SetProgress(1, this);
            } else if (Time >= transform.StartTime && (Time <= transform.EndTime || !transform.Performed)) {
                if (!_affectedElements.Contains(transform)) _affectedElements.Add(transform);

                double progress = (Time - transform.StartTime) / transform.Duration;

                if (progress <= 0) {
                    progress = 0;
                } else if (progress >= 1) {
                    progress = 1;
                    _affectedElements.Remove(transform);
                }

                transform.SetProgress(progress, this);
            }
        }
    }

    public void Reset() {
        foreach (Transform? transform in Sequence) {
            transform.Performed = false;
            if (transform.Duration > 0) transform.Reset(this);
        }

        Time = 0;
    }

    public void ResetTo(int point) {
        foreach (Transform? transform in Sequence)
            if (transform.StartTime > LoopPoints[point])
                transform.Reset(this);

        Time = LoopPoints[point] + Precision.Epsilon;
    }
}
