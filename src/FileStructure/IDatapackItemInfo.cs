namespace MinecraftDatapackCreator.FileStructure;

internal interface IDatapackItemInfo : ISolutionItemInfo
{
    ReadOnlySpan<char> PathRelativeToDataDirectory { get; }
    ReadOnlySpan<char> PathRelativeToSolution { get; }
    ReadOnlySpan<char> PathRelativeToNamespace { get; }
    string Name { get; }
    new string FullName { get; }
    new DatapackStructureFolder? DatapackStructureFolder { get; }
    DatapackDirectoryInfo? Parent { get; }
    bool Exist { get; }
    Datapack Datapack { get; }
    DatapackItemType Type { get; }
    bool IsNameInvalid { get; }

    DatapackFsOperationResult Rename(ReadOnlySpan<char> newName);
    DatapackFsOperationResult MoveTo(DatapackDirectoryInfo destinationDirectory);
    DatapackFsOperationResult MoveTo(DatapackDirectoryInfo destinationDirectory, ReadOnlySpan<char> newName);
    DatapackFsOperationResult CopyTo(DatapackDirectoryInfo destinationDirectory);
    DatapackFsOperationResult CopyTo(DatapackDirectoryInfo destinationDirectory, ReadOnlySpan<char> newName);
    void Update(string newFullName);
    void EnsureExist();
}