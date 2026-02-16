using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MinecraftDatapackCreator;

internal sealed class MinecraftStructure
{
    private readonly List<MinecraftFolder>? folders;


    private MinecraftStructure(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException(null, filename);
        using StreamReader sr = new(filename);
        using JsonTextReader jr = new(sr);

        JArray root = JArray.Load(jr, Settings.jsonLoadSettings);
        folders = new List<MinecraftFolder>(root.Count);

        for (int i = 0; i < root.Count; i++)
        {
            folders.Add(new MinecraftFolder((JObject)root[i], null));
        }
    }
    private MinecraftStructure() {}
    public static MinecraftStructure CreateEmpty() => new MinecraftStructure();


    public static MinecraftStructure Load(string filename) => new MinecraftStructure(filename);

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