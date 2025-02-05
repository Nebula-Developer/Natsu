#nullable disable

namespace Natsu.Mathematics;

/// <summary>
///     A delta-time based clock that is used for time keeping.
/// </summary>
public class Clock {
    /// <summary>
    ///     The time scale of the clock.
    ///     <br />
    ///     This is used to scale the delta time, and can slow down or speed up the clock.
    ///     <br />
    ///     For bypassing the time scale, use <see cref="RawDeltaTime" /> or <see cref="RawTime" />.
    /// </summary>
    public double TimeScale { get; set; } = 1;

    /// <summary>
    ///     The delta time from the last call to <see cref="Update" />.
    /// </summary>
    public double DeltaTime { get; private set; }

    /// <summary>
    ///     The total time of the clock.
    /// </summary>
    public double Time { get; private set; }

    /// <summary>
    ///     The raw delta time from the last call to <see cref="Update" />.
    /// </summary>
    public double RawDeltaTime { get; private set; }

    /// <summary>
    ///     The raw total time of the clock.
    /// </summary>
    public double RawTime { get; private set; }

    /// <summary>
    ///     The amount of calls to <see cref="Update" /> since the clock was created.
    /// </summary>
    public long Ticks { get; private set; }

    /// <summary>
    ///     The ticks per second of the clock.
    ///     <br />
    ///     Can be used to determine the frames per second if used for rendering purposes.
    /// </summary>
    public double TPS { get; private set; }

    /// <summary>
    ///     Updates the clock with the given time.
    /// </summary>
    /// <param name="time">The time to update the clock with</param>
    public void Update(double time) {
        RawDeltaTime = time;
        RawTime += time;

        DeltaTime = RawDeltaTime * TimeScale;
        Time += DeltaTime;

        Ticks++;
        TPS = 1 / time;
    }
}
