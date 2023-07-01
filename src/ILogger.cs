using System.IO;

namespace MinecraftDatapackCreator;

public interface ILogger
{
    public string Filename { get; }
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
    void Fatal(string message);
    void Exception(Exception ex);
}
internal sealed class Logger : ILogger, IDisposable
{
    private StreamWriter streamWriter;
    public string Filename { get; }


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

    public void Error(string message) => LogMessage("ERROR", message);
    public void Fatal(string message) => LogMessage("FATAL", message);
    public void Info(string message) => LogMessage("INFO", message);
    public void Debug(string message) => LogMessage("DEBUG", message);
    public void Warn(string message) => LogMessage("WARNING", message);
    public void Exception(Exception ex) => WriteMessage($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}][EXCEPTION]\t{("\"" + ex.Message + "\" " + ex.StackTrace).Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");

    private void LogMessage(string level, string message)
    {
        WriteMessage($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}][{level}]\t{message.Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");
    }

    private void WriteMessage(string message)
    {
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
