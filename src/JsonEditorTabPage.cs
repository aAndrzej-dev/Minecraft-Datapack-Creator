using Aadev.JTF;
using Aadev.JTF.Editor;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MinecraftDatapackCreator;

internal class JsonEditorTabPage : EditorTabPage
{

    private readonly JsonJtfEditor editor;
    private bool isNotSaved;

    public override event EventHandler? SavedStateChanged;

    public override bool IsNotSaved { get => isNotSaved; protected set { isNotSaved = value; SavedStateChanged?.Invoke(this, EventArgs.Empty); } }


    public override bool CanUndo => false;
    public override bool CanRedo => false;
    public override bool ReadOnly => readOnly;
    private readonly Settings settings;
    private readonly bool readOnly;

    public JsonEditorTabPage(JTemplate jTemplate, DatapackFileInfo fileInfo, Settings settings, Func<JtIdentifier, IEnumerable<IJtSuggestion>> getDynamicSource) : base(fileInfo)
    {
        this.settings = settings;
        System.IO.FileInfo fi = new FileInfo(fileInfo.FullName);

        readOnly = fi.IsReadOnly;

        try
        {
            string text = File.ReadAllText(fileInfo.FullName);
            JToken value = string.IsNullOrWhiteSpace(text) ? JValue.CreateNull() : JToken.Parse(text);
            editor = new JsonJtfEditor
            {
                BackColor = Color.FromArgb(50, 50, 50),
                AutoScaleMode = AutoScaleMode.None,
                Font = settings.JsonEditorFont,
                Dock = DockStyle.Fill,
                Value = value,
                NormalizeTwinNodeOrder = true,
                GetDynamicSource = getDynamicSource,
                Template = jTemplate,
                ReadOnly = readOnly,
                MaximumSuggestionCountForComboBox = 0,
                ShowEmptyNodesInReadOnlyMode = settings.ShowEmptyNodesInReadOnlyJsonFiles

            };
            Text = $"{fileInfo.Name.SetStringLenghtMiddle(25)}{(readOnly ? " (ReadOnly)" : "")}";


            editor.ValueChanged += (s, ev) => IsNotSaved = true;
            Controls.Add(editor);

        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"{ex.Message}\n{fileInfo.FullName}", Program.ProductTitle);
            editor = new JsonJtfEditor
            {
                BackColor = Color.FromArgb(50, 50, 50),
                AutoScaleMode = AutoScaleMode.None,
                Font = settings.JsonEditorFont,
                Dock = DockStyle.Fill,
                GetDynamicSource = getDynamicSource,
                Template = jTemplate,
            };

            Text = $"{fileInfo.Name}{(readOnly ? " (ReadOnly)" : "")}";
            editor.ValueChanged += (s, ev) => IsNotSaved = true;

            Controls.Add(editor);
        }



    }



    public override void Save()
    {
        if(readOnly)
        {
            return;
        }
        editor.Save(FileInfo.FullName, settings.ReduceJsonFilesSize ? Newtonsoft.Json.Formatting.None : Newtonsoft.Json.Formatting.Indented);

        IsNotSaved = false;

    }

    public override void Undo() => throw new NotImplementedException();
    public override void Redo() => throw new NotImplementedException(); 
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            editor.Dispose();
        }
        base.Dispose(disposing);
    }
}
