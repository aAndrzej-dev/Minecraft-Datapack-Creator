using System.Diagnostics;
using System.IO;
using System.Windows.Threading;

namespace MinecraftDatapackCreator.FileStructure;
internal sealed class DatapackFileStructure : IDisposable
{
    private readonly FileSystemWatcher fileSystemWatcher;
    private Dispatcher fsDispatcher;
    private bool disposedValue;
    private List<MovingItem>? movingItems;
    public event EventHandler<DatapackItemChangedEventArgs>? ItemCreated;
    public event EventHandler<DatapackItemChangedEventArgs>? ItemDelated;
    public event EventHandler<DatapackItemRenamedEventArgs>? ItemRenamed;


    public List<MovingItem> MovingItems => movingItems ??= new List<MovingItem>();
    public DatapackDirectoryInfo RootFolder { get; }
    private DatapackFileStructure(Datapack datapack, bool preload = false)
    {
        using ManualResetEvent fsDispatcherInitialized = new ManualResetEvent(false);
        new Thread(() =>
        {
            fsDispatcher = Dispatcher.CurrentDispatcher;
            fsDispatcherInitialized.Set();
            Dispatcher.Run();
        })
        { IsBackground = true }.Start();

        fsDispatcherInitialized.WaitOne();
        if(fsDispatcher is null)
            throw new UnreachableException();

        fileSystemWatcher = new FileSystemWatcher
        {
            Path = datapack.Path,
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite
        };
        fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
        fileSystemWatcher.Created += FileSystemWatcher_Created;
        fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
        fileSystemWatcher.Error += FileSystemWatcher_Error;
        fileSystemWatcher.Changed += FileSystemWatcher_Changed;
        fileSystemWatcher.InternalBufferSize = 256 * 256;


        RootFolder = new DatapackDirectoryInfo(datapack.Path, datapack);
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    internal static DatapackFileStructure Load(Datapack datapack) => new DatapackFileStructure(datapack);
    internal static Task<DatapackFileStructure> LoadAsync(Datapack datapack) => Task.Run(() => new DatapackFileStructure(datapack));
    private bool suspendChangedEvent;
    private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (suspendChangedEvent)
            return;
        suspendChangedEvent = true;
        fsDispatcher.BeginInvoke(() =>
        {
            try
            {
                DatapackDirectoryInfo? parent = RootFolder.GetAbsoluteDirectory(Path.GetDirectoryName(e.FullPath.AsSpan()));
                if (parent is null)
                {
                    return;
                }
                DatapackFileInfo? file = parent.GetRelativeFile(Path.GetFileName(e.FullPath.AsSpan()));
                if (file is not null)
                {
                    file.ContentChanged();
                }
            }
            finally
            {
                suspendChangedEvent = false;
            }
        });
    }
    private void FileSystemWatcher_Error(object sender, ErrorEventArgs e) => throw e.GetException();

