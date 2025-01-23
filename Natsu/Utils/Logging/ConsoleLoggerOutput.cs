using Natsu.Graphics;

namespace Natsu.Utils.Logging;

public class ConsoleLoggerOutput : ILoggerOutput {
    public bool Colorise { get; set; } = true;
    public ConsoleLoggerTheme Theme { get; set; } = ConsoleLoggerTheme.Default;

    private string resetColor => "\u001b[0m";
    public LogLevel MinimumLevel { get; set; } = LogLevel.Debug;
    public bool Enabled { get; set; } = true;

    public void Log(LogLevel level, string message) {
        if (!Enabled || level < MinimumLevel) return;

        string msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToString().ToUpper()}] {message}";
        (Color fg, Color bg) = Theme.GetColors(level);
        Console.WriteLine(Colorise ? $"{toTrueColor(fg, bg)}{msg}{resetColor}" : msg);
    }

    private string toTrueColor(Color color, bool fg = true) => color.A == 0 ? "" : $"\u001b[{(fg ? 38 : 48)};2;{color.R};{color.G};{color.B}m";
    private string toTrueColor(Color fg, Color bg) => $"{toTrueColor(fg)}{toTrueColor(bg, false)}";
}
