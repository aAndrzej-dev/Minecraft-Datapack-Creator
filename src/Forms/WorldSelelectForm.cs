using System.IO;

namespace MinecraftDatapackCreator.Forms;
public partial class WorldSelelectForm : Form
{
    public string? SelectedWorld => lbWorlds.SelectedItem?.ToString();


    internal WorldSelelectForm(Settings settings)
    {
        InitializeComponent();
        DirectoryInfo di = new DirectoryInfo(Path.Combine(settings.MinecraftDir, "saves"));
        if (!di.Exists)
            return;

        foreach (DirectoryInfo item in di.GetDirectories())
        {
            lbWorlds.Items.Add(item.Name);

        }

    }

    private void LbWorlds_DrawItem(object sender, DrawItemEventArgs e)
    {

        Graphics g = e.Graphics;
        string? itemText = lbWorlds.Items[e.Index].ToString();
        e.DrawBackground();

        SizeF textSize = g.MeasureString(itemText, Font);
        g.DrawString(itemText, Font, new SolidBrush(e.ForeColor), new PointF(16, e.Bounds.Top + e.Bounds.Height / 2 - textSize.Height / 2));


    }

    private void LbWorlds_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        int index = lbWorlds.IndexFromPoint(e.Location);
        if (index is not ListBox.NoMatches)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
