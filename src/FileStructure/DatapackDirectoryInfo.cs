using MinecraftDatapackCreator;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MinecraftDatapackCreator.FileStructure;
[DebuggerDisplay("{Name}")]
internal sealed class DatapackDirectoryInfo : IDatapackItemInfo
{
    private List<DatapackFileInfo>? files;
    private List<DatapackDirectoryInfo>? directories;
    private int commonPrefix;
    private int commonPrefixWithSolution;
    private int namespaceEndIndex;

    public string Name { get; private set; }
    public string FullName { get; private set; }
    public Datapack Datapack { get; }
    public bool Exist => Directory.Exists(FullName);
    public DatapackItemType Type { get; private set; }
    public DatapackStructureFolder? DatapackStructureFolder { get; private set; }
    public DatapackDirectoryInfo? Parent { get; }
    public ReadOnlySpan<char> PathRelativeToDataDirectory => commonPrefix + 1 >= FullName.Length ? string.Empty : FullName.AsSpan(commonPrefix + 1);
    public ReadOnlySpan<char> PathRelativeToSolution => commonPrefixWithSolution + 1 >= FullName.Length ? string.Empty : FullName.AsSpan(commonPrefixWithSolution + 1);
    public ReadOnlySpan<char> Namespace => namespaceEndIndex == -1 ? default : PathRelativeToDataDirectory[..namespaceEndIndex];
    public ReadOnlySpan<char> PathRelativeToNamespace => PathRelativeToDataDirectory[(namespaceEndIndex + 1)..];
    public bool IsNameInvalid => !Datapack.IsValidResourceName(Name);

    IDatapackItemInfo ISolutionItemInfo.ItemInfo => this;
    SolutionNodeType ISolutionItemInfo.SolutionNodeType => (SolutionNodeType)Type;

    public DatapackDirectoryInfo(string fullName, Datapack datapack, bool createOnDrive = true)
    {
        Datapack = datapack;
        FullName = fullName;
        Name = Path.GetFileName(fullName);
        UpdateStructureFolder();
        LoadStructure(createOnDrive);
    }
    public DatapackDirectoryInfo(string name, DatapackDirectoryInfo parent, bool createOnDrive = true)
    {
        Datapack = parent.Datapack;
        FullName = Path.Join(parent.FullName, name);
        Name = name;
        Parent = parent;
        UpdateStructureFolder();
        LoadStructure(createOnDrive);
    }

    public List<DatapackFileInfo>? TryGetFiles() => files;
    public List<DatapackDirectoryInfo>? TryGetDirectories() => directories;

    private void LoadStructure(bool createOnDrive)
    {
        DirectoryInfo di = new DirectoryInfo(FullName);
        if (!createOnDrive && !di.Exists)
            return;
        if (!di.Exists)
            di.Create();

        DirectoryInfo[] dirs = di.GetDirectories();
        if (dirs.Length > 0)
        {
            directories ??= new List<DatapackDirectoryInfo>(dirs.Length);
            for (int i = 0; i < dirs.Length; i++)
            {
                directories.Add(new DatapackDirectoryInfo(dirs[i].Name, this));
            }
        }

        FileInfo[] files = di.GetFiles();
        if (files.Length > 0)
        {
            this.files ??= new List<DatapackFileInfo>(files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                this.files.Add(new DatapackFileInfo(files[i].Name, this));
            }
        }
    }
    public void Update(string newFullName)
    {
        FullName = newFullName;
        Name = Path.GetFileName(newFullName);
        UpdateStructureFolder();
        if (directories is not null)
            foreach (DatapackDirectoryInfo item in directories)
                item.Update(Path.Join(FullName, item.Name));
        if (files is not null)
            foreach (DatapackFileInfo item in files)
                item.Update(Path.Join(FullName, item.Name));
    }

