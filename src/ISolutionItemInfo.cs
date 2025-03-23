using MinecraftDatapackCreator.FileStructure;

namespace MinecraftDatapackCreator;

internal interface ISolutionItemInfo
{
    DatapackStructureFolder? DatapackStructureFolder { get; }
    string? FullName { get; }
    IDatapackItemInfo? ItemInfo { get; }
    SolutionNodeType SolutionNodeType { get; }
}