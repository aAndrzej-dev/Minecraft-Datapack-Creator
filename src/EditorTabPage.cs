namespace MinecraftDatapackCreator;
internal abstract class EditorTabPage : TabPage
{
    public DatapackFileInfo FileInfo { get; }
    public Color TabBackColor => FileInfo.DatapackStructureFolder?.TabBackColor ?? Color.RoyalBlue;
    public Color TabForeColor => FileInfo.DatapackStructureFolder?.TabForeColor ?? Color.White;
    public abstract bool IsNotSaved { get; protected set; }
    public abstract bool ReadOnly { get; }
    public abstract bool CanUndo { get; }
    public abstract bool CanRedo { get; }


    public abstract event EventHandler? SavedStateChanged;


    public EditorTabPage(DatapackFileInfo fileInfo)
    {
        FileInfo = fileInfo;

        Text = $"{fileInfo.Name.SetStringLenghtMiddle(25)}{(ReadOnly ? $" ({Properties.Resources.ReadOnly})" : string.Empty)}";
        if (FileInfo.DatapackStructureFolder is null)
            ToolTipText = FileInfo.FullName;
        else
            ToolTipText = $"{FileInfo.DatapackStructureFolder.DisplayName}\n{FileInfo.NamespacedId}\n{FileInfo.FullName}";


        FileInfo.FileChanged += (sender, e) =>
        {
            Invoke(() => {
                Text = $"{fileInfo.Name.SetStringLenghtMiddle(25)}{(ReadOnly ? $" ({Properties.Resources.ReadOnly})" : string.Empty)}";
                if (FileInfo.DatapackStructureFolder is null)
                    ToolTipText = FileInfo.FullName;
                else 
                    ToolTipText = $"{FileInfo.DatapackStructureFolder?.DisplayName}\n{FileInfo.NamespacedId}\n{FileInfo.FullName}";
            });
        };
    }
    public abstract void Undo();
    public abstract void Redo();
    public abstract void Save();
}
