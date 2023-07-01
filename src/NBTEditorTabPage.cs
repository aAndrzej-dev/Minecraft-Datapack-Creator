using Aadev.NBT;

namespace MinecraftDatapackCreator;
internal sealed class NBTEditorTabPage : EditorTabPage
{

    public override bool IsNotSaved { get => false; protected set { } }

    public override bool ReadOnly => true;

    public override bool CanUndo => false;

    public override bool CanRedo => false;

    public override event EventHandler? SavedStateChanged;

    public override void Save() { }

    public NBTEditorTabPage(DatapackFileInfo fileInfo, Settings settings) : base(fileInfo)
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
        TreeView tw = new TreeView()
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(50, 50, 50),
            ForeColor = Color.White,
            Font = settings.TextEditorFont,
            ImageList = il
        };
        try
        {


            NTag nTag = NReader.FromGzippedFile(fileInfo.FullName);

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
            MessageBox.Show(ex.Message, Program.ProductTitle);
        }
        ResumeLayout();
    }
    private void CreateForNode(TreeNode node, INTag nTag)
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
                currentNode.Text = $"<Emelent {node.Nodes.Count - 1}>";
            }
            foreach (INTag? item in nTagParent.Children)
            {

                CreateForNode(currentNode, item);

            }
        }
    }

    public override void Undo() => throw new NotImplementedException();
    public override void Redo() => throw new NotImplementedException();
}
