using System.IO;

namespace MinecraftDatapackCreator;
internal sealed class DatapackFileStructure : IDisposable
{
    private bool disposedValue;


    public event EventHandler<DatapackItemChangedEventArgs>? FileChanged;

    public Datapack Datapack { get; }
    private FileSystemWatcher FileSystemWatcher { get; }
    public DatapackFolderInfo RootFolder { get; }
    public DatapackFileStructure(Datapack datapack)
    {
        Datapack = datapack;
        FileSystemWatcher = new FileSystemWatcher
        {
            Path = datapack.Path,
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName
        };
        FileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
        FileSystemWatcher.Created += FileSystemWatcher_Created;
        FileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
        FileSystemWatcher.Error += FileSystemWatcher_Error;
        FileSystemWatcher.EnableRaisingEvents = true;

        RootFolder = new DatapackFolderInfo(datapack.Path, datapack);
    }

    private void FileSystemWatcher_Error(object sender, ErrorEventArgs e) => Reload();

    private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
    {
        DatapackFolderInfo? parent = RootFolder.GetFolder(Path.GetDirectoryName(e.FullPath));
        if (parent is null)
        {
            Reload();
            return;
        }
        DatapackFileInfo? file = parent.GetFile(e.FullPath);
        if (file is not null)
        {
            parent.RemoveFile(file);
            FileChanged?.Invoke(this, new DatapackItemChangedEventArgs(file, DatapackFileChangedType.Delated));
            return;
        }
        DatapackFolderInfo? folder = parent.GetFolder(e.FullPath);
        if (folder is not null)
        {
            parent.RemoveFolder(folder);
            FileChanged?.Invoke(this, new DatapackItemChangedEventArgs(folder, DatapackFileChangedType.Delated));
            return;
        }
        Reload();
    }
    private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
    {
        if (File.Exists(e.FullPath))
        {
            DatapackFolderInfo? folder = RootFolder.GetFolder(Path.GetDirectoryName(e.FullPath.AsSpan()));
            DatapackFileInfo newItem = folder!.CreateFile(e.FullPath);
            FileChanged?.Invoke(this, new DatapackItemChangedEventArgs(newItem, DatapackFileChangedType.Created));
        }
        else if (Directory.Exists(e.FullPath))
        {
            DatapackFolderInfo? folder = RootFolder.GetFolder(Path.GetDirectoryName(e.FullPath.AsSpan()));
            DatapackFolderInfo newItem = folder!.CreateFolder(e.FullPath);
            FileChanged?.Invoke(this, new DatapackItemChangedEventArgs(newItem, DatapackFileChangedType.Created));
        }

    }

    private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
    {
        if (Directory.Exists(e.FullPath))
        {
            DatapackFolderInfo? dir = RootFolder.GetFolder(e.OldFullPath);

            dir!.Update(e.FullPath);
            FileChanged?.Invoke(this, new DatapackItemRenamedEventArgs(dir, DatapackFileChangedType.Renamed,e.OldFullPath));
        }


        if (!File.Exists(e.FullPath))
            return;
        DatapackFileInfo? item = RootFolder.GetFile(e.OldFullPath);
        if (item is null)
        {
            Reload();
            return;
        }
        item.Update(e.FullPath);
        FileChanged?.Invoke(this, new DatapackItemRenamedEventArgs(item, DatapackFileChangedType.Renamed,e.OldFullPath));
    }
    private void Reload()
    {
        throw new Exception("Something went worng during indexing files");
    }
    public DatapackFolderInfo? GetDirectory(ReadOnlySpan<char> path) => RootFolder.GetFolder(path);
    public DatapackFolderInfo? GetRelativeDirectory(ReadOnlySpan<char> path) => RootFolder.GetRelativeFolder(path);
    public DatapackFileInfo? GetFile(ReadOnlySpan<char> path) => RootFolder.GetFile(path);
    public IEnumerable<DatapackFileInfo> GetFiles() => RootFolder.GetAllFiles();
    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                FileSystemWatcher.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    ~DatapackFileStructure()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
internal abstract class DatapackItemInfo
{
    public abstract string Name { get; protected set; }
    public abstract string FullName { get; protected set; }
    public DatapackStructureFolder? DatapackStructureFolder { get; protected set; }
    public DatapackFolderInfo? Parent { get; protected set; }


    public abstract Datapack Datapack { get; }

    public override string ToString() => Name;
}
internal sealed class DatapackFolderInfo : DatapackItemInfo
{
    public override string Name { get; protected set; }
    public override string FullName { get; protected set; }
    public DatapackFolderType Type { get; private set; }
    public override Datapack Datapack { get; }
    private List<DatapackFileInfo> Files { get; } = new List<DatapackFileInfo>();
    private List<DatapackFolderInfo> Folders { get; } = new List<DatapackFolderInfo>();
    public DatapackFolderInfo(string fullName, Datapack datapack)
    {
        Datapack = datapack;
        FullName = fullName;
        Name = Path.GetFileName(fullName);
        LoadStructure();
    }
    public DatapackFolderInfo(string name, DatapackFolderInfo parent)
    {
        Datapack = parent.Datapack;
        FullName = Path.Combine(parent.FullName, name);
        Name = Path.GetFileName(FullName);
        Parent = parent;
        parent.AddFolder(this);
        LoadStructure();
    }

