
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
    public double StartTime { get; set; }
    public double EndTime { get; set; }
    public double Duration {
        get => EndTime - StartTime;
        set => EndTime = StartTime + value;
    }

    public bool Performed { get; set; } = false;

    public Easing Ease { get; set; } = Easings.Linear;

    public Action<double> Setter { get; set; }

    public void SetProgress(double progress) {
        progress = Math.Clamp(progress, 0, 1);
        Setter(Ease(progress));
    }

    public Transform(Action<double> setter, double duration, double startTime = 0) {
        if (setter == null) throw new ArgumentNullException(nameof(setter));
        if (duration < 0) throw new ArgumentOutOfRangeException(nameof(duration), "Transform duration cannot be negative.");
        if (startTime < 0) throw new ArgumentOutOfRangeException(nameof(startTime), "Transform start time cannot be negative.");

        Setter = setter;
        StartTime = startTime;
        Duration = duration;
    }
}

public class TransformSequence {
    public List<Transform> Sequence { get; } = new();
    public double BaseTime { get; set; } = 0;
    public double EndTime => Math.Max(BaseTime, Sequence.Max(t => t.EndTime));
    public double Time { get; set; } = 0;

    public string Name { get; set; } = "";

    public void Update(double dt) {
        Time += dt;
        if (Time > EndTime) Time = EndTime;
        Update();
    }

    private readonly List<Transform> _affectedElements = new();

    public bool IsComplete => Time >= EndTime;

    public bool Stopped { get; set; } = false;
    public void Stop() => Stopped = true;

    public void Update() {
        foreach (var transform in Sequence) {
            if (transform.Duration == 0 && Time >= transform.StartTime) {
                if (transform.Performed) continue;
                transform.Performed = true;
                transform.SetProgress(1);
                continue;
            }

            if (Time >= transform.StartTime && Time <= transform.EndTime) {
                if (!_affectedElements.Contains(transform))
                    _affectedElements.Add(transform);

                double progress = (Time - transform.StartTime) / transform.Duration;

                if (Precision.Approximately(progress, 0))
                    progress = 0;
                else if (Precision.Approximately(progress, 1)) {
                    progress = 1;
                    _affectedElements.Remove(transform);
                }

                transform.SetProgress(progress);
            } else if (_affectedElements.Contains(transform)) {
                _affectedElements.Remove(transform);
                transform.Performed = true;
                transform.SetProgress(1);
            }
        }
    }

    public TransformSequence(string name) {
        Name = name;
    }

    public Dictionary<int, double> LoopPoints { get; } = new() {
        {0, 0}
    };

    public void Reset() {
        Time = 0;
        foreach (var transform in Sequence) {
            transform.Performed = false;
            if (transform.Duration > 0)
                transform.SetProgress(0);
        }
    }

    public void ResetTo(int point) {
        Time = LoopPoints[point];
        foreach (var transform in Sequence) {
            if (Time > transform.StartTime) {
                transform.Performed = false;
                transform.SetProgress(0);
            }
        }
    }
}

public class TransformSequence<T> : TransformSequence where T : class {
    public T Target { get; set; }

    public TransformSequence(T target, string name) : base(name) {
        Target = target;
    }

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
        
        int count = 0;
        Transform t = new Transform(_ => {
            if (times == -1 || count++ < times)
                this.ResetTo(point);
        }, 0);
        t.StartTime = EndTime + delay;
        t.EndTime = EndTime + delay;
        BaseTime = EndTime + delay;
        Sequence.Add(t);
        return this;
    }

    public TransformSequence<T> SetLoopPoint(int point) {
        LoopPoints[point] = EndTime + Precision.Epsilon;
        return this;
    }
}
