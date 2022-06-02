using System.ComponentModel;
using System.Text;

namespace MinecraftDatapackCreator.Forms;

public partial class MainForm : Form
{
    private Datapack? Solution { get => solutionExplorer.Solution; set { solutionExplorer.Solution = value; Text = Solution is null ? "Minecraft Datapack Creator" : $"{Solution.Name} - Minecraft Datapack Creator"; OnSolutionChanged(); } }
    private SelectTabPageForm? stpf;

    private void OnSolutionChanged()
    {
        bool isSolutionNotNull = Solution is not null;


        closeProjectToolStripMenuItem.Enabled = isSolutionNotNull;
        addNamespaceToolStripMenuItem.Enabled = isSolutionNotNull;
        addFileToolStripMenuItem.Enabled = isSolutionNotNull;

    }



    private void SetStyleMenuItem(ToolStripItem item)
    {
        item.ForeColor = Color.White;
        if (item is ToolStripMenuItem tsmi)
        {
            foreach (ToolStripItem i in tsmi.DropDownItems)
            {
                SetStyleMenuItem(i);
            }

        }

    }

    private readonly string DataPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "data");
    public MainForm(string[] args)
    {
        SplashForm? sw = null;

        Thread t = new(() =>
       {
           sw = new SplashForm();
           sw.ShowDialog();
       });
        t.Start();
        InitializeComponent();
      
        closeAllTabsToolStripMenuItem.Enabled = false;
        closeTabToolStripMenuItem.Enabled = false;
        saveAllToolStripMenuItem.Enabled = false;
        saveToolStripMenuItem.Enabled = false;


        OnSolutionChanged();
        if (args.Length > 0)
        {
            string path = args[0];

            if (File.Exists(path))
            {
                Solution = new Datapack(Path.GetDirectoryName(path)!);
            }
            else
            {
               
                MessageBox.Show(this, "File not exist:\n{path}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            }
        }

        mainMenuStrip.Renderer = new ToolStripRenderer();
        mainStatusStrip.Renderer = new ToolStripRenderer();
        mainMenuStrip.ForeColor = Color.White;


        foreach (ToolStripMenuItem item in mainMenuStrip.Items)
        {

            if (item is ToolStripMenuItem i)
            {
                SetStyleMenuItem(i);
            }

        }
        try
        {
            solutionExplorer.DatapackStructure = DatapackStructureItemsCollection.CreateDatapackStructure(Path.Combine(DataPath, "structure.json"));



        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        solutionExplorer.FileOpened += SolutionExplorer_FileOpened;
        sw?.Invoke(new FormDelagate(() => { sw.Close(); sw.Dispose(); }));

        t.Join();
    }


    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Activate();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (!CloseProject())
        {
            e.Cancel = true;
            return;
        }
        Application.Exit();
        base.OnClosing(e);
    }
    private delegate void FormDelagate();
    private void SolutionExplorer_FileOpened(object? sender, SolutionFileEventArgs e)
    {
        string fn = Path.Combine(Solution!.Path, "data", e.Namespace!, e.RelativePath!);
        if (tcMain.TabPages.Count > 0 && tcMain.TabPages.Cast<ITabPage>().Any(x => x.Filename == fn))
        {
            tcMain.SelectedTab = tcMain.TabPages.Cast<ITabPage>().First(x => x.Filename == fn) as TabPage;
            return;
        }

        ITabPage? page = e.StructureFolder is DatapackStructureFolderJTF j
            ? new JsonEditorTabPage(j.JTemplate, fn)
            : new TextEditorTabPage(fn);
        tcMain.Focus();
        tcMain.TabPages.Add((TabPage)page);
        tcMain.SelectedTab = (TabPage)page;
    }


    private void NewProjectToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        CreateProjectForm cpf = new();

        if (!CloseProject())
        {
            return;
        }
        if (cpf.ShowDialog() == DialogResult.OK)
        {

            Solution = new Datapack(Path.Combine(cpf.Path, cpf.ProjectName));
            File.Create(Path.Combine(Path.GetDirectoryName(Solution.Path)!, Solution.Name + "\\pack.mcmeta")).Close();
            Aadev.JTF.JTemplate template = new(Path.Combine(DataPath, "pack.mcmeta.jtf"));
            ITabPage page = new JsonEditorTabPage(template, Path.Combine(Path.GetDirectoryName(Solution.Path)!, Solution.Name + "\\pack.mcmeta"));
            tcMain.Focus();
            tcMain.TabPages.Add((TabPage)page);
            tcMain.SelectedTab = (TabPage)page;

        }
    }

    private void OpenProjectToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            OpenProject(openFileDialog.FileName);
        }
    }

    private void OpenProject(string path)
    {
        if (!CloseProject())
        {
            return;
        }

        Solution = new Datapack(Path.GetDirectoryName(path)!);

    }
    private bool CloseProject()
    {


        try
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Do you want to save changes in files:");
            bool hasChanged = false;
            for (int i = 0; i < tcMain.TabCount; i++)
            {
                if (tcMain.TabPages[i] is not ITabPage tp)
                    continue;

                if (tp.IsNotSaved)
                {
                    msg.AppendLine(Path.GetFileName(tp.Filename));
                    hasChanged = true;
                }
            }

            if (!hasChanged)
            {
                tcMain.TabPages.Clear();

                Solution = null;
                return true;
            }

            DialogResult dr = MessageBox.Show(this, msg.ToString(), Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (dr == DialogResult.Cancel)
                return false;

            if (dr == DialogResult.Yes)
            {
                foreach (ITabPage item in tcMain.TabPages)
                {
                    item.Save();
                }
            }

            tcMain.TabPages.Clear();

            Solution = null;
            return true;

        }
        catch (Exception)
        {

            throw;
        }

    }




    private void CloseProjectToolStripMenuItem_Click(object? sender, EventArgs e) => CloseProject();

    private void SaveToolStripMenuItem_Click(object? sender, EventArgs e) => (tcMain.SelectedTab as ITabPage)?.Save();
    private void SaveAllToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        foreach (ITabPage item in tcMain.TabPages)
        {
            item.Save();

        }
    }

    private void ExitToolStripMenuItem_Click(object? sender, EventArgs e) => Close();

    private void AddNamespaceToolStripMenuItem_Click(object? sender, EventArgs e) => solutionExplorer.AddNewNamespace();


    private void CloseTabToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        if (tcMain.SelectedTab != null)
        {
            tcMain.TabPages.Remove(tcMain.SelectedTab);
        }
    }

    private void WindowToolStripMenuItem_DropDownOpening(object? sender, EventArgs e)
    {


        while (windowToolStripMenuItem.DropDownItems.Count > 2)
        {
            windowToolStripMenuItem.DropDownItems.RemoveAt(windowToolStripMenuItem.DropDownItems.Count - 1);
        }
        if (tcMain.TabPages.Count > 0)
        {
            windowToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

            ToolStripMenuItem tsmi = new(tcMain.SelectedTab.Text);
            windowToolStripMenuItem.DropDownItems.Add(tsmi);
            tsmi.Tag = tcMain.SelectedTab;
            tsmi.Checked = true;
            tsmi.ForeColor = Color.White;
            tsmi.Click += (s, ev) =>
            {
                if (tcMain.TabPages.Contains((TabPage)tsmi.Tag))
                {
                    tcMain.SelectedTab = (TabPage)tsmi.Tag;
                }
            };


            for (int i = 0; i < Math.Min(tcMain.TabPages.Count, 10); i++)
            {
                if (i == tcMain.SelectedIndex)
                {
                    continue;
                }

                ToolStripMenuItem tsmi2 = new(tcMain.TabPages[i].Text);
                windowToolStripMenuItem.DropDownItems.Add(tsmi2);
                tsmi2.Tag = tcMain.TabPages[i];
                tsmi2.ForeColor = Color.White;
                tsmi2.Click += (s, ev) =>
                {
                    if (tcMain.TabPages.Contains((TabPage)tsmi2.Tag))
                    {
                        tcMain.SelectedTab = (TabPage)tsmi2.Tag;
                    }
                };
            }

        }
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Tab && tcMain.TabPages.Count > 0)
        {
            stpf = new SelectTabPageForm(tcMain.TabPages.Cast<TabPage>().ToArray(), tcMain.SelectedIndex, this);
            stpf.ShowDialog();
            tcMain.SelectedIndex = stpf.SelectedIndex;
            stpf = null;
        }
    }

    private void MainForm_KeyUp(object? sender, KeyEventArgs e)
    {

    }

    private void SolutionExplorer_FileSelected(object? sender, SolutionFileEventArgs e) => toolStripStatusLabel3.Text = Path.Combine(Solution!.Path, "data", e.Namespace!, e.RelativePath!);


    private void SolutionExplorer_Leave(object? sender, EventArgs e) => toolStripStatusLabel3.Text = "";

    private void SolutionExplorer_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not SolutionExplorer.SolutionNodeType.File)
        {
            toolStripStatusLabel3.Text = "";
        }
    }

    private void CloseAllTabsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        try
        {
            tcMain.TabPages.Clear();
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void AboutToolStripMenuItem_Click(object? sender, EventArgs e) => new AboutForm().ShowDialog();

    private void SolutionExplorer_MetaFileOpened(object? sender, FileEventArgs e)
    {


        if (tcMain.TabPages.Count > 0 && tcMain.TabPages.Cast<ITabPage>().Any(x => x.Filename == Path.Combine(Path.GetDirectoryName(Solution!.Path)!, Solution.Name + "\\pack.mcmeta")))
        {
            tcMain.SelectedTab = tcMain.TabPages.Cast<ITabPage>().First(x => x.Filename == Path.Combine(Path.GetDirectoryName(Solution!.Path)!, Solution.Name + "\\pack.mcmeta")) as TabPage;
            return;
        }
        Aadev.JTF.JTemplate template = new(Path.Combine(DataPath, "pack.mcmeta.jtf"));
        ITabPage page = new JsonEditorTabPage(template, Path.Combine(Path.GetDirectoryName(Solution!.Path)!, Solution.Name + "\\pack.mcmeta"));
        tcMain.Focus();
        tcMain.TabPages.Add((TabPage)page);
        tcMain.SelectedTab = (TabPage)page;


    }

    private void SettingsToolStripMenuItem_Click(object? sender, EventArgs e) { }// new SettingsForm().ShowDialog();





    private void TcMain_ControlAdded(object sender, ControlEventArgs e)
    {
        if (tcMain.TabPages.Count > 0)
        {
            closeTabToolStripMenuItem.Enabled = true;
            closeAllTabsToolStripMenuItem.Enabled = true;
            saveAllToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }
        else
        {
            closeAllTabsToolStripMenuItem.Enabled = false;
            closeTabToolStripMenuItem.Enabled = false;
            saveAllToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
        }
    }

    private void TcMain_ControlRemoved(object sender, ControlEventArgs e)
    {
        if (tcMain.TabPages.Count <= 1)
        {
            closeAllTabsToolStripMenuItem.Enabled = false;
            closeTabToolStripMenuItem.Enabled = false;
            saveAllToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
        }

    }

    private void AddFileToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }


















}
public class DarkColorTable : ProfessionalColorTable
{
    public override Color MenuItemSelected => Color.FromArgb(80, 80, 80);
    public override Color MenuBorder => Color.FromArgb(30, 30, 30);
    public override Color MenuItemBorder => Color.FromArgb(30, 30, 30);
    public override Color ToolStripDropDownBackground => Color.FromArgb(50, 50, 50);
    public override Color ImageMarginGradientBegin => Color.FromArgb(50, 50, 50);
    public override Color ImageMarginGradientEnd => Color.FromArgb(50, 50, 50);
    public override Color ImageMarginGradientMiddle => Color.FromArgb(50, 50, 50);
    public override Color MenuStripGradientBegin => Color.FromArgb(50, 50, 50);
    public override Color MenuStripGradientEnd => Color.FromArgb(50, 50, 50);
    public override Color MenuItemPressedGradientBegin => Color.FromArgb(50, 50, 50);
    public override Color MenuItemPressedGradientEnd => Color.FromArgb(50, 50, 50);
    public override Color MenuItemPressedGradientMiddle => Color.FromArgb(50, 50, 50);
    public override Color MenuItemSelectedGradientBegin => Color.FromArgb(50, 50, 50);
    public override Color MenuItemSelectedGradientEnd => Color.FromArgb(50, 50, 50);

}
public class ToolStripRenderer : ToolStripProfessionalRenderer
{
    public ToolStripRenderer() : base(new DarkColorTable())
    {

    }
}