namespace MinecraftDatapackCreator;

public class FileEventArgs : EventArgs
{
    public string Filename { get; }
    public bool? ReadOnly { get; }

    public FileEventArgs(string? filename, bool? readOnly = null)
    {
        if(filename is null)
            throw new ArgumentNullException(nameof(filename));
        Filename = filename;
        ReadOnly = readOnly;
    }
}
