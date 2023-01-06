using Newtonsoft.Json.Linq;
using System.IO;

namespace MinecraftDatapackCreator;

internal sealed class Settings
{
    private static readonly Font _defaultFont = new Font(new FontFamily("Verdana"), 9);
    [NonSerialized]
    private static readonly Settings _default = new Settings();
    public static Settings Default => _default;



    public Font TextEditorFont { get; set; }
    public Font JsonEditorFont { get; set; }
    public bool ReduceJsonFilesSize { get; set; }
    public bool ShowEmptyNodesInReadOnlyJsonFiles { get; set; }
    public string MinecraftDir { get; set; }
    public string DatapackStructureDataFolder { get; set; }


    public Settings()
    {
        TextEditorFont = _defaultFont;
        JsonEditorFont = _defaultFont;
        ReduceJsonFilesSize = true;
        ShowEmptyNodesInReadOnlyJsonFiles = false;
        MinecraftDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
        DatapackStructureDataFolder = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "data");
    }


    public void Save(string filename)
    {
        if (filename is null)
            throw new ArgumentNullException(nameof(filename));


        File.WriteAllText(filename, JObject.FromObject(this).ToString(Newtonsoft.Json.Formatting.None));

    }

    public static Settings? Load(string filename) => JObject.Parse(File.ReadAllText(filename)).ToObject<Settings>();

}
