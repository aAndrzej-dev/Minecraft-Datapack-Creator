using System.Text.RegularExpressions;

namespace MinecraftDatapackCreator.Forms;

internal sealed partial class CreateProjectForm : Form
{
    public string Path => txtPath.Text;
    public string ProjectName => txtName.Text;
    public string NamespaceName => txtNamespace.Text;

    private string? projectName;
    private string? namespaceName;
    private static string defaultPath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Minecraft Datapack Creator", "datapacks");

    public CreateProjectForm()
    {
        InitializeComponent();
        txtPath.Text = defaultPath;
    }

    private void BtnFiles_Click(object? sender, EventArgs e)
    {
        DialogResult dr = folderBrowserDialog.ShowDialog(this);
        if (dr is DialogResult.OK)
        {
            txtPath.Text = folderBrowserDialog.SelectedPath;
        }
    }

    private void BtnCreate_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show(this, Properties.Resources.DialogNullName, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if (string.IsNullOrWhiteSpace(txtPath.Text))
        {
            MessageBox.Show(this, Properties.Resources.DialogNullPath, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if (!IsValidPath(txtPath.Text))
        {
            MessageBox.Show(this, Properties.Resources.DialogInvalidPath, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if(System.IO.Directory.Exists(System.IO.Path.Join(Path, ProjectName)))
        {
            MessageBox.Show(this, Properties.Resources.DialogDatapackAlreadyExist, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }


        DialogResult = DialogResult.OK;
    }
    private static bool IsValidPath(ReadOnlySpan<char> path)
    {
        if (path.Length < 3)
        {
            return false;
        }

        if (!driveCheck.IsMatch(path[..3]))
        {
            return false;
        }

        return !containsABadCharacter.IsMatch(path[3..]);
    }
    private static readonly Regex containsABadCharacter = new Regex("[" + Regex.Escape($"{new string(System.IO.Path.GetInvalidPathChars())}:/?*\"") + "]", RegexOptions.Compiled);
    private static readonly Regex driveCheck = DriveCheckRegex();

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
            btnCreate.Enabled = false;
            return;
        }



        btnCreate.Enabled = true;
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
        if (!Datapack.IsValidResourceName(stackalloc char[1] { e.KeyChar }))
            e.Handled = true;
    }

    private void TxtNamespace_TextChanged(object sender, EventArgs e)
    {
        namespaceName = txtNamespace.Text;

        CheckValues();
    }

    [GeneratedRegex("^[a-zA-Z]:\\\\$", RegexOptions.Compiled)]
    private static partial Regex DriveCheckRegex();
}