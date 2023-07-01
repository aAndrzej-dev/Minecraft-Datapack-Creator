using System.Windows.Forms.Integration;

namespace MinecraftDatapackCreator.Forms;
internal sealed partial class GoToFileForm : Form
{
    internal DatapackFileStructure Structure { get; }
    public string? SelectedFile { get; private set; }

    private readonly ElementHost host = new ElementHost();
    private readonly GoToFileFormContent gtfc;
    public GoToFileForm(DatapackFileStructure structure)
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterParent;
        Controls.Add(host);
        host.Dock = DockStyle.Fill;
        gtfc = new GoToFileFormContent(structure);
        host.Child = gtfc;
        gtfc.RequestClose += (s, e) =>
            {
                SelectedFile = gtfc.SelectedFile;
                Hide();
            };
        Structure = structure;
    }
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        gtfc.Show();
    }
}
