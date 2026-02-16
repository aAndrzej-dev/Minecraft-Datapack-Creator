using System.Text;

namespace MinecraftDatapackCreator;
internal static class CompositeFormats
{
    public static readonly CompositeFormat DialogFileDeleteQuestion = CompositeFormat.Parse(Properties.Resources.DialogFileDeleateQuestion);
    public static readonly CompositeFormat DialogFileNotFound = CompositeFormat.Parse(Properties.Resources.DialogFileNotFound);
    public static readonly CompositeFormat NewFilePlaceholder = CompositeFormat.Parse(Properties.Resources.NewFilePlaceholder);
    public static readonly CompositeFormat DialogNamespaceDeleteQuestion = CompositeFormat.Parse(Properties.Resources.DialogNamespaceDeleteQuestion);
    public static readonly CompositeFormat DialogDirectoryDeleteQuestion = CompositeFormat.Parse(Properties.Resources.DialogDirectoryDeleteQuestion);
}
