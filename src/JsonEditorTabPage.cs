using Aadev.JTF;
using Aadev.JTF.Editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MinecraftDatapackCreator;

internal sealed class JsonEditorTabPage : EditorTabPage
{
    private static readonly JtColorTable jtColorTable = new JtColorTable()
    {
        AddItemButtonBackColor = Color.SeaGreen,
        ExpandButtonBackColor = Color.SeaGreen,
        ActiveBorderColor = Color.Cyan,
        RemoveItemButtonBackColor = Color.Crimson
    };


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
            JToken value;
            try
            {
                using StreamReader sr = new StreamReader(fileInfo.FullName);
                using JsonReader jr = new JsonTextReader(sr);

                if (sr.BaseStream.Length == 0)
                    value = JValue.CreateNull();
                else
                    value = JToken.ReadFrom(jr);

                jr.Close();
            }
            catch (Exception ex)
            {
                Program.logger!.Exception(ex);
                MessageBox.Show($"Cannot read the file!\n {ex.Message}");
                value = JValue.CreateNull();
            }

            editor = new JsonJtfEditor
            {
                BackColor = Color.FromArgb(50, 50, 50),
                AutoScaleMode = AutoScaleMode.None,
                Font = settings.JsonEditorFont,
                Dock = DockStyle.Fill,
                Value = value,
                NormalizeTwinNodeOrder = true,
                DynamicSuggestionsSource = getDynamicSource,
                Template = jTemplate,
                ReadOnly = readOnly,
                MaximumSuggestionCountForComboBox = 10,
                ShowEmptyNodesInReadOnlyMode = settings.ShowEmptyNodesInReadOnlyJsonFiles,
                ColorTable = jtColorTable

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
                DynamicSuggestionsSource = getDynamicSource,
                Template = jTemplate,
                ColorTable = jtColorTable,
                MaximumSuggestionCountForComboBox = 10,
            };

            Text = $"{fileInfo.Name}{(readOnly ? " (ReadOnly)" : "")}";
            editor.ValueChanged += (s, ev) => IsNotSaved = true;

            Controls.Add(editor);
        }



    }



    public override void Save()
    {
        if (readOnly)
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
