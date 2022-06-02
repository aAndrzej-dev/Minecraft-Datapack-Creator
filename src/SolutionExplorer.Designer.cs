using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftDatapackCreator;

partial class SolutionExplorer
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
        cmsSolution = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiSlnAddNamespace = new System.Windows.Forms.ToolStripMenuItem();
        cmsNamespace = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiNsDelateNamespace = new System.Windows.Forms.ToolStripMenuItem();
        tsmiNsRenameNamespace = new System.Windows.Forms.ToolStripMenuItem();
        cmsStructure = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiStrAddFile = new System.Windows.Forms.ToolStripMenuItem();
        tsmiStrAddFolder = new System.Windows.Forms.ToolStripMenuItem();
        cmsFile = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiFileRename = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileDelate = new System.Windows.Forms.ToolStripMenuItem();
        tsmiFileRepair = new System.Windows.Forms.ToolStripMenuItem();
        cmsDirectory = new System.Windows.Forms.ContextMenuStrip(components);
        tsmiDirAddFile = new System.Windows.Forms.ToolStripMenuItem();
        tsmiDirAddFolder = new System.Windows.Forms.ToolStripMenuItem();
        tsmiDirRename = new System.Windows.Forms.ToolStripMenuItem();
        tsmiDirDelate = new System.Windows.Forms.ToolStripMenuItem();
        imageList = new ImageList();
        cmsSolution.SuspendLayout();
        cmsNamespace.SuspendLayout();
        cmsStructure.SuspendLayout();
        cmsFile.SuspendLayout();
        cmsDirectory.SuspendLayout();
        SuspendLayout();

        //
        // imageList
        //

        imageList.ColorDepth = ColorDepth.Depth32Bit;
        imageList.ImageSize = new Size(20, 20);
        imageList.Images.AddRange(new Image[]
        {
            Properties.Resources.None,
            Properties.Resources.Folder,
            Properties.Resources.File,
            Properties.Resources.Namespace,
            Properties.Resources.Project,
            Properties.Resources.StructureFolder,
            Properties.Resources.BadFile
        });

        // 
        // cmsSolution
        // 
        cmsSolution.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiSlnAddNamespace});
        cmsSolution.Name = "cmsSolution";
        cmsSolution.Size = new System.Drawing.Size(162, 26);
        cmsSolution.Renderer = new ToolStripProfessionalRenderer(new Forms.DarkColorTable());
        cmsSolution.ForeColor = Color.White;
        // 
        // tsmiSlnAddNamespace
        // 
        tsmiSlnAddNamespace.Name = "tsmiSlnAddNamespace";
        tsmiSlnAddNamespace.Size = new System.Drawing.Size(161, 22);
        tsmiSlnAddNamespace.Text = "New Namespace";
        tsmiSlnAddNamespace.Click += new System.EventHandler(TsmiSlnAddNamespace_Click);
        tsmiSlnAddNamespace.Image = Properties.Resources.NewNamespace;
        // 
        // cmsNamespace
        // 
        cmsNamespace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiNsDelateNamespace,
        tsmiNsRenameNamespace});
        cmsNamespace.Name = "cmsNamespace";
        cmsNamespace.Size = new System.Drawing.Size(132, 48);
        cmsNamespace.Renderer = new ToolStripProfessionalRenderer(new Forms.DarkColorTable());
        cmsNamespace.ForeColor = Color.White;
        // 
        // tsmiNsDelateNamespace
        // 
        tsmiNsDelateNamespace.Name = "tsmiNsDelateNamespace";
        tsmiNsDelateNamespace.ShortcutKeys = System.Windows.Forms.Keys.Delete;
        tsmiNsDelateNamespace.Size = new System.Drawing.Size(131, 22);
        tsmiNsDelateNamespace.Text = "Delate";
        tsmiNsDelateNamespace.Click += new System.EventHandler(TsmiNsDelateNamespace_Click);
        tsmiNsDelateNamespace.Image = Properties.Resources.DelateNamespace;
        // 
        // tsmiNsRenameNamespace
        // 
        tsmiNsRenameNamespace.Name = "tsmiNsRenameNamespace";
        tsmiNsRenameNamespace.Size = new System.Drawing.Size(131, 22);
        tsmiNsRenameNamespace.Text = "Rename";
        tsmiNsRenameNamespace.Click += new EventHandler(TsmiNsRenameNamespace_Click);
        tsmiNsRenameNamespace.Image = Properties.Resources.Rename;
        // 
        // cmsStructure
        // 
        cmsStructure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiStrAddFile,
        tsmiStrAddFolder});
        cmsStructure.Name = "cmsNamespace";
        cmsStructure.Size = new System.Drawing.Size(133, 48);
        cmsStructure.Renderer = new ToolStripProfessionalRenderer(new Forms.DarkColorTable());
        cmsStructure.ForeColor = Color.White;
        // 
        // tsmiStrAddFile
        // 
        tsmiStrAddFile.Name = "tsmiStrAddFile";
        tsmiStrAddFile.Size = new System.Drawing.Size(132, 22);
        tsmiStrAddFile.Text = "New File";
        tsmiStrAddFile.Click += new System.EventHandler(TsmiStrAddFile_Click);
        tsmiStrAddFile.Image = Properties.Resources.NewFile;
        // 
        // tsmiStrAddFolder
        // 
        tsmiStrAddFolder.Name = "tsmiStrAddFolder";
        tsmiStrAddFolder.Size = new System.Drawing.Size(132, 22);
        tsmiStrAddFolder.Text = "New Folder";
        tsmiStrAddFolder.Click += new System.EventHandler(TsmiStrAddFolder_Click);
        tsmiStrAddFolder.Image = Properties.Resources.NewFolder;
        // 
        // cmsFile
        // 
        cmsFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        tsmiFileRename,
        tsmiFileDelate,
        tsmiFileRepair});
        cmsFile.Name = "cmsNamespace";
        cmsFile.Size = new System.Drawing.Size(132, 48);
        cmsFile.Renderer = new ToolStripProfessionalRenderer(new Forms.DarkColorTable());
        cmsFile.ForeColor = Color.White;
        cmsFile.Opening += new System.ComponentModel.CancelEventHandler(CmsFile_Opening);
        // 
        // tsmiFileRename
        // 
        tsmiFileRename.Name = "tsmiFileRename";
        tsmiFileRename.Size = new System.Drawing.Size(131, 22);
        tsmiFileRename.Text = "Rename";
        tsmiFileRename.Click += new System.EventHandler(TsmiFileRename_Click);
        tsmiFileRename.Image = Properties.Resources.Rename;
        // 
        // tsmiFileRepair
        // 
        tsmiFileRepair.Name = "tsmiFileRepair";
        tsmiFileRepair.Size = new System.Drawing.Size(131, 22);
        tsmiFileRepair.Text = "Repair";
        tsmiFileRepair.Click += new System.EventHandler(TsmiFileRepair_Click);
        tsmiFileRepair.Visible = false;
        // 
        // tsmiFileDelate
        // 
        tsmiFileDelate.Name = "tsmiFileDelate";
        tsmiFileDelate.ShortcutKeys = System.Windows.Forms.Keys.Delete;
        tsmiFileDelate.Size = new System.Drawing.Size(131, 22);
        tsmiFileDelate.Text = "Delate";
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
        cmsDirectory.Renderer = new ToolStripProfessionalRenderer(new Forms.DarkColorTable());
        cmsDirectory.ForeColor = Color.White;
        // 
        // tsmiDirAddFile
        // 
        tsmiDirAddFile.Name = "tsmiDirAddFile";
        tsmiDirAddFile.Size = new System.Drawing.Size(132, 22);
        tsmiDirAddFile.Text = "New File";
        tsmiDirAddFile.Click += new System.EventHandler(TsmiStrAddFile_Click);

        tsmiDirAddFile.Image = Properties.Resources.NewFile;
        // 
        // tsmiDirAddFolder
        // 
        tsmiDirAddFolder.Name = "tsmiDirAddFolder";
        tsmiDirAddFolder.Size = new System.Drawing.Size(132, 22);
        tsmiDirAddFolder.Text = "Add Folder";
        tsmiDirAddFolder.Click += new System.EventHandler(TsmiStrAddFolder_Click);
        tsmiDirAddFolder.Image = Properties.Resources.NewFolder;
        // 
        // tsmiDirRename
        // 
        tsmiDirRename.Name = "tsmiDirRename";
        tsmiDirRename.Size = new System.Drawing.Size(132, 22);
        tsmiDirRename.Text = "Rename";
        tsmiDirRename.Click += new System.EventHandler(TsmiDirRename_Click);
        tsmiDirRename.Image = Properties.Resources.Rename;
        // 
        // tsmiDirDelate
        // 
        tsmiDirDelate.Name = "tsmiDirDelate";
        tsmiDirDelate.Size = new System.Drawing.Size(132, 22);
        tsmiDirDelate.Text = "Delate";
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
    private ToolStripMenuItem tsmiNsDelateNamespace;
    private ToolStripMenuItem tsmiNsRenameNamespace;
    private ToolStripMenuItem tsmiStrAddFile;
    private ToolStripMenuItem tsmiFileRename;
    private ToolStripMenuItem tsmiFileDelate;
    private ToolStripMenuItem tsmiFileRepair;
    private ToolStripMenuItem tsmiStrAddFolder;
    private ToolStripMenuItem tsmiDirAddFile;
    private ToolStripMenuItem tsmiDirAddFolder;
    private ToolStripMenuItem tsmiDirRename;
    private ToolStripMenuItem tsmiDirDelate;
    private FileSystemWatcher solutionWatcher;
    private ImageList imageList;
}

