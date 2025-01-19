namespace Natsu.Utils.Logging;

/// <summary>
///     An interface for classes that can output log messages.
///     <br />
///     Implementations of this interface should be thread-safe.
/// </summary>
public interface ILoggerOutput {
    /// <summary>
    ///     The minimum level of log messages that this output should handle.
    /// </summary>
    LogLevel MinimumLevel { get; set; }

    /// <summary>
    ///     Whether this logger should handle any log messages.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    ///     Outputs a log message.
    /// </summary>
    /// <param name="level">The level of the log message</param>
    /// <param name="message">The message to log</param>
    void Log(LogLevel level, string message);

    /// <summary>
    ///     Outputs a debug log message.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Debug(string message) => Log(LogLevel.Debug, message);

    /// <summary>
    ///     Outputs an info log message.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Info(string message) => Log(LogLevel.Info, message);

    /// <summary>
    ///     Outputs a warning log message.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Warning(string message) => Log(LogLevel.Warning, message);

    /// <summary>
    ///     Outputs an error log message.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Error(string message) => Log(LogLevel.Error, message);

    /// <summary>
    ///     Outputs a fatal log message.
    /// </summary>
    /// <param name="message">The message to log</param>
    void Fatal(string message) => Log(LogLevel.Fatal, message);
}
