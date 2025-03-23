using Aadev.JTF;
using MinecraftDatapackCreator.FileStructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace MinecraftDatapackCreator;

internal sealed partial class Datapack
{
    internal const string DATA_FOLDER_NAME = "data";
    internal const string PACK_MCMETA_FILE = "pack.mcmeta";
    private static readonly string[] allowedStructureDynamicSources = new string[]
    {
        "advancements",
        "advancement",
        "functions",
        "function",
        "loot_tables",
        "loot_table",
        "item_modifiers",
        "item_modifier",
        "predicates",
        "predicate",
        "recipes",
        "recipe",
        "structures",
        "structure",
        "dimension_type",
        "damage_type",
        "dimension",
        "trim_pattern",
        "trim_material",
        "chat_type",
        "banner_pattern",
        "enchantment",
        "enchantment_provider",
        "jukebox_song",
        "painting_variant",
        "trial_spawner",
        "wolf_variant",
        "instrument",
        "worldgen/biome",
        "worldgen/configured_carver",
        "worldgen/configured_feature",
        "worldgen/configured_structure_feature",
        "worldgen/configured_surface_builder",
        "worldgen/noise_settings",
        "worldgen/processor_list",
        "worldgen/template_pool",
        "worldgen/structure_set",
        "worldgen/density_function",
        "worldgen/flat_level_generator_preset",
        "worldgen/noise",
        "worldgen/placed_feature",
        "worldgen/structure",
        "worldgen/world_preset",
        "tags/blocks",
        "tags/block",
        "tags/entity_types",
        "tags/entity_type",
        "tags/fluids",
        "tags/fluid",
        "tags/functions",
        "tags/function",
        "tags/items",
        "tags/item",
        "tags/game_events",
        "tags/enchantment",
        "tags/game_event",
        "tags/damage_type",
        "tags/banner_pattern",
        "tags/cat_variant",
        "tags/instrument",
        "tags/painting_variant",
        "tags/point_of_interest_type",
        "tags/worldgen/biome",
        "tags/worldgen/structure",
        "tags/worldgen/world_preset",
        "tags/worldgen/flat_level_generator_preset"
    };

    private readonly Controller controller;


    public string Path { get; }
    internal string DataFolderPath { get; }
    internal DatapackFileStructure FileStructure { get; }
    internal MinecraftVersion Sources { get; }
    public string Name { get; }


