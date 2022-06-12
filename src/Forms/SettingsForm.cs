using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftDatapackCreator.Forms
{
    public partial class SettingsForm : Form
    {
        internal SettingsForm(Settings settings, string filename)
        {
            InitializeComponent();

            this.settings = settings;
            this.filename = filename;


            cbConditionsCount.Checked = settings.ShowConditionsInJsonEditor;
            cbJsonFormatting.Checked = settings.ReduceJsonFilesSize;


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
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                btnTextEditorFont.Text = $"{fontDialog.Font.FontFamily.Name}, {fontDialog.Font.Size}pt";
                btnTextEditorFont.Font = new Font(fontDialog.Font.FontFamily, btnTextEditorFont.Font.Size, fontDialog.Font.Style);
                _textEditorFont = fontDialog.Font;
            }
        }

        private void BtnJtfEditorFont_Click(object sender, EventArgs e)
        {
            fontDialog.Font = _jsonEditorFont;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                btnJtfEditorFont.Text = $"{fontDialog.Font.FontFamily.Name}, {fontDialog.Font.Size}pt";
                btnJtfEditorFont.Font = new Font(fontDialog.Font.FontFamily, btnJtfEditorFont.Font.Size, fontDialog.Font.Style);
                _jsonEditorFont = fontDialog.Font;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            settings.ShowConditionsInJsonEditor = cbConditionsCount.Checked;
            settings.ReduceJsonFilesSize = cbJsonFormatting.Checked;
            settings.TextEditorFont = _textEditorFont ?? settings.TextEditorFont;
            settings.JsonEditorFont = _jsonEditorFont ?? settings.JsonEditorFont;
            settings.Save(filename);

            Close();
        }
    }
}
