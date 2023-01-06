namespace MinecraftDatapackCreator;


internal sealed class DatapackFileEventArgs : FileEventArgs
{
    public DatapackFileInfo FileInfo { get; }

    public DatapackFileEventArgs(DatapackFileInfo? fileInfo, bool? readOnly = null) : base(fileInfo?.FullName, readOnly)
    {
        if(fileInfo is null)
            throw new ArgumentNullException(nameof(fileInfo));
        FileInfo = fileInfo;
    }
}
