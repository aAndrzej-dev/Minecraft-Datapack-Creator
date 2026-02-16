using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Frozen;
using System.IO;

namespace MinecraftDatapackCreator;

internal class MinecraftTranslateKeys
{
    private readonly FrozenDictionary<string, string>? translationKeys;
    private MinecraftTranslateKeys(string filename)
    {
        using StreamReader sr = new(filename);
        using JsonTextReader jr = new(sr);

        JObject root = JObject.Load(jr, Settings.jsonLoadSettings);
        translationKeys = FrozenDictionary.ToFrozenDictionary(root.Properties().Select(x => new KeyValuePair<string, string>(x.Name, (string)x.Value!)));
    }
    private MinecraftTranslateKeys()
    {

    }

    internal static MinecraftTranslateKeys CreateEmpty() => new MinecraftTranslateKeys();
    internal static MinecraftTranslateKeys Load(string filename) => new MinecraftTranslateKeys(filename);
    public IReadOnlyDictionary<string, string>? GetTranslationKeys() => translationKeys;

  
}
