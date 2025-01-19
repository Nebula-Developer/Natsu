namespace Natsu.Mathematics.Transforms;

public class LoopTransform : ITransform {
    private int _loopCount = -1;

    /// <summary>
    ///     The amount of times the loop should be performed.
    ///     <br />
    ///     If set to -1, the loop will be performed indefinitely.
    /// </summary>
    public int LoopCount {
        get => _loopCount;
        set {
            _loopCount = value;
            RemainingLoops = value;
        }
    }

    /// <summary>
    ///     The remaining amount of loops to perform.
    /// </summary>
    public int RemainingLoops { get; set; }

    /// <summary>
    ///     The point in time to reset to.
    /// </summary>
    public float LoopTime { get; set; }

    public int Index { get; set; }

    public string Name => "Loop";

    public ITransformSequence Sequence { get; set; } = null!;

    public float StartTime { get; set; }

    public float Duration {
        get => 0;
        set { }
    }

    public Easing Easing { get; set; } = Easings.Linear;

    public bool IsCompleted {
        get => RemainingLoops == 0;
        set { }
    }

    public void Seek(float time) { }

    public void Complete() {
        RemainingLoops--;
        Sequence.ResetTo(LoopTime, Index);
    }

    public void Reset(bool seek = true) => RemainingLoops = LoopCount;
}
