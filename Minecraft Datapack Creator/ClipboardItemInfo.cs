using MinecraftDatapackCreator.FileStructure;


namespace MinecraftDatapackCreator;
internal sealed partial class SolutionExplorer
{
    [Serializable]
    internal readonly struct ClipboardItemInfo
    {
        public readonly string instanceId;
        public readonly string fullPath;
        public readonly string? structureFolder;
        public readonly bool cut;

        public ClipboardItemInfo(IDatapackItemInfo itemInfo, bool cut)
        {
            instanceId = Program.InstanceId;
            fullPath = itemInfo.FullName;
            structureFolder = itemInfo.DatapackStructureFolder?.Path;
            this.cut = cut;
        }


        public bool IsValid(Datapack datapack, DatapackStructureFolder? structureFolder)
        {
            if (Program.InstanceId != instanceId)
                return false;

            if (!fullPath.AsSpan(0, datapack.Path.Length).SequenceEqual(datapack.Path))
                return false;

            if (structureFolder is not null)
            {
                if (structureFolder.Path != this.structureFolder)
                    return false;
            }

            return true;
        }
    }
}