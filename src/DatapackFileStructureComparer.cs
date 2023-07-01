namespace MinecraftDatapackCreator;
internal partial class SolutionExplorer
{
    private sealed class DatapackFileStructureComparer : System.Collections.IComparer
    {
        public int Compare(object? x, object? y)
        {
            TreeNode left = (TreeNode)x!;
            TreeNode right = (TreeNode)y!;

            if (left.Tag is not SolutionNodeInfo lTag)
                return string.CompareOrdinal(left.Name, right.Name);
            if (right.Tag is not SolutionNodeInfo rTag)
                return string.CompareOrdinal(left.Name, right.Name);

            if (lTag.solutionNodeType is SolutionNodeType.Solution)
            {
                if (rTag.solutionNodeType is SolutionNodeType.Solution)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.solutionNodeType is SolutionNodeType.Solution)
                return 1;



            if (lTag.solutionNodeType is SolutionNodeType.MetaFile)
            {
                if (rTag.solutionNodeType is SolutionNodeType.MetaFile)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.solutionNodeType is SolutionNodeType.MetaFile)
                return 1;



            if (lTag.solutionNodeType is SolutionNodeType.Structure)
            {
                if (rTag.solutionNodeType is SolutionNodeType.Structure)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.solutionNodeType is SolutionNodeType.Structure)
                return 1;



            if (lTag.solutionNodeType is SolutionNodeType.Directory)
            {
                if (rTag.solutionNodeType is SolutionNodeType.Directory)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.solutionNodeType is SolutionNodeType.Directory)
                return 1;


            if (lTag.solutionNodeType is SolutionNodeType.File)
            {
                if (rTag.solutionNodeType is SolutionNodeType.File)
                    return string.CompareOrdinal(left.Name, right.Name);
                return -1;
            }
            if (rTag.solutionNodeType is SolutionNodeType.File)
                return 1;

            return string.CompareOrdinal(left.Name, right.Name);
        }
    }
}
