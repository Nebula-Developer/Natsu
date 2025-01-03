namespace Natsu.Mathematics.Transforms;

public class TransformSequence<T> : TransformSequence where T : class {
    public TransformSequence(T target, string name) : base(name) => Target = target;

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

    public TransformSequence<T> Loop(double delay = 0, int times = -1, int point = 0) {
        if (!LoopPoints.ContainsKey(point)) throw new ArgumentOutOfRangeException(nameof(point), "Loop point does not exist.");

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
