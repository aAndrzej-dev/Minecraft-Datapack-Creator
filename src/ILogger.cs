using System.IO;

namespace MinecraftDatapackCreator;

internal interface ILogger
{
    LoggerLevel MinLevel { get; set; }
    string Filename { get; }
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
    void Fatal(string message);
    void Exception(Exception ex);
}
internal sealed class Logger : ILogger, IDisposable
{
    private readonly StreamWriter streamWriter;
    public string Filename { get; }

    public LoggerLevel MinLevel { get; set; }

    public Logger(string filename)
    {
        DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filename)!);
        if (!di.Exists)
        {
            di.Create();
        }

        Filename = filename;
        streamWriter = new StreamWriter(File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read));
    }

    public void Error(string message) => LogMessage(LoggerLevel.ERROR, message);
    public void Fatal(string message) => LogMessage(LoggerLevel.FATAL, message);
    public void Info(string message) => LogMessage(LoggerLevel.INFO, message);
    public void Debug(string message) => LogMessage(LoggerLevel.DEBUG, message);
    public void Warn(string message) => LogMessage(LoggerLevel.WARNING, message);
    public void Exception(Exception ex) => LogMessage(LoggerLevel.EXCEPTION, "\"" + ex.Message + "\" " + ex.StackTrace);


    private void LogMessage(LoggerLevel level, string message)
    {
        if (MinLevel > level)
            return;
        WriteMessage($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}][{FastLevelToString(level)}]\t{message.Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");
    }
    private static string FastLevelToString(LoggerLevel level)
    {
        return level switch
        {
            LoggerLevel.DEBUG => "DEBUG",
            LoggerLevel.INFO => "INFO",
            LoggerLevel.WARNING => "WARNING",
            LoggerLevel.ERROR => "ERROR",
            LoggerLevel.FATAL => "FATAL",
            LoggerLevel.EXCEPTION => "EXCEPTION",
            _ => "UNKNOWN",
        };
    }

    private void WriteMessage(string message)
    {
        System.Diagnostics.Debug.WriteLine(message);
        try
        {
            streamWriter.WriteLine(message);
            streamWriter.Flush();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"FATAL ERROR: {ex.Message}", Program.ProductTitle);
            throw;
        }
    }

    public void Dispose()
    {
        streamWriter.Close();
        streamWriter.Dispose();
    }
}
internal enum LoggerLevel
{
    DEBUG,
    INFO,
    WARNING,
    ERROR,
    FATAL,
    EXCEPTION
}