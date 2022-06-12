using Aadev.JTF.Editor;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace MinecraftDatapackCreator.Forms;

internal partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        lblName.Text = Application.ProductName;
        lblVersion.Text = $"Version: {Application.ProductVersion}";
        lblCopyright.Text = $"{((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright}";

        lblInstanceId.Text = $"Instance Id: {Program.InstanceId}";


    }

    private void AboutForm_HelpButtonClicked(object? sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;

    private void LlblMinecraftWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo("https://minecraft.fandom.com/wiki/Data_pack") { UseShellExecute = true });

    private void LlblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo("https://creativecommons.org/licenses/by-sa/3.0/") { UseShellExecute = true });
}
