using System.ComponentModel;
using System.IO.Compression;
using System.Text;
using static MinecraftDatapackCreator.SolutionExplorer;

namespace MinecraftDatapackCreator.Forms;

public partial class MainForm : Form
{
    private Datapack? Solution { get => solutionExplorer.Solution; set { solutionExplorer.Solution = value; Text = Solution is null ? "Minecraft Datapack Creator" : $"{Solution.Name} - {productTitle}"; OnSolutionChanged(); } }
    private SelectTabPageForm? stpf;
    private readonly ILogger logger;


    internal static readonly string productTitle = $"Minecraft Datapack Creator (v{Application.ProductVersion})";

    private void OnSolutionChanged()
    {
        if (Solution is not null)
            logger.Debug($"Loading datapack: {Solution?.Path}");
        bool isSolutionNotNull = Solution is not null;


        closeProjectToolStripMenuItem.Enabled = isSolutionNotNull;
        addNamespaceToolStripMenuItem.Enabled = isSolutionNotNull;
        exportToZipToolStripMenuItem.Enabled = isSolutionNotNull;

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

    private readonly Settings settings;
    private readonly string dataPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "data");
    public MainForm(string[] args, ILogger logger)
    {
        this.logger = logger;
        logger.Debug($"Initializing {Application.ProductName} (v{Application.ProductVersion})");


        string? settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator", "settings.json");
        if (File.Exists(settingsFile))
        {
            settings = Settings.Load(settingsFile)!;

        }
        else
        {
            settings = Settings.Default;
            settings.Save(settingsFile);
        }




        SplashForm? sw = null;

        Thread t = new(() =>
       {
           sw = new SplashForm();
           sw.ShowDialog();
       });
        t.Start();
        InitializeComponent();

        Text = productTitle;
        try
        {
            solutionExplorer.DatapackStructure = DatapackStructureItemsCollection.CreateDatapackStructure(Path.Combine(dataPath, "structure.json"), logger);
        }
        catch (Exception ex)
        {
            logger.Error($"Cannot load datapack structure: {ex.Message}");
            MessageBox.Show(ex.Message);
        }
        solutionExplorer.FileOpened += SolutionExplorer_FileOpened;


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
                logger.Error($"Cannot load datapack: File not exist: {path}");
                MessageBox.Show(this, $"File not exist:\n{path}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

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



        sw?.Invoke(new FormDelagate(() => { sw.Close(); sw.Dispose(); }));

        t.Join();
        logger.Debug($"Initialization compleated");

    }


    protected override void OnShown(EventArgs e)
    {
        logger.Debug($"Showing Main Window");
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
        logger.Debug($"Closing Datapack Creator");
        Application.Exit();
        base.OnClosing(e);
    }
    private delegate void FormDelagate();
    private void SolutionExplorer_FileOpened(object? sender, SolutionFileEventArgs e)
    {
        if (tcMain.TabPages.Count > 0 && tcMain.TabPages.Cast<ITabPage>().Any(x => x.Filename == e.Filename))
        {
            tcMain.SelectedTab = tcMain.TabPages.Cast<ITabPage>().First(x => x.Filename == e.Filename) as TabPage;
            return;
        }

        logger.Debug($"Loading file: {e.Filename}");

        ITabPage? page = e.StructureFolder is DatapackStructureFolderJTF j
            ? new JsonEditorTabPage(j.JTemplate, e.Filename, settings) { TabBackColor = e.StructureFolder.TabBackColor, TabForeColor = e.StructureFolder.TabForeColor }
            : new TextEditorTabPage(e.Filename, settings) { TabBackColor = e.StructureFolder.TabBackColor, TabForeColor = e.StructureFolder.TabForeColor };
        tcMain.Focus();

        TabPage tp = (TabPage)page;
        tp.ToolTipText = $"{e.StructureFolder.DisplayName}\n{e.Namespace}:{e.RelativePath}\n{e.Filename}";


        tcMain.TabPages.Add(tp);
        tcMain.SelectedTab = tp;
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
            Aadev.JTF.JTemplate template = new(Path.Combine(dataPath, "pack.mcmeta.jtf"));
            logger.Debug($"Loading file: {Path.Combine(Path.GetDirectoryName(Solution.Path)!, Solution.Name + "\\pack.mcmeta")}");
            ITabPage page = new JsonEditorTabPage(template, Path.Combine(Path.GetDirectoryName(Solution.Path)!, Solution.Name + "\\pack.mcmeta"), settings);
            tcMain.Focus();

            TabPage tp = (TabPage)page;

            tp.ToolTipText = Path.Combine(Path.GetDirectoryName(Solution.Path)!, Solution.Name + "\\pack.mcmeta");

            tcMain.TabPages.Add(tp);
            tcMain.SelectedTab = tp;

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

        logger.Debug($"Closing datapack");
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
        catch (Exception ex)
        {
            logger.Error($"Cannot close solution: {ex.Message}");
            return false;
        }

    }




    private void CloseProjectToolStripMenuItem_Click(object? sender, EventArgs e) => CloseProject();

    private void SaveToolStripMenuItem_Click(object? sender, EventArgs e) => (tcMain.SelectedTab as ITabPage)?.Save();//if (tcMain.SelectedTab?.Controls[0] is JsonJTFEditorLibrary.JsonJtfEditor editor)//{//    editor.Save();//    if (tcMain.SelectedTab.Text.EndsWith("*"))//    {//        tcMain.SelectedTab.Text = tcMain.SelectedTab.Text[0..^1];//    }//}//else if (tcMain.SelectedTab?.Controls[0] is RichTextBox txtbox)//{//    File.WriteAllText(txtbox.Tag.ToString(), txtbox.Text);//    if (tcMain.SelectedTab.Text.EndsWith("*"))//    {//        tcMain.SelectedTab.Text = tcMain.SelectedTab.Text[0..^1];//    }//}

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

            ToolStripMenuItem tsmi = new($"&1 {tcMain.SelectedTab.Text}");
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

            int x = 2;
            for (int i = 0; i < Math.Min(tcMain.TabPages.Count, 10); i++)
            {


                if (i == tcMain.SelectedIndex)
                {
                    x--;
                    continue;
                }

                string prefix = i == 10 - x ? "1&0 " : $"&{i + x} ";


                ToolStripMenuItem tsmi2 = new(prefix + tcMain.TabPages[i].Text);
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


    private void SolutionExplorer_FileSelected(object? sender, SolutionFileEventArgs e)
    {
        toolStripStatusLabel3.Text = e.Filename;
        toolStripStatusLabel4.Text = $"{e.Namespace}:{e.RelativePath?.Replace('\\', '/')}";
    }


    private void SolutionExplorer_Leave(object? sender, EventArgs e)
    {
        toolStripStatusLabel3.Text = string.Empty;
        toolStripStatusLabel4.Text = string.Empty;
    }

    private void SolutionExplorer_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        SolutionNodeInfo tag = (SolutionNodeInfo)e.Node?.Tag!;
        if (tag.solutionNodeType is not SolutionNodeType.File)
        {
            toolStripStatusLabel3.Text = string.Empty;
            toolStripStatusLabel4.Text = string.Empty;
        }
    }

    private void CloseAllTabsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        try
        {
            //tcMain.SelectedIndex = 0;
            while (tcMain.SelectedTab != null)
            {
                tcMain.TabPages.Remove(tcMain.SelectedTab);

            }
        }
        catch (Exception ex)
        {
            logger.Error($"Cannot close all tabs: {ex.Message}");
            throw;
        }
    }

