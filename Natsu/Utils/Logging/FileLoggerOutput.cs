namespace Natsu.Utils.Logging;

public class FileLoggerOutput : ILoggerOutput {
    private readonly object _lock = new();
    private readonly string _path;

    public FileLoggerOutput(string path) => _path = path;

    public LogLevel MinimumLevel { get; set; } = LogLevel.Info;
    public bool Enabled { get; set; } = true;

    public void Log(LogLevel level, string message) {
        if (!Enabled || level < MinimumLevel) return;

        lock (_lock) {
            File.AppendAllText(_path, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToString().ToUpper()}] {message}\n");
        }
    }
}
