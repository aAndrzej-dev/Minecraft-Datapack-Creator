using System.ComponentModel;

namespace MinecraftDatapackCreator.Forms;

internal sealed partial class SettingsForm : Form
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool RequireReload { get; private set; }
    internal SettingsForm(Controller controller, string filename)
    {
        InitializeComponent();

        this.settings = controller.Settings;
        this.controller = controller;
        this.filename = filename;
        Text = $"{Program.ProductTitle} - Settings";

        cbJsonFormatting.Checked = settings.ReduceJsonFilesSize;
        cbFileFullPath.Checked = settings.AlwaysShowFullFilePathInDialogs;

        txtMinecraftDir.Text = settings.MinecraftDir;
        _dataFolder = settings.DatapackStructureDataFolder;
        txtStructureFolder.Text = settings.DatapackStructureDataFolder;

        btnTextEditorFont.Text = $"{settings.TextEditorFont.FontFamily.Name}, {settings.TextEditorFont.Size}pt";
        btnTextEditorFont.Font = new Font(settings.TextEditorFont.FontFamily, btnTextEditorFont.Font.Size, settings.TextEditorFont.Style);
        _textEditorFont = settings.TextEditorFont;


        btnJtfEditorFont.Text = $"{settings.JsonEditorFont.FontFamily.Name}, {settings.JsonEditorFont.Size}pt";
        btnJtfEditorFont.Font = new Font(settings.JsonEditorFont.FontFamily, btnJtfEditorFont.Font.Size, settings.JsonEditorFont.Style);
        _jsonEditorFont = settings.JsonEditorFont;

    }
    private readonly Settings settings;
    private readonly Controller controller;
    private readonly string filename;
    private Font _textEditorFont;
    private Font _jsonEditorFont;
    private string _dataFolder;
    private void BtnTextEditorFont_Click(object sender, EventArgs e)
    {
        fontDialog.Font = _textEditorFont;
        if (fontDialog.ShowDialog(this) == DialogResult.OK)
        {
            btnTextEditorFont.Text = $"{fontDialog.Font.FontFamily.Name}, {fontDialog.Font.Size}pt";
            btnTextEditorFont.Font = new Font(fontDialog.Font.FontFamily, btnTextEditorFont.Font.Size, fontDialog.Font.Style);
            _textEditorFont = fontDialog.Font;
        }
    }

    private void BtnJtfEditorFont_Click(object sender, EventArgs e)
    {
        fontDialog.Font = _jsonEditorFont;
        if (fontDialog.ShowDialog(this) == DialogResult.OK)
        {
            btnJtfEditorFont.Text = $"{fontDialog.Font.FontFamily.Name}, {fontDialog.Font.Size}pt";
            btnJtfEditorFont.Font = new Font(fontDialog.Font.FontFamily, btnJtfEditorFont.Font.Size, fontDialog.Font.Style);
            _jsonEditorFont = fontDialog.Font;
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        settings.ReduceJsonFilesSize = cbJsonFormatting.Checked;
        settings.TextEditorFont = _textEditorFont ?? settings.TextEditorFont;
        settings.JsonEditorFont = _jsonEditorFont ?? settings.JsonEditorFont;
        settings.MinecraftDir = txtMinecraftDir.Text;
        settings.DatapackStructureDataFolder = txtStructureFolder.Text;
        settings.AlwaysShowFullFilePathInDialogs = cbFileFullPath.Checked;
        settings.Save(filename);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnMinecraftDir_Click(object sender, EventArgs e)
    {
        fbdMinecraftDir.SelectedPath = txtMinecraftDir.Text;

        if (fbdMinecraftDir.ShowDialog(this) == DialogResult.OK)
        {
            txtMinecraftDir.Text = fbdMinecraftDir.SelectedPath;
        }
    }

    private void BtnStructureFolder_Click(object sender, EventArgs e)
    {
        fbdStructureFolder.SelectedPath = txtStructureFolder.Text;

        if (fbdStructureFolder.ShowDialog(this) == DialogResult.OK)
        {
            txtStructureFolder.Text = fbdStructureFolder.SelectedPath;
        }
    }

    private void TxtStructureFolder_TextChanged(object sender, EventArgs e)
    {
        if (!string.Equals(_dataFolder, txtStructureFolder.Text, StringComparison.OrdinalIgnoreCase))
        {
            btnSave.Text = "Reload";
            RequireReload = true;
        }
        else
        {
            btnSave.Text = "Save";
            RequireReload = false;
        }
    }

    //private void BtnDataUpdate_Click(object sender, EventArgs e)
    //{
    //    using DataUpdateForm duf = new DataUpdateForm(controller);
    //    duf.ShowDialog(this);
    //}
}
