using System.IO;
using System.Windows.Forms;

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
internal sealed class Logger : ILogger
{
    private readonly string filename;
    private readonly string instanceId;

    public string Filename => filename;

    public Logger(string filename, string instanceId)
    {
        DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filename)!);
        if (!di.Exists)
        {
            di.Create();
        }

        this.filename = filename;
        this.instanceId = instanceId;
    }

    public void Error(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][ERROR]\t{message.Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");
    public void Fatal(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][FATAL]\t{message.Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");
    public void Info(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][INFO]\t{message.Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");
    public void Debug(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][DEBUG]\t{message.Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");
    public void Warn(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][WARN]\t{message.Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");
    public void Exception(Exception ex) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][EXCEPTION]\t{("\"" + ex.Message + "\" " + ex.StackTrace).Replace("\n", "\\n", StringComparison.Ordinal).Replace("\r", "\\r", StringComparison.Ordinal)}");

    private void WriteMessage(string message)
    {
        try
        {
            File.AppendAllText(filename, "\n" + message);
        }
        catch(Exception ex)
        {
            MessageBox.Show($"FATAL ERROR: {ex.Message}", Program.ProductTitle);
            throw;
        }
    }


}
