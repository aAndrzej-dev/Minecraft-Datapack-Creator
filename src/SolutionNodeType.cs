namespace MinecraftDatapackCreator;

internal enum SolutionNodeType
{
    Unknown = 0, //0000
    File = 1, // 0001
    MetaFile = 2, // 0010
    Directory = 3, // 0011
    Namespace = 4, // 0100
    StructureFolder = 5, // 0101
    DataFolder = 6, // 0110
    SolutionFolder = 7, // 0111
    Creating = 8 // 1000
}