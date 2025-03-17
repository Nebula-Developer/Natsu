using System.Diagnostics;
using Natsu.Utils.Logging;

namespace Natsu.Debugging;

public class Timer {
    private readonly string _name;
    private readonly Stopwatch _stopwatch;

    public Timer(string? name = null, bool start = true) {
        _name = name ?? "Timer " + Guid.NewGuid().ToString().Substring(0, 8);
        _stopwatch = new();
        if (start) Start();
    }

    public Timer Stop(bool log = true) {
        _stopwatch.Stop();
        if (log) Logging.Debug($"{_name} took {_stopwatch.ElapsedMilliseconds}ms");
        return this;
    }

    public Timer Start() {
        _stopwatch.Start();
        return this;
    }
}
