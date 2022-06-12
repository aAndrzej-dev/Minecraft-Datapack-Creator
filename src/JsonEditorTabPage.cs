using Aadev.JTF;
using Aadev.JTF.Editor;

namespace MinecraftDatapackCreator;

internal class JsonEditorTabPage : TabPage, ITabPage
{

    private readonly JsonJtfEditor editor;
    private bool isNotSaved;

    public event EventHandler? SavedStateChanged;

    public string Filename { get; }
    public bool IsNotSaved { get => isNotSaved; private set { isNotSaved = value; SavedStateChanged?.Invoke(this, EventArgs.Empty); } }

    public Color TabBackColor { get; set; } = Color.RoyalBlue;
    public Color TabForeColor { get; set; } = Color.White;

    private readonly Settings settings;

    public JsonEditorTabPage(JTemplate jTemplate, string filename, Settings settings)
    {
        Filename = filename;
        this.settings = settings;
        editor = new JsonJtfEditor
        {
            Template = jTemplate,
            Filename = filename,
            BackColor = Color.FromArgb(50, 50, 50),
            AutoScaleMode = AutoScaleMode.None,
            Font = settings.JsonEditorFont,
            Dock = DockStyle.Fill,
            ShowConditionsCount = settings.ShowConditionsInJsonEditor
        };


        Text = Path.GetFileName(Filename);



        editor.ValueChanged += (s, ev) => IsNotSaved = true;

        Controls.Add(editor);

    }



    public void Save()
    {
        editor.Save(settings.ReduceJsonFilesSize ? Newtonsoft.Json.Formatting.None : Newtonsoft.Json.Formatting.Indented);

        IsNotSaved = false;

    }
}
