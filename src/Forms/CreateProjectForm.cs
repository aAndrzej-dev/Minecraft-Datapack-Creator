using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MinecraftDatapackCreator.Forms;

internal partial class CreateProjectForm : Form
{
    public string Path => txtPath.Text;
    public string ProjectName => txtName.Text;
    public string NamespaceName => txtNamespace.Text;

    private string? projectName;
    private string? namespaceName;


    public CreateProjectForm()
    {
        InitializeComponent();
        txtPath.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Minecraft Datapack Creator", "datapacks");
    }

    private void BtnFiles_Click(object? sender, EventArgs e)
    {
        DialogResult dr = folderBrowserDialog.ShowDialog(this);
        if (dr is DialogResult.OK)
        {
            txtPath.Text = folderBrowserDialog.SelectedPath;
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show(this, Properties.Resources.DialogNullName, Program.ProductTitle);
            return;
        }
        if (string.IsNullOrWhiteSpace(txtPath.Text))
        {
            MessageBox.Show(this, Properties.Resources.DialogNullPath, Program.ProductTitle);
            return;
        }
        if (!IsValidPath(txtPath.Text))
        {
            MessageBox.Show(this, Properties.Resources.DialogInvalidPath, Program.ProductTitle);
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

        if (!driveCheck.IsMatch(path.AsSpan(0, 3).ToString()))
        {
            return false;
        }

        return !containsABadCharacter.IsMatch(path[3..]);
    }
    private static readonly Regex containsABadCharacter = new("[" + Regex.Escape($"{new string(System.IO.Path.GetInvalidPathChars())}:/?*\"") + "]", RegexOptions.Compiled);
    private static readonly Regex driveCheck = new(@"^[a-zA-Z]:\\$", RegexOptions.Compiled);

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }



    private void CbtnClose_Click(object? sender, EventArgs e) => DialogResult = DialogResult.Cancel;
    
    private void TxtName_TextChanged(object? sender, EventArgs e)
    {
        if (projectName == namespaceName)
        {
            namespaceName = txtName.Text;
            txtNamespace.Text = namespaceName;
        }
        projectName = txtName.Text;

        CheckValues();
    }

    private void CheckValues()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPath.Text) || string.IsNullOrWhiteSpace(txtNamespace.Text) || !Datapack.IsValidResourceName(txtName.Text) || !IsValidPath(txtPath.Text) || !Datapack.IsValidResourceName(txtNamespace.Text))
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
        e.KeyChar = char.ToLowerInvariant(e.KeyChar);
        if (!Datapack.IsValidResourceName(e.KeyChar.ToString()))
            e.Handled = true;
    }

    private void TxtNamespace_TextChanged(object sender, EventArgs e)
    {
        namespaceName = txtNamespace.Text;

        CheckValues();
    }
}