    private void LoadStructure()
    {
        DirectoryInfo di = new DirectoryInfo(FullName);
        foreach (DirectoryInfo item in di.GetDirectories())
        {
            new DatapackFolderInfo(item.Name, this);
        }
        foreach (FileInfo item in di.GetFiles())
        {
            new DatapackFileInfo(item.Name, this);
        }

    }
    internal void Update(string newFullName)
    {
        FullName = newFullName;
        Name = Path.GetFileName(newFullName);

        foreach (DatapackFolderInfo item in Folders)
        {
            item.Update(Path.Combine(FullName, item.Name));
        }
        foreach (DatapackFileInfo item in Files)
        {
            item.Update(Path.Combine(FullName, item.Name));
        }
    }

    public DatapackFileInfo? GetFile(ReadOnlySpan<char> path) => GetRelativeFile(Path.GetRelativePath(FullName, path.ToString()).TrimEnd('\\'));
    public DatapackFolderInfo? GetFolder(ReadOnlySpan<char> path) => path.SequenceEqual(FullName) ? this : GetRelativeFolder(Path.GetRelativePath(FullName, path.ToString()).TrimEnd('\\'));
    public DatapackFileInfo CreateFile(ReadOnlySpan<char> path) => CreateRelativeFile(Path.GetRelativePath(FullName, path.ToString()).TrimEnd('\\'));
    public DatapackFolderInfo CreateFolder(ReadOnlySpan<char> path) => path.SequenceEqual(FullName) ? this : CreateRelativeFolder(Path.GetRelativePath(FullName, path.ToString()).TrimEnd('\\'));

    public DatapackFileInfo? GetRelativeFile(ReadOnlySpan<char> path)
    {
        if (path.Contains('\\'))
        {
            return GetFileFormFolder(path);
        }
        else
        {
            return GetFileInternal(path);
        }
    }

    public DatapackFolderInfo? GetRelativeFolder(ReadOnlySpan<char> path)
    {
        if (path.Contains('\\'))
        {
            return GetFolderFormFolder(path);
        }
        else
        {
            return GetFolderInternal(path);
        }
    }
    private DatapackFolderInfo? GetFolderInternal(ReadOnlySpan<char> path)
    {
        foreach (DatapackFolderInfo item in Folders)
        {
            if (path.SequenceEqual(item.Name))
                return item;
        }
        return null;
    }
    private DatapackFolderInfo? GetFolderFormFolder(ReadOnlySpan<char> path)
    {
        ReadOnlySpan<char> folder = path[..path.IndexOf('\\')];
        foreach (DatapackFolderInfo item in Folders)
        {
            if (folder.SequenceEqual(item.Name))
                return item.GetRelativeFolder(path[(path.IndexOf('\\') + 1)..]);
        }
        return null;
    }

