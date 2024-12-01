namespace Natsu.Mathematics;

public enum TransformLoopMode {
    None,
    Forward,
    PingPong,
    Backward
}

public enum TransformDirection {
    Forward,
    Backward
}

/*
A transform sequence contains a list of transforms.
To sequence transforms one after the other, we have a baseTime
value which keeps track of the base start time for any new transforms.

If we do .Then(), the new baseTime becomes the slowest transform end time + any delay.
All progress is based on one time value, so looping or reversing is just a matter of
changing the manipulation of the time value (go in reverse, set to 0, etc).

We also keep track of the total end time of the sequence, so we can know when the sequence is complete.
*/

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

    public virtual void OnSetProgress(double progress, TransformSequence? sequence = null) { }
}

public class TransformSequence {

    private readonly List<Transform> _affectedElements = new();

    public TransformSequence(string name) {
        Name = name;
    }

    public List<Transform> Sequence { get; } = new();
    public double BaseTime { get; set; }
    public double EndTime => Math.Max(BaseTime, Sequence.Max(t => t.EndTime));
    public double Time { get; set; }
    public double DeltaTime { get; set; }

    public string Name { get; set; } = "";

    public bool IsComplete => Time >= EndTime;

    public bool Stopped { get; set; }

    public Dictionary<int, double> LoopPoints { get; } = new() {
        { 0, 0 }
    };

    public void Update(double dt) {
        Time += dt;
        DeltaTime = dt;
        if (Time > EndTime) Time = EndTime;
        Update();
    }

    public void Stop() => Stopped = true;

    public void Update() {
        foreach (Transform transform in Sequence) {
            if (transform.Duration == 0 && Time >= transform.StartTime) {
                if (transform.Performed) continue;

                transform.Performed = true;
                transform.SetProgress(1, this);
                continue;
            }

            if (Time >= transform.StartTime && (Time <= transform.EndTime || !transform.Performed)) {
                if (!_affectedElements.Contains(transform))
                    _affectedElements.Add(transform);

                double progress = (Time - transform.StartTime) / transform.Duration;

                if (progress <= 0)
                    progress = 0;
                else if (progress >= 1) {
                    progress = 1;
                    _affectedElements.Remove(transform);
                }

                transform.SetProgress(progress, this);
            } else if (_affectedElements.Contains(transform)) {
                _affectedElements.Remove(transform);
                transform.Performed = true;
                transform.SetProgress(1, this);
            }
        }
    }

    public void Reset() {
        foreach (Transform transform in Sequence) {
            transform.Performed = false;
            if (transform.Duration > 0) transform.Reset(this);
        }

        Time = 0;
    }

    public void ResetTo(int point) {
        foreach (Transform transform in Sequence)
            if (transform.StartTime > LoopPoints[point])
                transform.Reset(this);
        Time = LoopPoints[point] + Precision.Epsilon;
    }
}

public class TransformSequence<T> : TransformSequence where T : class {

    public TransformSequence(T target, string name) : base(name) {
        Target = target;
    }

    public T Target { get; set; }

    /*
    Eg. If we want to scale from where we currently are to 2,2, then back to 1,1,
    we have to know where the value would be at the end of the first transform.
    This is where FutureData comes in. We can store the last future value of a value
    so we can use it in the next transform.
    */
    public Dictionary<string, object> FutureData { get; } = new();


    public TransformSequence<T> Then(float delay = 0) {
        BaseTime = EndTime + delay;
        return this;
    }

    public TransformSequence<T> Append(Transform transform) {
        Sequence.Add(transform);
        transform.StartTime += BaseTime;
        transform.EndTime += BaseTime;
        return this;
    }


    public TransformSequence<T> Loop(double delay, int times = -1, int point = 0) {
        if (!LoopPoints.ContainsKey(point))
            throw new ArgumentOutOfRangeException(nameof(point), "Loop point does not exist.");

        LoopTransform t = new(EndTime + delay, times, point);
        BaseTime = EndTime + delay;

        Sequence.Add(t);
        return this;
    }

    public TransformSequence<T> SetLoopPoint(int point) {
        LoopPoints[point] = EndTime + Precision.Epsilon;
        return this;
    }
}

public class LoopTransform : Transform {

    public LoopTransform(double startTime, int times = -1, int point = 0) : base(_ => { }, 0) {
        StartTime = startTime;
        Times = times;
        Point = point;
    }

    public int Times { get; set; }
    public int Count { get; set; }
    public int Point { get; set; }

    public void Loop(TransformSequence? sequence) {
        if (sequence == null) return;

        if (Times == -1 || Count++ < Times)
            sequence.ResetTo(Point);
    }

    public override void OnSetProgress(double progress, TransformSequence? sequence) => Loop(sequence);

    public override void Reset(TransformSequence? sequence = null) {
        base.Reset(sequence);
        if (sequence?.Time > StartTime + sequence?.DeltaTime)
            Count = 0;
    }
}
