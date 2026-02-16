using MinecraftDatapackCreator.FileStructure;

namespace MinecraftDatapackCreator.Forms;
internal sealed partial class GoToFileForm : Form
{
    public DatapackFileInfo? SelectedFile => gtfc.SelectedFile;

    private readonly GoToFileFormContent gtfc;
    public GoToFileForm(DatapackFileStructure structure)
    {
        InitializeComponent();

        gtfc = new GoToFileFormContent(structure);
        host.Child = gtfc;
        gtfc.RequestClose += (s, e) => Hide();
    }
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        gtfc.Show();
    }
}
