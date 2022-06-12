using System.Diagnostics.CodeAnalysis;

namespace MinecraftDatapackCreator;

internal struct SolutionNodeInfo
{
    public SolutionNodeType solutionNodeType;
    public string? fullPath;
    public string? @namespace;
    public DatapackStructureFolder? structureFolder;
    public string? relativePath;

    public SolutionNodeInfo(SolutionNodeType solutionNodeType, string? fullPath, string? @namespace = null, DatapackStructureFolder? structureFolder = null, string? relativePath = null) : this()
    {
        this.solutionNodeType = solutionNodeType;

        if (solutionNodeType is SolutionNodeType.File)
        {
            if (fullPath is null) throw new ArgumentNullException(nameof(fullPath));
            if (@namespace is null) throw new ArgumentNullException(nameof(@namespace));
            if (structureFolder is null) throw new ArgumentNullException(nameof(structureFolder));
            if (relativePath is null) throw new ArgumentNullException(nameof(relativePath));
        }


        this.fullPath = fullPath;
        this.@namespace = @namespace;
        this.structureFolder = structureFolder;
        this.relativePath = relativePath;
    }
}