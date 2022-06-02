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

    public JsonEditorTabPage(JTemplate jTemplate, string filename)
    {
        Filename = filename;
        editor = new JsonJtfEditor
        {
            Template = jTemplate,
            Filename = filename,
            BackColor = Color.FromArgb(50, 50, 50),
            Dock = DockStyle.Fill,

        };

        Text = Path.GetFileName(Filename);
        editor.ValueChanged += (s, ev) => IsNotSaved = true;

        Controls.Add(editor);

    }



    public void Save()
    {
        editor.Save();

        IsNotSaved = false;

    }
}
