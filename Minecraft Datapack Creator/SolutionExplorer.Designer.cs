namespace MinecraftDatapackCreator;

internal partial class SolutionExplorer
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsSolution = new ContextMenuStrip(components);
        tsmiSlnAddNamespace = new ToolStripMenuItem();
        toolStripSeparator6 = new ToolStripSeparator();
        tsmiSlnOpenInExplorer = new ToolStripMenuItem();
        cmsNamespace = new ContextMenuStrip(components);
        tsmiNsDelateNamespace = new ToolStripMenuItem();
        tsmiNsRenameNamespace = new ToolStripMenuItem();
        toolStripSeparator5 = new ToolStripSeparator();
        tsmiNsOpenInExplorer = new ToolStripMenuItem();
        cmsStructure = new ContextMenuStrip(components);
        tsmiStrAddFile = new ToolStripMenuItem();
        tsmiStrAddFolder = new ToolStripMenuItem();
        tsmiStrAddOverride = new ToolStripMenuItem();
        tsmiStrPaste = new ToolStripMenuItem();
        toolStripSeparator4 = new ToolStripSeparator();
        tsmiStrOpenInExplorer = new ToolStripMenuItem();
        cmsFile = new ContextMenuStrip(components);
        tsmiFileCut = new ToolStripMenuItem();
        tsmiFileCopy = new ToolStripMenuItem();
        tsmiFileDelate = new ToolStripMenuItem();
        tsmiFileRename = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        tsmiFileCopyNamespacedId = new ToolStripMenuItem();
        tsmiFileShowInExplorer = new ToolStripMenuItem();
        tsmiFileCopyPath = new ToolStripMenuItem();
        tsmiFileCopyRelativePath = new ToolStripMenuItem();
        tsmiFileRepair = new ToolStripMenuItem();
        cmsDirectory = new ContextMenuStrip(components);
        tsmiDirAddFile = new ToolStripMenuItem();
        tsmiDirAddFolder = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        tsmiDirCut = new ToolStripMenuItem();
        tsmiDirCopy = new ToolStripMenuItem();
        tsmiDirDelate = new ToolStripMenuItem();
        tsmiDirRename = new ToolStripMenuItem();
        toolStripSeparator3 = new ToolStripSeparator();
        toolStripSeparator7 = new ToolStripSeparator();
        toolStripSeparator8 = new ToolStripSeparator();
        toolStripSeparator9 = new ToolStripSeparator();
        tsmiDirOpenInExplorer = new ToolStripMenuItem();
        tsmiDirPaste = new ToolStripMenuItem();
        cmsSolution.SuspendLayout();
        cmsNamespace.SuspendLayout();
        cmsStructure.SuspendLayout();
        cmsFile.SuspendLayout();
        cmsDirectory.SuspendLayout();
        SuspendLayout();
        // 
        // cmsSolution
        // 
        cmsSolution.ForeColor = Color.White;
        cmsSolution.Items.AddRange(new ToolStripItem[] { tsmiSlnAddNamespace, toolStripSeparator6, tsmiSlnOpenInExplorer });
        cmsSolution.Name = "cmsSolution";
        cmsSolution.Size = new Size(163, 54);        // 
        // tsmiSlnAddNamespace
        // 
        tsmiSlnAddNamespace.Image = Properties.Resources.NewNamespace;
        tsmiSlnAddNamespace.Name = "tsmiSlnAddNamespace";
        tsmiSlnAddNamespace.Size = new Size(162, 22);
        tsmiSlnAddNamespace.Text = Properties.Resources.MenuItemAddNamespace;
        tsmiSlnAddNamespace.Click += TsmiSlnAddNamespace_Click;
        // 
        // toolStripSeparator6
        // 
        toolStripSeparator6.Name = "toolStripSeparator6";
        toolStripSeparator6.Size = new Size(159, 6);
        // 
        // toolStripSeparator7
        // 
        toolStripSeparator7.Name = "toolStripSeparator7";
        toolStripSeparator7.Size = new Size(159, 6);
        // 
        // toolStripSeparator8
        // 
        toolStripSeparator8.Name = "toolStripSeparator8";
        toolStripSeparator8.Size = new Size(159, 6);
        // 
        // tsmiSlnOpenInExplorer
        // 
        tsmiSlnOpenInExplorer.Name = "tsmiSlnOpenInExplorer";
        tsmiSlnOpenInExplorer.Size = new Size(162, 22);
        tsmiSlnOpenInExplorer.Text = Properties.Resources.MenuItemOpenInExplorer;
        tsmiSlnOpenInExplorer.Click += TsmiSlnOpenInExplorer_Click;
        // 
        // cmsNamespace
        // 
        cmsNamespace.ForeColor = Color.White;
        cmsNamespace.Items.AddRange(new ToolStripItem[] { tsmiNsDelateNamespace, tsmiNsRenameNamespace, toolStripSeparator5, tsmiNsOpenInExplorer });
        cmsNamespace.Name = "cmsNamespace";
        cmsNamespace.Size = new Size(163, 76);
        // 
        // tsmiNsDelateNamespace
        // 
        tsmiNsDelateNamespace.Image = Properties.Resources.DelateNamespace;
        tsmiNsDelateNamespace.Name = "tsmiNsDelateNamespace";
        tsmiNsDelateNamespace.ShortcutKeys = Keys.Delete;
        tsmiNsDelateNamespace.Size = new Size(162, 22);
        tsmiNsDelateNamespace.Text = Properties.Resources.MenuItemDelete;
        tsmiNsDelateNamespace.Click += TsmiNsDelateNamespace_Click;
        // 
        // tsmiNsRenameNamespace
        // 
        tsmiNsRenameNamespace.Image = Properties.Resources.Rename;
        tsmiNsRenameNamespace.Name = "tsmiNsRenameNamespace";
        tsmiNsRenameNamespace.Size = new Size(162, 22);
        tsmiNsRenameNamespace.Text = Properties.Resources.MenuItemRename;
        tsmiNsRenameNamespace.Click += TsmiNsRenameNamespace_Click;
        tsmiNsRenameNamespace.ShortcutKeys = Keys.F2;
        // 
        // toolStripSeparator5
        // 
        toolStripSeparator5.Name = "toolStripSeparator5";
        toolStripSeparator5.Size = new Size(159, 6);
        // 
        // tsmiNsOpenInExplorer
        // 
        tsmiNsOpenInExplorer.Name = "tsmiNsOpenInExplorer";
        tsmiNsOpenInExplorer.Size = new Size(162, 22);
        tsmiNsOpenInExplorer.Text = Properties.Resources.MenuItemOpenInExplorer;
        tsmiNsOpenInExplorer.Click += TsmiSlnOpenInExplorer_Click;
        // 
        // cmsStructure
        // 
        cmsStructure.ForeColor = Color.White;
        cmsStructure.Items.AddRange(new ToolStripItem[] { tsmiStrAddFile, tsmiStrAddOverride, tsmiStrAddFolder, toolStripSeparator4, tsmiStrPaste, toolStripSeparator8,  tsmiStrOpenInExplorer });
        cmsStructure.Name = "cmsStructure";
        cmsStructure.Size = new Size(163, 76);
        cmsStructure.Opening += CmsStructure_Opening;
        // 
        // tsmiStrAddFile
        // 
        tsmiStrAddFile.Image = Properties.Resources.NewFile;
        tsmiStrAddFile.Name = "tsmiStrAddFile";
        tsmiStrAddFile.Size = new Size(162, 22);
        tsmiStrAddFile.Text = Properties.Resources.MenuItemNewFile;
        tsmiStrAddFile.Click += TsmiStrAddFile_Click;
        tsmiStrAddFile.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
        // 
        // tsmiStrAddFolder
        // 
        tsmiStrAddFolder.Image = Properties.Resources.NewFolder;
        tsmiStrAddFolder.Name = "tsmiStrAddFolder";
        tsmiStrAddFolder.Size = new Size(162, 22);
        tsmiStrAddFolder.Text = Properties.Resources.MenuItemNewFolder;
        tsmiStrAddFolder.Click += TsmiStrAddFolder_Click;
        // 
        // tsmiStrAddOverride
        // 
        tsmiStrAddOverride.Name = "tsmiStrAddOverride";
        tsmiStrAddOverride.Size = new Size(162, 22);
        tsmiStrAddOverride.Text = "Override Minecraft File";
        tsmiStrAddOverride.Click += TsmiStrAddOverride_Click;
        // 
        // tsmiStrAddOverride
        // 
        tsmiStrPaste.Name = "tsmiStrPaste";
        tsmiStrPaste.Size = new Size(162, 22);
        tsmiStrPaste.Text = "Paste";
        tsmiStrPaste.Click += TsmiStrPaste_Click;
        // 
        // toolStripSeparator4
        // 
        toolStripSeparator4.Name = "toolStripSeparator4";
        toolStripSeparator4.Size = new Size(159, 6);
        // 
        // tsmiStrOpenInExplorer
        // 
        tsmiStrOpenInExplorer.Name = "tsmiStrOpenInExplorer";
        tsmiStrOpenInExplorer.Size = new Size(162, 22);
        tsmiStrOpenInExplorer.Text = Properties.Resources.MenuItemOpenInExplorer;
        tsmiStrOpenInExplorer.Click += TsmiSlnOpenInExplorer_Click;
        // 
        // cmsFile
        // 
        cmsFile.ForeColor = Color.White;
        cmsFile.Items.AddRange(new ToolStripItem[] { tsmiFileRepair, toolStripSeparator9, tsmiFileCut, tsmiFileCopy, tsmiFileDelate, tsmiFileRename, toolStripSeparator1, tsmiFileShowInExplorer, toolStripSeparator7, tsmiFileCopyNamespacedId, tsmiFileCopyPath, tsmiFileCopyRelativePath });
        cmsFile.Name = "cmsNamespace";
        cmsFile.Size = new Size(188, 208);
        cmsFile.Opening += CmsFile_Opening;
        // 
        // tsmiFileCut
        // 
        tsmiFileCut.Name = "tsmiFileCut";
        tsmiFileCut.ShortcutKeys = Keys.Control | Keys.X;
        tsmiFileCut.Size = new Size(187, 22);
        tsmiFileCut.Text = "Cut";
        tsmiFileCut.Click += TsmiFileCut_Click;
        // 
        // tsmiFileCopy
        // 
        tsmiFileCopy.Name = "tsmiFileCopy";
        tsmiFileCopy.ShortcutKeys = Keys.Control | Keys.C;
        tsmiFileCopy.Size = new Size(187, 22);
        tsmiFileCopy.Text = "Copy";
        tsmiFileCopy.Click += TsmiFileCopy_Click;
        // 
        // tsmiFileDelate
        // 
        tsmiFileDelate.Image = Properties.Resources.DelateFile;
        tsmiFileDelate.Name = "tsmiFileDelate";
        tsmiFileDelate.ShortcutKeys = Keys.Delete;
        tsmiFileDelate.Size = new Size(187, 22);
        tsmiFileDelate.Text = Properties.Resources.MenuItemDelete;
        tsmiFileDelate.Click += TsmiFileDelate_Click;
        // 
        // tsmiFileRename
        // 
        tsmiFileRename.Image = Properties.Resources.Rename;
        tsmiFileRename.Name = "tsmiFileRename";
        tsmiFileRename.Size = new Size(187, 22);
        tsmiFileRename.Text = Properties.Resources.MenuItemRename;
        tsmiFileRename.Click += TsmiFileRename_Click;
        tsmiFileRename.ShortcutKeys = Keys.F2;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(184, 6);
        // 
        // toolStripSeparator9
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(184, 6);
        // 
        // tsmiFileCopyNamespacedId
        // 
        tsmiFileCopyNamespacedId.Name = "tsmiFileCopyNamespacedId";
        tsmiFileCopyNamespacedId.Size = new Size(187, 22);
        tsmiFileCopyNamespacedId.Text = Properties.Resources.MenuItemCopyNamespacedId;
        tsmiFileCopyNamespacedId.Click += TsmiFileCopyNamespacedId_Click;
        // 
        // tsmiFileShowInExplorer
        // 
        tsmiFileShowInExplorer.Name = "tsmiFileShowInExplorer";
        tsmiFileShowInExplorer.Size = new Size(187, 22);
        tsmiFileShowInExplorer.Text = Properties.Resources.MenuItemShowInExplorer;
        tsmiFileShowInExplorer.Click += TsmiFileShowInExplorer_Click;
        // 
        // tsmiFileCopyPath
        // 
        tsmiFileCopyPath.Name = "tsmiFileCopyPath";
        tsmiFileCopyPath.Size = new Size(187, 22);
        tsmiFileCopyPath.Text = Properties.Resources.MenuItemCopyPath;
        tsmiFileCopyPath.Click += TsmiFileCopyPath_Click;
        // 
        // tsmiFileCopyRelativePath
        // 
        tsmiFileCopyRelativePath.Name = "tsmiFileCopyRelativePath";
        tsmiFileCopyRelativePath.Size = new Size(187, 22);
        tsmiFileCopyRelativePath.Text = Properties.Resources.MenuItemCopyRelativePath;
        tsmiFileCopyRelativePath.Click += TsmiFileCopyRelativePath_Click;
        // 
        // tsmiFileRepair
        // 
        tsmiFileRepair.Name = "tsmiFileRepair";
        tsmiFileRepair.Size = new Size(187, 22);
        tsmiFileRepair.Text = "Repair";
        tsmiFileRepair.Visible = false;
        tsmiFileRepair.Click += TsmiFileRepair_Click;
        // 
        // cmsDirectory
        // 
        cmsDirectory.ForeColor = Color.White;
        cmsDirectory.Items.AddRange(new ToolStripItem[] { tsmiDirAddFile, tsmiDirAddFolder, toolStripSeparator2, tsmiDirCut, tsmiDirCopy, tsmiDirPaste, tsmiDirDelate, tsmiDirRename, toolStripSeparator3, tsmiDirOpenInExplorer });
        cmsDirectory.Name = "cmsDirectory";
        cmsDirectory.Size = new Size(163, 192);
        cmsDirectory.Opening += CmsDirectory_Opening;
        // 
        // tsmiDirAddFile
        // 
        tsmiDirAddFile.Image = Properties.Resources.NewFile;
        tsmiDirAddFile.Name = "tsmiDirAddFile";
        tsmiDirAddFile.Size = new Size(162, 22);
        tsmiDirAddFile.Text = Properties.Resources.MenuItemNewFile;
        tsmiDirAddFile.Click += TsmiStrAddFile_Click;
        tsmiDirAddFile.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
        // 
        // tsmiDirAddFolder
        // 
        tsmiDirAddFolder.Image = Properties.Resources.NewFolder;
        tsmiDirAddFolder.Name = "tsmiDirAddFolder";
        tsmiDirAddFolder.Size = new Size(162, 22);
        tsmiDirAddFolder.Text = Properties.Resources.MenuItemNewFolder;
        tsmiDirAddFolder.Click += TsmiStrAddFolder_Click;
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(159, 6);
        // 
        // tsmiDirCut
        // 
        tsmiDirCut.Name = "tsmiDirCut";
        tsmiDirCut.ShortcutKeys = Keys.Control | Keys.X;
        tsmiDirCut.Size = new Size(162, 22);
        tsmiDirCut.Text = "Cut";
        tsmiDirCut.Click += TsmiDirCut_Click;
        // 
        // tsmiDirCopy
        // 
        tsmiDirCopy.Name = "tsmiDirCopy";
        tsmiDirCopy.ShortcutKeys = Keys.Control | Keys.C;
        tsmiDirCopy.Size = new Size(162, 22);
        tsmiDirCopy.Text = "Copy";
        tsmiDirCopy.Click += TsmiDirCopy_Click;
        // 
        // tsmiDirDelate
        // 
        tsmiDirDelate.Image = Properties.Resources.DelateFolder;
        tsmiDirDelate.Name = "tsmiDirDelate";
        tsmiDirDelate.ShortcutKeys = Keys.Delete;
        tsmiDirDelate.Size = new Size(162, 22);
        tsmiDirDelate.Text = Properties.Resources.MenuItemDelete;
        tsmiDirDelate.Click += TsmiDirDelate_Click;
        // 
        // tsmiDirRename
        // 
        tsmiDirRename.Image = Properties.Resources.Rename;
        tsmiDirRename.Name = "tsmiDirRename";
        tsmiDirRename.Size = new Size(162, 22);
        tsmiDirRename.Text = Properties.Resources.MenuItemRename;
        tsmiDirRename.Click += TsmiDirRename_Click;
        tsmiDirRename.ShortcutKeys = Keys.F2;
        // 
        // toolStripSeparator3
        // 
        toolStripSeparator3.Name = "toolStripSeparator3";
        toolStripSeparator3.Size = new Size(159, 6);
        // 
        // tsmiDirOpenInExplorer
        // 
        tsmiDirOpenInExplorer.Name = "tsmiDirOpenInExplorer";
        tsmiDirOpenInExplorer.Size = new Size(162, 22);
        tsmiDirOpenInExplorer.Text = Properties.Resources.MenuItemOpenInExplorer;
        tsmiDirOpenInExplorer.Click += TsmiSlnOpenInExplorer_Click;
        // 
        // tsmiDirPaste
        // 
        tsmiDirPaste.Name = "tsmiDirPaste";
        tsmiDirPaste.Size = new Size(162, 22);
        tsmiDirPaste.Text = "Paste";
        tsmiDirPaste.ShortcutKeys = Keys.Control | Keys.V;
        tsmiDirPaste.Click += TsmiDirPaste_Click;
        // 
        // SolutionExplorer
        // 
        AllowDrop = true;
        BorderStyle = BorderStyle.None;
        FullRowSelect = true;
        HideSelection = false;
        Indent = 10;
        LabelEdit = true;
        LineColor = Color.Black;
        ShowLines = false;
        ShowNodeToolTips = true;
        cmsSolution.ResumeLayout(false);
        cmsNamespace.ResumeLayout(false);
        cmsStructure.ResumeLayout(false);
        cmsFile.ResumeLayout(false);
        cmsDirectory.ResumeLayout(false);
        ResumeLayout(false);
    }


    private ContextMenuStrip cmsSolution;
    private ContextMenuStrip cmsNamespace;
    private ContextMenuStrip cmsStructure;
    private ContextMenuStrip cmsFile;
    private ContextMenuStrip cmsDirectory;
    private ToolStripMenuItem tsmiSlnAddNamespace;
    private ToolStripMenuItem tsmiSlnOpenInExplorer;
    private ToolStripMenuItem tsmiNsDelateNamespace;
    private ToolStripMenuItem tsmiNsRenameNamespace;
    private ToolStripMenuItem tsmiNsOpenInExplorer;
    private ToolStripMenuItem tsmiStrAddFile;
    private ToolStripMenuItem tsmiStrAddFolder;
    private ToolStripMenuItem tsmiStrAddOverride;
    private ToolStripMenuItem tsmiStrPaste;
    private ToolStripMenuItem tsmiStrOpenInExplorer;
    private ToolStripMenuItem tsmiFileRename;
    private ToolStripMenuItem tsmiFileCopyNamespacedId;
    private ToolStripMenuItem tsmiFileDelate;
    private ToolStripMenuItem tsmiFileCopyPath;
    private ToolStripMenuItem tsmiFileCopyRelativePath;
    private ToolStripMenuItem tsmiFileRepair;
    private ToolStripMenuItem tsmiFileShowInExplorer;
    private ToolStripMenuItem tsmiDirAddFile;
    private ToolStripMenuItem tsmiDirAddFolder;
    private ToolStripMenuItem tsmiDirRename;
    private ToolStripMenuItem tsmiDirDelate;
    private ToolStripMenuItem tsmiDirOpenInExplorer;
    private ToolStripMenuItem tsmiFileCut;
    private ToolStripMenuItem tsmiFileCopy;
    private ToolStripMenuItem tsmiDirCut;
    private ToolStripMenuItem tsmiDirCopy;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripSeparator toolStripSeparator6;
    private ToolStripSeparator toolStripSeparator7;
    private ToolStripSeparator toolStripSeparator8;
    private ToolStripSeparator toolStripSeparator9;
    private ToolStripMenuItem tsmiDirPaste;
}

