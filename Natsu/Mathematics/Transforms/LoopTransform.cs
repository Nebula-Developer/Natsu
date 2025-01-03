namespace Natsu.Mathematics.Transforms;

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

        if (Times == -1 || Count++ < Times) sequence.ResetTo(Point);
    }

    protected override void OnSetProgress(double progress, TransformSequence? sequence) => Loop(sequence);

    public override void Reset(TransformSequence? sequence = null) {
        base.Reset(sequence);
        if (sequence == null || sequence.Time > StartTime + sequence.DeltaTime) Count = 0;
    }
}
