using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace MinecraftDatapackCreator;
internal sealed class MinecraftVersionManager
{
    private const string VERSIONS_FILE_NAME = "versions.json";
    private readonly Controller controller;

    private List<MinecraftVersion> versions;

    public MinecraftVersionManager(Controller controller)
    {
        this.controller = controller;
        string versionsFilename = Path.Join(controller.Settings.DatapackStructureDataFolder, VERSIONS_FILE_NAME);
        // TODO: File not found?
        using StreamReader sr = new StreamReader(versionsFilename);
        using JsonReader jr = new JsonTextReader(sr);

        JArray versionsRoot = JArray.Load(jr, Settings.jsonLoadSettings);

        versions = new List<MinecraftVersion>(versionsRoot.Count);
        foreach (JObject item in versionsRoot)
        {
            int packFormat = (int?)item["pack_format"] ?? throw new NotImplementedException();
            string packDirectory = (string?)item["pack_directory"] ?? throw new NotImplementedException();
            string minVersion = (string?)item["min_version"] ?? string.Empty;
            string maxVersion = (string?)item["max_version"] ?? string.Empty;

            versions.Add(new MinecraftVersion(controller, packFormat, packDirectory, minVersion, maxVersion));
        }
    }
    public void UnloadAllDatapackStructures()
    {
        Span<MinecraftVersion> versionsSpan = CollectionsMarshal.AsSpan(versions);
        for (int i = 0; i < versionsSpan.Length; i++)
        {
            versionsSpan[i].UnloadDatapackStructure();
        }
    }
    public void UnloadAllMinecraftStructures()
    {
        Span<MinecraftVersion> versionsSpan = CollectionsMarshal.AsSpan(versions);
        for (int i = 0; i < versionsSpan.Length; i++)
        {
            versionsSpan[i].UnloadMinecraftStructure();
        }
    }

    public MinecraftVersion? GetVersion(int packFormat)
    {
        Span<MinecraftVersion> versionsSpan = CollectionsMarshal.AsSpan(versions);
        for (int i = 0; i < versionsSpan.Length; i++)
        {
            MinecraftVersion version = versionsSpan[i];
            if (version.PackFormat == packFormat)
            {
                return version;
            }
        }
        return null;
    }
    public MinecraftVersion GetNewest()
    {
        if (versions.Count == 0)
            throw new NotImplementedException();
        Span<MinecraftVersion> versionsSpan = CollectionsMarshal.AsSpan(versions);
        MinecraftVersion maxVersion = versionsSpan[0];
        for (int i = 1; i < versionsSpan.Length; i++)
        {
            MinecraftVersion version = versionsSpan[i];
            if (version.PackFormat > maxVersion.PackFormat)
            {
                maxVersion = version;
            }
        }
        return maxVersion;
    }
}