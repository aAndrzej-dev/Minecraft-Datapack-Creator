namespace MinecraftDatapackCreator;
internal partial class SolutionExplorer
{
    private sealed class DatapackFileStructureComparer : System.Collections.IComparer
    {
        public int Compare(object? x, object? y)
        {
            TreeNode left = (TreeNode)x!;
            TreeNode right = (TreeNode)y!;

            if (left.Tag is not ISolutionItemInfo lTag)
                return string.CompareOrdinal(left.Name, right.Name);
            if (right.Tag is not ISolutionItemInfo rTag)
                return string.CompareOrdinal(left.Name, right.Name);

            if (lTag.SolutionNodeType is SolutionNodeType.SolutionFolder)
            {
                if (rTag.SolutionNodeType is SolutionNodeType.SolutionFolder)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.SolutionNodeType is SolutionNodeType.SolutionFolder)
                return 1;



            if (lTag.SolutionNodeType is SolutionNodeType.MetaFile)
            {
                if (rTag.SolutionNodeType is SolutionNodeType.MetaFile)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.SolutionNodeType is SolutionNodeType.MetaFile)
                return 1;



            if (lTag.SolutionNodeType is SolutionNodeType.StructureFolder)
            {
                if (rTag.SolutionNodeType is SolutionNodeType.StructureFolder)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.SolutionNodeType is SolutionNodeType.StructureFolder)
                return 1;

            if (lTag.SolutionNodeType is SolutionNodeType.Namespace)
            {
                if (rTag.SolutionNodeType is SolutionNodeType.Namespace)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.SolutionNodeType is SolutionNodeType.Namespace)
                return 1;


            if (lTag.SolutionNodeType is SolutionNodeType.Directory)
            {
                if (rTag.SolutionNodeType is SolutionNodeType.Directory)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.SolutionNodeType is SolutionNodeType.Directory)
                return 1;


            if (lTag.SolutionNodeType is SolutionNodeType.File)
            {
                if (rTag.SolutionNodeType is SolutionNodeType.File)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.SolutionNodeType is SolutionNodeType.File)
                return 1;

            return string.CompareOrdinal(left.Name, right.Name);
        }
    }
}
