using CommunityToolkit.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace MinecraftDatapackCreator;

internal class DatapackStructureFolder
{
    private DatapackStructureFoldersCollection? children;

    public string Name { get; }
    public string DisplayName { get; }
    public string? Description { get; }
    public bool AllowFilesAndDirectories { get; }
    public string? FilesExtension { get; }
    public Color TabBackColor { get; }
    public Color TabForeColor { get; }
    public string? NamespacedIdPrefix { get; }
    public FileEditor Editor { get; }

    public DatapackStructureFolder? Parent { get; }
    public string Path => Parent is null ? Name : $"{Parent.Path}/{Name}";

    internal DatapackStructureFoldersCollection? TryGetChildren() => children;
    internal void SetChildrenCollection(DatapackStructureFoldersCollection? children) => this.children = children;
    internal DatapackStructureFolder(string name, string displayName, DatapackStructureFolder? parent)
    {
        Name = name;
        DisplayName = displayName;
        Parent = parent;
        Editor = FileEditor.TextEditor;
    }

    internal DatapackStructureFolder(JObject source, DatapackStructureFolder? parent, string? displayName = null)
    {
        Guard.IsNotNull(source);
        Name = (string?)source["name"] ?? ThrowHelper.ThrowArgumentException<string?>("JToken doesn't have 'name' property", nameof(source));
        DisplayName = displayName ?? Helpers.ConvertToFriendlyName(Name);
        AllowFilesAndDirectories = (bool)(source["allowSubitems"] ?? true);
        FilesExtension = (string?)source["filesExtension"];
        Description = (string?)source["description"];
        NamespacedIdPrefix = (string?)source["namespacedIdPrefix"];
        Editor = FileEditor.GetEditorForExtension((string?)source["editor"], FilesExtension);
        TabBackColor = ColorTranslator.FromHtml((string?)source["tabBackColor"] ?? "royalBlue");
        TabForeColor = ColorTranslator.FromHtml((string?)source["tabForeColor"] ?? "white");
        Parent = parent;
    }
    

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(CultureInfo.InvariantCulture, $"{nameof(DatapackStructureFolder)} ({nameof(Name)}: \"{Name}\"; {nameof(DisplayName)}: \"{DisplayName}\"; {nameof(FilesExtension)}: \"{FilesExtension}\";)");

        if (children is not null && children.Count > 0)
        {
            sb.Append('\n');

            sb.Append(children.ToString());
        }


        return sb.ToString();
    }
}