    private void AboutToolStripMenuItem_Click(object? sender, EventArgs e) => new AboutForm().ShowDialog();

    private void SolutionExplorer_MetaFileOpened(object? sender, FileEventArgs e)
    {
        if (Solution is null)
            return;
        if (tcMain.TabPages.Count > 0 && tcMain.TabPages.Cast<ITabPage>().Any(x => x.Filename == e.Filename))
        {
            tcMain.SelectedTab = tcMain.TabPages.Cast<ITabPage>().First(x => x.Filename == e.Filename) as TabPage;
            return;
        }
        Aadev.JTF.JTemplate template = new(Path.Combine(dataPath, "pack.mcmeta.jtf"));
        logger.Debug($"Loading file: {e.Filename}");
        ITabPage page = new JsonEditorTabPage(template, e.Filename, settings);
        tcMain.Focus();

        TabPage tp = (TabPage)page;

        tp.ToolTipText = e.Filename;

        tcMain.TabPages.Add(tp);
        tcMain.SelectedTab = tp;
    }

    private void SettingsToolStripMenuItem_Click(object? sender, EventArgs e) => new SettingsForm(settings, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator", "settings.json")).ShowDialog();





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


    private void ExportToZipToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (Solution is null)
            return;

        sfdExportToZip.FileName = Path.GetFileName(Solution.Path);

        if (sfdExportToZip.ShowDialog() == DialogResult.OK)
        {
            logger.Debug($"Exporting datapack to zip. File name: {sfdExportToZip.FileName}");
            ZipFile.CreateFromDirectory(Solution.Path, sfdExportToZip.FileName, CompressionLevel.Optimal, false);
        }
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