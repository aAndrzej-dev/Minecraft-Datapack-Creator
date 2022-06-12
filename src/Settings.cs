using Newtonsoft.Json.Linq;

namespace MinecraftDatapackCreator
{
    internal class Settings
    {
        private static readonly Font _defaultFont = new Font(new FontFamily("Verdana"), 9);
        [NonSerialized]
        private static readonly Settings _default = new Settings()
        {
            TextEditorFont = _defaultFont,
            JsonEditorFont = _defaultFont,
            ReduceJsonFilesSize = true,
            ShowConditionsInJsonEditor = false
        };
        public static Settings Default => _default;



        public Font TextEditorFont { get; set; }
        public Font JsonEditorFont { get; set; }
        public bool ShowConditionsInJsonEditor { get; set; }
        public bool ReduceJsonFilesSize { get; set; }

        public Settings()
        {
            TextEditorFont = _defaultFont;
            JsonEditorFont = _defaultFont;
            ShowConditionsInJsonEditor = false;
            ReduceJsonFilesSize = true;

        }


        public void Save(string filename)
        {
            if (filename is null)
                throw new ArgumentNullException(nameof(filename));


            File.WriteAllText(filename, JObject.FromObject(this).ToString(Newtonsoft.Json.Formatting.None));

        }

        public static Settings? Load(string filename) => JObject.Parse(File.ReadAllText(filename)).ToObject<Settings>();

    }
}
