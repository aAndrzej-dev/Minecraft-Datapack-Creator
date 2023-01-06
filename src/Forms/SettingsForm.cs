namespace MinecraftDatapackCreator.Forms;

public partial class SettingsForm : Form
{
    internal SettingsForm(Settings settings, string filename)
    {
        InitializeComponent();

        this.settings = settings;
        this.filename = filename;
        Text = $"{Program.ProductTitle} - Settings";

        cbJsonFormatting.Checked = settings.ReduceJsonFilesSize;
        txtMinecraftDir.Text = settings.MinecraftDir;
        txtStructureFolder.Text = settings.DatapackStructureDataFolder;

        btnTextEditorFont.Text = $"{settings.TextEditorFont.FontFamily.Name}, {settings.TextEditorFont.Size}pt";
        btnTextEditorFont.Font = new Font(settings.TextEditorFont.FontFamily, btnTextEditorFont.Font.Size, settings.TextEditorFont.Style);
        _textEditorFont = settings.TextEditorFont;


        btnJtfEditorFont.Text = $"{settings.JsonEditorFont.FontFamily.Name}, {settings.JsonEditorFont.Size}pt";
        btnJtfEditorFont.Font = new Font(settings.JsonEditorFont.FontFamily, btnJtfEditorFont.Font.Size, settings.JsonEditorFont.Style);
        _jsonEditorFont = settings.JsonEditorFont;
    }
    private readonly Settings settings;
    private readonly string filename;
    private Font _textEditorFont;
    private Font _jsonEditorFont;

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
        settings.Save(filename);

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
}
