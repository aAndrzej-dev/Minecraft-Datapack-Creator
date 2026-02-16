namespace MinecraftDatapackCreator.Forms;

internal sealed partial class SplashForm : Form
{
    public SplashForm(bool reload = false)
    {
        InitializeComponent();
        label1.Visible = reload;
    }
}
