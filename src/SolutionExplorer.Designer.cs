using MinecraftDatapackCreator.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftDatapackCreator;

internal partial class SolutionExplorer
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            this.imageList.Dispose();
        }
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsSolution = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiSlnAddNamespace = new System.Windows.Forms.ToolStripMenuItem();
        tsmiSlnOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
        cmsNamespace = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiNsDelateNamespace = new System.Windows.Forms.ToolStripMenuItem();
        tsmiNsRenameNamespace = new System.Windows.Forms.ToolStripMenuItem();
        tsmiNsOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
        cmsStructure = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiStrAddFile = new System.Windows.Forms.ToolStripMenuItem();
        tsmiStrAddFolder = new System.Windows.Forms.ToolStripMenuItem();
        cmsFile = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiFileRename = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileDelate = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileRepair = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileShowInExplorer = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileCopyNamespacedId = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileCopyPath = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileCopyRelativePath = new System.Windows.Forms.ToolStripMenuItem();
        cmsDirectory = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiDirAddFile = new System.Windows.Forms.ToolStripMenuItem();
        tsmiDirAddFolder = new System.Windows.Forms.ToolStripMenuItem();
        tsmiDirRename = new System.Windows.Forms.ToolStripMenuItem();
        tsmiDirDelate = new System.Windows.Forms.ToolStripMenuItem();
        cmsSolution.SuspendLayout();
        cmsNamespace.SuspendLayout();
        cmsStructure.SuspendLayout();
        cmsFile.SuspendLayout();
        cmsDirectory.SuspendLayout();
        SuspendLayout();


        // 
        // cmsSolution
        // 
        cmsSolution.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiSlnAddNamespace, tsmiSlnOpenInExplorer});
        cmsSolution.Name = "cmsSolution";
        cmsSolution.Size = new System.Drawing.Size(162, 26);
        cmsSolution.Renderer = new DarkToolStripRenderer();
        cmsSolution.ForeColor = Color.White;
        // 
        // tsmiSlnAddNamespace
        // 
        tsmiSlnAddNamespace.Name = "tsmiSlnAddNamespace";
        tsmiSlnAddNamespace.Size = new System.Drawing.Size(161, 22);
        tsmiSlnAddNamespace.Text = Properties.Resources.MenuItemAddNamespace;
        tsmiSlnAddNamespace.Click += new System.EventHandler(TsmiSlnAddNamespace_Click);
        tsmiSlnAddNamespace.Image = Properties.Resources.NewNamespace;
        // 
        // tsmiSlnAddNamespace
        // 
        tsmiSlnOpenInExplorer.Name = "tsmiSlnOpenInExplorer";
        tsmiSlnOpenInExplorer.Size = new System.Drawing.Size(161, 22);
        tsmiSlnOpenInExplorer.Text = Properties.Resources.MenuItemOpenInExplorer;
        tsmiSlnOpenInExplorer.Click += new System.EventHandler(TsmiSlnOpenInExplorer_Click);

        // 
        // cmsNamespace
        // 
        cmsNamespace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiNsDelateNamespace,
        tsmiNsRenameNamespace,
        tsmiNsOpenInExplorer});
        cmsNamespace.Name = "cmsNamespace";
        cmsNamespace.Size = new System.Drawing.Size(132, 48);
        cmsNamespace.Renderer = new DarkToolStripRenderer();
        cmsNamespace.ForeColor = Color.White;
        // 
        // tsmiNsDelateNamespace
        // 
        tsmiNsDelateNamespace.Name = "tsmiNsDelateNamespace";
        tsmiNsDelateNamespace.ShortcutKeys = System.Windows.Forms.Keys.Delete;
        tsmiNsDelateNamespace.Size = new System.Drawing.Size(131, 22);
        tsmiNsDelateNamespace.Text = Properties.Resources.MenuItemDelete;
        tsmiNsDelateNamespace.Click += new System.EventHandler(TsmiNsDelateNamespace_Click);
        tsmiNsDelateNamespace.Image = Properties.Resources.DelateNamespace;
        // 
        // tsmiNsRenameNamespace
        // 
        tsmiNsRenameNamespace.Name = "tsmiNsRenameNamespace";
        tsmiNsRenameNamespace.Size = new System.Drawing.Size(131, 22);
        tsmiNsRenameNamespace.Text = Properties.Resources.MenuItemRename;
        tsmiNsRenameNamespace.Click += new EventHandler(TsmiNsRenameNamespace_Click);
        tsmiNsRenameNamespace.Image = Properties.Resources.Rename;
        // 
        // tsmiNsOpenInExplorer
        // 
        tsmiNsOpenInExplorer.Name = "tsmiNsOpenInExplorer";
        tsmiNsOpenInExplorer.Size = new System.Drawing.Size(131, 22);
        tsmiNsOpenInExplorer.Text = Properties.Resources.MenuItemOpenInExplorer;
        tsmiNsOpenInExplorer.Click += new EventHandler(TsmiSlnOpenInExplorer_Click);
        // 
        // cmsStructure
        // 
        cmsStructure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiStrAddFile,
        tsmiStrAddFolder});
        cmsStructure.Name = "cmsNamespace";
        cmsStructure.Size = new System.Drawing.Size(133, 48);
        cmsStructure.Renderer = new DarkToolStripRenderer();
        cmsStructure.ForeColor = Color.White;
        // 
        // tsmiStrAddFile
        // 
        tsmiStrAddFile.Name = "tsmiStrAddFile";
        tsmiStrAddFile.Size = new System.Drawing.Size(132, 22);
        tsmiStrAddFile.Text = Properties.Resources.MenuItemNewFile;
        tsmiStrAddFile.Click += new System.EventHandler(TsmiStrAddFile_Click);
        tsmiStrAddFile.Image = Properties.Resources.NewFile;
        // 
        // tsmiStrAddFolder
        // 
        tsmiStrAddFolder.Name = "tsmiStrAddFolder";
        tsmiStrAddFolder.Size = new System.Drawing.Size(132, 22);
        tsmiStrAddFolder.Text = Properties.Resources.MenuItemNewFolder;
        tsmiStrAddFolder.Click += new System.EventHandler(TsmiStrAddFolder_Click);
        tsmiStrAddFolder.Image = Properties.Resources.NewFolder;
        // 
        // cmsFile
        // 
        cmsFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiFileRename,
        tsmiFileDelate,
        tsmiFileCopyNamespacedId,
        tsmiFileShowInExplorer,
        tsmiFileCopyPath,
        tsmiFileCopyRelativePath,
        tsmiFileRepair});
        cmsFile.Name = "cmsNamespace";
        cmsFile.Size = new System.Drawing.Size(132, 48);
        cmsFile.Renderer = new DarkToolStripRenderer();
        cmsFile.ForeColor = Color.White;
        // 
        // tsmiFileRename
        // 
        tsmiFileRename.Name = "tsmiFileRename";
        tsmiFileRename.Size = new System.Drawing.Size(131, 22);
        tsmiFileRename.Text = Properties.Resources.MenuItemRename;
        tsmiFileRename.Click += new System.EventHandler(TsmiFileRename_Click);
        tsmiFileRename.Image = Properties.Resources.Rename;
        // 
        // tsmiFileShowInExplorer
        // 
        tsmiFileShowInExplorer.Name = "tsmiFileShowInExplorer";
        tsmiFileShowInExplorer.Size = new System.Drawing.Size(131, 22);
        tsmiFileShowInExplorer.Text = Properties.Resources.MenuItemShowInExplorer;
        tsmiFileShowInExplorer.Click += new System.EventHandler(TsmiFileShowInExplorer_Click);
        // 
        // tsmiFileRepair
        // 
        tsmiFileRepair.Name = "tsmiFileRepair";
        tsmiFileRepair.Size = new System.Drawing.Size(131, 22);
        tsmiFileRepair.Text = "Repair";
        tsmiFileRepair.Visible = false;
        // 
        // tsmiFileCopyNamespacedId
        // 
        tsmiFileCopyNamespacedId.Name = "tsmiFileCopyNamespacedId";
        tsmiFileCopyNamespacedId.Size = new System.Drawing.Size(131, 22);
        tsmiFileCopyNamespacedId.Text = Properties.Resources.MenuItemCopyNamespacedId;
        tsmiFileCopyNamespacedId.Click += new System.EventHandler(TsmiFileCopyNamespacedId_Click);
        // 
        // tsmiFileCopyPath
        // 
        tsmiFileCopyPath.Name = "tsmiFileCopyPath";
        tsmiFileCopyPath.Size = new System.Drawing.Size(131, 22);
        tsmiFileCopyPath.Text = "Copy Path";
        tsmiFileCopyPath.Click += new System.EventHandler(TsmiFileCopyPath_Click);
        // 
        // tsmiFileCopyRelativePath
        // 
        tsmiFileCopyRelativePath.Name = "tsmiFileCopyRelativePath";
        tsmiFileCopyRelativePath.Size = new System.Drawing.Size(131, 22);
        tsmiFileCopyRelativePath.Text = "Copy Relative Path";
        tsmiFileCopyRelativePath.Click += new System.EventHandler(TsmiFileCopyRelativePath_Click);
        // 
        // tsmiFileDelate
        // 
        tsmiFileDelate.Name = "tsmiFileDelate";
        tsmiFileDelate.ShortcutKeys = System.Windows.Forms.Keys.Delete;
        tsmiFileDelate.Size = new System.Drawing.Size(131, 22);
        tsmiFileDelate.Text = Properties.Resources.MenuItemDelete;
        tsmiFileDelate.Click += new System.EventHandler(TsmiFileDelate_Click);
        tsmiFileDelate.Image = Properties.Resources.DelateFile;
        // 
        // cmsDirectory
        // 
        cmsDirectory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiDirAddFile,
        tsmiDirAddFolder,
        tsmiDirRename,
        tsmiDirDelate});
        cmsDirectory.Name = "cmsDirectory";
        cmsDirectory.Size = new System.Drawing.Size(133, 92);
        cmsDirectory.Renderer = new DarkToolStripRenderer();
        cmsDirectory.ForeColor = Color.White;
        // 
        // tsmiDirAddFile
        // 
        tsmiDirAddFile.Name = "tsmiDirAddFile";
        tsmiDirAddFile.Size = new System.Drawing.Size(132, 22);
        tsmiDirAddFile.Text = Properties.Resources.MenuItemNewFile;
        tsmiDirAddFile.Click += new System.EventHandler(TsmiStrAddFile_Click);

        tsmiDirAddFile.Image = Properties.Resources.NewFile;
        // 
        // tsmiDirAddFolder
        // 
        tsmiDirAddFolder.Name = "tsmiDirAddFolder";
        tsmiDirAddFolder.Size = new System.Drawing.Size(132, 22);
        tsmiDirAddFolder.Text = Properties.Resources.MenuItemNewFolder;
        tsmiDirAddFolder.Click += new System.EventHandler(TsmiStrAddFolder_Click);
        tsmiDirAddFolder.Image = Properties.Resources.NewFolder;
        // 
        // tsmiDirRename
        // 
        tsmiDirRename.Name = "tsmiDirRename";
        tsmiDirRename.Size = new System.Drawing.Size(132, 22);
        tsmiDirRename.Text = Properties.Resources.MenuItemRename;
        tsmiDirRename.Click += new System.EventHandler(TsmiDirRename_Click);
        tsmiDirRename.Image = Properties.Resources.Rename;
        // 
        // tsmiDirDelate
        // 
        tsmiDirDelate.Name = "tsmiDirDelate";
        tsmiDirDelate.Size = new System.Drawing.Size(132, 22);
        tsmiDirDelate.Text = Properties.Resources.MenuItemDelete;
        tsmiDirDelate.Click += new System.EventHandler(TsmiDirDelate_Click);
        tsmiDirDelate.Image = Properties.Resources.DelateFolder;
        // 
        // SolutionExplorer
        //
        FullRowSelect = true;
        Indent = 10;
        BorderStyle = BorderStyle.None;
        ShowLines = false;
        ShowNodeToolTips = true;
        HideSelection = false;
        LabelEdit = true;
        TabIndex = 0;
        ImageList = imageList;
        LineColor = System.Drawing.Color.Black;
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
    private ToolStripMenuItem tsmiFileRename;
    private ToolStripMenuItem tsmiFileCopyNamespacedId;
    private ToolStripMenuItem tsmiFileDelate;
    private ToolStripMenuItem tsmiFileCopyPath;
    private ToolStripMenuItem tsmiFileCopyRelativePath;
    private ToolStripMenuItem tsmiFileRepair;
    private ToolStripMenuItem tsmiFileShowInExplorer;
    private ToolStripMenuItem tsmiStrAddFolder;
    private ToolStripMenuItem tsmiDirAddFile;
    private ToolStripMenuItem tsmiDirAddFolder;
    private ToolStripMenuItem tsmiDirRename;
    private ToolStripMenuItem tsmiDirDelate;

}

