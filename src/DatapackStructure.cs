using Aadev.JTF;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

namespace MinecraftDatapackCreator;

public class DatapackStructureFolder
{
    public string Name { get; init; }
    public string DisplayName { get; init; }
    public string? Description { get; init; }
    public bool AllowFiles { get; init; } = true;
    public bool AllowFolders { get; init; } = true;
    public string? FilesExtension { get; init; }
    public bool ForceFileExtension { get; init; }
    public Color TabBackColor { get; init; }
    public Color TabForeColor { get; init; }
    public string? NamespacedIdPrefix { get; init; }
    public string Editor { get; init; }
    public bool Required { get; init; }
    public DatapackStructureFoldersCollection Children { get; set; }

    public DatapackStructureFolder? Parent { get; }
    public string Path => Parent is null ? Name : $"{Parent.Path}/{Name}";

    public DatapackStructureFolder(string name, string displayName, DatapackStructureFolder? parent)
    {
        Name = name;
        DisplayName = displayName;
        Children = new DatapackStructureFoldersCollection();
        Parent = parent;
        Editor = "aadev:textEditor";
    }

    public DatapackStructureFolder(JObject source, DatapackStructureFolder? parent) : this((string)source["name"]!, (string)source["name"]!, parent)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        AllowFolders = (bool)(source["allowDirectories"] ?? true);
        AllowFiles = (bool)(source["allowFiles"] ?? true);
        Required = (bool)(source["required"] ?? false);
        FilesExtension = (string?)source["filesExtension"];
        Description = (string?)source["description"];
        NamespacedIdPrefix = (string?)source["namespacedIdPrefix"];
        Editor = ValidateEditor((string?)source["editor"], FilesExtension);
        ForceFileExtension = true;
        TabBackColor = ColorTranslator.FromHtml((string?)source["tabBackColor"] ?? "royalBlue");
        TabForeColor = ColorTranslator.FromHtml((string?)source["tabForeColor"] ?? "white");
        Parent = parent;
    }
    protected static string ValidateEditor(string? editor, string? extension)
    {
        if (editor is "aadev:textEditor" or "aadev:jsonEditor" or "aadev:nbtEditor")
            return editor;

        if (extension is "json")
            return "aadev:jsonEditor";
        if (extension is "nbt")
            return "aadev:nbtEditor";

        return "aadev:textEditor";
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(CultureInfo.InvariantCulture, $"{nameof(DatapackStructureFolder)} ({nameof(Name)}: \"{Name}\"; {nameof(DisplayName)}: \"{DisplayName}\"; {nameof(AllowFiles)}: {AllowFiles}; {nameof(AllowFolders)}: {AllowFolders}; {nameof(FilesExtension)}: \"{FilesExtension}\"; {nameof(ForceFileExtension)}: {ForceFileExtension})");

        if (Children.Count > 0)
        {
            sb.Append('\n');

            sb.Append(Children.ToString());
        }


        return sb.ToString();
    }
}
public class DatapackStructureFolderJTF : DatapackStructureFolder
{
    public JTemplate Template { get; init; }

    public DatapackStructureFolderJTF(string name, JTemplate template, DatapackStructureFolder? parent) : base(name, template.Name, parent)
    {
        Template = template;
    }
    public DatapackStructureFolderJTF(JObject source, JTemplate template, DatapackStructureFolder? parent) : this((string)source["name"]!, template, parent)
    {
        AllowFolders = (bool)(source["allowDirectories"] ?? true);
        AllowFiles = (bool)(source["allowFiles"] ?? true);
        FilesExtension = (string?)source["filesExtension"];
        Required = (bool)(source["required"] ?? false);
        Description = (string?)source["description"];
        NamespacedIdPrefix = (string?)source["namespacedIdPrefix"];
        Editor = ValidateEditor((string?)source["editor"], FilesExtension);
        ForceFileExtension = true;
        TabBackColor = ColorTranslator.FromHtml((string?)source["tabBackColor"] ?? "royalBlue");
        TabForeColor = ColorTranslator.FromHtml((string?)source["tabForeColor"] ?? "white");
    }
}

public class DatapackStructureFoldersCollection : IList<DatapackStructureFolder>, IList
{
    private readonly List<DatapackStructureFolder> structureItems;

    public int Count => structureItems.Count;

    public bool IsReadOnly => ((ICollection<DatapackStructureFolder>)structureItems).IsReadOnly;

    public bool IsFixedSize => ((IList)structureItems).IsFixedSize;

    public bool IsSynchronized => ((ICollection)structureItems).IsSynchronized;

    public object SyncRoot => ((ICollection)structureItems).SyncRoot;

    object? IList.this[int index] { get => ((IList)structureItems)[index]; set => ((IList)structureItems)[index] = value; }
    public DatapackStructureFolder this[int index] { get => ((IList<DatapackStructureFolder>)structureItems)[index]; set => ((IList<DatapackStructureFolder>)structureItems)[index] = value; }

