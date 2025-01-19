namespace Natsu.Utils.Logging;

public class ConsoleLoggerOutput : ILoggerOutput {
    public LogLevel MinimumLevel { get; set; } = LogLevel.Debug;
    public bool Enabled { get; set; } = true;

    public void Log(LogLevel level, string message) {
        if (!Enabled || level < MinimumLevel) return;

        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToString().ToUpper()}] {message}");
    }
}
