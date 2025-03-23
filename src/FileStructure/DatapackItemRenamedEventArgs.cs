namespace MinecraftDatapackCreator.FileStructure;

internal sealed class DatapackItemRenamedEventArgs : DatapackItemChangedEventArgs
{
    public string OldPath { get; }
    public DatapackItemRenamedEventArgs(IDatapackItemInfo subject, string oldPath) : base(subject)
    {
        OldPath = oldPath;
    }
}
