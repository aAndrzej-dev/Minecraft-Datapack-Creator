using System;

namespace MinecraftDatapackCreator;

internal partial class SolutionExplorer
{
    [Flags]
    internal enum SolutionNodeType
    {
        Blank = 0,
        Creating = 1,
        Solution = 2,
        Namespace = 4,
        Structure = 8,
        Directory = 16,
        File = 32,
        MetaFile = 64,
        Corrupted = 128,
    }
}