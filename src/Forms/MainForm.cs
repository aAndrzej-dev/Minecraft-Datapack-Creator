using Aadev.JTF;
using CommunityToolkit.Diagnostics;
using MinecraftDatapackCreator.FileStructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace MinecraftDatapackCreator.Forms;

public partial class MainForm : Form
{
    private static readonly string recentProjectFile = Path.Join(Controller.appDataFolder, "recentProjects.json");
    private GoToFileForm? goToForm;


    private delegate void ControlInvokeDelegate();

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal Datapack? Solution
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
            //if(Solution.Name == "Minecraft 1.21.4 Data")
            //{
            //    OverrideGenerator og = new OverrideGenerator(Solution.FileStructure);
            //    Aadev.DebugTools.TextView.Show(og.Generate().ToString(Formatting.None));
            //}

            OpenDatapackFile(Solution.GetMetaFile());



            JArray root;
            try
            {
                if (File.Exists(recentProjectFile))
                {
                    using StreamReader sr = new StreamReader(recentProjectFile);
                    using JsonReader jr = new JsonTextReader(sr);

                    if (sr.BaseStream.Length != 0)
                    {
                        root = JArray.Load(jr, Settings.jsonLoadSettings);
                    }
                    else
                    {
                        root = new JArray();
                    }

                    jr.Close();
                }
                else
                {
                    root = new JArray();
                }
            }
            catch (Exception ex)
            {
                Controller.Logger.Exception(ex);
                root = new JArray();
            }

            try
            {
                string? fullPath = Solution.FileStructure.RootFolder.GetRelativeFile(Datapack.PACK_MCMETA_FILE)?.FullName;

                JToken? similar = null;
                if (fullPath is not null)
                {
                    foreach (JToken item in root)
                    {
                        if ((item["path"]?.ToString()) != fullPath)
                            continue;

                        similar = item;
                        break;
                    }
                }


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
                }

                using StreamWriter sw = new StreamWriter(recentProjectFile);
                using JsonWriter jw = new JsonTextWriter(sw);

                root.WriteTo(jw);

