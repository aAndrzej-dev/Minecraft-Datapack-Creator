using CommunityToolkit.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MinecraftDatapackCreator;

internal sealed class Settings
{
    internal static readonly JsonLoadSettings jsonLoadSettings = new JsonLoadSettings()
    {
        CommentHandling = CommentHandling.Ignore,
        LineInfoHandling = LineInfoHandling.Ignore
    };
    [NonSerialized]
    private static readonly Font _defaultFont = new Font(new FontFamily("Verdana"), 9);
    [NonSerialized]
    private static readonly Settings _default = new Settings(true);
    public static Settings Default => _default;



    public Font TextEditorFont { get; set; }
    public Font JsonEditorFont { get; set; }
    public bool ReduceJsonFilesSize { get; set; }
    public bool ShowEmptyNodesInReadOnlyJsonFiles { get; set; }
    public string MinecraftDir { get; set; }
    public string DatapackStructureDataFolder { get; set; }
    public bool AlwaysShowFullFilePathInDialogs { get; set; }
    public int RecentProjectsCount { get; set; }


    private Settings()
    {
        TextEditorFont = _defaultFont;
        JsonEditorFont = _defaultFont;
        ReduceJsonFilesSize = true;
        ShowEmptyNodesInReadOnlyJsonFiles = false;
        AlwaysShowFullFilePathInDialogs = false;
        MinecraftDir = string.Empty;
        DatapackStructureDataFolder = string.Empty;
        RecentProjectsCount = 10;
    }
    private Settings(bool useDefault)
    {
        TextEditorFont = _defaultFont;
        JsonEditorFont = _defaultFont;
        ReduceJsonFilesSize = true;
        ShowEmptyNodesInReadOnlyJsonFiles = false;
        AlwaysShowFullFilePathInDialogs = false;
        RecentProjectsCount = 10;
        if (!useDefault)
        {
            MinecraftDir = string.Empty;
            DatapackStructureDataFolder = string.Empty;
            return;
        }
        MinecraftDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
        DatapackStructureDataFolder = Path.Join(Path.GetDirectoryName(Environment.ProcessPath.AsSpan()), "data");
    }


    public void Save(string filename)
    {
        Guard.IsNotNull(filename);


        using StreamWriter sw = new StreamWriter(filename);
        using JsonTextWriter jw = new JsonTextWriter(sw);
        jw.Formatting = Formatting.None;

        JObject.FromObject(this).WriteTo(jw);

        jw.Close();
    }

    public static Settings? Load(string filename)
    {
        using StreamReader sr = new StreamReader(filename);
        using JsonTextReader jr = new JsonTextReader(sr);

        Settings? settings = JObject.Load(jr, jsonLoadSettings).ToObject<Settings>();
        jr.Close();
        return settings;
    }


}