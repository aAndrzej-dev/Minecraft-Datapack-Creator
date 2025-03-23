using Aadev.NBT;
using MinecraftDatapackCreator.FileStructure;
using System.IO;
using System.ComponentModel;

namespace MinecraftDatapackCreator;
internal sealed class NBTEditorTabPage : EditorTabPage
{

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool IsNotSaved { get => false; protected set { } }

    public override bool ReadOnly => true;

    public override bool CanUndo => false;

    public override bool CanRedo => false;

    public override event EventHandler? SavedStateChanged { add { } remove { } }
    private TreeView tw;
    public override void Save() { }

    public NBTEditorTabPage(Controller controller, DatapackFileInfo fileInfo) : base(controller, fileInfo)
    {
        SuspendLayout();

        ImageList il = new ImageList();
        il.Images.Add(Properties.Resources.None);
        il.Images.Add(Properties.Resources.NByte);
        il.Images.Add(Properties.Resources.NShort);
        il.Images.Add(Properties.Resources.NInt);
        il.Images.Add(Properties.Resources.NLong);
        il.Images.Add(Properties.Resources.NFloat);
        il.Images.Add(Properties.Resources.NDouble);
        il.Images.Add(Properties.Resources.None);
        il.Images.Add(Properties.Resources.NString);
        il.Images.Add(Properties.Resources.NList);
        il.Images.Add(Properties.Resources.NCompound);
        il.Images.Add(Properties.Resources.None);
        il.Images.Add(Properties.Resources.None);
        tw = new TreeView()
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(50, 50, 50),
            ForeColor = Color.White,
            Font = controller.Settings.TextEditorFont,
            ImageList = il
        };
        Reload(false);
        ResumeLayout();
    }
    private static void CreateForNode(TreeNode node, INTag nTag)
    {
        if (nTag.HasValue)
        {
            TreeNode? currentNode = node.Nodes.Add(nTag.Name, nTag.HasName ? $"{nTag.Name}: {nTag.StringValue}" : nTag.StringValue, nTag.Type.Id, nTag.Type.Id);
            currentNode.Tag = nTag;
            if (!nTag.HasName)
                currentNode.ForeColor = Color.Yellow;
        }
        else if (nTag is INTagParent nTagParent)
        {

            TreeNode? currentNode = node.Nodes.Add(nTag.Name, nTag.Name, nTag.Type.Id, nTag.Type.Id);
            currentNode.Tag = nTag;
            if (!nTag.HasName)
            {
                currentNode.ForeColor = Color.Yellow;
                currentNode.Text = $"<Element {node.Nodes.Count - 1}>";
            }
            foreach (INTag? item in nTagParent.Children)
            {

                CreateForNode(currentNode, item);

            }
        }
    }

    public override void Undo() => throw new NotImplementedException();
    public override void Redo() => throw new NotImplementedException();
    public override void Reload(bool askToSave)
    {
        try
        {
            NTag nTag = NReader.FromGzippedFile(FileInfo.FullName);

            TreeNode? currentNode = tw.Nodes.Add(nTag.Name ?? "root", nTag.Name, nTag.Type.Id, nTag.Type.Id);
            currentNode.Tag = nTag;

            if (nTag is INTagParent tagParent)
            {
                foreach (INTag? item in tagParent.Children)
                {
                    CreateForNode(currentNode, item);
                }
            }
            Controls.Add(tw);
        }
        catch (Exception ex)
        {
            Program.logger!.Exception(ex);
            throw;
        }
    }

    internal static EditorTabPage Create(Controller controller, DatapackFileInfo fileInfo) => new NBTEditorTabPage(controller, fileInfo);
}
