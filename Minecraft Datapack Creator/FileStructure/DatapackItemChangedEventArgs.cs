namespace MinecraftDatapackCreator.FileStructure;

internal class DatapackItemChangedEventArgs : EventArgs
{
    public IDatapackItemInfo Subject { get; }
    public DatapackItemChangedEventArgs(IDatapackItemInfo subject)
    {
        Subject = subject;
    }
}
