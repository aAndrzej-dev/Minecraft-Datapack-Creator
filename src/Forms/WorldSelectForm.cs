using System.ComponentModel;

namespace MinecraftDatapackCreator.Forms;
internal sealed partial class WorldSelectForm : Form
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? SelectedWorld { get; private set; }

    private readonly WorldSelectFormContent gtfc;
    public WorldSelectForm(Controller controller)
    {
        InitializeComponent();

        gtfc = new WorldSelectFormContent(controller);
        host.Child = gtfc;
        gtfc.RequestClose += (s, e) =>
        {
            SelectedWorld = gtfc.SelectedWorld;
            DialogResult = DialogResult.OK;
        };
    }
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        gtfc.Show();
    }
}
