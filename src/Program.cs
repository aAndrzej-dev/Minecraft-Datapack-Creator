using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace MinecraftDatapackCreator;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>

    public static string InstanceId { get; private set; } = string.Empty;

    private static readonly string productTitle = $"Minecraft Datapack Creator (v{Application.ProductVersion})";
    public static string ProductTitle => productTitle;

    public static ILogger? logger;


    [STAThread]
    private static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        Application.ThreadException += (sender, e) =>
        {

            logger?.Fatal($"Thread exception: \"{e.Exception.Message}\"{e.Exception.StackTrace}");
            MessageBox.Show($"Thread exception: {e.Exception.Message}", ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        };

        Process process = Process.GetCurrentProcess();
        AppDomain.CurrentDomain.AssemblyLoad += (sender, e) => logger?.Debug($"Assembly Loaded: {e.LoadedAssembly.FullName}");
        Guid guid = Guid.NewGuid();


        ReadOnlySpan<char> b64 = Convert.ToBase64String(guid.ToByteArray()).Replace('+', '-').Replace('/', '_');

        b64 = b64[..^2];

        InstanceId = b64.ToString();

        string? logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator", "logs");
        logger = new Logger(Path.Combine(logFolder, DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + $".{InstanceId}.log"));


        logger.Debug($"Process Started. Process Name: {process.ProcessName}; Command Line: {Environment.CommandLine}");


        DirectoryInfo di = new DirectoryInfo(logFolder);
        ReadOnlySpan<char> now = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).AsSpan();

        foreach (FileInfo? item in di.GetFiles("*.log"))
        {
            if (item.Name.AsSpan(0, 10).SequenceEqual(now))
            {
                continue;
            }

            string? zipName = item.FullName[..^3] + "zip";

            if (File.Exists(zipName))
            {
                continue;
            }

            DirectoryInfo fdi = new DirectoryInfo(item.FullName[..^4]);

            if (fdi.Exists)
            {
                continue;
            }

            fdi.Create();

            item.MoveTo(fdi.FullName + "\\" + item.Name);



            ZipFile.CreateFromDirectory(fdi.FullName, zipName, CompressionLevel.SmallestSize, false);

            fdi.Delete(true);


        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Forms.MainForm(args, logger));
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        try
        {
            if (e.ExceptionObject is Exception ex)
            {
                logger?.Fatal($"Unhandled exception: \"{ex.Message}\"{ex.StackTrace}");
                MessageBox.Show($"Unhandled exception: {ex.Message}", ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);


                return;
            }

            logger?.Fatal($"Unhandled exception: {e.ExceptionObject}");
            MessageBox.Show($"Unhandled exception: {e.ExceptionObject}", ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);



        }
        catch (Exception)
        {

            throw;
        }
    }
}