                jw.Close();
            }
            catch (Exception)
            {
                throw;
            }

            DisplayRecentProject(root);
        }
    }

    internal Controller Controller { get; }

    private void DisplayRecentProject(JArray root)
    {
        recentProjectsToolStripMenuItem.DropDownItems.Clear();
        int index = 1;
        foreach (JToken item in root.OrderByDescending(x => (DateTime?)x["lastUsed"]))
        {
            string indexString = index == 10 ? "1&0" : (index > 10 ? index.ToString(CultureInfo.InvariantCulture) : $"&{index}");

            if (index > Controller.Settings.RecentProjectsCount)
                return;

            string? path = item["path"]?.ToString();

            if (path is null)
                continue;

            ToolStripMenuItem tsmi = new ToolStripMenuItem()
            {
                Text = $"{indexString} {path}",
                ToolTipText = path,
                AutoToolTip = true,
                ForeColor = Color.White
            };
            tsmi.Click += (s, e) => OpenProject(path);
            recentProjectsToolStripMenuItem.DropDownItems.Add(tsmi);
            index++;
        }
    }

    internal MainForm(string[] args, ILogger logger)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        logger.Debug($"Initializing {Application.ProductName} (v{Application.ProductVersion})");

        SplashForm? splashForm = null;

        Thread t = new Thread(() =>
        {
            splashForm = new SplashForm();
            splashForm.ShowDialog();
        });

        t.Start();


        Controller = new Controller(logger);
        Controller.ReloadRequested += Controller_ReloadRequested;
        logger.Debug("Initializing MainForm");
        InitializeComponent();
        logger.Debug("Initialization completed");






        Text = Program.ProductTitle;
        
        solutionExplorer.FileOpened += SolutionExplorer_FileOpened;
        solutionExplorer.FileSelected += SolutionExplorer_FileSelected;

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
                Controller.Logger.Error($"Cannot load datapack: File not exist: {path}");


                splashForm?.Invoke(new ControlInvokeDelegate(splashForm.Hide));
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, CompositeFormats.DialogFileNotFound, path), Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);


                splashForm?.Show();
            }
        }


        if (Solution is null && File.Exists(recentProjectFile))
        {
            using StreamReader sr = new StreamReader(recentProjectFile);
            using JsonReader jr = new JsonTextReader(sr);

            if (sr.BaseStream.Length != 0)
            {
                DisplayRecentProject(JArray.Load(jr, Settings.jsonLoadSettings));
            }

            jr.Close();

        }



        mainMenuStrip.Renderer = DarkToolStripRenderer.Instance;
        mainToolStrip.Renderer = DarkToolStripRenderer.Instance;
        mainStatusStrip.Renderer = DarkToolStripRenderer.Instance;
        mainMenuStrip.ForeColor = Color.White;


        foreach (ToolStripMenuItem item in mainMenuStrip.Items)
        {

            if (item is ToolStripMenuItem i)
            {
                SetStyleMenuItem(i);
            }

        }


        splashForm?.Invoke(new ControlInvokeDelegate(() => { splashForm.Close(); splashForm.Dispose(); }));

        t.Join();

        sw.Stop();

        Controller.Logger.Debug($"Initialization completed in {sw.Elapsed}");
    }

    private void Controller_ReloadRequested(object? sender, ReloadRequestEventArgs e)
    {
        this.Invoke(() =>
        {
            if (Solution is null)
                return;
            if (e.Reason is ReloadRequestEventArgs.ReloadRequestReason.PackFormatChanged)
            {
                if (MessageBox.Show(this, "The target Minecraft version (pack_format) has been changed. Solution has to be reloaded for applying the change. Do you want to reload solution or change target version to the previous one?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    var solutionPath = Solution.FileStructure.RootFolder.GetRelativeFile(Datapack.PACK_MCMETA_FILE)!.FullName;
                    CloseProject();
                    OpenProject(solutionPath);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        });
    }

    private void OnSolutionChanged()
    {
        if (Solution is not null)
        {
            Controller.Logger.Debug($"Loading datapack: {Solution?.Path}");
        }

        bool isSolutionNotNull = Solution is not null;


        closeProjectToolStripMenuItem.Enabled = isSolutionNotNull;
        addNamespaceToolStripMenuItem.Enabled = isSolutionNotNull;
        exportToZipToolStripMenuItem.Enabled = isSolutionNotNull;
        goToFileToolStripMenuItem.Enabled = isSolutionNotNull;
        attachDatapackToWorldToolStripMenuItem.Enabled = isSolutionNotNull;

        if (!isSolutionNotNull)
        {
            addItemToolStripMenuItem.Visible = false;
            tsbAddItem.Visible = false;
            newFolderToolStripMenuItem.Visible = false;
            tsbAddFolder.Visible = false;
            toolStripSeparator10.Visible = false;
        }
    }



    private static void SetStyleMenuItem(ToolStripItem item)
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


    protected override void OnShown(EventArgs e)
    {
        Controller.Logger.Debug($"Showing Main Window");
        base.OnShown(e);
        Activate();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        Guard.IsNotNull(e);
        if (!CloseProject())
        {
            e.Cancel = true;
            return;
        }
        Controller.Logger.Debug($"Closing Datapack Creator");
        Application.Exit();
        base.OnClosing(e);
    }

    private void SolutionExplorer_FileOpened(object? sender, DatapackFileInfo fileInfo) => OpenDatapackFile(fileInfo);
    private void OpenDatapackFile(DatapackFileInfo? file)
    {
        if (Solution is null || file is null)
            return;
        foreach (EditorTabPage item in tcMain.TabPages)
        {
            if (item.FileInfo == file)
            {
                tcMain.SelectedTab = item;
                return;
            }
        }

        if (file.Name.AsSpan().SequenceEqual(Datapack.PACK_MCMETA_FILE) && file != Solution.GetMetaFile())
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                Arguments = "\"" + file.FullName + "\"",
                FileName = Environment.ProcessPath
            };
            Process.Start(psi);
            return;
        }

        Controller.Logger.Debug($"Loading file: {file.FullName}");

        EditorTabPage page = file.Editor.CreateEditor(Controller, file);
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
            string solutionPath = Path.Join(createProjectForm.Path, createProjectForm.ProjectName);

            Directory.CreateDirectory(Path.Join(solutionPath, Datapack.DATA_FOLDER_NAME, createProjectForm.NamespaceName));

            DirectoryInfo functionsTagDirectoryInfo = Directory.CreateDirectory(Path.Join(solutionPath, Datapack.DATA_FOLDER_NAME, "minecraft\\tags\\functions"));

            File.Create(Path.Join(functionsTagDirectoryInfo.FullName, "load.json")).Close();
            File.Create(Path.Join(functionsTagDirectoryInfo.FullName, "tick.json")).Close();
            File.Create(Path.Join(solutionPath, Datapack.PACK_MCMETA_FILE)).Close();

            Solution = new Datapack(Controller, solutionPath);
        }
    }

    private void OpenProjectToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
            OpenProject(openFileDialog.FileName);
        }
    }

    private void OpenProject(ReadOnlySpan<char> path)
    {
        if (!Path.GetFileName(path).SequenceEqual(Datapack.PACK_MCMETA_FILE))
        {
            MessageBox.Show(this, $"Cannot open datapack: \n{path}", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!CloseProject())
        {
            return;
        }
        Solution = new Datapack(Controller, Path.GetDirectoryName(path));
    }

    private bool ClearTabsSafe()
    {
        bool hasChanged = false;
        List<DatapackFileInfo> unsavedFiles = new List<DatapackFileInfo>();
        for (int i = 0; i < tcMain.TabCount; i++)
        {
            if (tcMain.TabPages[i] is not EditorTabPage tp)
            {
                continue;
            }

            if (tp.IsNotSaved)
            {
                unsavedFiles.Add(tp.FileInfo);
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

        using SaveFilesForm sff = new SaveFilesForm(Controller, CollectionsMarshal.AsSpan(unsavedFiles));
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
        if (Solution is null)
            return true;
        Controller.Logger.Debug($"Closing datapack");
        if (!ClearTabsSafe())
            return false;


        Solution = null;
        return true;

    }
    private void CloseProjectToolStripMenuItem_Click(object? sender, EventArgs e) => CloseProject();

    private void SaveToolStripMenuItem_Click(object? sender, EventArgs e) =>(tcMain.SelectedTab as EditorTabPage)?.Save(); 
    private void SaveAllToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        foreach (EditorTabPage item in tcMain.TabPages)
        {
            item.Save();
        }
    }

    private void ExitToolStripMenuItem_Click(object? sender, EventArgs e) => Close();
    private void AddNamespaceToolStripMenuItem_Click(object? sender, EventArgs e) => solutionExplorer.AddNewNamespacePlaceholder();


    private void CloseTabToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        if (tcMain.SelectedTab is not null)
        {
            if (tcMain.SelectedTab is EditorTabPage tabPage)
            {
                if (tabPage.IsNotSaved)
                {
                    DatapackFileInfo[] array = new DatapackFileInfo[1] { tabPage.FileInfo };
                    using SaveFilesForm sff = new SaveFilesForm(Controller, array);
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

            ToolStripMenuItem tsmi = new ToolStripMenuItem($"&1 {tcMain.SelectedTab?.Text}");
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


                ToolStripMenuItem tsmi2 = new ToolStripMenuItem(prefix + tcMain.TabPages[i].Text);
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
            using SelectTabPageForm selectTabPageForm = new SelectTabPageForm(tcMain.TabPages.Cast<EditorTabPage>().ToArray(), tcMain.SelectedIndex, this);
            selectTabPageForm.ShowDialog(this);
            tcMain.SelectedIndex = selectTabPageForm.SelectedIndex;
        }
    }


    private void SolutionExplorer_FileSelected(object? sender, DatapackFileInfo fileInfo)
    {
        toolStripStatusLabel3.Text = Controller.Settings.AlwaysShowFullFilePathInDialogs ? fileInfo.FullName : fileInfo.PathRelativeToSolution.ToString();
        toolStripStatusLabel4.Text = fileInfo.NamespacedId;
    }


    private void SolutionExplorer_Leave(object? sender, EventArgs e)
    {
        toolStripStatusLabel3.Text = string.Empty;
        toolStripStatusLabel4.Text = string.Empty;
    }

    private void SolutionExplorer_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not ISolutionItemInfo tag)
            return;

        if (tag.SolutionNodeType is not SolutionNodeType.File)
        {
            toolStripStatusLabel3.Text = string.Empty;
            toolStripStatusLabel4.Text = string.Empty;
        }


        DatapackStructureFolder? folder = tag.DatapackStructureFolder;
        if (folder != null)
        {
            tsbAddItem.Text = $"Add {folder.DisplayName.TrimEnd('s')}";
            addItemToolStripMenuItem.Text = $"Add {folder.DisplayName.TrimEnd('s')}";
            tsbAddItem.Visible = true;
            addItemToolStripMenuItem.Visible = true;
            newFolderToolStripMenuItem.Visible = true;
            tsbAddFolder.Visible = true;
            toolStripSeparator10.Visible = true;
        }
        else
        {
            addItemToolStripMenuItem.Visible = false;
            tsbAddItem.Visible = false;
            newFolderToolStripMenuItem.Visible = false;
            tsbAddFolder.Visible = false;
            toolStripSeparator10.Visible = false;
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
            Controller.Logger.Error($"Cannot close all tabs: {ex.Message}");
            throw;
        }
    }

    private void AboutToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        using AboutForm aboutForm = new AboutForm();
        aboutForm.ShowDialog(this);
    }

    private void SettingsToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        using SettingsForm settingsForm = new SettingsForm(Controller, Controller.settingsFilename);
        if (settingsForm.ShowDialog(this) == DialogResult.OK)
        {
            if (settingsForm.RequireReload)
                Reload();
        }

    }




    private void TcMain_ControlAdded(object sender, ControlEventArgs e)
    {
        bool enable = tcMain.TabPages.Count > 0;

        closeTabToolStripMenuItem.Enabled = enable;
        closeAllTabsToolStripMenuItem.Enabled = enable;
        saveAllToolStripMenuItem.Enabled = enable;
        saveToolStripMenuItem.Enabled = enable;
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
            Controller.Logger.Debug($"Exporting datapack to zip. File name: {sfdExportToZip.FileName}");
            ZipFile.CreateFromDirectory(Solution.Path, sfdExportToZip.FileName, CompressionLevel.Optimal, false);
        }
    }

    private void AttachDatapackToWorldToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }

        using WorldSelectForm wsf = new WorldSelectForm(Controller);
        if (wsf.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }


        if (!Directory.Exists(wsf.SelectedWorld))
        {
            MessageBox.Show(this, $"Invalid World! \n{wsf.SelectedWorld}", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        string datapacksDirectoryPath = Path.Join(wsf.SelectedWorld, "datapacks");
        DirectoryInfo di = new DirectoryInfo(datapacksDirectoryPath);
        if (!di.Exists)
        {
            di.Create();
        }

        string filename = Path.Combine(datapacksDirectoryPath, Solution.Name + ".zip");

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
        MessageBox.Show("Export completed successfully!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Reload();
    }

    internal void Reload()
    {
        string? solutionPath = null;

        Controller.Logger.Debug($"Reloading structure");
        if (Solution is not null)
        {
            solutionPath = Solution.Path;
            if (!CloseProject())
            {
                return;
            }
        }

        SplashForm? splashForm = null;

        Thread t = new Thread(() =>
        {
            splashForm = new SplashForm(true);
            splashForm.ShowDialog();
        });


        t.Start();
        JTemplate.ClearCustomSourceCache();
        tcMain.Controls.Clear();
        Controller.VersionManager.UnloadAllDatapackStructures();
        Controller.VersionManager.UnloadAllMinecraftStructures();

        if (solutionPath is not null)
        {
            Solution = new Datapack(Controller, solutionPath);
        }

        Controller.Logger.Debug($"Reloaded successfully");


        splashForm?.Invoke(new ControlInvokeDelegate(() => { splashForm.Close(); splashForm.Dispose(); }));

        t.Join();
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
            undoToolStripMenuItem.Enabled = tabPage.CanUndo;
            redoToolStripMenuItem.Enabled = tabPage.CanRedo;
        }
        else
        {
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
        }
        if (tcMain.SelectedTab is TextEditorTabPage)
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


        if (goToForm.SelectedFile is null)
        {
            return;
        }
        solutionExplorer.Focus();
        if (!solutionExplorer.SelectItem(goToForm.SelectedFile))
        {
            MessageBox.Show(this, $"Cannot select file: '{goToForm.SelectedFile}'", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            goto showGoToForm;
        }
        else
        {
            OpenDatapackFile(goToForm.SelectedFile);
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
            solutionExplorer.SelectItem(tp.FileInfo);
        }
    }

    private void ViewLogsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using LogViewerForm logViewer = new LogViewerForm(Controller.Logger);
        logViewer.ShowDialog();
    }

    private void HelpToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        viewLogsToolStripMenuItem.Visible = (ModifierKeys & Keys.Shift) != 0;
    }

    private void AddItemToolStripMenuItem_Click(object sender, EventArgs e)
    {
        TreeNode? selectedNode = solutionExplorer.SelectedNode;

        if (selectedNode?.Tag is not ISolutionItemInfo { SolutionNodeType: SolutionNodeType.Directory or SolutionNodeType.StructureFolder or SolutionNodeType.File } tag)
            return;


        TreeNode parentNode = tag.SolutionNodeType is SolutionNodeType.File ? selectedNode.Parent : selectedNode;

        solutionExplorer.AddNewFilePlaceholder(parentNode);
    }

    private void NewFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        TreeNode? selectedNode = solutionExplorer.SelectedNode;

        if (selectedNode?.Tag is not ISolutionItemInfo { SolutionNodeType: SolutionNodeType.Directory or SolutionNodeType.StructureFolder or SolutionNodeType.File } tag)
            return;

        TreeNode parentNode = tag.SolutionNodeType is SolutionNodeType.File ? selectedNode.Parent : selectedNode;

        solutionExplorer.AddNewFolderPlaceholder(parentNode);
    }
}