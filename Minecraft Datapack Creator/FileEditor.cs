using MinecraftDatapackCreator.FileStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackCreator;
internal class FileEditor
{
    private static int nextId;

    private readonly Func<Controller, DatapackFileInfo, EditorTabPage> tabPageFactory;
    private FileEditor(string editorId, string editorName, Func<Controller, DatapackFileInfo, EditorTabPage> tabPageFactory)
    {
        Id = nextId++;
        EditorId = editorId;
        EditorName = editorName;
        this.tabPageFactory = tabPageFactory;
    }

    public string EditorName { get; }
    public string EditorId { get; }
    public int Id { get; }

    public static readonly FileEditor TextEditor = new FileEditor("aadev:textEditor", "Text Editor", TextEditorTabPage.Create);
    public static readonly FileEditor JsonEditor = new FileEditor("aadev:jsonEditor", "Json Editor", JsonEditorTabPage.Create);
    public static readonly FileEditor NBTEditor = new FileEditor("aadev:nbtEditor", "NBT Editor", NBTEditorTabPage.Create);

    public EditorTabPage CreateEditor(Controller controller, DatapackFileInfo fileInfo) => tabPageFactory(controller, fileInfo);

    
    internal static FileEditor GetEditorForExtension(string? editor, string? extension)
    {
        if ("aadev:textEditor".Equals(editor, StringComparison.OrdinalIgnoreCase)) 
            return TextEditor;
        if ("aadev:jsonEditor".Equals(editor, StringComparison.OrdinalIgnoreCase))
            return JsonEditor;
        if ("aadev:nbtEditor".Equals(editor, StringComparison.OrdinalIgnoreCase))
            return NBTEditor;

        if ("json".Equals(extension, StringComparison.OrdinalIgnoreCase))
            return JsonEditor;
        if ("nbt".Equals(extension, StringComparison.OrdinalIgnoreCase))
            return NBTEditor;

        return TextEditor;
    }
}


