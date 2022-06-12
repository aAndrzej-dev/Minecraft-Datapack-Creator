using Aadev.JTF;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text;

namespace MinecraftDatapackCreator;

public abstract class DatapackStructureItem
{
    public string Name { get; init; }
    public string DisplayName { get; init; }
    public string? Description { get; init; }

    public StructureItemType Type { get; }

    protected DatapackStructureItem(string name, string displayName, StructureItemType type)
    {
        Name = name;
        DisplayName = displayName;
        Type = type;
    }

    public enum StructureItemType
    {
        File,
        Folder
    }

    public override string ToString() => $"{nameof(DatapackStructureItem)} ({Type}) {{ {nameof(Name)}: \"{Name}\"; {nameof(DisplayName)}: \"{DisplayName}\" }}";
}
public class DatapackStructureFolder : DatapackStructureItem
{
    public bool AllowFiles { get; init; } = true;
    public bool AllowFolders { get; init; } = true;
    public string? FileExtension { get; init; }
    public bool ForceFileExtension { get; init; } = false;
    public Color TabBackColor { get; init; }
    public Color TabForeColor { get; init; }
    public DatapackStructureItemsCollection Children { get; set; }

    public DatapackStructureFolder(string name, string displayName) : base(name, displayName, StructureItemType.Folder)
    {
        Children = new DatapackStructureItemsCollection();
    }


    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"{nameof(DatapackStructureFolder)} ({nameof(Name)}: \"{Name}\"; {nameof(DisplayName)}: \"{DisplayName}\"; {nameof(AllowFiles)}: {AllowFiles}; {nameof(AllowFolders)}: {AllowFolders}; {nameof(FileExtension)}: \"{FileExtension}\"; {nameof(ForceFileExtension)}: {ForceFileExtension})");

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
    public JTemplate JTemplate { get; init; }

    public DatapackStructureFolderJTF(string name, JTemplate jTemplate) : base(name, jTemplate.Name)
    {

        JTemplate = jTemplate;
    }
}

public class DatapackStructureItemsCollection : IDatapackStructureItemsCollection
{
    private readonly List<DatapackStructureItem> structureItems;

    public int Count => ((ICollection<DatapackStructureItem>)structureItems).Count;

    public bool IsReadOnly => ((ICollection<DatapackStructureItem>)structureItems).IsReadOnly;

    public bool IsFixedSize => ((IList)structureItems).IsFixedSize;

    public bool IsSynchronized => ((ICollection)structureItems).IsSynchronized;

    public object SyncRoot => ((ICollection)structureItems).SyncRoot;

    object? IList.this[int index] { get => ((IList)structureItems)[index]; set => ((IList)structureItems)[index] = value; }
    public DatapackStructureItem this[int index] { get => ((IList<DatapackStructureItem>)structureItems)[index]; set => ((IList<DatapackStructureItem>)structureItems)[index] = value; }

    public DatapackStructureItemsCollection()
    {
        structureItems = new List<DatapackStructureItem>();
    }



