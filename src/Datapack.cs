using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;

namespace MinecraftDatapackCreator;

internal partial class Datapack
{
    public string Path { get; set; }
    internal DatapackStructureFoldersCollection DatapackStructure { get; set; }
    internal DatapackFileStructure FileStructure { get; }
    public string Name { get; set; }


    internal Datapack(string? path, DatapackStructureFoldersCollection datapackStructure)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
        DatapackStructure = datapackStructure;
        Name = path.AsSpan(path.LastIndexOf('\\') + 1).ToString();
        DirectoryInfo di = new(path);
        if (!di.Exists)
            di.Create();
        FileStructure = new DatapackFileStructure(this);
    }

    internal const string DATA_FOLDER_NAME = "data";

    public string[] GetNamespaces()
    {
        DatapackFolderInfo? folder = FileStructure.RootFolder.GetRelativeFolder(DATA_FOLDER_NAME);
        if (folder is null)
            return Array.Empty<string>();

        return folder.GetFolders().Select(x => x.Name).ToArray();
    }
    public string[] GetNamespacesFullName()
    {
        string data = System.IO.Path.Combine(Path, DATA_FOLDER_NAME);
        DirectoryInfo di = new(data);
        if (!di.Exists)
            di.Create();
        return Directory.GetDirectories(data);
    }
    public bool IsValidNamespace(string @namespace) => IsValidResourceName(@namespace) && !GetNamespaces().Contains(@namespace);
    public static bool IsValidResourceName(string name) => ResourceNameRegex().IsMatch(name);
    public static bool IsValidNamespacedId(string namespacedId) => NamespacedIdRegex().IsMatch(namespacedId);

    public static bool TryGetValidResourceName([NotNullWhen(true)] string? name, [NotNullWhen(true)][NotNullIfNotNull("name")] out string? validResourceName)
    {
        if (name is null)
        {
            validResourceName = null;
            return false;

        }



        validResourceName = name.TrimEnd().TrimStart().Replace(" ", "_", StringComparison.Ordinal).ToLowerInvariant();

        return IsValidResourceName(validResourceName);
    }

    [GeneratedRegex("^[a-z0-9\\-_\\.]*$", RegexOptions.Compiled)]
    private static partial Regex ResourceNameRegex();

    [GeneratedRegex("^([a-z0-9_\\-\\.]+:)?[a-z0-9_\\-\\./]+$", RegexOptions.Compiled)]
    private static partial Regex NamespacedIdRegex();
}