    private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
    {
        fsDispatcher.BeginInvoke(() =>
        {
            DatapackDirectoryInfo? parent = RootFolder.GetAbsoluteDirectory(Path.GetDirectoryName(e.FullPath.AsSpan()));
            if (parent is null)
            {
                Reload();
                return;
            }
            DatapackFileInfo? file = parent.GetRelativeFile(Path.GetFileName(e.FullPath.AsSpan()));
            if (file is not null)
            {
                parent.RemoveFile(file);
                file.Changed();
                ItemDelated?.Invoke(this, new DatapackItemChangedEventArgs(file));
                return;
            }
            DatapackDirectoryInfo? folder = parent.GetRelativeDirectory(Path.GetFileName(e.FullPath.AsSpan()));
            if (folder is not null)
            {
                parent.RemoveDirectory(folder);
                ItemDelated?.Invoke(this, new DatapackItemChangedEventArgs(folder));
                return;
            }
            Reload();
        });

    }
    private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
    {
        fsDispatcher.BeginInvoke(() =>
        {
            MovingItem movingItem = MovingItems.FirstOrDefault(x => x.newPath == e.FullPath);
            if (movingItem != default)
            {
                MovingItems.Remove(movingItem);
                DatapackDirectoryInfo? newParent = RootFolder.GetAbsoluteDirectory(Path.GetDirectoryName(e.FullPath.AsSpan()));

                if (movingItem.item is DatapackFileInfo dfi)
                {
                    movingItem.item.Parent?.RemoveFile(dfi);
                    newParent?.AddFile(dfi);
                }
                else if (movingItem.item is DatapackDirectoryInfo ddi)
                {
                    movingItem.item.Parent?.RemoveDirectory(ddi);
                    newParent?.AddDirectory(ddi);
                }
                movingItem.item.Update(movingItem.newPath);



                ItemCreated?.Invoke(this, new DatapackItemChangedEventArgs(movingItem.item));
                return;
            }
            if (File.Exists(e.FullPath))
            {
                DatapackDirectoryInfo? folder = RootFolder.GetAbsoluteDirectory(Path.GetDirectoryName(e.FullPath.AsSpan()));
                DatapackFileInfo newItem = folder!.CreateAbsoluteFile(e.FullPath);
                ItemCreated?.Invoke(this, new DatapackItemChangedEventArgs(newItem));
            }
            else if (Directory.Exists(e.FullPath))
            {
                DatapackDirectoryInfo? folder = RootFolder.GetAbsoluteDirectory(Path.GetDirectoryName(e.FullPath.AsSpan()));
                DatapackDirectoryInfo newItem = folder!.CreateAbsoluteDirectory(e.FullPath);
                ItemCreated?.Invoke(this, new DatapackItemChangedEventArgs(newItem));
            }
        });

    }

    private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
    {
        fsDispatcher.BeginInvoke(() =>
        {
            MovingItem movingItem = MovingItems.FirstOrDefault(x => x.newPath == e.FullPath);
            if (movingItem != default)
            {
                MovingItems.Remove(movingItem);
                DatapackDirectoryInfo? newParent = movingItem.item.Parent;

                if (movingItem.item is DatapackFileInfo dfi)
                {
                    movingItem.item.Parent?.RemoveFile(dfi);
                    newParent?.AddFile(dfi);
                }
                else if (movingItem.item is DatapackDirectoryInfo ddi)
                {
                    movingItem.item.Parent?.RemoveDirectory(ddi);
                    newParent?.AddDirectory(ddi);
                }
                movingItem.item.Update(movingItem.newPath);

                ItemRenamed?.Invoke(this, new DatapackItemRenamedEventArgs(movingItem.item, e.OldFullPath));
                return;
            }
            if (Directory.Exists(e.FullPath))
            {
                DatapackDirectoryInfo? dir = RootFolder.GetAbsoluteDirectory(e.OldFullPath);

                dir!.Update(e.FullPath);
                ItemRenamed?.Invoke(this, new DatapackItemRenamedEventArgs(dir, e.OldFullPath));
                return;
            }


            if (!File.Exists(e.FullPath))
                return;
            DatapackFileInfo? item = RootFolder.GetAbsoluteFile(e.OldFullPath);
            if (item is null)
            {
                Reload();
                return;
            }
            item.Update(e.FullPath);
            ItemRenamed?.Invoke(this, new DatapackItemRenamedEventArgs(item, e.OldFullPath));
        });
    }
    private void Reload()
    {
        throw new Exception("Something went wrong during indexing files");
    }
    public IEnumerable<DatapackFileInfo> GetFiles() => RootFolder.GetAllFiles();
    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                // TODO: dispose managed state (managed objects)
                fileSystemWatcher.Dispose();

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
internal readonly struct MovingItem : IEquatable<MovingItem>
{
    public readonly IDatapackItemInfo item;
    public readonly string newPath;

    public MovingItem(IDatapackItemInfo item, string newPath)
    {
        this.item = item;
        this.newPath = newPath;
    }

    public override bool Equals(object? obj) => obj is MovingItem item && Equals(item);
    public bool Equals(MovingItem other) => EqualityComparer<IDatapackItemInfo>.Default.Equals(item, other.item) && newPath == other.newPath;
    public override int GetHashCode() => HashCode.Combine(item, newPath);

    public static bool operator ==(MovingItem left, MovingItem right) => left.Equals(right);
    public static bool operator !=(MovingItem left, MovingItem right) => !(left == right);
}