    private void UpdateStructureFolder()
    {
        commonPrefix = Datapack.DataFolderPath.AsSpan().CommonPrefixLength(FullName);
        commonPrefixWithSolution = Datapack.Path.AsSpan().CommonPrefixLength(FullName);

        if (commonPrefixWithSolution == FullName.Length && Name == Datapack.Name)
        {
            Type = DatapackItemType.SolutionDirectory;
            return;
        }
        if (commonPrefix == FullName.Length && Name == Datapack.DATA_FOLDER_NAME)
        {
            Type = DatapackItemType.DataDirectory;
            return;
        }

        if (commonPrefix == FullName.Length)
            return;

        ReadOnlySpan<char> relative = PathRelativeToDataDirectory;

        int indexOfA = relative.IndexOf('\\');
        int indexOfB = relative.IndexOf('\\', indexOfA + 1);

        namespaceEndIndex = indexOfA;

        if (indexOfA == -1) // No \\ in path
        {
            Type = DatapackItemType.Namespace;
            return;
        }
        if (indexOfB == -1) // Directory in namespace
            indexOfB = relative.Length;

        ReadOnlySpan<char> n = relative.Slice(indexOfA + 1, indexOfB - indexOfA - 1); // Directory name after namespace

        DatapackStructureFolder? dpsf = Datapack.Sources.DatapackStructure.GetDatapackStructureItemByName(n); // First directory name

        if (dpsf is not null && indexOfB == relative.Length) // Structure folder in namespace
        {
            Type = DatapackItemType.StructureFolder;

            DatapackStructureFolder = dpsf;
            return;
        }
        if (dpsf?.TryGetChildren() is not null)
            while (true)
            {
                indexOfA = indexOfB;
                if (indexOfA == relative.Length)
                {
                    Type = DatapackItemType.StructureFolder;
                    break;
                }
                indexOfB = relative.IndexOf('\\', indexOfA + 1);
                if (indexOfB == -1)
                    indexOfB = relative.Length;
                ReadOnlySpan<char> n2 = relative.Slice(indexOfA + 1, indexOfB - indexOfA - 1);
                DatapackStructureFolder? folder = dpsf.TryGetChildren()?.GetDatapackStructureItemByName(n2);
                if (folder is null)
                {
                    Type = DatapackItemType.Directory;
                    break;
                }
                else if (indexOfB == relative.Length)
                {
                    Type = DatapackItemType.StructureFolder;
                }
                dpsf = folder;
            }
        else
            Type = DatapackItemType.Directory;


        DatapackStructureFolder = dpsf;


    }
    private ReadOnlySpan<char> GetRelativePath(ReadOnlySpan<char> path)
    {
        int common = path.CommonPrefixLength(FullName);
        if (common != FullName.Length)
            return default;
        return path[(common + 1)..];
    }
    public DatapackFileInfo? GetAbsoluteFile(ReadOnlySpan<char> path) => GetRelativeFile(GetRelativePath(path).TrimEnd('\\'));
    public DatapackDirectoryInfo? GetAbsoluteDirectory(ReadOnlySpan<char> path) => path.SequenceEqual(FullName) ? this : GetRelativeDirectory(GetRelativePath(path).TrimEnd('\\'));
    public DatapackFileInfo CreateAbsoluteFile(ReadOnlySpan<char> path) => CreateRelativeFile(GetRelativePath(path).TrimEnd('\\'));
    public DatapackDirectoryInfo CreateAbsoluteDirectory(ReadOnlySpan<char> path) => path.SequenceEqual(FullName) ? this : CreateRelativeDirectory(GetRelativePath(path).TrimEnd('\\'));

    public DatapackFileInfo? GetRelativeFile(ReadOnlySpan<char> path)
    {
        int indexOf = path.IndexOf('\\');
        int indexOf2 = path.IndexOf('/');
        if ((indexOf2 < indexOf && indexOf2 != -1) || indexOf == -1)
        {
            indexOf = indexOf2;
        }
        if (indexOf != -1)
            return GetFileFormSubdirectory(path, indexOf);
        else
            return GetFileInternal(path);
    }

