using MinecraftDatapackCreator;
using System.Diagnostics;
using System.IO;

namespace MinecraftDatapackCreator.FileStructure;

[DebuggerDisplay("{Name}")]
internal sealed class DatapackFileInfo : IDatapackItemInfo
{
    private int commonPrefixWithSolution;
    private int commonPrefixWithData;
    private int namespaceEndIndex;

    public event EventHandler? FileChanged;
    public event EventHandler? FileContentChanged;
    public DatapackDirectoryInfo? Parent { get; }
    public Datapack Datapack { get; }
    public string Name { get; private set; }
    public string FullName { get; private set; }
    public string? NamespacedId { get; private set; }
    public string? RelativePath { get; private set; }
    public DatapackStructureFolder? DatapackStructureFolder { get; private set; }

    public FileEditor Editor => Type is DatapackItemType.MetaFile ? FileEditor.JsonEditor : (DatapackStructureFolder?.Editor ?? FileEditor.TextEditor);


    public bool Exist => File.Exists(FullName);
    public ReadOnlySpan<char> PathRelativeToDataDirectory => commonPrefixWithData == -1 ? default : FullName.AsSpan(commonPrefixWithData + 1);
    public ReadOnlySpan<char> PathRelativeToSolution => FullName.AsSpan(commonPrefixWithSolution + 1);
    public ReadOnlySpan<char> PathRelativeToNamespace => PathRelativeToDataDirectory[(namespaceEndIndex + 1)..];
    public ReadOnlySpan<char> Namespace => namespaceEndIndex == -1 ? default : PathRelativeToDataDirectory[..namespaceEndIndex];

    public DatapackItemType Type => commonPrefixWithSolution + 12 == FullName.Length && Name == Datapack.PACK_MCMETA_FILE ? DatapackItemType.MetaFile : DatapackItemType.File;

    public bool IsNameInvalid
    {
        get
        {
            if(!Datapack.IsValidResourceName(Name))
                return true;
            var extension = DatapackStructureFolder?.FilesExtension;
            if (!Helpers.HasFileExtension(Name, extension)) 
                return true;
            return false;
        }
    }

    IDatapackItemInfo ISolutionItemInfo.ItemInfo => this;
    SolutionNodeType ISolutionItemInfo.SolutionNodeType => (SolutionNodeType)Type;

    public DatapackFileInfo(string name, DatapackDirectoryInfo parent, bool createOnDrive = true)
    {
        FullName = Path.Join(parent.FullName, name);
        Datapack = parent.Datapack;
        Parent = parent;
        Name = name;
        UpdateNamespacedId();
        FileInfo fi = new FileInfo(FullName);

        if (createOnDrive && !fi.Exists)
        {
            parent.EnsureExist();
            fi.Create().Close();
        }
    }


    public void Update(string newFullName)
    {
        FullName = newFullName;
        Name = Path.GetFileName(newFullName);
        UpdateNamespacedId();
        FileChanged?.Invoke(this, EventArgs.Empty);
    }


