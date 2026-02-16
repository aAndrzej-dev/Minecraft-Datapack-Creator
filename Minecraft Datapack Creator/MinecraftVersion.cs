using Aadev.JTF;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MinecraftDatapackCreator;
internal sealed class MinecraftVersion
{
    private readonly Controller controller;
    private JTemplate? mcmetaFileTemplate;
    private DatapackStructureFoldersCollection? datapackStructure;
    private MinecraftStructure? minecraftStructure;
    private MinecraftTranslateKeys? translationKeys;
    public int PackFormat { get; }
    public string MinVersion { get; }
    public string MaxVersion { get; }
    public string PackDirectory { get; }


    public MinecraftStructure MinecraftStructure
    {
        get
        {
            if (minecraftStructure is null)
                LoadMinecraftStructure();
            return minecraftStructure;
        }
    }
    public JTemplate McmetaFileTemplate
    {
        get
        {
            if (mcmetaFileTemplate is null)
                LoadDatapackStructure();
            return mcmetaFileTemplate;
        }
    }
    public DatapackStructureFoldersCollection DatapackStructure
    {
        get
        {
            if (datapackStructure is null)
                LoadDatapackStructure();
            return datapackStructure;
        }
    }
    public MinecraftTranslateKeys TranslationKeys
    {
        get
        {
            if (translationKeys is null)
                LoadTranslateKeys();
            return translationKeys;
        }
    }

    public MinecraftVersion(Controller controller, int packFormat, string packDirectory, string minVersion, string maxVersion)
    {
        this.controller = controller;
        PackFormat = packFormat;
        PackDirectory = packDirectory;
        MinVersion = minVersion;
        MaxVersion = maxVersion;
    }
    [MemberNotNull(nameof(minecraftStructure))]
    private void LoadMinecraftStructure()
    {
        try
        {
            minecraftStructure = MinecraftStructure.Load(Path.Join(controller.Settings.DatapackStructureDataFolder, PackDirectory, "minecraft_structure.json"));
        }
        catch (Exception ex)
        {
            minecraftStructure = MinecraftStructure.CreateEmpty();
            controller.Logger.Exception(ex);
            MessageBox.Show($"Cannot load minecraft structure.\n{ex.Message}", "Minecraft Datapack Creator", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    [MemberNotNull(nameof(translationKeys))]
    private void LoadTranslateKeys()
    {
        try
        {
            translationKeys = MinecraftTranslateKeys.Load(Path.Join(controller.Settings.DatapackStructureDataFolder, PackDirectory, "minecraft_translations.json"));
        }
        catch (Exception ex)
        {
            translationKeys = MinecraftTranslateKeys.CreateEmpty();
            controller.Logger.Exception(ex);
            MessageBox.Show($"Cannot load minecraft structure.\n{ex.Message}", "Minecraft Datapack Creator", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    [MemberNotNull(nameof(datapackStructure))]
    [MemberNotNull(nameof(mcmetaFileTemplate))]
    private void LoadDatapackStructure()
    {
        try
        {
            controller.Logger.Debug("Loading datapack structure");
            mcmetaFileTemplate = JTemplate.Load(Path.Join(controller.Settings.DatapackStructureDataFolder, PackDirectory, "pack.mcmeta.jtf"));
            datapackStructure = DatapackStructureFoldersCollection.Load(Path.Join(controller.Settings.DatapackStructureDataFolder, PackDirectory, "structure.json"), controller.Logger);
            controller.Logger.Debug("Datapack structure loaded successfully");
        }
        catch (Exception ex)
        {
            datapackStructure = DatapackStructureFoldersCollection.CreateEmpty();
            controller.Logger.Exception(ex);
            MessageBox.Show($"Cannot load datapack structure.\n{ex.Message}", "Minecraft Datapack Creator", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    public void UnloadDatapackStructure()
    {
        datapackStructure = null;
        mcmetaFileTemplate = null;
    }
    public void UnloadMinecraftStructure() => minecraftStructure = null;
    public void UnloadMinecraftTranslations() => translationKeys = null;
}