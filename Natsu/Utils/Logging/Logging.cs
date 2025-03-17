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
    public static void Log(string message, LogLevel level = LogLevel.Info) => Logger.Log(level, message);

    /// <inheritdoc cref="ILoggerOutput.Debug" />
    public static void Debug(string message) => Log(message, LogLevel.Debug);

    /// <inheritdoc cref="ILoggerOutput.Info" />
    public static void Info(string message) => Log(message);

    /// <inheritdoc cref="ILoggerOutput.Warn" />
    public static void Warn(string message) => Log(message, LogLevel.Warning);

    /// <inheritdoc cref="ILoggerOutput.Error" />
    public static void Error(string message) => Log(message, LogLevel.Error);

    /// <inheritdoc cref="ILoggerOutput.Fatal" />
    public static void Fatal(string message) => Log(message, LogLevel.Fatal);
}
