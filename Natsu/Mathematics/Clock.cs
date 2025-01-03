#nullable disable

namespace Natsu.Mathematics;

public class Clock {
    public double TimeScale { get; set; } = 1;

    public double DeltaTime { get; private set; }
    public double Time { get; private set; }

    public double RawDeltaTime { get; private set; }
    public double RawTime { get; private set; }

    public long Ticks { get; private set; }
    public double TPS { get; private set; }

    public void Update(double time) {
        RawDeltaTime = time;
        RawTime += time;

        DeltaTime = RawDeltaTime * TimeScale;
        Time += DeltaTime;

        Ticks++;
        TPS = 1 / time;
    }
}
