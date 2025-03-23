using Aadev.JTF;
using CommunityToolkit.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MinecraftDatapackCreator;

internal sealed class DatapackStructureFoldersCollection : IList<DatapackStructureFolder>
{
    private List<DatapackStructureFolder>? structureItems;

    public int Count => structureItems?.Count ?? 0;
    public bool IsReadOnly => true;

    public DatapackStructureFolder this[int index] { get => structureItems is null ? ThrowHelper.ThrowArgumentOutOfRangeException<DatapackStructureFolder>(nameof(index)) : structureItems[index]; set { EnsureStructureItems(); structureItems[index] = value; } }


    private DatapackStructureFoldersCollection(int capacity)
    {
        if (capacity == 0)
            return;
        structureItems = new List<DatapackStructureFolder>(capacity);
    }


    [MemberNotNull(nameof(structureItems))]
    private void EnsureStructureItems() => structureItems ??= new List<DatapackStructureFolder>();
    private DatapackStructureFolder? GetFolderInCurrentFolder(ReadOnlySpan<char> name)
    {
        if (structureItems is null)
            return null;
        Span<DatapackStructureFolder> structureItemsSpan = CollectionsMarshal.AsSpan(structureItems);
        for (int i = 0; i < structureItemsSpan.Length; i++)
        {
            if (MemoryExtensions.Equals(structureItemsSpan[i].Name.AsSpan(), name, StringComparison.OrdinalIgnoreCase))
                return structureItemsSpan[i];
        }
        return null;
    }

    public DatapackStructureFolder? GetDatapackStructureItemByName(ReadOnlySpan<char> name)
    {
        if (structureItems is null)
            return null;
        int start = name.IndexOf('\\');
        while (start == 0)
        {
            start--;
            name = name[1..];
        }
        if (start == -1)
        {
            return GetFolderInCurrentFolder(name);
        }
        else
        {
            ReadOnlySpan<char> folderName = name[..start];
            DatapackStructureFolder? folder = GetFolderInCurrentFolder(folderName);
            return folder?.TryGetChildren()?.GetDatapackStructureItemByName(name[(start + 1)..]) ?? folder;
        }
    }
    
    
    [return: NotNullIfNotNull(nameof(jArray))]
    private static DatapackStructureFoldersCollection? GetDatapackStructureFolderSubs(JArray? jArray, string dirName, ILogger logger, DatapackStructureFolder? parent, string workingDir)
    {
        if (jArray is null || jArray.Count == 0)
            return null;
        DatapackStructureFoldersCollection collection = new DatapackStructureFoldersCollection(jArray.Count);

        foreach (JToken token in jArray)
        {
            if (token is not JObject item)
            {
                continue;
            }
            if (item["filesExtension"]?.ToString() == "json")
            {
                string? source = (string?)item["source"];
                if (source is null)
                {
                    collection.Add(CreateFolder(dirName, logger, parent, workingDir, item));
                    continue;
                }
                string absoluteSource = Path.GetFullPath(source, dirName);
                if (!File.Exists(absoluteSource))
                {
                    collection.Add(CreateFolder(dirName, logger, parent, workingDir, item));
                    continue;
                }

                logger.Debug($"Loading template: {absoluteSource}");
                JTemplate? template = null;
                try
                {
                    template = JTemplate.Load(absoluteSource, workingDir);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                }

                if (template is null)
                {
                    collection.Add(CreateFolder(dirName, logger, parent, workingDir, item));
                }
                else
                {

                    DatapackStructureFolder folder = new DatapackStructureFolderJTF(item, template, parent);
                    collection.Add(folder);
                    if (item["children"] is JArray array)
                    {
                        DatapackStructureFoldersCollection? children = GetDatapackStructureFolderSubs(array, dirName, logger, folder, workingDir);
                        if (children is not null)
                            folder.SetChildrenCollection(children);

                    }
                }
            }
            else
            {
                collection.Add(CreateFolder(dirName, logger, parent, workingDir, item));
            }
        }
        return collection;

        static DatapackStructureFolder CreateFolder(string dirName, ILogger logger, DatapackStructureFolder? parent, string workingDir, JObject item)
        {
            DatapackStructureFolder folder = new DatapackStructureFolder(item, parent);
            if (item["children"] is JArray array)
            {
                DatapackStructureFoldersCollection? children = GetDatapackStructureFolderSubs(array, dirName, logger, folder, workingDir);
                if (children is not null)
                    folder.SetChildrenCollection(children);
            }
            return folder;
        }
    }
    public static DatapackStructureFoldersCollection Load(string filename, ILogger logger)
    {
        using StreamReader sr = new StreamReader(filename);
        using JsonTextReader jr = new JsonTextReader(sr);

        string dirName = Path.GetDirectoryName(filename)!;
        DatapackStructureFoldersCollection collection = GetDatapackStructureFolderSubs(JArray.Load(jr, Settings.jsonLoadSettings), dirName, logger, null, dirName);
        jr.Close();
        return collection;
    }
    public static DatapackStructureFoldersCollection CreateEmpty() => new DatapackStructureFoldersCollection(0);

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('{');

        foreach (DatapackStructureFolder? child in this)
        {
            sb.Append("\n\t" + child.ToString());
        }
        sb.Append("\n}");


        return sb.ToString();
    }
    public int IndexOf(DatapackStructureFolder item) => structureItems is null ? -1 : structureItems.IndexOf(item);
    public void Insert(int index, DatapackStructureFolder item) { EnsureStructureItems(); structureItems.Insert(index, item); }
    public void RemoveAt(int index) => structureItems?.RemoveAt(index);
    public void Add(DatapackStructureFolder item) { EnsureStructureItems(); structureItems.Add(item); }
    public void Clear() => structureItems?.Clear();
    public bool Contains(DatapackStructureFolder item) => structureItems is not null && structureItems.Contains(item);
    public void CopyTo(DatapackStructureFolder[] array, int arrayIndex) { EnsureStructureItems(); structureItems.CopyTo(array, arrayIndex); }
    public bool Remove(DatapackStructureFolder item) => structureItems?.Remove(item) ?? false;
    public IEnumerator<DatapackStructureFolder> GetEnumerator() { EnsureStructureItems(); return structureItems.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { EnsureStructureItems(); return structureItems.GetEnumerator(); }
}