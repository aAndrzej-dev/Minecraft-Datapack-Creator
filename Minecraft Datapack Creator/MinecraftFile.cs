using Newtonsoft.Json.Linq;

namespace MinecraftDatapackCreator;

internal sealed class MinecraftFile
{
    public MinecraftFile(JObject obj, MinecraftFolder parent)
    {
        Name = (string?)obj["name"] ?? throw new ArgumentException("JSON property 'name' doesn't exist", nameof(parent));
        Id = (string?)obj["id"] ?? throw new ArgumentException("JSON property 'id' doesn't exist", nameof(parent));
        Path = System.IO.Path.Join(parent.Path, Name);
    }

    public string Name { get; }
    public string Id { get; }
    public string Path { get; }
}
