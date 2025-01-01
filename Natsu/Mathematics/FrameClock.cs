#nullable disable

using System.Diagnostics;

namespace Natsu.Mathematics;

public class FrameClock {
    private readonly Stopwatch _stopwatch = new();
    public double DeltaTime { get; private set; }
    public double TotalTime { get; private set; }
    public int Frames { get; private set; }
    public int FPS { get; private set; }

    public void Start() => _stopwatch.Start();

    public void Stop() => _stopwatch.Stop();

    public void Update() {
        if (!_stopwatch.IsRunning) _stopwatch.Start();

        DeltaTime = _stopwatch.Elapsed.TotalSeconds;
        TotalTime += DeltaTime;
        Frames++;
        FPS = (int)(1 / DeltaTime);
        _stopwatch.Restart();
    }
}
