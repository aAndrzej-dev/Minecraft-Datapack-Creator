using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MinecraftDatapackCreator.Forms;

internal partial class CreateProjectForm : Form
{
    public string Path => txtPath.Text;
    public string ProjectName => txtName.Text;



    protected override void OnTextChanged(EventArgs e) => base.OnTextChanged(e);



    public CreateProjectForm()
    {
        InitializeComponent();
    }

    private void BtnFiles_Click(object? sender, EventArgs e)
    {
        DialogResult dr = folderBrowserDialog.ShowDialog();
        if (dr == DialogResult.OK)
        {
            txtPath.Text = folderBrowserDialog.SelectedPath;
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show(this, "'Name' cant be null", "Minecraft Datapack Creator");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtPath.Text))
        {
            MessageBox.Show(this, "'Path' cant be null", "Minecraft Datapack Creator");
            return;
        }
        if (!IsValidPath(txtPath.Text))
        {
            MessageBox.Show(this, "'Path' is not valid", "Minecraft Datapack Creator");
            return;
        }

        DialogResult = DialogResult.OK;
    }
    private static bool IsValidPath(string path)
    {
        if (path.Length < 3)
        {
            return false;
        }

        Regex driveCheck = new(@"^[a-zA-Z]:\\$");
        if (!driveCheck.IsMatch(path[..3]))
        {
            return false;
        }

        string strTheseAreInvalidFileNameChars = new(System.IO.Path.GetInvalidPathChars());
        strTheseAreInvalidFileNameChars += ":/?*\"";
        Regex containsABadCharacter = new("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
        return !containsABadCharacter.IsMatch(path[3..]);
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {

    }

    private void CbtnClose_Click(object? sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void TxtName_TextChanged(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPath.Text) || !Datapack.IsValidResourceName(txtName.Text) || !IsValidPath(txtPath.Text))
        {
            btnAdd.Enabled = false;
            return;
        }



        btnAdd.Enabled = true;
    }

    private void TxtName_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar))
            return;
        if (e.KeyChar is ' ')
        {
            if (txtName.Text.Length == 0)
                e.Handled = true;
            e.KeyChar = '_';
        }
        e.KeyChar = char.ToLower(e.KeyChar);
        if (!Datapack.IsValidResourceName(e.KeyChar.ToString()))
            e.Handled = true;
    }
}
