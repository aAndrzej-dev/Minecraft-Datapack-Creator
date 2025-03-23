
using MinecraftDatapackCreator.FileStructure;
using System.Windows.Forms;

namespace MinecraftDatapackCreator.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            mainMenuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newProjectToolStripMenuItem = new ToolStripMenuItem();
            openProjectToolStripMenuItem = new ToolStripMenuItem();
            recentProjectsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            closeProjectToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAllToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            reloadToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            goToToolStripMenuItem = new ToolStripMenuItem();
            goToFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            selectAllToolStripMenuItem = new ToolStripMenuItem();
            projectToolStripMenuItem = new ToolStripMenuItem();
            addNamespaceToolStripMenuItem = new ToolStripMenuItem();
            exportToZipToolStripMenuItem = new ToolStripMenuItem();
            attachDatapackToWorldToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator10 = new ToolStripSeparator();
            addItemToolStripMenuItem = new ToolStripMenuItem();
            newFolderToolStripMenuItem = new ToolStripMenuItem();
            windowToolStripMenuItem = new ToolStripMenuItem();
            closeTabToolStripMenuItem = new ToolStripMenuItem();
            closeAllTabsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem1 = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            viewLogsToolStripMenuItem = new ToolStripMenuItem();
            mainStatusStrip = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusLabel5 = new ToolStripStatusLabel();
            toolStripStatusLabel4 = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            openFileDialog = new OpenFileDialog();
            splitContainer1 = new SplitContainer();
            sfdExportToZip = new SaveFileDialog();
            mainToolStrip = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripButton4 = new ToolStripButton();
            toolStripButton5 = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            toolStripButton6 = new ToolStripButton();
            tsbAddFolder = new ToolStripButton();
            tsbAddItem = new ToolStripButton();
            solutionExplorer = new SolutionExplorer(Controller);
            this.tcMain = new MyTabControl(Controller);
            mainMenuStrip.SuspendLayout();
            mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.SuspendLayout();
            mainToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.BackColor = Color.FromArgb(50, 50, 50);
            mainMenuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, projectToolStripMenuItem, windowToolStripMenuItem, helpToolStripMenuItem });
            mainMenuStrip.Location = new Point(0, 0);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Padding = new Padding(8, 2, 0, 2);
            mainMenuStrip.Size = new Size(884, 24);
            mainMenuStrip.TabIndex = 0;
            mainMenuStrip.Text = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newProjectToolStripMenuItem, openProjectToolStripMenuItem, recentProjectsToolStripMenuItem, toolStripSeparator1, closeProjectToolStripMenuItem, toolStripSeparator2, saveToolStripMenuItem, saveAllToolStripMenuItem, toolStripSeparator3, reloadToolStripMenuItem, toolStripSeparator6, exitToolStripMenuItem });
            fileToolStripMenuItem.ForeColor = Color.White;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = Properties.Resources.MenuItemFile;
            fileToolStripMenuItem.DropDownOpening += FileToolStripMenuItem_DropDownOpening;
            // 
            // newProjectToolStripMenuItem
            // 
            newProjectToolStripMenuItem.Image = Properties.Resources.NewProject;
            newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            newProjectToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newProjectToolStripMenuItem.Size = new Size(187, 22);
            newProjectToolStripMenuItem.Text = Properties.Resources.MenuItemNewProject;
            newProjectToolStripMenuItem.Click += NewProjectToolStripMenuItem_Click;
            // 
            // openProjectToolStripMenuItem
            // 
            openProjectToolStripMenuItem.Image = Properties.Resources.OpenProject;
            openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            openProjectToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openProjectToolStripMenuItem.Size = new Size(187, 22);
            openProjectToolStripMenuItem.Text = Properties.Resources.MenuItemOpenProject;
            openProjectToolStripMenuItem.Click += OpenProjectToolStripMenuItem_Click;
            // 
            // recentProjectsToolStripMenuItem
            // 
            recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
            recentProjectsToolStripMenuItem.Size = new Size(187, 22);
            recentProjectsToolStripMenuItem.Text = Properties.Resources.MenuItemRecentProjects;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(184, 6);
            // 
            // closeProjectToolStripMenuItem
            // 
            closeProjectToolStripMenuItem.Image = Properties.Resources.CloseProject;
            closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            closeProjectToolStripMenuItem.Size = new Size(187, 22);
            closeProjectToolStripMenuItem.Text = Properties.Resources.MenuItemCloseProject;
            closeProjectToolStripMenuItem.Click += CloseProjectToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(184, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.Save;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(187, 22);
            saveToolStripMenuItem.Text = Properties.Resources.MenuItemSave;
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // saveAllToolStripMenuItem
            // 
            saveAllToolStripMenuItem.Image = Properties.Resources.SaveAll;
            saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            saveAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAllToolStripMenuItem.Size = new Size(187, 22);
            saveAllToolStripMenuItem.Text = Properties.Resources.MenuItemSaveAll;
            saveAllToolStripMenuItem.Click += SaveAllToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(184, 6);
            // 
            // reloadToolStripMenuItem
            // 
            reloadToolStripMenuItem.AutoToolTip = true;
            reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.Size = new Size(187, 22);
            reloadToolStripMenuItem.Text = "Reload";
            reloadToolStripMenuItem.ToolTipText = "Reloads solution and datapack structure";
            reloadToolStripMenuItem.Visible = false;
            reloadToolStripMenuItem.Click += ReloadToolStripMenuItem_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(184, 6);
            toolStripSeparator6.Visible = false;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Image = Properties.Resources.Exit;
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(187, 22);
            exitToolStripMenuItem.Text = Properties.Resources.MenuItemExit;
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { goToToolStripMenuItem, toolStripSeparator7, undoToolStripMenuItem, redoToolStripMenuItem, toolStripSeparator8, cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, toolStripSeparator9, selectAllToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = Properties.Resources.MenuItemEdit;
            editToolStripMenuItem.DropDownOpening += EditToolStripMenuItem_DropDownOpening;
            // 
            // goToToolStripMenuItem
            // 
            goToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { goToFileToolStripMenuItem });
            goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            goToToolStripMenuItem.Size = new Size(164, 22);
            goToToolStripMenuItem.Text = Properties.Resources.MenuItemGoTo;
            // 
            // goToFileToolStripMenuItem
            // 
            goToFileToolStripMenuItem.Name = "goToFileToolStripMenuItem";
            goToFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.T;
            goToFileToolStripMenuItem.Size = new Size(165, 22);
            goToFileToolStripMenuItem.Text = Properties.Resources.MenuItemGoToFile;
            goToFileToolStripMenuItem.Click += GoToFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(161, 6);
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            undoToolStripMenuItem.Size = new Size(164, 22);
            undoToolStripMenuItem.Text = Properties.Resources.MenuItemUndo;
            undoToolStripMenuItem.Click += UndoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            redoToolStripMenuItem.Size = new Size(164, 22);
            redoToolStripMenuItem.Text = Properties.Resources.MenuItemRedo;
            redoToolStripMenuItem.Click += RedoToolStripMenuItem_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(161, 6);
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Enabled = false;
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            cutToolStripMenuItem.Size = new Size(164, 22);
            cutToolStripMenuItem.Text = Properties.Resources.MenuItemCut;
            cutToolStripMenuItem.Click += CutToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Image = Properties.Resources.Copy;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            copyToolStripMenuItem.Size = new Size(164, 22);
            copyToolStripMenuItem.Text = Properties.Resources.MenuItemCopy;
            copyToolStripMenuItem.Click += CopyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Enabled = false;
            pasteToolStripMenuItem.Image = Properties.Resources.Paste;
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteToolStripMenuItem.Size = new Size(164, 22);
            pasteToolStripMenuItem.Text = Properties.Resources.MenuItemPaste;
            pasteToolStripMenuItem.Click += PasteToolStripMenuItem_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(161, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Enabled = false;
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            selectAllToolStripMenuItem.Size = new Size(164, 22);
            selectAllToolStripMenuItem.Text = Properties.Resources.MenuItemSelectAll;
            selectAllToolStripMenuItem.Click += SelectAllToolStripMenuItem_Click;
            // 
            // projectToolStripMenuItem
            // 
            projectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addNamespaceToolStripMenuItem, exportToZipToolStripMenuItem, attachDatapackToWorldToolStripMenuItem, toolStripSeparator10, addItemToolStripMenuItem, newFolderToolStripMenuItem });
            projectToolStripMenuItem.ForeColor = Color.White;
            projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            projectToolStripMenuItem.Size = new Size(56, 20);
            projectToolStripMenuItem.Text = Properties.Resources.MenuItemProject;
            // 
            // addNamespaceToolStripMenuItem
            // 
            addNamespaceToolStripMenuItem.Image = Properties.Resources.NewNamespace;
            addNamespaceToolStripMenuItem.Name = "addNamespaceToolStripMenuItem";
            addNamespaceToolStripMenuItem.Size = new Size(197, 22);
            addNamespaceToolStripMenuItem.Text = Properties.Resources.MenuItemAddNamespace;
            addNamespaceToolStripMenuItem.Click += AddNamespaceToolStripMenuItem_Click;
            // 
            // exportToZipToolStripMenuItem
            // 
            exportToZipToolStripMenuItem.Image = Properties.Resources.CompressedFolder;
            exportToZipToolStripMenuItem.Name = "exportToZipToolStripMenuItem";
            exportToZipToolStripMenuItem.Size = new Size(197, 22);
            exportToZipToolStripMenuItem.Text = Properties.Resources.MenuItemExportToZip;
            exportToZipToolStripMenuItem.Click += ExportToZipToolStripMenuItem_Click;
            // 
            // attachDatapackToWorldToolStripMenuItem
            // 
            attachDatapackToWorldToolStripMenuItem.Name = "attachDatapackToWorldToolStripMenuItem";
            attachDatapackToWorldToolStripMenuItem.Size = new Size(197, 22);
            attachDatapackToWorldToolStripMenuItem.Text = Properties.Resources.MenuItemExportToWorld;
            attachDatapackToWorldToolStripMenuItem.Click += AttachDatapackToWorldToolStripMenuItem_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(194, 6);
            // 
            // addItemToolStripMenuItem
            // 
            addItemToolStripMenuItem.Image = Properties.Resources.NewFile;
            addItemToolStripMenuItem.Name = "addItemToolStripMenuItem";
            addItemToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
            addItemToolStripMenuItem.Size = new Size(197, 22);
            addItemToolStripMenuItem.Text = "Add Item";
            addItemToolStripMenuItem.Click += AddItemToolStripMenuItem_Click;
            // 
            // newFolderToolStripMenuItem
            // 
            newFolderToolStripMenuItem.Image = Properties.Resources.NewFolder;
            newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            newFolderToolStripMenuItem.Size = new Size(197, 22);
            newFolderToolStripMenuItem.Text = "New Folder";
            newFolderToolStripMenuItem.Click += NewFolderToolStripMenuItem_Click;
            // 
            // windowToolStripMenuItem
            // 
            windowToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { closeTabToolStripMenuItem, closeAllTabsToolStripMenuItem });
            windowToolStripMenuItem.ForeColor = Color.White;
            windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            windowToolStripMenuItem.Size = new Size(63, 20);
            windowToolStripMenuItem.Text = Properties.Resources.MenuItemWindow;
            windowToolStripMenuItem.DropDownOpening += WindowToolStripMenuItem_DropDownOpening;
            // 
            // closeTabToolStripMenuItem
            // 
            closeTabToolStripMenuItem.Image = Properties.Resources.CloseTab;
            closeTabToolStripMenuItem.Name = "closeTabToolStripMenuItem";
            closeTabToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.W;
            closeTabToolStripMenuItem.Size = new Size(223, 22);
            closeTabToolStripMenuItem.Text = Properties.Resources.MenuItemCloseTab;
            closeTabToolStripMenuItem.Click += CloseTabToolStripMenuItem_Click;
            // 
            // closeAllTabsToolStripMenuItem
            // 
            closeAllTabsToolStripMenuItem.Image = Properties.Resources.CloseAllTabs;
            closeAllTabsToolStripMenuItem.Name = "closeAllTabsToolStripMenuItem";
            closeAllTabsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.W;
            closeAllTabsToolStripMenuItem.Size = new Size(223, 22);
            closeAllTabsToolStripMenuItem.Text = Properties.Resources.MenuItemCloseAllTabs;
            closeAllTabsToolStripMenuItem.Click += CloseAllTabsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { helpToolStripMenuItem1, settingsToolStripMenuItem, aboutToolStripMenuItem, viewLogsToolStripMenuItem });
            helpToolStripMenuItem.ForeColor = Color.White;
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = Properties.Resources.MenuItemHelp;
            helpToolStripMenuItem.DropDownOpening += HelpToolStripMenuItem_DropDownOpening;
            // 
            // helpToolStripMenuItem1
            // 
            helpToolStripMenuItem1.Image = Properties.Resources.Help;
            helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            helpToolStripMenuItem1.ShortcutKeys = Keys.F1;
            helpToolStripMenuItem1.Size = new Size(127, 22);
            helpToolStripMenuItem1.Text = Properties.Resources.MenuItemHelp;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(127, 22);
            settingsToolStripMenuItem.Text = Properties.Resources.MenuItemSettings;
            settingsToolStripMenuItem.Click += SettingsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Image = Properties.Resources.Icon;
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(127, 22);
            aboutToolStripMenuItem.Text = Properties.Resources.MenuItemAbout;
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // viewLogsToolStripMenuItem
            // 
            viewLogsToolStripMenuItem.Name = "viewLogsToolStripMenuItem";
            viewLogsToolStripMenuItem.Size = new Size(127, 22);
            viewLogsToolStripMenuItem.Text = "View Logs";
            viewLogsToolStripMenuItem.Click += ViewLogsToolStripMenuItem_Click;
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.BackColor = Color.FromArgb(50, 50, 50);
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel5, toolStripStatusLabel4, toolStripStatusLabel2, toolStripStatusLabel3 });
            mainStatusStrip.Location = new Point(0, 539);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Padding = new Padding(1, 0, 19, 0);
            mainStatusStrip.Size = new Size(884, 22);
            mainStatusStrip.TabIndex = 1;
            mainStatusStrip.Text = "mainStatusStrip";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(39, 17);
            toolStripStatusLabel1.Text = "Ready";
            // 
            // toolStripStatusLabel5
            // 
            toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            toolStripStatusLabel5.Padding = new Padding(100, 0, 0, 0);
            toolStripStatusLabel5.Size = new Size(100, 17);
            // 
            // toolStripStatusLabel4
            // 
            toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            toolStripStatusLabel4.Size = new Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Padding = new Padding(100, 0, 0, 0);
            toolStripStatusLabel2.Size = new Size(100, 17);
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(0, 17);
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "mcmeta";
            openFileDialog.Filter = "Minecraft datapack file|pack.mcmeta";
            // 
            // splitContainer1
            // 
            splitContainer1.Panel1.Controls.Add(this.solutionExplorer);

            this.splitContainer1.Panel2.Controls.Add(this.tcMain);
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 49);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.RightToLeft = RightToLeft.No;
            splitContainer1.Panel1MinSize = 300;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.RightToLeft = RightToLeft.No;
            splitContainer1.Panel2MinSize = 500;
            splitContainer1.Size = new Size(884, 490);
            splitContainer1.SplitterDistance = 300;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 5;
            // 
            // sfdExportToZip
            // 
            sfdExportToZip.DefaultExt = "zip";
            sfdExportToZip.Filter = "Compressed files|*.zip|All files|*.*";
            // 
            // mainToolStrip
            // 
            mainToolStrip.BackColor = Color.FromArgb(50, 50, 50);
            mainToolStrip.CanOverflow = false;
            mainToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            mainToolStrip.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2, toolStripButton3, toolStripSeparator4, toolStripButton4, toolStripButton5, toolStripSeparator5, toolStripButton6, tsbAddFolder, tsbAddItem });
            mainToolStrip.Location = new Point(0, 24);
            mainToolStrip.Name = "mainToolStrip";
            mainToolStrip.Size = new Size(884, 25);
            mainToolStrip.TabIndex = 6;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = Properties.Resources.NewProject;
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(23, 22);
            toolStripButton1.Text = Properties.Resources.MenuItemNewProject;
            toolStripButton1.Click += NewProjectToolStripMenuItem_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = Properties.Resources.OpenProject;
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(23, 22);
            toolStripButton2.Text = Properties.Resources.MenuItemOpenProject;
            toolStripButton2.Click += OpenProjectToolStripMenuItem_Click;
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = Properties.Resources.CloseProject;
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(23, 22);
            toolStripButton3.Text = Properties.Resources.MenuItemCloseProject;
            toolStripButton3.Click += CloseProjectToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 25);
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = Properties.Resources.Save;
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new Size(23, 22);
            toolStripButton4.Text = Properties.Resources.MenuItemSave;
            toolStripButton4.Click += SaveToolStripMenuItem_Click;
            // 
            // toolStripButton5
            // 
            toolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton5.Image = Properties.Resources.SaveAll;
            toolStripButton5.ImageTransparentColor = Color.Magenta;
            toolStripButton5.Name = "toolStripButton5";
            toolStripButton5.Size = new Size(23, 22);
            toolStripButton5.Text = Properties.Resources.MenuItemSaveAll;
            toolStripButton5.Click += SaveAllToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 25);
            // 
            // toolStripButton6
            // 
            toolStripButton6.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton6.Image = Properties.Resources.NewNamespace;
            toolStripButton6.ImageTransparentColor = Color.Magenta;
            toolStripButton6.Name = "toolStripButton6";
            toolStripButton6.Size = new Size(23, 22);
            toolStripButton6.Text = Properties.Resources.MenuItemAddNamespace;
            toolStripButton6.Click += AddNamespaceToolStripMenuItem_Click;
            // 
            // tsbAddFolder
            // 
            tsbAddFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddFolder.Image = Properties.Resources.NewFolder;
            tsbAddFolder.ImageTransparentColor = Color.Magenta;
            tsbAddFolder.Name = "tsbAddFolder";
            tsbAddFolder.Size = new Size(23, 22);
            tsbAddFolder.Text = "Add Folder";
            tsbAddFolder.Click += NewFolderToolStripMenuItem_Click;
            // 
            // tsbAddItem
            // 
            tsbAddItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddItem.Image = Properties.Resources.NewFile;
            tsbAddItem.ImageTransparentColor = Color.Magenta;
            tsbAddItem.Name = "tsbAddItem";
            tsbAddItem.Size = new Size(23, 22);
            tsbAddItem.Text = "Add Item";
            tsbAddItem.Click += AddItemToolStripMenuItem_Click;
            // 
            // solutionExplorer
            // 
            this.solutionExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.solutionExplorer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.solutionExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionExplorer.ForeColor = System.Drawing.Color.White;
            this.solutionExplorer.FullRowSelect = true;
            this.solutionExplorer.HideSelection = false;
            this.solutionExplorer.ImageIndex = 0;
            this.solutionExplorer.Indent = 20;
            this.solutionExplorer.LabelEdit = true;
            this.solutionExplorer.LineColor = System.Drawing.Color.White;
            this.solutionExplorer.Location = new System.Drawing.Point(0, 0);
            this.solutionExplorer.Name = "solutionExplorer";
            this.solutionExplorer.SelectedImageIndex = 0;
            this.solutionExplorer.ShowLines = false;
            this.solutionExplorer.ShowNodeToolTips = true;
            this.solutionExplorer.Size = new System.Drawing.Size(300, 490);
            this.solutionExplorer.Solution = null;
            this.solutionExplorer.Sorted = true;
            this.solutionExplorer.TabIndex = 1;
            this.solutionExplorer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SolutionExplorer_AfterSelect);
            this.solutionExplorer.Leave += new System.EventHandler(this.SolutionExplorer_Leave);
            // 
            // tcMain
            // 
            this.tcMain.AllowDrop = true;
            this.tcMain.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tcMain.Divider = System.Drawing.Color.RoyalBlue;
            this.tcMain.DividerSize = 2;
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcMain.HotTrack = true;
            this.tcMain.InActiveTabBackColor = System.Drawing.Color.Transparent;
            this.tcMain.InActiveTabForeColor = System.Drawing.Color.White;
            this.tcMain.ItemSize = new System.Drawing.Size(73, 24);
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Margin = new System.Windows.Forms.Padding(4);
            this.tcMain.Name = "tcMain";
            this.tcMain.Padding = new System.Drawing.Point(20, 0);
            this.tcMain.SelectedIndex = 0;
            this.tcMain.ShowToolTips = true;
            this.tcMain.Size = new System.Drawing.Size(579, 490);
            this.tcMain.TabIndex = 0;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.TcMain_SelectedIndexChanged);
            this.tcMain.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.TcMain_ControlAdded);
            this.tcMain.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.TcMain_ControlRemoved);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(884, 561);
            Controls.Add(splitContainer1);
            Controls.Add(mainToolStrip);
            Controls.Add(mainMenuStrip);
            Controls.Add(mainStatusStrip);
            Font = new Font("Microsoft Sans Serif", 9.75F);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = mainMenuStrip;
            Margin = new Padding(4);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Minecraft Datapack Creator";
            KeyDown += MainForm_KeyDown;
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            mainToolStrip.ResumeLayout(false);
            mainToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip mainMenuStrip;
        private StatusStrip mainStatusStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newProjectToolStripMenuItem;
        private ToolStripMenuItem openProjectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem closeProjectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAllToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem projectToolStripMenuItem;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem addNamespaceToolStripMenuItem;
        private ToolStripMenuItem closeTabToolStripMenuItem;
        private ToolStripMenuItem closeAllTabsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private SplitContainer splitContainer1;
        private MyTabControl tcMain;
        private ToolStripMenuItem exportToZipToolStripMenuItem;
        private SaveFileDialog sfdExportToZip;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripMenuItem attachDatapackToWorldToolStripMenuItem;
        private ToolStripMenuItem recentProjectsToolStripMenuItem;
        private ToolStrip mainToolStrip;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton toolStripButton6;
        private ToolStripMenuItem reloadToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem goToToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem goToFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private SolutionExplorer solutionExplorer;
        private ToolStripMenuItem viewLogsToolStripMenuItem;
        private ToolStripMenuItem addItemToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem newFolderToolStripMenuItem;
        private ToolStripButton tsbAddFolder;
        private ToolStripButton tsbAddItem;
    }
}

