namespace MinecraftDatapackCreator.Forms;
internal sealed partial class OverrideMinecraftFileForm : Form
{

    public OverrideMinecraftFileForm(MinecraftStructure minecraftStructure, ISolutionItemInfo parent)
    {
        InitializeComponent();

        ReadOnlySpan<char> path = parent is SolutionVirtualItemInfo ? parent.DatapackStructureFolder?.Path : parent.ItemInfo!.PathRelativeToNamespace;

        MinecraftFolder? folder = minecraftStructure.GetFolder(path);
        if (folder is null)
            return;

        AddFolderToLb(folder, path);


    }

    public void AddFolderToLb(MinecraftFolder folder, ReadOnlySpan<char> basePath)
    {
        if (folder.TryGetFiles() is not null)
            foreach (MinecraftFile item in folder.TryGetFiles()!)
            {
                lbFiles.Items.Add(item.Path.AsSpan(basePath.Length + 1).ToString());
            }
        if (folder.TryGetFolders() is not null)
        {
            foreach (MinecraftFolder item in folder.TryGetFolders()!)
            {
                AddFolderToLb(item, basePath);
            }
        }

    }
    private void LbFiles_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index == -1)
            return;
        Graphics g = e.Graphics;

        using SolidBrush bgBrush = new SolidBrush(e.BackColor);
        using SolidBrush fgBrush = new SolidBrush(Color.White);
        g.FillRectangle(bgBrush, e.Bounds);
        string? text = lbFiles.Items[e.Index].ToString();
        SizeF size = g.MeasureString(text, e.Font ?? Font);
        Point loc = e.Bounds.Location;
        loc.Offset(new Point(8, (int)(e.Bounds.Height / 2f - size.Height / 2f)));

        g.DrawString(lbFiles.Items[e.Index].ToString(), e.Font ?? Font, fgBrush, loc);
    }

    private void LbFiles_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        DialogResult = DialogResult.OK;
    }

    private void BtnAddOverride_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }

    public string? SelectedItem => lbFiles.SelectedItem as string;
}
