using Newtonsoft.Json.Linq;

namespace MinecraftDatapackCreator;
internal sealed class MinecraftFolder
{
    private readonly List<MinecraftFile>? files;
    private readonly List<MinecraftFolder>? folders;

    public string Path { get; }
    public string Name { get; }
    public MinecraftFolder(JObject obj, MinecraftFolder? parent)
    {
        Name = (string?)obj["name"] ?? throw new ArgumentException("JSON property 'name' doesn't exist", nameof(parent));
        if (parent is null)
            Path = Name;
        else
            Path = System.IO.Path.Join(parent.Path, Name);
        JArray? children = obj["children"] as JArray;
        if (children is not null)
        {
            folders = new List<MinecraftFolder>(children.Count);
            for (int i = 0; i < children.Count; i++)
            {
                JObject child = (JObject)children[i];
                folders.Add(new MinecraftFolder(child, this));
            }
        }
        JArray? files = obj["files"] as JArray;
        if (files is not null)
        {
            this.files = new List<MinecraftFile>(files.Count);
            for (int i = 0; i < files.Count; i++)
            {
                JObject file = (JObject)files[i];
                this.files.Add(new MinecraftFile(file, this));
            }
        }
    }

    public List<MinecraftFile> GetAllFiles()
    {
        List<MinecraftFile> list = new List<MinecraftFile>();
        GetAllFiles(list);
        return list;
    }
    private void GetAllFiles(List<MinecraftFile> list)
    {
        if (files is not null)
            list.AddRange(files);
        if (folders is not null)
            foreach (MinecraftFolder folder in folders)
                folder.GetAllFiles(list);
    }


    public List<MinecraftFolder>? TryGetFolders() => folders;
    public List<MinecraftFile>? TryGetFiles() => files;
    public MinecraftFolder? GetFolder(ReadOnlySpan<char> path)
    {
        if (folders is null)
            return null;
        int indexOf = path.IndexOf('\\');
        int indexOf2 = path.IndexOf('/');
        if ((indexOf2 < indexOf && indexOf2 != -1) || indexOf == -1)
        {
            indexOf = indexOf2;
        }
        if (indexOf == -1)
        {
            for (int i = 0; i < folders.Count; i++)
            {
                if (path.SequenceEqual(folders[i].Name))
                {
                    return folders[i];
                }
            }
            return null;
        }
        {
            ReadOnlySpan<char> first = path[..indexOf];
            for (int i = 0; i < folders.Count; i++)
            {
                if (first.SequenceEqual(folders[i].Name))
                {
                    return folders[i].GetFolder(path[(indexOf + 1)..]);
                }
            }
            return null;
        }
    }
}
