using System.Diagnostics;
using System.IO.Compression;

namespace MinecraftDatapackCreator;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>

    public static string InstanceId { get; private set; } = string.Empty;


    public static ILogger? logger;


    [STAThread]
    private static void Main(string[] args)
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += CurrentDomain_UnhandledException;



        Process process = Process.GetCurrentProcess();

        Guid guid = Guid.NewGuid();


        ReadOnlySpan<char> b64 = Convert.ToBase64String(guid.ToByteArray()).Replace('+', '-').Replace('/', '_');

        b64 = b64[..^2];

        InstanceId = b64.ToString();

        string? logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator", "logs");
        logger = new Logger(Path.Combine(logFolder, DateTime.Now.ToString("yyyy-MM-dd") + ".log"), InstanceId);


        logger.Debug($"Process Started. Process Name: {process.ProcessName}; Command Line: {Environment.CommandLine}");


        DirectoryInfo di = new DirectoryInfo(logFolder);

        foreach (FileInfo? item in di.GetFiles("*.log"))
        {
            if (item.Name == DateTime.Now.ToString("yyyy-MM-dd") + ".log")
                continue;
            string? zipName = item.FullName[..^3] + "zip";

            if (File.Exists(zipName))
                continue;

            DirectoryInfo fdi = new DirectoryInfo(item.FullName[..^4]);

            if (fdi.Exists)
                continue;
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
                logger?.Fatal($"Unhandled exception: {ex.Message}");
                MessageBox.Show($"Unhandled exception: {ex.Message}", Forms.MainForm.productTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);


                return;
            }

            logger?.Fatal($"Unhandled exception: {e.ExceptionObject}");
            MessageBox.Show($"Unhandled exception: {e.ExceptionObject}", Forms.MainForm.productTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);



        }
        catch (Exception)
        {

            throw;
        }
    }
}
