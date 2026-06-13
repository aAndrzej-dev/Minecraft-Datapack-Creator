using MinecraftDatapackCreator.FileStructure;


namespace MinecraftDatapackCreator;

[Serializable]
internal struct ClipboardItemInfo
{
    public string InstanceId { get; }
    public string FullPath { get; }
    public string? StructureFolder { get; }
    public bool Cut { get; }

    public ClipboardItemInfo(IDatapackItemInfo itemInfo, bool cut)
    {
        InstanceId = Program.InstanceId;
        FullPath = itemInfo.FullName;
        StructureFolder = itemInfo.DatapackStructureFolder?.Path;
        Cut = cut;
    }
    [System.Text.Json.Serialization.JsonConstructor]
    public ClipboardItemInfo(string instanceId, string fullPath, string? structureFolder, bool cut)
    {
        InstanceId = instanceId;
        FullPath = fullPath;
        StructureFolder = structureFolder;
        Cut = cut;
    }

    public bool IsValid(Datapack datapack, DatapackStructureFolder? structureFolder)
    {
        if (Program.InstanceId != InstanceId)
            return false;

        if (!FullPath.AsSpan(0, datapack.Path.Length).SequenceEqual(datapack.Path))
            return false;

        if (structureFolder is not null)
        {
            if (structureFolder.Path != this.StructureFolder)
                return false;
        }

        return true;
    }
}