    internal Datapack(Controller controller, ReadOnlySpan<char> path)
    {
        this.controller = controller;
        Path = path.TrimEnd('\\').ToString();
        Name = Path.AsSpan(Path.LastIndexOf('\\') + 1).ToString();
        DataFolderPath = System.IO.Path.Join(Path, DATA_FOLDER_NAME);

        DirectoryInfo di = new DirectoryInfo(Path);
        if (!di.Exists)
            di.Create();



        string mcMeta = System.IO.Path.Join(Path, PACK_MCMETA_FILE);
        if (File.Exists(mcMeta))
        {
            try
            {
                using TextReader tr = new StreamReader(mcMeta);
                using JsonReader jr = new JsonTextReader(tr);

                JObject jObject = JObject.Load(jr, Settings.jsonLoadSettings);
                tr.Close();

                int? packFormat = (int?)jObject["pack"]?["pack_format"];

                if (packFormat is int pf)
                {
                    Sources = controller.VersionManager.GetVersion(pf) ?? controller.VersionManager.GetNewest();
                }
                else
                {
                    Sources = controller.VersionManager.GetNewest();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Sources ??= controller.VersionManager.GetNewest();
            }
        }
        else
        {
            Sources = controller.VersionManager.GetNewest();
        }
        FileStructure = DatapackFileStructure.Load(this);


        DatapackFileInfo? mcMetaFile = FileStructure.RootFolder.GetRelativeFile(PACK_MCMETA_FILE);
        if (mcMetaFile != null)
        {
            mcMetaFile.FileContentChanged += McMetaFile_FileContentChanged;
        }
    }

    private void McMetaFile_FileContentChanged(object? sender, EventArgs e)
    {
        DatapackFileInfo? mcMetaFile = FileStructure.RootFolder.GetRelativeFile(PACK_MCMETA_FILE);
        if (mcMetaFile is null || !mcMetaFile.Exist)
        {
            return;
        }
        using TextReader tr = new StreamReader(mcMetaFile.FullName);
        using JsonReader jr = new JsonTextReader(tr);

        JObject jObject = JObject.Load(jr, Settings.jsonLoadSettings);
        tr.Close();

        int? packFormat = (int?)jObject["pack"]?["pack_format"];

        if (packFormat is int pf)
        {
            if (Sources.PackFormat != pf)
            {
                controller.RequestSolutionReload(new ReloadRequestEventArgs(ReloadRequestEventArgs.ReloadRequestReason.PackFormatChanged));
            }
        }

    }

    public DatapackFileInfo GetMetaFile() => FileStructure.RootFolder.GetRelativeFile(PACK_MCMETA_FILE) ?? FileStructure.RootFolder.CreateRelativeFile(PACK_MCMETA_FILE);


    internal List<DatapackDirectoryInfo>? TryGetNamespaces() => FileStructure.RootFolder.GetRelativeDirectory(DATA_FOLDER_NAME)?.TryGetDirectories();
    public static bool IsValidResourceName(ReadOnlySpan<char> name) => ResourceNameRegex().IsMatch(name);
    public static bool IsValidNamespacedId(ReadOnlySpan<char> namespacedId) => NamespacedIdRegex().IsMatch(namespacedId);


    public static bool TryGetValidResourceName(ReadOnlySpan<char> name, Span<char> validResourceName)
    {
        if (name.Length > validResourceName.Length)
            return false;

        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] is ' ')
                validResourceName[i] = '_';
            else
                validResourceName[i] = char.ToLowerInvariant(name[i]);
        }
        return IsValidResourceName(validResourceName);
    }

    [GeneratedRegex("^[a-z0-9\\-_\\.]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex ResourceNameRegex();

    [GeneratedRegex("^([a-z0-9_\\-\\.]+:)?[a-z0-9_\\-\\./]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex NamespacedIdRegex();


    internal IEnumerable<IJtSuggestion> GetNamespacedSourceAsSuggestions(JtIdentifier id)
    {
        if (id.Value?.StartsWith("structure:", StringComparison.OrdinalIgnoreCase) is true)
        {
            string structurePath = id.Value.AsSpan(10).ToString();
            bool valid = false;
            for (int i = 0; i < allowedStructureDynamicSources.Length; i++)
            {
                if (structurePath.Equals(allowedStructureDynamicSources[i], StringComparison.OrdinalIgnoreCase))
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
            {
                return Enumerable.Empty<IJtSuggestion>();
            }

            List<DatapackDirectoryInfo>? namespaces = TryGetNamespaces();
            if (namespaces is null)
            {
                return Enumerable.Empty<IJtSuggestion>();
            }

            List<IJtSuggestion> suggestions = new List<IJtSuggestion>();
            for (int i = 0; i < namespaces.Count; i++)
            {
                DatapackDirectoryInfo? folder = namespaces[i].GetRelativeDirectory(structurePath);
                if (folder is null)
                    continue;
                foreach (DatapackFileInfo file in folder.GetAllFiles())
                {
                    suggestions.Add(new JtSuggestion<string>(file.NamespacedId!, $"{file.NamespacedId} ({file.DatapackStructureFolder?.Path})"));
                }
            }

            MinecraftFolder? minecraftFolder = Sources.MinecraftStructure.GetFolder(structurePath);
            if (minecraftFolder is not null)
                foreach (MinecraftFile item in minecraftFolder.GetAllFiles())
                {
                    suggestions.Add(new JtSuggestion<string>(item.Id));
                }

            return suggestions;
        }
        else if (id.Value?.StartsWith("mcresource:", StringComparison.OrdinalIgnoreCase) is true)
        {
            string resourceName = id.Value.AsSpan(11).ToString();

            if(resourceName.Equals("translationKeys", StringComparison.OrdinalIgnoreCase))
            {
                IReadOnlyDictionary<string, string> allKeys = Sources.TranslationKeys.GetTranslationKeys()!;
                List<IJtSuggestion> suggestions = new List<IJtSuggestion>();


                foreach (KeyValuePair<string, string> item in allKeys)
                {
                    suggestions.Add(new JtSuggestion<string>(item.Key, item.Value.Replace("\n", "\\n")));
                }
                return suggestions;
            }
            
           
        }
        return Enumerable.Empty<IJtSuggestion>();

    }
}