    public DatapackDirectoryInfo? GetRelativeDirectory(ReadOnlySpan<char> path)
    {
        int indexOf = path.IndexOf('\\');
        int indexOf2 = path.IndexOf('/');
        if ((indexOf2 < indexOf && indexOf2 != -1) || indexOf == -1)
        {
            indexOf = indexOf2;
        }

        if (indexOf != -1)
            return GetDirectoryFormSubdirectory(path, indexOf);
        else
            return GetDirectoryInternal(path);
    }
    private DatapackDirectoryInfo? GetDirectoryInternal(ReadOnlySpan<char> path)
    {
        if (directories is null)
            return null;
        foreach (DatapackDirectoryInfo item in directories)
            if (path.SequenceEqual(item.Name))
                return item;
        return null;
    }
    private DatapackDirectoryInfo? GetDirectoryFormSubdirectory(ReadOnlySpan<char> path, int indexOf)
    {
        if (directories is null)
            return null;
        if (indexOf == 0)
            return GetRelativeDirectory(path[1..]);
        ReadOnlySpan<char> folder = path[..indexOf];
        foreach (DatapackDirectoryInfo item in directories)
            if (folder.SequenceEqual(item.Name))
                return item.GetRelativeDirectory(path[(indexOf + 1)..]);
        return null;
    }

    private DatapackFileInfo? GetFileInternal(ReadOnlySpan<char> path)
    {
        if (files is null)
            return null;
        foreach (DatapackFileInfo item in files)
            if (path.SequenceEqual(item.Name))
                return item;
        return null;
    }
    private DatapackFileInfo? GetFileFormSubdirectory(ReadOnlySpan<char> path, int indexOf)
    {
        if (directories is null)
            return null;
        if (indexOf == 0)
            return GetRelativeFile(path[1..]);
        ReadOnlySpan<char> folder = path[..indexOf];
        foreach (DatapackDirectoryInfo item in directories)
            if (folder.SequenceEqual(item.Name))
                return item.GetRelativeFile(path[(indexOf + 1)..]);
        return null;
    }
    public void RemoveFile(DatapackFileInfo file)
    {
        if (files is null)
            return;
        files.Remove(file);
    }
    public void AddFile(DatapackFileInfo file)
    {
        EnsureFiles();
        files.Add(file);
    }
    public void RemoveDirectory(DatapackDirectoryInfo folder)
    {
        if (directories is null)
            return;
        directories.Remove(folder);
    }
    public void AddDirectory(DatapackDirectoryInfo folder)
    {
        EnsureDirectories();
        directories.Add(folder);
    }

    [MemberNotNull(nameof(files))]
    private void EnsureFiles() => files ??= new List<DatapackFileInfo>();
    [MemberNotNull(nameof(directories))]
    private void EnsureDirectories() => directories ??= new List<DatapackDirectoryInfo>();

    public DatapackFileInfo CreateRelativeFile(ReadOnlySpan<char> path, bool createOnDrive = true)
    {
        int indexOf = path.IndexOf('\\');
        if (indexOf != -1)
        {
            ReadOnlySpan<char> folder = path[..indexOf];
            if (directories is not null)
            {
                foreach (DatapackDirectoryInfo item in directories)
                {
                    if (folder.SequenceEqual(item.Name))
                        return item.CreateRelativeFile(path[(indexOf + 1)..], createOnDrive);
                }
            }


            EnsureDirectories();
            DatapackDirectoryInfo newFolder = new DatapackDirectoryInfo(folder.ToString(), this, createOnDrive);
            directories.Add(newFolder);
            return newFolder.CreateRelativeFile(path[(indexOf + 1)..]);
        }
        else
        {

            DatapackFileInfo? existingFile = GetRelativeFile(path);
            if (existingFile is not null)
                return existingFile;
            DatapackFileInfo newFile = new DatapackFileInfo(path.ToString(), this, createOnDrive);
            EnsureFiles();
            files.Add(newFile);
            return newFile;

        }
    }
    public DatapackDirectoryInfo CreateRelativeDirectory(ReadOnlySpan<char> path, bool createOnDrive = true)
    {
        int indexOf = path.IndexOf('\\');
        if (indexOf != -1)
        {
            ReadOnlySpan<char> folder = path[..indexOf];
            if (directories is not null)
            {
                foreach (DatapackDirectoryInfo item in directories)
                {
                    if (folder.SequenceEqual(item.Name))
                        return item.CreateRelativeDirectory(path[(indexOf + 1)..], createOnDrive);
                }

            }
            DatapackDirectoryInfo newFolder = new DatapackDirectoryInfo(folder.ToString(), this, createOnDrive);
            EnsureDirectories();
            directories.Add(newFolder);
            return newFolder.CreateRelativeDirectory(path[(indexOf + 1)..], createOnDrive);
        }
        else
        {

            DatapackDirectoryInfo? existingFolder = GetRelativeDirectory(path);
            if (existingFolder is not null)
                return existingFolder;
            DatapackDirectoryInfo newFolder = new DatapackDirectoryInfo(path.ToString(), this, createOnDrive);
            EnsureDirectories();
            directories.Add(newFolder);
            return newFolder;

        }
    }
    public List<DatapackFileInfo> GetAllFiles()
    {
        List<DatapackFileInfo> list = new List<DatapackFileInfo>();
        GetAllFiles(list);
        return list;
    }
    private void GetAllFiles(List<DatapackFileInfo> files)
    {
        if (this.files is not null)
            files.AddRange(this.files);
        if (directories is not null)
            foreach (DatapackDirectoryInfo item in directories)
                item.GetAllFiles(files);
    }

