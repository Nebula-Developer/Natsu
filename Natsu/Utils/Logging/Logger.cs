namespace Natsu.Utils.Logging;

public class Logger {
    /// <summary>
    ///     The outputs of this logger.
    /// </summary>
    public List<ILoggerOutput> Outputs = new();

    /// <summary>
    ///     A prefix to append to all log messages.
    ///     <br />
    ///     Useful for identifying the source of log messages, such as the name of the logger.
    /// </summary>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    ///     Adds an output to this logger.
    /// </summary>
    /// <param name="output">The output to add</param>
    public void AddOutput(ILoggerOutput output) => Outputs.Add(output);

    /// <inheritdoc cref="ILoggerOutput.Log" />
    public void Log(LogLevel level, string message) {
        lock (Outputs) {
            foreach (ILoggerOutput? output in Outputs) output.Log(level, Prefix + message);
        }
    }

    /// <inheritdoc cref="ILoggerOutput.Debug" />
    public void Debug(string message) => Log(LogLevel.Debug, message);

    /// <inheritdoc cref="ILoggerOutput.Info" />
    public void Info(string message) => Log(LogLevel.Info, message);

    /// <inheritdoc cref="ILoggerOutput.Warn" />
    public void Warn(string message) => Log(LogLevel.Warning, message);

    /// <inheritdoc cref="ILoggerOutput.Error" />
    public void Error(string message) => Log(LogLevel.Error, message);

    /// <inheritdoc cref="ILoggerOutput.Fatal" />
    public void Fatal(string message) => Log(LogLevel.Fatal, message);
}
