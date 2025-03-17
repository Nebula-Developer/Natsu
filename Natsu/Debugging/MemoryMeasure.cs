using System.Diagnostics;
using Natsu.Utils.Logging;

namespace Natsu.Debugging;

public class MemoryMeasure {
    private readonly string _name;
    private readonly long _startMemory;
    private readonly long _startMemoryDelta;
    private readonly Stopwatch _stopwatch;

    public MemoryMeasure(string? name = null) {
        _name = name ?? "MemoryMeasure " + Guid.NewGuid().ToString()[..8];
        _startMemory = GC.GetTotalMemory(true);
        _startMemoryDelta = Process.GetCurrentProcess().PrivateMemorySize64;
        _stopwatch = Stopwatch.StartNew();
    }

    public MemoryMeasure Stop(bool log = true) {
        _stopwatch.Stop();
        long endMemory = GC.GetTotalMemory(true);
        long endMemoryDelta = Process.GetCurrentProcess().PrivateMemorySize64;
        long elapsedMs = _stopwatch.ElapsedMilliseconds;

        if (log) Logging.Log($"{_name} took {elapsedMs}ms, {endMemory - _startMemory} bytes GC, {endMemoryDelta - _startMemoryDelta} bytes private.");

        return this;
    }
}
