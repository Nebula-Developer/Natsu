namespace Natsu.Utils.Logging;

public static class Logging {
    /// <summary>
    ///     A global logger instance used for general logging.
    /// </summary>
    public static Logger Logger { get; } = new() {
        Outputs = { new ConsoleLoggerOutput() },
        Prefix = "[Natsu] "
    };

    /// <inheritdoc cref="ILoggerOutput.Log" />
    public static void Log(LogLevel level, string message) => Logger.Log(level, message);

    /// <inheritdoc cref="ILoggerOutput.Debug" />
    public static void Debug(string message) => Log(LogLevel.Debug, message);

    /// <inheritdoc cref="ILoggerOutput.Info" />
    public static void Info(string message) => Log(LogLevel.Info, message);

    /// <inheritdoc cref="ILoggerOutput.Warning" />
    public static void Warning(string message) => Log(LogLevel.Warning, message);

    /// <inheritdoc cref="ILoggerOutput.Error" />
    public static void Error(string message) => Log(LogLevel.Error, message);

    /// <inheritdoc cref="ILoggerOutput.Fatal" />
    public static void Fatal(string message) => Log(LogLevel.Fatal, message);
}