    public DatapackStructureItem? GetDatapackStructureItemByName(string name)
    {
        if (!name.Contains('\\'))
        {
            return this.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }
        else
        {
            string[] splited = name.Split('\\');
            DatapackStructureFolder? folder = this.FirstOrDefault(x => x.Name.ToLower() == splited[0].ToLower()) as DatapackStructureFolder;


            return folder?.Children.GetDatapackStructureItemByName(string.Join("\\", splited, 1, splited.Length - 1)) ?? folder;
        }
    }
    private static DatapackStructureItemsCollection? GetDatapackStructureFolderSubs(JToken? json, string filename, ILogger logger)
    {
        if (json is null)
            return null;
        DatapackStructureItemsCollection collection = new();

        JArray jArray = JArray.FromObject(json);

        foreach (JToken item in jArray)
        {
            DatapackStructureItemsCollection Subs = new();
            if (item["subs"] != null)
            {
                Subs = GetDatapackStructureFolderSubs(item["subs"], filename, logger) ?? new DatapackStructureItemsCollection();
            }

            if (item["extension"]?.ToString() == "json")
            {
                JTemplate? template = null;
                try
                {

                    template = new JTemplate(Path.Combine(Path.GetDirectoryName(filename)!, item["source"]?.ToString()!));
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot load tamplate: {ex.Message}");
                }

                if (template is null)
                {
                    collection.Add(new DatapackStructureFolder(item["name"]?.ToString()!, item["name"]?.ToString()!) { AllowFolders = (bool)(item["allow_folders"] ?? true), AllowFiles = (bool)(item["allow_files"] ?? true), FileExtension = item["extension"]?.ToString(), Children = Subs, Description = item["description"]?.ToString(), ForceFileExtension = true, TabBackColor = ColorTranslator.FromHtml((string?)item["tabBackColor"] ?? "royalBlue"), TabForeColor = ColorTranslator.FromHtml((string?)item["tabForeColor"] ?? "white") });
                }
                else
                {
                    collection.Add(new DatapackStructureFolderJTF(item["name"]?.ToString()!, template!) { AllowFolders = (bool)(item["allow_folders"] ?? true), AllowFiles = (bool)(item["allow_files"] ?? true), FileExtension = item["extension"]?.ToString(), Children = Subs, Description = item["description"]?.ToString(), ForceFileExtension = true, TabBackColor = ColorTranslator.FromHtml((string?)item["tabBackColor"] ?? "royalBlue"), TabForeColor = ColorTranslator.FromHtml((string?)item["tabForeColor"] ?? "white") });
                }
            }
            else
            {

                collection.Add(new DatapackStructureFolder(item["name"]?.ToString()!, item["name"]?.ToString()!) { AllowFolders = (bool)(item["allow_folders"] ?? true), AllowFiles = (bool)(item["allow_files"] ?? true), FileExtension = item["extension"]?.ToString(), Children = Subs, Description = item["description"]?.ToString(), ForceFileExtension = true, TabBackColor = ColorTranslator.FromHtml((string?)item["tabBackColor"] ?? "royalBlue"), TabForeColor = ColorTranslator.FromHtml((string?)item["tabForeColor"] ?? "white") });
            }
        }

        return collection;
    }
    public static DatapackStructureItemsCollection CreateDatapackStructure(string filename, ILogger logger)
    {


        DatapackStructureItemsCollection collection = new();

        JArray jArray = JArray.Parse(File.ReadAllText(filename));

        foreach (JToken item in jArray)
        {

            DatapackStructureItemsCollection Subs = new();
            if (item["subs"] is not null)
            {
                Subs = GetDatapackStructureFolderSubs(item["subs"]!, filename, logger) ?? new DatapackStructureItemsCollection();
            }

            if (item["extension"]?.ToString() is "json")
            {
                JTemplate? template = null;
                try
                {

                    template = new JTemplate(Path.Combine(Path.GetDirectoryName(filename)!, item["source"]?.ToString()!));
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot load tamplate: {ex.Message}");
                }
                if (template is null)
                    collection.Add(new DatapackStructureFolder(item["name"]?.ToString()!, item["name"]?.ToString()!) { AllowFolders = (bool)(item["allow_folders"] ?? true), AllowFiles = (bool)(item["allow_files"] ?? true), FileExtension = item["extension"]?.ToString(), Children = Subs, Description = item["description"]?.ToString(), ForceFileExtension = true, TabBackColor = ColorTranslator.FromHtml((string?)item["tabBackColor"] ?? "royalBlue"), TabForeColor = ColorTranslator.FromHtml((string?)item["tabForeColor"] ?? "white") });
                else
                    collection.Add(new DatapackStructureFolderJTF(item["name"]?.ToString()!, template) { AllowFolders = (bool)(item["allow_folders"] ?? true), AllowFiles = (bool)(item["allow_files"] ?? true), FileExtension = item["extension"]?.ToString(), Children = Subs, Description = item["description"]?.ToString(), ForceFileExtension = true, TabBackColor = ColorTranslator.FromHtml((string?)item["tabBackColor"] ?? "royalBlue"), TabForeColor = ColorTranslator.FromHtml((string?)item["tabForeColor"] ?? "white") });


            }
            else
            {
                collection.Add(new DatapackStructureFolder(item["name"]?.ToString()!, item["name"]?.ToString()!) { AllowFolders = (bool)(item["allow_folders"] ?? true), AllowFiles = (bool)(item["allow_files"] ?? true), FileExtension = item["extension"]?.ToString(), Children = Subs, Description = item["description"]?.ToString(), ForceFileExtension = true, TabBackColor = ColorTranslator.FromHtml((string?)item["tabBackColor"] ?? "royalBlue"), TabForeColor = ColorTranslator.FromHtml((string?)item["tabForeColor"] ?? "white") });

            }
        }

        return collection;
    }



    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('{');

        foreach (DatapackStructureItem? child in this)
        {
            sb.Append("\n\t" + child.ToString());
        }
        sb.Append("\n}");


        return sb.ToString();
    }

    public int IndexOf(DatapackStructureItem item) => ((IList<DatapackStructureItem>)structureItems).IndexOf(item);
    public void Insert(int index, DatapackStructureItem item) => ((IList<DatapackStructureItem>)structureItems).Insert(index, item);
    public void RemoveAt(int index) => ((IList<DatapackStructureItem>)structureItems).RemoveAt(index);
    public void Add(DatapackStructureItem item) => ((ICollection<DatapackStructureItem>)structureItems).Add(item);
    public void Clear() => ((ICollection<DatapackStructureItem>)structureItems).Clear();
    public bool Contains(DatapackStructureItem item) => ((ICollection<DatapackStructureItem>)structureItems).Contains(item);
    public void CopyTo(DatapackStructureItem[] array, int arrayIndex) => ((ICollection<DatapackStructureItem>)structureItems).CopyTo(array, arrayIndex);
    public bool Remove(DatapackStructureItem item) => ((ICollection<DatapackStructureItem>)structureItems).Remove(item);
    public IEnumerator<DatapackStructureItem> GetEnumerator() => ((IEnumerable<DatapackStructureItem>)structureItems).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)structureItems).GetEnumerator();
    public int Add(object? value) => ((IList)structureItems).Add(value);
    public bool Contains(object? value) => ((IList)structureItems).Contains(value);
    public int IndexOf(object? value) => ((IList)structureItems).IndexOf(value);
    public void Insert(int index, object? value) => ((IList)structureItems).Insert(index, value);
    public void Remove(object? value) => ((IList)structureItems).Remove(value);
    public void CopyTo(Array array, int index) => ((ICollection)structureItems).CopyTo(array, index);
}