    public DatapackFsOperationResult Rename(ReadOnlySpan<char> newName)
    {
        string newPath = Path.Join(Path.GetDirectoryName(FullName.AsSpan()), newName);
        if (Directory.Exists(newPath))
            return DatapackFsOperationResult.DestinationAlreadyExist;
        DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(newPath)!);
        if (!di.Exists)
            di.Create();
        Directory.Move(FullName, newPath);

        FullName = newPath;
        return DatapackFsOperationResult.Success;
    }
    public DatapackFsOperationResult MoveTo(DatapackDirectoryInfo destinationDirectory) => MoveTo(destinationDirectory, Name);
    public DatapackFsOperationResult CopyTo(DatapackDirectoryInfo destinationDirectory) => CopyTo(destinationDirectory, Name);

    public DatapackFsOperationResult MoveTo(DatapackDirectoryInfo destinationDirectory, ReadOnlySpan<char> newName)
    {
        string newPath = Path.Join(destinationDirectory.FullName, newName);

        int commonPrefix = newPath.AsSpan().CommonPrefixLength(FullName);
        if (commonPrefix == FullName.Length)
            return DatapackFsOperationResult.DestinationInsideSubject;

        if (Directory.Exists(newPath))
            return DatapackFsOperationResult.DestinationAlreadyExist;
        DirectoryInfo di = new DirectoryInfo(destinationDirectory.FullName);
        if (!di.Exists)
            di.Create();

        Datapack.FileStructure.MovingItems.Add(new MovingItem(this, newPath));


        Directory.Move(FullName, newPath);
        return DatapackFsOperationResult.Success;
    }
    public DatapackFsOperationResult CopyTo(DatapackDirectoryInfo destinationDirectory, ReadOnlySpan<char> newName)
    {
        int commonPrefix = destinationDirectory.FullName.AsSpan().CommonPrefixLength(FullName);
        if (commonPrefix == FullName.Length)
            return DatapackFsOperationResult.DestinationInsideSubject;
        if (destinationDirectory.GetRelativeDirectory(newName) is not null)
            return DatapackFsOperationResult.DestinationAlreadyExist;

        DatapackDirectoryInfo newDatapackDir = destinationDirectory.CreateRelativeDirectory(newName);

        if (files is not null)
        {
            foreach (DatapackFileInfo file in files)
            {
                file.CopyTo(newDatapackDir);
            }
        }
        if (directories is not null)
        {
            foreach (DatapackDirectoryInfo dir in directories)
            {
                dir.CopyTo(newDatapackDir);
            }
        }



        return DatapackFsOperationResult.Success;
    }

    public void EnsureExist()
    {
        DirectoryInfo di = new DirectoryInfo(FullName);
        if(!di.Exists)
            di.Create();
    }
}
