using Natsu.Graphics;

namespace Natsu.Utils.Logging;

public struct ConsoleLoggerTheme {
    public Color Debug { get; set; } = Colors.DarkGray;
    public Color Info { get; set; } = Colors.Gray;
    public Color Warning { get; set; } = Colors.Yellow;
    public Color Error { get; set; } = Colors.Red;
    public Color Fatal { get; set; } = Colors.Red;

    public Color DebugBackground { get; set; } = Colors.Transparent;
    public Color InfoBackground { get; set; } = Colors.Transparent;
    public Color WarningBackground { get; set; } = Colors.Transparent;
    public Color ErrorBackground { get; set; } = Colors.Transparent;
    public Color FatalBackground { get; set; } = Colors.Black;

    public (Color fg, Color bg) GetColors(LogLevel level) =>
        level switch {
            LogLevel.Debug => (Debug, DebugBackground),
            LogLevel.Info => (Info, InfoBackground),
            LogLevel.Warning => (Warning, WarningBackground),
            LogLevel.Error => (Error, ErrorBackground),
            LogLevel.Fatal => (Fatal, FatalBackground),
            _ => (Colors.White, Colors.Black)
        };

    public static ConsoleLoggerTheme Default { get; } = new();
    public ConsoleLoggerTheme() { }
}
