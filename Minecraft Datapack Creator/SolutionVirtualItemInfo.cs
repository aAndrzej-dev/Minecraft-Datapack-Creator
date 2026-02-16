using CommunityToolkit.Diagnostics;
using MinecraftDatapackCreator.FileStructure;

namespace MinecraftDatapackCreator;

internal sealed class SolutionVirtualItemInfo : ISolutionItemInfo
{
    public SolutionNodeType SolutionNodeType { get; }
    public IDatapackItemInfo? ItemInfo => null;
    public string? FullName { get; }
    public DatapackStructureFolder? DatapackStructureFolder { get; }

    internal SolutionVirtualItemInfo(SolutionNodeType solutionNodeType, string? fullPath, DatapackStructureFolder? folder)
    {
        SolutionNodeType = solutionNodeType;
        FullName = fullPath;
        DatapackStructureFolder = folder;
        if ((solutionNodeType & SolutionNodeType.Creating) == 0)
        {
            Guard.IsNotNull(fullPath);
        }
    }
}