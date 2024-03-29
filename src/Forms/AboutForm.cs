﻿using Aadev.JTF;
using Aadev.JTF.Editor;
using System.Diagnostics;
using System.Reflection;

namespace MinecraftDatapackCreator.Forms;

internal sealed partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        lblName.Text = Application.ProductName;
        lblVersion.Text = $"Version: {Application.ProductVersion}";
        lblCopyright.Text = $"{Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright}";

        lblInstanceId.Text = $"Instance Id: {Program.InstanceId}";


        lblJtfEditorVersion.Text = $"JSON Editor Version: {Assembly.GetAssembly(typeof(JsonJtfEditor))?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}";
        lblJTFVersion.Text = $"JTF Library Version: {Assembly.GetAssembly(typeof(JTemplate))?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion} (JTF_VERSION: {JTemplate.JTF_VERSION})";
    }

    private void AboutForm_HelpButtonClicked(object? sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;

    private void LlblMinecraftWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo("https://minecraft.fandom.com/wiki/Data_pack") { UseShellExecute = true });

    private void LlblLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo("https://creativecommons.org/licenses/by-sa/3.0/") { UseShellExecute = true });
}
