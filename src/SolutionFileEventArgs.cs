namespace MinecraftDatapackCreator;


public class SolutionFileEventArgs : EventArgs
{
    public string Namespace { get; }
    public string RelativePath { get; }
    public string Filename => Path.GetFileName(RelativePath);
    public DatapackStructureFolder? StructureFolder { get; }


    public SolutionFileEventArgs(string @namespace, string relativePath, DatapackStructureFolder? structureFolder)
    {
        Namespace = @namespace;
        RelativePath = relativePath;
        StructureFolder = structureFolder;
    }

}
