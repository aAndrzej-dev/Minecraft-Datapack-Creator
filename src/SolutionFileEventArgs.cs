namespace MinecraftDatapackCreator;


public class SolutionFileEventArgs : EventArgs
{
    public string Namespace { get; }
    public string RelativePath { get; }
    public string Filename { get; }
    public DatapackStructureFolder StructureFolder { get; }


    public SolutionFileEventArgs(string @namespace, string relativePath, DatapackStructureFolder structureFolder, string fullPath)
    {
        Namespace = @namespace;
        RelativePath = relativePath;
        StructureFolder = structureFolder;
        Filename = fullPath;
    }

}
