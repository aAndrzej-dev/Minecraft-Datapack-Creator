namespace MinecraftDatapackCreator;

public class FileEventArgs : EventArgs
{
    public string Filename { get; }

    public FileEventArgs(string filename)
    {
        Filename = filename;
    }
}