    private void UpdateNamespacedId()
    {
        commonPrefixWithData = Datapack.DataFolderPath.AsSpan().CommonPrefixLength(FullName);
        commonPrefixWithSolution = Datapack.Path.AsSpan().CommonPrefixLength(FullName);

        if (commonPrefixWithSolution != Datapack.Path.Length)
            throw new UnreachableException();
        if (commonPrefixWithData != Datapack.DataFolderPath.Length)
        {
            commonPrefixWithData = -1;
            return;
        }

        ReadOnlySpan<char> relative = PathRelativeToDataDirectory;


        int indexOfA = relative.IndexOf('\\');
        int indexOfB = relative.IndexOf('\\', indexOfA + 1);
        if (indexOfA == -1)
            return;
        if (indexOfB == -1)
            return;
        namespaceEndIndex = indexOfA;




        ReadOnlySpan<char> n = relative.Slice(indexOfA + 1, indexOfB - indexOfA - 1);
        DatapackStructureFolder? dpsf = Datapack.Sources.DatapackStructure.GetDatapackStructureItemByName(n);
        if (dpsf is not null)
            while (true)
            {
                indexOfA = indexOfB;
                indexOfB = relative.IndexOf('\\', indexOfA + 1);
                if (indexOfB == -1)
                    break;
                ReadOnlySpan<char> n2 = relative.Slice(indexOfA + 1, indexOfB - indexOfA - 1);
                DatapackStructureFolder? folder = dpsf.TryGetChildren()?.GetDatapackStructureItemByName(n2);
                if (folder is null)
                    break;
                dpsf = folder;
            }


        ReadOnlySpan<char> relativePath = relative[(indexOfA + 1)..];
        int lastDot = relativePath.LastIndexOf('.');
        if (lastDot != -1)
            relativePath = relativePath[..lastDot];

        if (relativePath.Length < Program.MAX_STACK_BUFFER)
        {
            Span<char> newRelativePath = stackalloc char[relativePath.Length];
            for (int i = 0; i < relativePath.Length; i++)
                if (relativePath[i] == '\\')
                    newRelativePath[i] = '/';
                else
                    newRelativePath[i] = relativePath[i];
            RelativePath = newRelativePath.ToString();
        }
        else
            RelativePath = relativePath.ToString().Replace('\\', '/');


        DatapackStructureFolder = dpsf;


        NamespacedId = $"{dpsf?.NamespacedIdPrefix}{Namespace}:{RelativePath}";
    }

    public DatapackFsOperationResult Rename(ReadOnlySpan<char> newName)
    {
        string newPath = Path.Join(Path.GetDirectoryName(FullName.AsSpan()), newName);
        if(string.Equals(newPath, FullName, StringComparison.OrdinalIgnoreCase))
        {
            Datapack.FileStructure.MovingItems.Add(new MovingItem(this, newPath));
            File.Move(FullName, newPath, false);
            FullName = newPath;
            return DatapackFsOperationResult.Success;
        }
        if (File.Exists(newPath))
            return DatapackFsOperationResult.DestinationAlreadyExist;
        DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(newPath)!);
        if (!di.Exists)
            di.Create();

        File.Move(FullName, newPath, false);

        FullName = newPath;
        return DatapackFsOperationResult.Success;
    }
    internal void Changed() => FileChanged?.Invoke(this, EventArgs.Empty);
    internal void ContentChanged() => FileContentChanged?.Invoke(this, EventArgs.Empty);

    public DatapackFsOperationResult MoveTo(DatapackDirectoryInfo destinationDirectory) => MoveTo(destinationDirectory, Name);
    public DatapackFsOperationResult CopyTo(DatapackDirectoryInfo destinationDirectory) => CopyTo(destinationDirectory, Name);


    public DatapackFsOperationResult MoveTo(DatapackDirectoryInfo destinationDirectory, ReadOnlySpan<char> newName)
    {
        string newPath = Path.Join(destinationDirectory.FullName, newName);

        if (File.Exists(newPath))
            return DatapackFsOperationResult.DestinationAlreadyExist;
        DirectoryInfo di = new DirectoryInfo(destinationDirectory.FullName);
        if (!di.Exists)
            di.Create();

        Datapack.FileStructure.MovingItems.Add(new MovingItem(this, newPath));

        File.Move(FullName, newPath, false);

        return DatapackFsOperationResult.Success;
    }
    public DatapackFsOperationResult CopyTo(DatapackDirectoryInfo destinationDirectory, ReadOnlySpan<char> newName)
    {
        string newPath = Path.Join(destinationDirectory.FullName, newName);

        if (File.Exists(newPath))
            return DatapackFsOperationResult.DestinationAlreadyExist;
        DirectoryInfo di = new DirectoryInfo(destinationDirectory.FullName);
        if (!di.Exists)
            di.Create();

        File.Copy(FullName, newPath, false);

        return DatapackFsOperationResult.Success;
    }

    public void EnsureExist()
    {
        FileInfo fi = new FileInfo(FullName);
        if (!fi.Exists)
            fi.Create().Close();
    }
}
