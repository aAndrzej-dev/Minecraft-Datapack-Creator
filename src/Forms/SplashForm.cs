namespace MinecraftDatapackCreator.Forms;

internal partial class SplashForm : Form
{
    public SplashForm(bool reload = false)
    {
        InitializeComponent();
        label1.Visible = reload;
    }
}
