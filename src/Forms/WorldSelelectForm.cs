using System.IO;
using System.Windows.Forms.Integration;

namespace MinecraftDatapackCreator.Forms;
internal sealed partial class WorldSelelectForm : Form
{
    public string? SelectedFile { get; private set; }

    private readonly ElementHost host = new ElementHost();
    private readonly WorldSelectFormContent gtfc;
    public WorldSelelectForm(Settings settings)
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterParent;
        Controls.Add(host);
        host.Dock = DockStyle.Fill;
        gtfc = new WorldSelectFormContent(settings);
        host.Child = gtfc;
    }
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        gtfc.Show();
    }
}
