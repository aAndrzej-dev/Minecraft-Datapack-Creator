using MinecraftDatapackCreator.FileStructure;
using System.ComponentModel;

namespace MinecraftDatapackCreator;
internal abstract class EditorTabPage : TabPage
{
    protected readonly Controller controller;
    public DatapackFileInfo FileInfo { get; }

    public Color TabBackColor => FileInfo.DatapackStructureFolder?.TabBackColor ?? Color.RoyalBlue;
    public Color TabForeColor => FileInfo.DatapackStructureFolder?.TabForeColor ?? Color.White;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public abstract bool IsNotSaved { get; protected set; }
    public abstract bool ReadOnly { get; }
    public abstract bool CanUndo { get; }
    public abstract bool CanRedo { get; }


    public abstract event EventHandler? SavedStateChanged;

    protected bool suspendChangeEvent;

    public EditorTabPage(Controller controller, DatapackFileInfo fileInfo)
    {
        FileInfo = fileInfo;
        this.controller = controller;
        Text = $"{fileInfo.Name.SetStringLengthMiddle(25)}{(ReadOnly ? $" ({Properties.Resources.ReadOnly})" : string.Empty)}";
        if (FileInfo.DatapackStructureFolder is null)
            ToolTipText = FileInfo.FullName;
        else
            ToolTipText = $"{FileInfo.DatapackStructureFolder.DisplayName.TrimEnd('s')}\n{FileInfo.NamespacedId}\n{FileInfo.FullName}";


        FileInfo.FileChanged += (sender, e) => Invoke(() =>
            {

                Text = $"{fileInfo.Name.SetStringLengthMiddle(25)}{(ReadOnly ? $" ({Properties.Resources.ReadOnly})" : string.Empty)}";
                if (!FileInfo.Exist)
                {
                    Text += " (NOT FOUND)";
                }
                if (FileInfo.DatapackStructureFolder is null)
                    ToolTipText = FileInfo.FullName;
                else
                    ToolTipText = $"{FileInfo.DatapackStructureFolder.DisplayName.TrimEnd('s')}\n{FileInfo.NamespacedId}\n{FileInfo.FullName}";
            });

        FileInfo.FileContentChanged += FileInfo_FileContentChanged;
    }

    private void FileInfo_FileContentChanged(object? sender, EventArgs e)
    {
        if (suspendChangeEvent)
            return;
    }
    public abstract void Undo();
    public abstract void Redo();
    public abstract void Save();
    public abstract void Reload(bool askToSave);
}
