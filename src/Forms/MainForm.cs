using Aadev.JTF;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace MinecraftDatapackCreator.Forms;

public partial class MainForm : Form
{
    private GoToFileForm? goToForm;

    private DatapackStructureFoldersCollection? Structure { get; set; }
    private Datapack? Solution
    {
        get => solutionExplorer.Solution;
        set
        {
            Solution?.FileStructure.Dispose();
            solutionExplorer.Solution = value;
            solutionExplorer.Solution = Solution;
            Text = Solution is null ? Program.ProductTitle : $"{Solution.Name} - {Program.ProductTitle}";


            OnSolutionChanged();

            goToForm = null;

            if (Solution is null)
                return;
            string recentProjectFile = Path.Combine(appDataFolder, "recentProjects.json");


            JArray root = File.Exists(recentProjectFile) ? JArray.Parse(File.ReadAllText(recentProjectFile)) : new JArray();
            JObject jobject = new JObject();
            string fullPath = Path.GetFullPath(Path.Combine(Solution.Path, "pack.mcmeta"));


            jobject.Add("path", fullPath);

            if (root.Any(x => x["path"]!.ToString() == fullPath))
            {
                return;
            }
            jobject.Add("lastUsed", DateTime.Now.ToString(CultureInfo.InvariantCulture));

            root.Add(jobject);
            File.WriteAllText(recentProjectFile, root.ToString(Newtonsoft.Json.Formatting.None));


            recentProjectsToolStripMenuItem.DropDownItems.Clear();
            int index = 1;
            foreach (JToken item in root)
            {
                string? path = item["path"]?.ToString();

                string indexString = index == 10 ? "1&0" : (index > 10 ? index.ToString(CultureInfo.InvariantCulture) : $"&{index}");
                index++;
                ToolStripMenuItem tsmi = new ToolStripMenuItem()
                {
                    Text = $"{indexString} {path}",
                    ToolTipText = path,
                    AutoToolTip = true,
                    ForeColor = Color.White
                };
                tsmi.Click += (s, e) => OpenProject(path!);
                recentProjectsToolStripMenuItem.DropDownItems.Add(tsmi);
            }




        }
    }
    private SelectTabPageForm? stpf;
    private readonly ILogger logger;
    private readonly string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator");
    private JTemplate? mcmetaFileTemplate;

