using System.IO;

namespace MinecraftDatapackCreator;
internal sealed class Controller
{
    internal static readonly string appDataFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator");
    internal static readonly string settingsFilename = Path.Join(appDataFolder, "settings.json");

    public ILogger Logger { get; }
    public Settings Settings { get; }
    public MinecraftVersionManager VersionManager { get; }






    internal event EventHandler<ReloadRequestEventArgs>? ReloadRequested;

    public Controller(ILogger logger)
    {
        Logger = logger;
        Logger.Debug("Initializing controller...");
        Logger.Debug("Loading setting...");
        if (File.Exists(settingsFilename))
        {
            Settings = Settings.Load(settingsFilename) ?? Settings.Default;
            Logger.Debug("Settings loaded successfully");
        }
        else
        {
            Logger.Debug("Settings not found");
            Settings = Settings.Default;
            Settings.Save(settingsFilename);
            Logger.Debug("Default settings saved successfully");
        }

        Logger.Debug("Initializing Version Manager...");
        VersionManager = new MinecraftVersionManager(this);
        Logger.Debug("Version Manager initialized successfully");
    }

    internal void RequestSolutionReload(ReloadRequestEventArgs args)
    {
        ReloadRequested?.Invoke(this, args);
    }
}
internal class ReloadRequestEventArgs : EventArgs
{
    public ReloadRequestReason Reason { get; }

    public ReloadRequestEventArgs(ReloadRequestReason reason)
    {
        Reason = reason;
    }

    internal enum ReloadRequestReason
    {
        Unknown,
        Exception,
        User,
        PackFormatChanged
    }
}