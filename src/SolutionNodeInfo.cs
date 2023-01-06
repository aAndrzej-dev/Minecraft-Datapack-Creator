namespace MinecraftDatapackCreator;
internal class SolutionNodeInfo
{
    public readonly SolutionNodeType solutionNodeType;
    public readonly string? fullPath;
    public readonly DatapackItemInfo? fileInfo;

    public SolutionNodeInfo(SolutionNodeType solutionNodeType, string? fullPath, DatapackItemInfo? itemInfo = null)
    {
        this.solutionNodeType = solutionNodeType;
        this.fullPath = fullPath;
        fileInfo = itemInfo;
        if (solutionNodeType is SolutionNodeType.File or SolutionNodeType.Directory or SolutionNodeType.Structure or SolutionNodeType.Namespace)
        {
            if (fullPath is null) throw new ArgumentNullException(nameof(fullPath));
        }
    }
}
internal class SolutionNewFilewNodeInfo : SolutionNodeInfo
{

    public SolutionNewFilewNodeInfo(SolutionNodeType solutionNodeType, DatapackStructureFolder folder) : base(solutionNodeType | SolutionNodeType.Creating | SolutionNodeType.File, null)
    {
        this.folder = folder;
    }

    public readonly DatapackStructureFolder folder;
}