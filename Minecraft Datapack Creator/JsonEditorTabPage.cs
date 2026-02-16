using Aadev.JTF;
using Aadev.JTF.Editor;
using Aadev.JTF.Editor.ViewModels;
using MinecraftDatapackCreator.FileStructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ComponentModel;

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

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool IsNotSaved { get => isNotSaved; protected set { if (isNotSaved == value) return; isNotSaved = value; SavedStateChanged?.Invoke(this, EventArgs.Empty); } }


    public override bool CanUndo => false;
    public override bool CanRedo => false;
    public override bool ReadOnly => readOnly;
    private readonly bool readOnly;

    public JsonEditorTabPage(Controller controller, DatapackFileInfo fileInfo) : base(controller, fileInfo)
    {
        JTemplate template;
        if (fileInfo.Type is DatapackItemType.MetaFile)
        {
            template = fileInfo.Datapack.Sources.McmetaFileTemplate;
        }
        else if (fileInfo.DatapackStructureFolder is DatapackStructureFolderJTF jtf)
        {
            template = jtf.Template;
        }
        else
            throw new UnreachableException();


        FileInfo fi = new FileInfo(fileInfo.FullName);
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
                    value = JToken.ReadFrom(jr, Settings.jsonLoadSettings);

                jr.Close();
            }
            catch (Exception ex)
            {
                Program.logger!.Exception(ex);
                MessageBox.Show(this, $"Cannot read the file!\n {ex.Message}", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                value = JValue.CreateNull();
            }
            JtRootViewModel vm = new JtRootViewModel
            {
                Value = value,
                Template = template,
                NormalizeTwinNodeOrder = true,
                DynamicSuggestionsSource = fileInfo.Datapack.GetNamespacedSourceAsSuggestions,
                //vm.IsReadOnly = readOnly;
                MaximumSuggestionCountForComboBox = 10,
                ShowEmptyNodesInReadOnlyMode = controller.Settings.ShowEmptyNodesInReadOnlyJsonFiles
            };

            editor = new JsonJtfEditor(vm)
            {
                BackColor = Color.FromArgb(50, 50, 50),
                AutoScaleMode = AutoScaleMode.None,
                Font = controller.Settings.JsonEditorFont,
                Dock = DockStyle.Fill,
                ColorTable = jtColorTable

            };
            Text = $"{fileInfo.Name.SetStringLengthMiddle(25)}{(readOnly ? " (ReadOnly)" : "")}";


            vm.ValueChanged += (s, ev) => IsNotSaved = true;
            Controls.Add(editor);

        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"{ex.Message}\n{fileInfo.FullName}", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            JtRootViewModel vm = new JtRootViewModel
            {
                Template = template,
                NormalizeTwinNodeOrder = true,
                DynamicSuggestionsSource = fileInfo.Datapack.GetNamespacedSourceAsSuggestions,
                //vm.IsReadOnly = readOnly;
                MaximumSuggestionCountForComboBox = 10,
                ShowEmptyNodesInReadOnlyMode = controller.Settings.ShowEmptyNodesInReadOnlyJsonFiles
            };
            editor = new JsonJtfEditor(vm)
            {
                BackColor = Color.FromArgb(50, 50, 50),
                AutoScaleMode = AutoScaleMode.None,
                Font = controller.Settings.JsonEditorFont,
                Dock = DockStyle.Fill,
                //DynamicSuggestionsSource = getDynamicSource,
                //Template = jTemplate,
                ColorTable = jtColorTable,
                //MaximumSuggestionCountForComboBox = 10,
            };

            Text = $"{fileInfo.Name}{(readOnly ? " (ReadOnly)" : "")}";
            vm.ValueChanged += (s, ev) => IsNotSaved = true;

            Controls.Add(editor);
        }



    }



    public override void Save()
    {
        if (readOnly)
        {
            return;
        }
        suspendChangeEvent = true;
        editor.Save(FileInfo.FullName, controller.Settings.ReduceJsonFilesSize ? Newtonsoft.Json.Formatting.None : Newtonsoft.Json.Formatting.Indented);

        IsNotSaved = false;
        suspendChangeEvent = false;
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

    public override void Reload(bool askToSave)
    {
        JToken value;
        try
        {
            using StreamReader sr = new StreamReader(FileInfo.FullName);
            using JsonReader jr = new JsonTextReader(sr);

            if (sr.BaseStream.Length == 0)
                value = JValue.CreateNull();
            else
                value = JToken.ReadFrom(jr, Settings.jsonLoadSettings);

            jr.Close();
        }
        catch (Exception ex)
        {
            Program.logger!.Exception(ex);
            MessageBox.Show(this, $"Cannot read the file!\n {ex.Message}", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            value = JValue.CreateNull();
        }
        editor.ViewModel.Value = value;
        IsNotSaved = false;
    }

    internal static EditorTabPage Create(Controller controller, DatapackFileInfo fileInfo) => new JsonEditorTabPage(controller, fileInfo);
}