    private DatapackFileInfo? GetFileInternal(ReadOnlySpan<char> path)
    {
        foreach (DatapackFileInfo item in Files)
        {
            if (path.SequenceEqual(item.Name))
                return item;
        }
        return null;
    }
    private DatapackFileInfo? GetFileFormFolder(ReadOnlySpan<char> path)
    {
        ReadOnlySpan<char> folder = path[..path.IndexOf('\\')];
        foreach (DatapackFolderInfo item in Folders)
        {
            if (folder.SequenceEqual(item.Name))
                return item.GetRelativeFile(path[(path.IndexOf('\\') + 1)..]);
        }
        return null;
    }
    public void RemoveFile(DatapackFileInfo file)
    {
        Files.Remove(file);
    }
    public void AddFile(DatapackFileInfo file)
    {
        Files.Add(file);
    }
    public void RemoveFolder(DatapackFolderInfo folder)
    {
        Folders.Remove(folder);
    }
    public void AddFolder(DatapackFolderInfo folder)
    {
        Folders.Add(folder);
    }
    public IEnumerable<DatapackFolderInfo> GetFolders() => Folders;
    public IEnumerable<DatapackFileInfo> GetFiles() => Files;
    public DatapackFileInfo CreateRelativeFile(ReadOnlySpan<char> path)
    {
        if (path.Contains('\\'))
        {
            ReadOnlySpan<char> folder = path[..path.IndexOf('\\')];
            foreach (DatapackFolderInfo item in Folders)
            {
                if (folder.SequenceEqual(item.Name))
                    return item.CreateRelativeFile(path[(path.IndexOf('\\') + 1)..]);
            }
            DatapackFolderInfo newFolder = new DatapackFolderInfo(folder.ToString(), this);
            return newFolder.CreateRelativeFile(path[(path.IndexOf('\\') + 1)..]);
        }
        else
        {

            DatapackFileInfo? existingFile = GetRelativeFile(path);
            if (existingFile is not null)
                return existingFile;

            return new DatapackFileInfo(path.ToString(), this);

        }
    }
    public DatapackFolderInfo CreateRelativeFolder(ReadOnlySpan<char> path)
    {
        if (path.Contains('\\'))
        {
            ReadOnlySpan<char> folder = path[..path.IndexOf('\\')];
            foreach (DatapackFolderInfo item in Folders)
            {
                if (folder.SequenceEqual(item.Name))
                    return item.CreateRelativeFolder(path[(path.IndexOf('\\') + 1)..]);
            }
            DatapackFolderInfo newFolder = new DatapackFolderInfo(folder.ToString(), this);
            return newFolder.CreateRelativeFolder(path[(path.IndexOf('\\') + 1)..]);
        }
        else
        {

            DatapackFolderInfo? existingFolder = GetRelativeFolder(path);
            if (existingFolder is not null)
                return existingFolder;

            return new DatapackFolderInfo(path.ToString(), this);

        }
    }
    public IEnumerable<DatapackFileInfo> GetAllFiles()
    {
        List<DatapackFileInfo> list = new List<DatapackFileInfo>();
        GetAllFiles(list);
        return list;
    }
    private void GetAllFiles(List<DatapackFileInfo> files)
    {
        files.AddRange(Files);
        foreach (DatapackFolderInfo item in Folders)
        {
            item.GetAllFiles(files);
        }
    }
}

internal enum DatapackFolderType
{
    Root,
    Data,
    Namespace,
    Strucute,
    Normal
}

internal sealed class DatapackFileInfo : DatapackItemInfo
{
    public event EventHandler? FileChanged;
    public override string Name { get; protected set; }
    public override string FullName { get; protected set; }
    public override Datapack Datapack { get; }
    public string? NamespacedId { get; private set; }
    public string? Namespace { get; private set; }
    public string? RelativePath { get; private set; }

    public DatapackFileInfo(string fullName, Datapack datapack)
    {
        FullName = fullName;
        Datapack = datapack;

        Name = Path.GetFileName(fullName);
        UpdateNamespacedId();
    }
    public DatapackFileInfo(string name, DatapackFolderInfo parent)
    {
        FullName = Path.Combine(parent.FullName, name);
        Datapack = parent.Datapack;
        Parent = parent;
        parent.AddFile(this);
        Name = Path.GetFileName(FullName);
        UpdateNamespacedId();
    }


    internal void Update(string newFullName)
    {
        FullName = newFullName;
        Name = Path.GetFileName(newFullName);
        UpdateNamespacedId();
        FileChanged?.Invoke(this, EventArgs.Empty);
    }


    private void UpdateNamespacedId()
    {
        string dataFolder = Path.Combine(Datapack.Path, Datapack.DATA_FOLDER_NAME);

        string relative = Path.GetRelativePath(dataFolder, FullName);

        if (relative.Length == 0)
        {
            return;
        }

        string[] splitted = relative.Split('\\');


        if (splitted.Length < 3)
        {
            return;
        }
        Namespace = splitted[0];

        int skip = 1;
        DatapackStructureFolder? dpsf = Datapack.DatapackStructure.GetDatapackStructureItemByName(splitted[skip]);
        if (dpsf is not null)
        {
            skip++;
            while (true)
            {

                DatapackStructureFolder? folder = dpsf.Children.GetDatapackStructureItemByName(splitted[skip]);
                if (folder is null)
                    break;
                dpsf = folder;
                skip++;
            }
        }


        string relativePath = string.Join('/', splitted, skip, splitted.Length - skip);

        RelativePath = relativePath.Contains('.', StringComparison.Ordinal) ? relativePath.AsSpan(0, relativePath.LastIndexOf('.')).ToString() : relativePath;
        NamespacedId = $"{dpsf?.NamespacedIdPrefix}{Namespace}:{RelativePath}";
        DatapackStructureFolder = dpsf;
    }

}

internal class DatapackItemChangedEventArgs : EventArgs
{
    public DatapackItemInfo Subject { get; }
    public DatapackFileChangedType ChangedType { get; }
    public DatapackItemChangedEventArgs(DatapackItemInfo subject, DatapackFileChangedType changedType)
    {
        Subject = subject;
        ChangedType = changedType;
    }
}
internal sealed class DatapackItemRenamedEventArgs : DatapackItemChangedEventArgs
{
    public string OldPath { get; }
    public DatapackItemRenamedEventArgs(DatapackItemInfo subject, DatapackFileChangedType changedType, string oldPath) : base(subject, changedType)
    {
        OldPath = oldPath;
    }
}
internal enum DatapackFileChangedType
{
    Created,
    Renamed,
    Delated,
    PathChanged
}