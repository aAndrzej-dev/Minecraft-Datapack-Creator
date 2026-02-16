using Aadev.JTF;
using Newtonsoft.Json.Linq;

namespace MinecraftDatapackCreator;

internal sealed class DatapackStructureFolderJTF : DatapackStructureFolder
{
    public JTemplate Template { get; }

    internal DatapackStructureFolderJTF(JObject source, JTemplate template, DatapackStructureFolder? parent) : base(source, parent, template.Name)
    {
        Template = template;
    }
}
