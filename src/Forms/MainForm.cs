using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Aadev.JTF;
using Newtonsoft.Json.Linq;

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
            Text = Solution is null ? Program.ProductTitle : $"{Solution.Name} - {Program.ProductTitle}";


            OnSolutionChanged();

            goToForm = null;

            if (Solution is null)
                return;


            OpenMetaFile();

            string recentProjectFile = Path.Combine(appDataFolder, "recentProjects.json");


            JArray root = File.Exists(recentProjectFile) ? JArray.Parse(File.ReadAllText(recentProjectFile)) : new JArray();
            string fullPath = Path.GetFullPath(Path.Combine(Solution.Path, "pack.mcmeta"));

            JToken? similar = root.FirstOrDefault(x => x["path"]!.ToString() == fullPath);


            if (similar is not null)
            {
                similar["lastUsed"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                JObject jobject = new JObject
                {
                    { "path", fullPath },
                    { "lastUsed", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                };

                root.Add(jobject);
                File.WriteAllText(recentProjectFile, root.ToString(Newtonsoft.Json.Formatting.None));
            }





            recentProjectsToolStripMenuItem.DropDownItems.Clear();
            int index = 1;
            foreach (JToken item in root.Cast<JToken>().OrderByDescending(x => (DateTime)x["lastUsed"]))
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
    private SelectTabPageForm? selectTabPageForm;
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
    internal MainForm(string[] args, ILogger logger)
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




        SplashForm? splashForm = null;

        Thread t = new(() =>
       {
           splashForm = new SplashForm();
           splashForm.ShowDialog();
       });
        t.Start();
        InitializeComponent();



        string recentProjectFile = Path.Combine(appDataFolder, "recentProjects.json");
        if (File.Exists(recentProjectFile))
        {
            JArray root = JArray.Parse(File.ReadAllText(recentProjectFile));
            int index = 1;
            foreach (JToken item in root.Cast<JToken>().OrderByDescending(x => (DateTime)x["lastUsed"]))
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
            mcmetaFileTemplate = JTemplate.Load(Path.Combine(settings.DatapackStructureDataFolder, "pack.mcmeta.jtf")!);
            Structure = DatapackStructureFoldersCollection.CreateDatapackStructure(Path.Combine(settings.DatapackStructureDataFolder, "structure.json"), logger, settings.DatapackStructureDataFolder);
        }
        catch (Exception ex)
        {
            logger.Error($"Cannot load datapack structure: {ex.Message}");
            //MessageBox.Show(this, ex.Message, Program.ProductTitle);
            //throw;
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
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Properties.Resources.DialogFileNotFound, path), Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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


        splashForm?.Invoke(new FormDelagate(() => { splashForm.Close(); splashForm.Dispose(); }));

        t.Join();
        logger.Debug($"Initialization completed");

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
            page = new NBTEditorTabPage(file, settings);
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
        using CreateProjectForm createProjectForm = new CreateProjectForm();

        if (!CloseProject())
        {
            return;
        }
        if (createProjectForm.ShowDialog(this) == DialogResult.OK)
        {
            string solutionPath = Path.Combine(createProjectForm.Path, createProjectForm.ProjectName);

            DirectoryInfo di = new DirectoryInfo(Path.Combine(solutionPath, Datapack.DATA_FOLDER_NAME, createProjectForm.NamespaceName));
            if (!di.Exists)
            {
                di.Create();
            }

            DirectoryInfo mctagsdi = new DirectoryInfo(Path.Combine(solutionPath, Datapack.DATA_FOLDER_NAME, "minecraft", "tags", "functions"));
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

            File.Create(Path.Combine(solutionPath, "pack.mcmeta")).Close();
            Datapack solution = new Datapack(solutionPath, Structure!);
            Solution = solution;



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
        bool hasChanged = false;
        List<string> unsavedFiles = new List<string>();
        for (int i = 0; i < tcMain.TabCount; i++)
        {
            if (tcMain.TabPages[i] is not EditorTabPage tp)
            {
                continue;
            }

            if (tp.IsNotSaved)
            {
                unsavedFiles.Add(Path.GetRelativePath(Solution!.Path, tp.FileInfo.FullName));
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

        using SaveFilesForm sff = new SaveFilesForm(CollectionsMarshal.AsSpan(unsavedFiles));
        DialogResult dr = sff.ShowDialog();

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
        if (!ClearTabsSafe())
            return false;


        Solution = null;
        return true;

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
                    string[] array = new string[1] { Path.GetRelativePath(Solution!.Path, tabPage.FileInfo.FullName) };
                    using SaveFilesForm sff = new SaveFilesForm(array);
                    DialogResult dr = sff.ShowDialog();

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
            selectTabPageForm = new SelectTabPageForm(tcMain.TabPages.Cast<EditorTabPage>().ToArray(), tcMain.SelectedIndex, this);
            selectTabPageForm.ShowDialog(this);
            tcMain.SelectedIndex = selectTabPageForm.SelectedIndex;
            selectTabPageForm = null;
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

    private void AboutToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        using AboutForm aboutForm = new AboutForm();
        aboutForm.ShowDialog(this);
    }

    private void SolutionExplorer_MetaFileOpened(object? sender, FileEventArgs e)
    {
        OpenMetaFile();
    }

    private void SettingsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        using SettingsForm settingsForm = new SettingsForm(settings, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minecraft Datapack Creator", "settings.json"));
        settingsForm.ShowDialog(this);
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

        using WorldSelelectForm wsf = new WorldSelelectForm(settings);
        if (wsf.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

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

        SplashForm? splashForm = null;

        Thread t = new(() =>
        {
            splashForm = new SplashForm(true);
            splashForm.ShowDialog();
        });
        t.Start();
        JTemplate.ClearCustomSourceCache();
        tcMain.Controls.Clear();
        try
        {
            mcmetaFileTemplate = JTemplate.Load(Path.Combine(settings.DatapackStructureDataFolder, "pack.mcmeta.jtf"));
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

        logger.Debug($"Reloaded successfully");


        splashForm?.Invoke(new FormDelagate(() => { splashForm.Close(); splashForm.Dispose(); }));

        t.Join();
    }

    private static readonly string[] allowedStructureDynamicSources = new string[]
    {
        "advancements",
        "functions",
        "loot_tables",
        "item_modifiers",
        "predicates",
        "recipes",
        "structures",
        "dimension_type",
        "damage_type",
        "dimension",
        "trim_pattern",
        "trim_material",
        "chat_type",
        "worldgen/biome",
        "worldgen/configured_carver",
        "worldgen/configured_feature",
        "worldgen/configured_structure_feature",
        "worldgen/configured_surface_builder",
        "worldgen/noise_settings",
        "worldgen/processor_list",
        "worldgen/template_pool",
        "worldgen/structure_set",
        "worldgen/density_function",
        "worldgen/flat_level_generator_preset",
        "worldgen/noise",
        "worldgen/placed_feature",
        "worldgen/structure",
        "worldgen/world_preset",
        "tags/blocks",
        "tags/entity_types",
        "tags/fluids",
        "tags/functions",
        "tags/items",
        "tags/game_events",
        "tags/damage_type",
        "tags/worldgen/biome",
        "tags/worldgen/structure",
        "tags/worldgen/world_preset",
        "tags/worldgen/flat_level_generator_preset"
    };
    private IEnumerable<IJtSuggestion> GetNamespacedSourceAsSuggestions(JtIdentifier id)
    {

        if (id.Value?.StartsWith("structure:", StringComparison.OrdinalIgnoreCase) is false or null || Solution is null)
        {
            return Enumerable.Empty<IJtSuggestion>();
        }

        string structurePath = id.Value.AsSpan(10).ToString();
        if (!allowedStructureDynamicSources.Contains(structurePath))
        {
            return Enumerable.Empty<IJtSuggestion>();
        }
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

    private void ViewLogsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using LogViewerForm logViewer = new LogViewerForm(logger);
        logViewer.ShowDialog();
    }

    private void HelpToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
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