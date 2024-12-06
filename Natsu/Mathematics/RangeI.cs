namespace Natsu.Mathematics;

public struct RangeI {
    public int Start { get; }
    public int End { get; }

    public RangeI(int start, int end) {
        Start = start;
        End = end;
    }

    public static implicit operator RangeI(Range range) => new(range.Start.Value, range.End.Value);
    public static implicit operator Range(RangeI range) => new(range.Start, range.End);

    public int Length => End - Start;
}