    public DatapackStructureFoldersCollection()
    {
        structureItems = new List<DatapackStructureFolder>();
    }



    public DatapackStructureFolder? GetDatapackStructureItemByName(string? name)
    {
        if (name is null)
            return null;
        if (!name.Contains('\\', StringComparison.Ordinal))
        {
            return this.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            string[] splited = name.Split('\\');
            DatapackStructureFolder? folder = this.FirstOrDefault(x => x.Name.Equals(splited[0], StringComparison.OrdinalIgnoreCase));


            return folder?.Children.GetDatapackStructureItemByName(string.Join("\\", splited, 1, splited.Length - 1)) ?? folder;
        }
    }
    [return: NotNullIfNotNull(nameof(jArray))]
    private static DatapackStructureFoldersCollection? GetDatapackStructureFolderSubs(JArray? jArray, string filename, ILogger logger, DatapackStructureFolder? parent, string workingDir)
    {
        if (jArray is null)
            return null;
        DatapackStructureFoldersCollection collection = new();

        foreach (JObject item in jArray)
        {

            if (item["filesExtension"]?.ToString() == "json")
            {
                string? source = (string?)item["source"];
                if (source is null)
                {
                    CreateFolder(filename, logger, parent, workingDir, collection, item);
                    continue;
                }
                string absoluteSource = Path.GetFullPath(source, Path.GetDirectoryName(filename)!);
                if (!File.Exists(absoluteSource))
                {
                    CreateFolder(filename, logger, parent, workingDir, collection, item);
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
                    CreateFolder(filename, logger, parent, workingDir, collection, item);
                }
                else
                {

                    DatapackStructureFolder folder = new DatapackStructureFolderJTF(item, template, parent);
                    collection.Add(folder);
                    if (item["children"] != null)
                    {
                        folder.Children = GetDatapackStructureFolderSubs((JArray?)item["children"], filename, logger, folder, workingDir) ?? new DatapackStructureFoldersCollection();
                    }
                }
            }
            else
            {
                DatapackStructureFolder folder = new DatapackStructureFolder(item, parent);
                collection.Add(folder);
                if (item["children"] != null)
                {
                    folder.Children = GetDatapackStructureFolderSubs((JArray?)item["children"], filename, logger, folder, workingDir) ?? new DatapackStructureFoldersCollection();
                }
            }
        }

        return collection;

        static void CreateFolder(string filename, ILogger logger, DatapackStructureFolder? parent, string workingDir, DatapackStructureFoldersCollection collection, JObject item)
        {
            DatapackStructureFolder folder = new DatapackStructureFolder(item, parent);
            collection.Add(folder);
            if (item["children"] != null)
            {
                folder.Children = GetDatapackStructureFolderSubs((JArray?)item["children"], filename, logger, folder, workingDir) ?? new DatapackStructureFoldersCollection();
            }
        }
    }
    public static DatapackStructureFoldersCollection CreateDatapackStructure(string filename, ILogger logger, string workingDir) => GetDatapackStructureFolderSubs(JArray.Parse(File.ReadAllText(filename)), filename, logger, null, workingDir);


    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('{');

        foreach (DatapackStructureFolder? child in this)
        {
            sb.Append("\n\t" + child.ToString());
        }
        sb.Append("\n}");


        return sb.ToString();
    }

    public int IndexOf(DatapackStructureFolder item) => ((IList<DatapackStructureFolder>)structureItems).IndexOf(item);
    public void Insert(int index, DatapackStructureFolder item) => ((IList<DatapackStructureFolder>)structureItems).Insert(index, item);
    public void RemoveAt(int index) => ((IList<DatapackStructureFolder>)structureItems).RemoveAt(index);
    public void Add(DatapackStructureFolder item) => ((ICollection<DatapackStructureFolder>)structureItems).Add(item);
    public void Clear() => ((ICollection<DatapackStructureFolder>)structureItems).Clear();
    public bool Contains(DatapackStructureFolder item) => ((ICollection<DatapackStructureFolder>)structureItems).Contains(item);
    public void CopyTo(DatapackStructureFolder[] array, int arrayIndex) => ((ICollection<DatapackStructureFolder>)structureItems).CopyTo(array, arrayIndex);
    public bool Remove(DatapackStructureFolder item) => ((ICollection<DatapackStructureFolder>)structureItems).Remove(item);
    public IEnumerator<DatapackStructureFolder> GetEnumerator() => ((IEnumerable<DatapackStructureFolder>)structureItems).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)structureItems).GetEnumerator();
    public int Add(object? value) => ((IList)structureItems).Add(value);
    public bool Contains(object? value) => ((IList)structureItems).Contains(value);
    public int IndexOf(object? value) => ((IList)structureItems).IndexOf(value);
    public void Insert(int index, object? value) => ((IList)structureItems).Insert(index, value);
    public void Remove(object? value) => ((IList)structureItems).Remove(value);
    public void CopyTo(Array array, int index) => ((ICollection)structureItems).CopyTo(array, index);
}