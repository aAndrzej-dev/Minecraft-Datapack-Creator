using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace MinecraftDatapackCreator;

public class Datapack
{
    public string Path { get; set; }
    public string Name { get; set; }

    public Datapack(string? path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
        Name = path.AsSpan(path.LastIndexOf('\\') + 1).ToString();
        DirectoryInfo di = new(path);
        if (!di.Exists)
        {
            di.Create();
        }
    }

    public string[] GetNamespaces()
    {
        string data = System.IO.Path.Combine(Path, "data");
        DirectoryInfo di = new(data);
        if (!di.Exists)
        {
            di.Create();
        }

        List<string> result = new();

        foreach (string ns in Directory.GetDirectories(data))
        {
            result.Add(ns.AsSpan(ns.LastIndexOf('\\') + 1).ToString());
        }
        return result.ToArray();
    }

    public bool IsValidNamespace(string @namespace) => IsValidResourceName(@namespace) && !GetNamespaces().Contains(@namespace);

    private static readonly Regex resourceNameRegex = new("^[a-z0-9-_.]*$", RegexOptions.Compiled);
    public static bool IsValidResourceName(string name) => resourceNameRegex.IsMatch(name);

    public static bool TryGetValidResourceName([NotNullWhen(true)] string? name, [NotNullWhen(true)][NotNullIfNotNull("name")] out string? validResourceName)
    {
        if (name is null)
        {
            validResourceName = null;
            return false;

        }



        validResourceName = name.TrimEnd().TrimStart().Replace(" ", "_").ToLower();

        return IsValidResourceName(validResourceName);
    }

}