    private void OnSolutionChanged()
    {
        if (Solution is not null)
        {
            logger.Debug($"Loading datapack: {Solution?.Path}");
        }

        bool isSolutionNotNull = Solution is not null;


        closeProjectToolStripMenuItem.Enabled = isSolutionNotNull;
        addNamespaceToolStripMenuItem.Enabled = isSolutionNotNull;
        exportToZipToolStripMenuItem.Enabled = isSolutionNotNull;
        goToFileToolStripMenuItem.Enabled = isSolutionNotNull;
        attachDatapackToWorldToolStripMenuItem.Enabled = isSolutionNotNull;
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
    public MainForm(string[] args, ILogger logger)
    {
        this.logger = logger;
        logger.Debug($"Initializing {Application.ProductName} (v{Application.ProductVersion})");


        string settingsFile = Path.Combine(appDataFolder, "settings.json");
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



        string recentProjectFile = Path.Combine(appDataFolder, "recentProjects.json");
        if (File.Exists(recentProjectFile))
        {
            JArray root = JArray.Parse(File.ReadAllText(recentProjectFile));
            int index = 1;
            foreach (JToken item in root)
            {
                string? path = item["path"]?.ToString();

                string indexString = index == 10 ? "1&0" : (index > 10 ? index.ToString(CultureInfo.InvariantCulture) : $"&{index}");
                index++;
                ToolStripMenuItem tsmi = new ToolStripMenuItem()
                {
                    Text = $"{indexString} {path}",
                    ToolTipText = path,
                    AutoToolTip = true,
                };
                tsmi.Click += (s, e) => OpenProject(path!);
                recentProjectsToolStripMenuItem.DropDownItems.Add(tsmi);
            }
        }




        Text = Program.ProductTitle;
        try
        {
            mcmetaFileTemplate = new(Path.Combine(settings.DatapackStructureDataFolder, "pack.mcmeta.jtf"));
            Structure = DatapackStructureFoldersCollection.CreateDatapackStructure(Path.Combine(settings.DatapackStructureDataFolder, "structure.json"), logger, settings.DatapackStructureDataFolder);
        }
        catch (Exception ex)
        {
            logger.Error($"Cannot load datapack structure: {ex.Message}");
            //MessageBox.Show(this, ex.Message, Program.ProductTitle);
            throw;
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
                OpenProject(path);

            }
            else
            {
                logger.Error($"Cannot load datapack: File not exist: {path}");
                MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Properties.Resources.DialogFileNotFound, path), Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        mainMenuStrip.Renderer = new DarkToolStripRenderer();
        mainToolStrip.Renderer = new DarkToolStripRenderer();
        mainStatusStrip.Renderer = new DarkToolStripRenderer();
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
    private void SolutionExplorer_FileOpened(object? sender, DatapackFileEventArgs e) => OpenDatapackFile(e.FileInfo);
    private void OpenDatapackFile(DatapackFileInfo file)
    {
        foreach (EditorTabPage item in tcMain.TabPages)
        {
            if (item.FileInfo == file)
            {
                tcMain.SelectedTab = item;
                return;
            }
        }






        logger.Debug($"Loading file: {file.FullName}");

        EditorTabPage? page;
        if (file.DatapackStructureFolder?.Editor is "aadev:nbtEditor")
            page = new NBTEditorTabpage(file, settings);
        else if (file.DatapackStructureFolder is DatapackStructureFolderJTF j)
            page = new JsonEditorTabPage(j.Template, file, settings, GetNamespacedSourceAsSuggestions);
        else
            page = new TextEditorTabPage(file, settings);
        tcMain.Focus();




        tcMain.TabPages.Add(page);
        tcMain.SelectedTab = page;
    }
    private void OpenMetaFile()
    {
        if (Solution is null)
        {
            return;
        }
        string filename = Path.Combine(Solution.Path, "pack.mcmeta");

        DatapackFileInfo? file = Solution.FileStructure.GetFile(filename);

        EditorTabPage? existingTab = tcMain.TabPages.Cast<EditorTabPage>().FirstOrDefault(x => x.FileInfo == file);

        if (existingTab is not null)
        {
            tcMain.SelectedTab = existingTab;
            return;
        }
        logger.Debug($"Loading file: {filename}");
        EditorTabPage page = new JsonEditorTabPage(mcmetaFileTemplate!, file!, settings, GetNamespacedSourceAsSuggestions);
        tcMain.Focus();

        tcMain.TabPages.Add(page);
        tcMain.SelectedTab = page;
    }

    private void NewProjectToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        CreateProjectForm cpf = new();

        if (!CloseProject())
        {
            return;
        }
        if (cpf.ShowDialog(this) == DialogResult.OK)
        {

            Datapack solution = new Datapack(Path.Combine(cpf.Path, cpf.ProjectName), Structure!);

            DirectoryInfo di = new DirectoryInfo(Path.Combine(solution.Path, Datapack.DATA_FOLDER_NAME, cpf.NamespaceName));
            if (!di.Exists)
            {
                di.Create();
            }

            DirectoryInfo mctagsdi = new DirectoryInfo(Path.Combine(solution.Path, Datapack.DATA_FOLDER_NAME, "minecraft", "tags", "functions"));
            if (!mctagsdi.Exists)
            {
                mctagsdi.Create();
            }

            FileInfo loadfi = new FileInfo(Path.Combine(mctagsdi.FullName, "load.json"));
            if (!loadfi.Exists)
            {
                loadfi.Create().Close();
            }

            FileInfo tickfi = new FileInfo(Path.Combine(mctagsdi.FullName, "tick.json"));
            if (!tickfi.Exists)
            {
                tickfi.Create().Close();
            }

            Solution = solution;


            File.Create(Path.Combine(Path.GetDirectoryName(Solution.Path)!, Solution.Name + "\\pack.mcmeta")).Close();

            OpenMetaFile();
        }
    }

    private void OpenProjectToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
            OpenProject(openFileDialog.FileName);
        }
    }

    private void OpenProject(string path)
    {
        if (Path.GetFileName(path) != "pack.mcmeta")
        {
            MessageBox.Show(this, $"Cannot open datapack: \n{path}", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!CloseProject())
        {
            return;
        }



        Solution = new Datapack(Path.GetDirectoryName(path)!, Structure!);
    }

    private bool ClearTabsSafe()
    {
        StringBuilder msg = new StringBuilder();
        msg.AppendLine(Properties.Resources.DialogSaveFilesQuestion);
        bool hasChanged = false;
        for (int i = 0; i < tcMain.TabCount; i++)
        {
            if (tcMain.TabPages[i] is not EditorTabPage tp)
            {
                continue;
            }

            if (tp.IsNotSaved)
            {
                msg.AppendLine(tp.FileInfo.Name);
                hasChanged = true;
            }
        }

        if (!hasChanged)
        {
            tcMain.DisableSelect = true;
            tcMain.TabPages.Clear();
            tcMain.DisableSelect = false;
            return true;
        }

        DialogResult dr = MessageBox.Show(this, msg.ToString(), Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

        if (dr is DialogResult.Cancel)
        {
            return false;
        }

        if (dr is DialogResult.Yes)
        {
            foreach (EditorTabPage item in tcMain.TabPages)
            {
                item.Save();
            }
        }

        tcMain.DisableSelect = true;
        tcMain.TabPages.Clear();
        tcMain.DisableSelect = false;
        return true;
    }
    private bool CloseProject()
    {

        logger.Debug($"Closing datapack");
        try
        {
            if (!ClearTabsSafe())
                return false;


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

    private void SaveToolStripMenuItem_Click(object? sender, EventArgs e) => (tcMain.SelectedTab as EditorTabPage)?.Save();

    private void SaveAllToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        foreach (EditorTabPage item in tcMain.TabPages)
        {
            item.Save();
        }
    }

    private void ExitToolStripMenuItem_Click(object? sender, EventArgs e) => Close();

    private void AddNamespaceToolStripMenuItem_Click(object? sender, EventArgs e) => solutionExplorer.AddNewNamespace();


    private void CloseTabToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        if (tcMain.SelectedTab is not null)
        {
            if (tcMain.SelectedTab is EditorTabPage tabPage)
            {
                if (tabPage.IsNotSaved)
                {
                    DialogResult dr = MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Properties.Resources.DialogSaveFileQuestion, Path.GetRelativePath(Solution!.Path, tabPage.FileInfo.FullName)), Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (dr is DialogResult.Yes)
                    {
                        tabPage.Save();
                    }
                    if (dr is DialogResult.Cancel)
                        return;
                }
            }
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
            stpf = new SelectTabPageForm(tcMain.TabPages.Cast<EditorTabPage>().ToArray(), tcMain.SelectedIndex, this);
            stpf.ShowDialog(this);
            tcMain.SelectedIndex = stpf.SelectedIndex;
            stpf = null;
        }
    }


    private void SolutionExplorer_FileSelected(object? sender, DatapackFileEventArgs e)
    {
        toolStripStatusLabel3.Text = e.Filename;
        toolStripStatusLabel4.Text = e.FileInfo.NamespacedId;
    }


    private void SolutionExplorer_Leave(object? sender, EventArgs e)
    {
        toolStripStatusLabel3.Text = string.Empty;
        toolStripStatusLabel4.Text = string.Empty;
    }

    private void SolutionExplorer_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not SolutionNodeInfo tag)
            return;
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
            ClearTabsSafe();
        }
        catch (Exception ex)
        {
            logger.Error($"Cannot close all tabs: {ex.Message}");
            throw;
        }
    }

    private void AboutToolStripMenuItem_Click(object? sender, EventArgs e) => new AboutForm().ShowDialog(this);

    private void SolutionExplorer_MetaFileOpened(object? sender, FileEventArgs e)
    {
        OpenMetaFile();
    }

    private void SettingsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        new SettingsForm(settings, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator", "settings.json")).ShowDialog(this);
    }




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
        {
            return;
        }

        sfdExportToZip.FileName = Path.GetFileName(Solution.Path);

        if (sfdExportToZip.ShowDialog(this) == DialogResult.OK)
        {
            logger.Debug($"Exporting datapack to zip. File name: {sfdExportToZip.FileName}");
            ZipFile.CreateFromDirectory(Solution.Path, sfdExportToZip.FileName, CompressionLevel.Optimal, false);
        }
    }

    private void AttachDatapackToWorldToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }

        WorldSelelectForm wsf = new WorldSelelectForm(settings);
        if (wsf.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        string worldPath = Path.Combine(settings.MinecraftDir, "saves", wsf.SelectedWorld!);



        if (!Directory.Exists(worldPath))
        {
            MessageBox.Show(this, $"Invalid World! \n{worldPath}", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        DirectoryInfo di = new DirectoryInfo(Path.Combine(worldPath, "datapacks"));
        if (!di.Exists)
        {
            di.Create();
        }

        string filename = Path.Combine(worldPath, "datapacks", Path.GetFileName(Solution.Path) + ".zip");

        if (File.Exists(filename))
        {
            DialogResult dr = MessageBox.Show(this, $"Do you want to override file: {filename}?", Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr is not DialogResult.Yes)
            {
                return;
            }

            File.Delete(filename);
        }



        ZipFile.CreateFromDirectory(Solution.Path, filename, CompressionLevel.Optimal, false);
    }

    private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string? solutionPath = null;

        logger.Debug($"Reloading structure");
        if (Solution is not null)
        {
            solutionPath = Solution.Path;
            if (!CloseProject())
            {
                return;
            }
        }

        SplashForm? sf = null;

        Thread t = new(() =>
        {
            sf = new SplashForm(true);
            sf.ShowDialog();
        });
        t.Start();
        JTemplate.RemoveCustomSourcesCache();
        tcMain.Controls.Clear();
        try
        {
            mcmetaFileTemplate = new(Path.Combine(settings.DatapackStructureDataFolder, "pack.mcmeta.jtf"));
            Structure = DatapackStructureFoldersCollection.CreateDatapackStructure(Path.Combine(settings.DatapackStructureDataFolder, "structure.json"), logger, settings.DatapackStructureDataFolder);
        }
        catch (Exception ex)
        {
            logger.Error($"Cannot load datapack structure: {ex.Message}");
            MessageBox.Show(this, ex.Message, Program.ProductTitle);
        }
        if (solutionPath is not null)
        {
            Solution = new Datapack(solutionPath, Structure!);
        }

        logger.Debug($"Reloaded succesfully");


        sf?.Invoke(new FormDelagate(() => { sf.Close(); sf.Dispose(); }));

        t.Join();
    }

    private static readonly string[] allowedDynamicSources = new string[]
    {
        "structure:advancements",
        "structure:functions",
        "structure:loot_tables",
        "structure:item_modifiers",
        "structure:predicates",
        "structure:recipes",
        "structure:structures",
        "structure:dimension_type",
        "structure:dimension",
        "structure:worldgen/biome",
        "structure:worldgen/configured_carver",
        "structure:worldgen/configured_feature",
        "structure:worldgen/configured_structure_feature",
        "structure:worldgen/configured_surface_builder",
        "structure:worldgen/noise_settings",
        "structure:worldgen/processor_list",
        "structure:worldgen/template_pool",
        "structure:worldgen/structure_set",
        "structure:worldgen/density_function",
        "structure:worldgen/flat_level_generator_preset",
        "structure:worldgen/noise",
        "structure:worldgen/placed_feature",
        "structure:worldgen/structure",
        "structure:worldgen/world_preset",
        "structure:tags/blocks",
        "structure:tags/entity_types",
        "structure:tags/fluids",
        "structure:tags/functions",
        "structure:tags/items",
        "structure:tags/game_events",
        "structure:tags/worldgen/biome",
        "structure:tags/worldgen/structure",
        "structure:tags/worldgen/world_preset",
        "structure:tags/worldgen/flat_level_generator_preset"
    };
    private IEnumerable<IJtSuggestion> GetNamespacedSourceAsSuggestions(JtIdentifier id)
    {

        if (id.Identifier?.StartsWith("structure:", StringComparison.OrdinalIgnoreCase) is false or null || Solution is null)
        {
            return Enumerable.Empty<IJtSuggestion>();
        }

        if (!allowedDynamicSources.Contains(id.Identifier))
        {
            return Enumerable.Empty<IJtSuggestion>();
        }
        string structurePath = id.Identifier.AsSpan(10).ToString();
        return Solution.FileStructure.GetFiles().Where(x => x.DatapackStructureFolder?.Path == structurePath).Select(x => new JtSuggestion<string>(x.NamespacedId!, $"{x.NamespacedId} ({x.DatapackStructureFolder?.Path})")).ToArray();
    }

    private void FileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        if ((ModifierKeys & Keys.Shift) != 0)
        {
            reloadToolStripMenuItem.Visible = true;
            toolStripSeparator6.Visible = true;
        }
        else
        {
            reloadToolStripMenuItem.Visible = false;
            toolStripSeparator6.Visible = false;
        }
    }

    private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is EditorTabPage tabPage && tabPage.CanUndo)
        {
            tabPage.Undo();
        }
    }

    private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is EditorTabPage tabPage && tabPage.CanRedo)
        {
            tabPage.Redo();
        }
    }

    private void EditToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is EditorTabPage tabPage)
        {
            if (tabPage.CanUndo)
                undoToolStripMenuItem.Enabled = true;
            else
                undoToolStripMenuItem.Enabled = false;
            if (tabPage.CanRedo)
                redoToolStripMenuItem.Enabled = true;
            else
                redoToolStripMenuItem.Enabled = false;
        }
        else
        {
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
        }
        if (tcMain.SelectedTab is TextEditorTabPage tetp)
        {
            cutToolStripMenuItem.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;
            selectAllToolStripMenuItem.Enabled = true;
        }
        else
        {
            cutToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
            pasteToolStripMenuItem.Enabled = false;
            selectAllToolStripMenuItem.Enabled = false;
        }
    }

    private void GoToFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
    showGoToForm:
        if (Solution is null)
            return;
        goToForm ??= new GoToFileForm(Solution.FileStructure);
        goToForm.ShowDialog(this);


        if (goToForm.SelectedFile is not null)
        {
            solutionExplorer.Focus();
            if (!solutionExplorer.SelectFile(goToForm.SelectedFile))
            {
                MessageBox.Show(this, $"Cannot select file: '{goToForm.SelectedFile}'", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                goto showGoToForm;
            }
            else
            {
                DatapackFileInfo? file = Solution.FileStructure.GetFile(goToForm.SelectedFile);
                if (file?.FullName == Path.Combine(Solution.Path, "pack.mcmeta"))
                {
                    OpenMetaFile();
                }
                else
                {
                    OpenDatapackFile(file!);
                }
            }
        }
    }

    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is TextEditorTabPage editor)
        {
            editor.Cut();
        }
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is TextEditorTabPage editor)
        {
            editor.Copy();
        }
    }

    private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is TextEditorTabPage editor)
        {
            editor.Paste();
        }
    }

    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is TextEditorTabPage editor)
        {
            editor.SelectAll();
        }
    }

    private void TcMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tcMain.SelectedTab is EditorTabPage tp)
        {
            solutionExplorer.SelectFile(tp.FileInfo.FullName);
        }
    }

    private void viewLogsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new LogViewerForm(logger).ShowDialog();
    }

    private void helpToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        if ((ModifierKeys & Keys.Shift) != 0)
        {
            viewLogsToolStripMenuItem.Visible = true;
        }
        else
        {
            viewLogsToolStripMenuItem.Visible = false;
        }
    }
}