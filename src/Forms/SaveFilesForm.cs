using MinecraftDatapackCreator.FileStructure;

namespace MinecraftDatapackCreator.Forms;
internal sealed partial class SaveFilesForm : Form
{
    internal SaveFilesForm(Controller controller, ReadOnlySpan<DatapackFileInfo> files)
    {
        InitializeComponent();
        Text = Program.ProductTitle;
        label1.Text = Properties.Resources.DialogSaveFilesQuestion;
        lbFiles.BeginUpdate();
        for (int i = 0; i < files.Length; i++)
        {
            if (controller.Settings.AlwaysShowFullFilePathInDialogs)
                lbFiles.Items.Add(files[i].FullName);
            else
                lbFiles.Items.Add(files[i].PathRelativeToSolution.ToString());

        }
        lbFiles.EndUpdate();
    }

    private void BtnYes_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Yes;
    }

    private void BtnNo_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void LbFiles_DrawItem(object sender, DrawItemEventArgs e)
    {
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
}
