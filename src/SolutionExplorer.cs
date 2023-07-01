using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace MinecraftDatapackCreator;
internal sealed partial class SolutionExplorer : TreeView
{
    private Datapack? solution;
    private readonly ImageList imageList;

    public Datapack? Solution { get => solution; set { solution = value; ReloadSolution(); } }

    public bool IsLabelEditing { get; private set; }

    public event EventHandler<DatapackFileEventArgs>? FileOpened;
    public event EventHandler<DatapackFileEventArgs>? FileSelected;
    public event EventHandler<FileEventArgs>? MetaFileOpened;

    private string? focusOnChanged;



    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetScrollPos(IntPtr hWnd, System.Windows.Forms.Orientation nBar);
    [DllImport("user32.dll")]
    public static extern int SetScrollPos(IntPtr hWnd, System.Windows.Forms.Orientation nBar, int nPos, bool bRedraw);

    private Point ScrollPosition
    {
        get => new Point(GetScrollPos(Handle, Orientation.Horizontal), GetScrollPos(Handle, Orientation.Vertical));
        set
        {
            SetScrollPos(Handle, Orientation.Horizontal, value.X, true);
            SetScrollPos(Handle, Orientation.Vertical, value.Y, true);
        }
    }


    public SolutionExplorer()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        HideSelection = false;
        FullRowSelect = true;
        ShowLines = false;
        LabelEdit = true;

        imageList = new ImageList
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(20, 20),
            Images =
            {
                Properties.Resources.None,
                Properties.Resources.Folder,
                Properties.Resources.File,
                Properties.Resources.Namespace,
                Properties.Resources.Project,
                Properties.Resources.StructureFolder,
                Properties.Resources.BadFile,

            }
        };
        ImageList = imageList;
        TreeViewNodeSorter = new DatapackFileStructureComparer();
        InitializeComponent();
    }

    private void ReloadSolution()
    {
        if (Solution is null)
        {
            Nodes.Clear();
            return;
        }
        BeginUpdate();
        Nodes.Clear();
        TreeNode root = Nodes.Add(Solution.Name, Solution.Name, 4, 4);
        root.Tag = new SolutionNodeInfo(SolutionNodeType.Solution, Solution.Path, Solution.FileStructure.RootFolder);
        root.ContextMenuStrip = cmsSolution;

        TreeNode packmemeta = root.Nodes.Add("pack.mcmeta", "pack.mcmeta", 2, 2);
        packmemeta.Tag = new SolutionNodeInfo(SolutionNodeType.MetaFile, Path.Combine(Solution.Path, "pack.mcmeta"), Solution.FileStructure.RootFolder.GetRelativeFile("pack.mcmeta"));


        TreeNode dataNode = Nodes[Solution.Name].Nodes.Add(Datapack.DATA_FOLDER_NAME, "data", 5, 5);

        dataNode.Tag = new SolutionNodeInfo(SolutionNodeType.None, Path.Combine(Solution.Path, Datapack.DATA_FOLDER_NAME), Solution.FileStructure.RootFolder.GetRelativeFolder(Datapack.DATA_FOLDER_NAME));
        foreach (string item in Solution.GetNamespaces())
        {
            TreeNode nsNode = dataNode.Nodes.Add(item, item, 3, 3);
            nsNode.Tag = new SolutionNodeInfo(SolutionNodeType.Namespace, Path.Combine(Solution.Path, Datapack.DATA_FOLDER_NAME, item), Solution.FileStructure.RootFolder.GetRelativeFolder($"{Datapack.DATA_FOLDER_NAME}\\{item}"));
            nsNode.ContextMenuStrip = cmsNamespace;
            CreateDatapackStructure(nsNode, Solution.DatapackStructure, Path.Combine(Solution.Path, Datapack.DATA_FOLDER_NAME, item));
        }


        EndUpdate();
        Sort();
        Solution.FileStructure.FileChanged -= FileStructure_FileChanged;
        Solution.FileStructure.FileChanged += FileStructure_FileChanged;
    }

    private void FileStructure_FileChanged(object? sender, DatapackItemChangedEventArgs e)
    {
        if (Solution is null)
            return;
        if (e.ChangedType == DatapackFileChangedType.Created)
        {
            TreeNode? parentNode = GetNodeByPath(Path.GetRelativePath(Path.GetDirectoryName(Solution.Path)!, Path.GetDirectoryName(e.Subject.FullName)!));
            bool isDir = Directory.Exists(e.Subject.FullName);
            bool isNamespace = e.Subject.Parent == e.Subject.Datapack.FileStructure.GetDirectory(Path.Combine(Solution.Path, Datapack.DATA_FOLDER_NAME));

            foreach (TreeNode item in parentNode!.Nodes)
            {
                if (item.Name != e.Subject.Name)
                {
                    continue;
                }

                if (item.Tag is not SolutionNodeInfo tag)
                    return;

                if (tag.solutionNodeType is SolutionNodeType.Structure)
                {
                    return;
                }
            }

            Invoke(() =>
            {
                BeginUpdate();
                Point spos = ScrollPosition;
                TreeNode node = parentNode!.Nodes.Add(e.Subject.Name, e.Subject.Name, isNamespace ? 3 : (isDir ? 1 : 2), isNamespace ? 3 : (isDir ? 1 : 2));
                if (isNamespace)
                    node.ContextMenuStrip = cmsNamespace;
                else if (isDir)
                    node.ContextMenuStrip = cmsDirectory;
                else
                    node.ContextMenuStrip = cmsFile;

                node.Tag = new SolutionNodeInfo(isNamespace ? SolutionNodeType.Namespace : (isDir ? SolutionNodeType.Directory : SolutionNodeType.File), e.Subject.FullName, e.Subject);
                if (isNamespace)
                {
                    CreateDatapackStructure(node, Solution.DatapackStructure, Path.Combine(Solution.Path, Datapack.DATA_FOLDER_NAME, e.Subject.Name));
                }
                Sort();
                ScrollPosition = spos;
                if (focusOnChanged?.Equals(e.Subject.FullName, StringComparison.OrdinalIgnoreCase) is true)
                {
                    SelectedNode = node;
                }
                EndUpdate();
            });
        }
        else if (e.ChangedType == DatapackFileChangedType.Delated)
        {

            TreeNode? parentNode = GetNodeByPath(Path.GetRelativePath(Path.GetDirectoryName(Solution.Path)!, Path.GetDirectoryName(e.Subject.FullName)!));
            if (parentNode is null)
                throw new Exception("Internal Error: parent node is null");
            Invoke(() =>
            {
                BeginUpdate();
                Point spos = ScrollPosition;
                if (parentNode.Nodes.ContainsKey(e.Subject.Name))
                    parentNode?.Nodes.Remove(parentNode.Nodes[e.Subject.Name]);

                Sort();
                ScrollPosition = spos;
                SelectedNode = parentNode;
                EndUpdate();
            });
        }
        else if (e.ChangedType == DatapackFileChangedType.Renamed)
        {
            DatapackItemRenamedEventArgs eventArgs = (DatapackItemRenamedEventArgs)e;
            TreeNode? parentNode = GetNodeByPath(Path.GetRelativePath(Path.GetDirectoryName(Solution.Path)!, Path.GetDirectoryName(eventArgs.OldPath)!));

            Invoke(() =>
            {
                BeginUpdate();
                Point spos = ScrollPosition;
                TreeNode? node = parentNode?.Nodes[Path.GetFileName(eventArgs.OldPath)];
                if (node is null)
                    return;
                node.Name = e.Subject.Name;
                node.Text = e.Subject.Name;
                Sort();
                ScrollPosition = spos;
                if (focusOnChanged?.Equals(e.Subject.FullName, StringComparison.OrdinalIgnoreCase) is true)
                {
                    SelectedNode = node;
                }
                EndUpdate();
            });

        }
    }
    private TreeNode? GetNodeByPath(ReadOnlySpan<char> path)
    {
        if (!path.Contains('\\'))
        {
            TreeNode node = Nodes[path.ToString()];
            return node;
        }
        return GetNodeByPath(Nodes[path[..path.IndexOf('\\')].ToString()], path[(path.IndexOf('\\') + 1)..]);
    }
    private TreeNode? GetNodeByPath(TreeNode parent, ReadOnlySpan<char> path)
    {
        if (!path.Contains('\\'))
        {
            TreeNode node = parent.Nodes[path.ToString()];
            return node;
        }
        return GetNodeByPath(parent.Nodes[path[..path.IndexOf('\\')].ToString()], path[(path.IndexOf('\\') + 1)..]);
    }
    private void CreateDatapackStructure(TreeNode parent, DatapackStructureFoldersCollection folders, string path)
    {
        foreach (DatapackStructureFolder item in folders)
        {
            TreeNode node = parent.Nodes.Add(item.Name, item.Name, 5, 5);
            node.Tag = new SolutionNodeInfo(SolutionNodeType.Structure, Path.Combine(path, item.Path), Solution!.FileStructure.RootFolder.GetFolder(Path.Combine(path, item.Name)));
            node.ContextMenuStrip = cmsStructure;
            CreateDatapackStructure(node, item.Children, Path.Combine(path, item.Name));
            CreateDatapackFileStructure(node, item, Path.Combine(path, item.Name));

        }
    }
    private void CreateDatapackFileStructure(TreeNode parent, DatapackStructureFolder folder, string path)
    {
        DatapackFolderInfo? folderInfo = Solution!.FileStructure.RootFolder.GetFolder(path);
        if (folderInfo is null)
            return;

        foreach (DatapackFolderInfo item in folderInfo.GetFolders().Where(x => !folder.Children.Any(y => y.Name == x.Name)))
        {
            TreeNode node = parent.Nodes.Add(item.Name, item.Name, 1, 1);
            node.Tag = new SolutionNodeInfo(SolutionNodeType.Directory, Path.Combine(path, item.Name), Solution.FileStructure.RootFolder.GetFolder(Path.Combine(path, item.Name)));
            node.ContextMenuStrip = cmsDirectory;
            CreateDatapackFileStructure(node, folder, Path.Combine(path, item.Name));
        }
        foreach (DatapackFileInfo item in folderInfo.GetFiles())
        {
            TreeNode node = parent.Nodes.Add(item.Name, item.Name, 2, 2);
            node.Tag = new SolutionNodeInfo(SolutionNodeType.File, Path.Combine(path, item.Name), Solution.FileStructure.RootFolder.GetFile(Path.Combine(path, item.Name)));
            node.ContextMenuStrip = cmsFile;
        }
    }
    protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
    {
        base.OnNodeMouseClick(e);

        if (e.Button is MouseButtons.Right)
        {
            SelectedNode = e.Node;
        }
    }
    protected override void OnAfterSelect(TreeViewEventArgs e)
    {
        base.OnAfterSelect(e);

        if (Solution is null)
        {
            return;
        }

        if (e.Node is null)
        {
            return;
        }

        SolutionNodeInfo tag = (SolutionNodeInfo)e.Node.Tag;

        if (tag.solutionNodeType is not SolutionNodeType.File)
        {
            return;
        }
        FileSelected?.Invoke(this, new DatapackFileEventArgs((DatapackFileInfo)tag.fileInfo!));

    }
    protected override void OnKeyDown(KeyEventArgs e)
    {

        if (e.KeyCode is Keys.Space)
        {
            e.SuppressKeyPress = true;
            if (SelectedNode?.IsExpanded is true)
            {
                SelectedNode.Collapse();
            }
            else
            {
                SelectedNode?.Expand();
            }
        }

        if (e.KeyCode is Keys.Enter)
        {
            if (Solution is null)
            {
                return;
            }



            if (SelectedNode?.Tag is not SolutionNodeInfo tag)
                return;

            if (tag.solutionNodeType.HasFlag(SolutionNodeType.MetaFile))
            {
                e.SuppressKeyPress = true;
                MetaFileOpened?.Invoke(this, new FileEventArgs(tag.fullPath!));
                return;
            }
            if (tag.solutionNodeType is not SolutionNodeType.File)
            {
                return;
            }

            e.SuppressKeyPress = true;

            FileOpened?.Invoke(this, new DatapackFileEventArgs((DatapackFileInfo)tag.fileInfo!));

        }
        base.OnKeyDown(e);
    }
    protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
    {
        base.OnNodeMouseDoubleClick(e);

        if (Solution is null || e.Button != MouseButtons.Left)
        {
            return;
        }




        if (e.Node.Tag is not SolutionNodeInfo tag)
            return;

        if (tag.solutionNodeType.HasFlag(SolutionNodeType.MetaFile))
        {
            MetaFileOpened?.Invoke(this, new FileEventArgs(tag.fullPath!));
            return;
        }
        if (tag.solutionNodeType is not SolutionNodeType.File)
        {
            return;
        }

        FileOpened?.Invoke(this, new DatapackFileEventArgs((DatapackFileInfo)tag.fileInfo!));
    }
    public void AddNewNamespace()
    {
        if (Solution is null)
        {
            return;
        }

        TreeNode tmpNode = new(Properties.Resources.NewNamespacePlaceholder)
        {
            Tag = new SolutionNodeInfo(SolutionNodeType.Namespace | SolutionNodeType.Creating, null),
            ImageIndex = 3,
            SelectedImageIndex = 3
        };

        Nodes[Solution.Name].Nodes[Datapack.DATA_FOLDER_NAME].Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();
    }
    public bool SelectFile(string path)
    {
        return SelectFile(Nodes);
        bool SelectFile(TreeNodeCollection nodes)
        {
            foreach (TreeNode item in nodes)
            {
                if (item.Tag is SolutionNodeInfo sni)
                {
                    if (Path.GetFullPath(path) == sni.fullPath)
                    {
                        SelectedNode = item;
                        return true;
                    }
                }
                if (SelectFile(item.Nodes))
                    return true;
            }
            return false;
        }
    }
    protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
    {
        base.OnBeforeLabelEdit(e);
        if (e.Node == null)
        {
            e.CancelEdit = true;
            return;
        }


        if (e.Node.Tag is not SolutionNodeInfo tag)
            return;

        if ((tag.solutionNodeType & (SolutionNodeType.Namespace | SolutionNodeType.Directory | SolutionNodeType.File)) == 0)
        {
            e.CancelEdit = true;
            return;
        }

        IsLabelEditing = true;

    }
    protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
    {
        base.OnAfterLabelEdit(e);
        if (Solution is null)
            return;

        IsLabelEditing = false;

        BeginUpdate();

        ProcessLabelEdit(e);

        EndUpdate();
    }
    private void ProcessLabelEdit(NodeLabelEditEventArgs e)
    {
        if (e.Node?.Tag is not SolutionNodeInfo tag)
            return;

        bool isCreating = tag.solutionNodeType.HasFlag(SolutionNodeType.Creating);
        if (e.Label is null)
        {

            if (isCreating)
            {
                SelectedNode = e.Node.Parent;
                e.Node.Remove();
            }

            e.CancelEdit = true;

            return;
        }
        if (tag.solutionNodeType.HasFlag(SolutionNodeType.Namespace))
        {
            string value = e.Label.TrimEnd().TrimStart().Replace(" ", "_", StringComparison.Ordinal).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(e.Label) || string.IsNullOrWhiteSpace(value))
            {
                if (isCreating)
                {
                    SelectedNode = e.Node.Parent;
                    e.Node.Remove();
                }

                e.CancelEdit = true;
                return;
            }
            if (!Solution!.IsValidNamespace(value))
            {
                if (isCreating)
                {
                    SelectedNode = e.Node.Parent;
                    e.Node.Remove();
                }
                MessageBox.Show(this, Properties.Resources.DialogInvalidNamespaceName, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                e.CancelEdit = true;

                return;

            }

            DirectoryInfo di = new(Path.Combine(Solution.Path, Datapack.DATA_FOLDER_NAME, SelectedNode.Text));
            string newPath = Path.Combine(Solution.Path, Datapack.DATA_FOLDER_NAME, value);
            if (di.Exists)
            {
                di.MoveTo(newPath);
            }
            else
            {
                DirectoryInfo ndi = new(newPath);
                ndi.Create();
            }
            focusOnChanged = newPath;

            if (isCreating)
            {
                SelectedNode = e.Node.Parent;
                e.Node.Remove();
            }
        }
        else if (tag.solutionNodeType.HasFlag(SolutionNodeType.Directory))
        {
            if (string.IsNullOrWhiteSpace(e.Label))
            {
                if (isCreating)
                {
                    SelectedNode = e.Node.Parent;
                    e.Node.Remove();
                }

                e.CancelEdit = true;
                return;
            }
            if (!Datapack.TryGetValidResourceName(e.Label, out string? value))
            {
                e.CancelEdit = true;
                if (isCreating)
                {
                    SelectedNode = e.Node.Parent;
                    e.Node.Remove();
                }
                MessageBox.Show(this, $"Invalid name", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }
            DirectoryInfo di = new(Path.Combine(Path.GetDirectoryName(Solution!.Path)!, e.Node.FullPath));

            string newPath = Path.Combine(Path.GetDirectoryName(Solution!.Path)!, Path.GetDirectoryName(e.Node.FullPath)!, value);

            if (di.Exists)
            {
                di.MoveTo(Path.Combine(newPath));
            }
            else
            {
                DirectoryInfo ndi = new(newPath);
                ndi.Create();
            }
            focusOnChanged = newPath;
            if (isCreating)
            {
                SelectedNode = e.Node.Parent;
                e.Node.Remove();
            }
        }
        else if (tag.solutionNodeType.HasFlag(SolutionNodeType.File))
        {
            if (string.IsNullOrWhiteSpace(e.Label))
            {
                if (isCreating)
                {
                    SelectedNode = e.Node.Parent;
                    e.Node.Remove();
                }

                e.CancelEdit = true;
                return;
            }
            string[]? pathSplitted = e.Node.FullPath.Replace("/", "\\", StringComparison.Ordinal).Split("\\");

            string filename = e.Label.TrimEnd().TrimStart();

            if (e.Label is null || filename is null)
            {
                e.CancelEdit = true;
                return;
            }
            if (!filename.EndsWith("." + tag.GetStructureFolder()!.FilesExtension, StringComparison.OrdinalIgnoreCase))
            {
                filename += "." + tag.GetStructureFolder()!.FilesExtension;
            }



            if (!Datapack.TryGetValidResourceName(filename, out string? fn))
            {



                if (isCreating)
                {
                    SelectedNode = e.Node.Parent;
                    e.Node.Remove();
                }

                e.CancelEdit = true;
                MessageBox.Show(this, $"Invalid name", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;

            }


            string newPath = Path.Combine(Solution!.Path, string.Join("\\", pathSplitted, 1, pathSplitted.Length - 2), fn);
            if (File.Exists(newPath))
            {
                MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExist, Program.ProductTitle);
                if (isCreating)
                {
                    SelectedNode = e.Node.Parent;
                    e.Node.Remove();
                }
                e.CancelEdit = true;
                return;
            }
            DirectoryInfo di = new(Path.GetDirectoryName(newPath)!);
            if (!di.Exists)
            {
                di.Create();
            }

            if (isCreating)
            {
                FileInfo fi = new(newPath);
                fi.Create().Close();

                SelectedNode = e.Node.Parent;
                e.Node.Remove();
            }
            else
            {
                string oldFullPath = Path.Combine(Solution.Path, string.Join("\\", pathSplitted, 1, pathSplitted.Length - 1));
                FileInfo fi = new(oldFullPath);

                fi.MoveTo(newPath, false);
            }
            focusOnChanged = newPath;
        }
    }





    private void TsmiSlnAddNamespace_Click(object? sender, EventArgs e) => AddNewNamespace();
    private void TsmiNsRenameNamespace_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiNsDelateNamespace_Click(object? sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }

        DialogResult dr = MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Properties.Resources.DialogNamespaceDeleteQuestion, SelectedNode.Text), Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        if (dr is not DialogResult.Yes)
        {
            return;
        }

        string @namespace = SelectedNode.Text;
        SelectedNode.Collapse(false);
        DirectoryInfo di = new(Path.Combine(Solution!.Path, Datapack.DATA_FOLDER_NAME, @namespace));
        di.Delete(true);

    }
    private void TsmiStrAddFile_Click(object? sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }

        string[]? pathSplitted = SelectedNode.FullPath.Replace("/", "\\", StringComparison.Ordinal).Split("\\");
        DatapackStructureFolder? folder = Solution.DatapackStructure.GetDatapackStructureItemByName(string.Join("\\", pathSplitted, 3, pathSplitted.Length - 3));
        TreeNode tmpNode = new("New File")
        {
            Tag = new SolutionNewFilewNodeInfo(SolutionNodeType.None, folder),
            ImageIndex = 2,
            SelectedImageIndex = 2
        };

        SelectedNode.Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();

    }
    private void TsmiFileDelate_Click(object? sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }

        SolutionNodeInfo nodeInfo = (SolutionNodeInfo)SelectedNode.Tag;

        if (nodeInfo.fullPath is null)
        {
            MessageBox.Show(this, "Something went wrong", Program.ProductTitle);
        }

        if (MessageBox.Show(this, $"Do you want to delate {Path.GetFileName(nodeInfo.fullPath)}", Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) is not DialogResult.Yes)
        {
            return;
        }

        File.Delete(nodeInfo.fullPath!);
    }
    private void TsmiFileRename_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiFileCopyNamespacedId_Click(object? sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }

        SolutionNodeInfo nodeInfo = (SolutionNodeInfo)SelectedNode.Tag;
        Clipboard.SetDataObject(((DatapackFileInfo?)nodeInfo.fileInfo)?.NamespacedId!);
    }
    private void TsmiStrAddFolder_Click(object? sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }

        TreeNode tmpNode = new("New Folder")
        {
            Tag = new SolutionNodeInfo(SolutionNodeType.Directory | SolutionNodeType.Creating, null),
            ImageIndex = 1,
            SelectedImageIndex = 1
        };

        SelectedNode.Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();
    }
    private void TsmiDirDelate_Click(object? sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }
        if (SelectedNode.Tag is not SolutionNodeInfo nodeInfo)
            return;

        if (MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Properties.Resources.DialogDirectoryDeleteQuestion, nodeInfo.fullPath), Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) is not DialogResult.Yes)
        {
            return;
        }

        DirectoryInfo di = new(nodeInfo.fullPath!);


        di.Delete(true);
    }
    private void TsmiDirRename_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();


    private void TsmiSlnOpenInExplorer_Click(object sender, EventArgs e)
    {
        if (SelectedNode.Tag is not SolutionNodeInfo selectedItem)
        {
            return;
        }

        if (selectedItem.fullPath is null)
        {
            return;
        }

        Process.Start("explorer.exe", selectedItem.fullPath);

    }
    private void TsmiFileShowInExplorer_Click(object sender, EventArgs e)
    {
        if (SelectedNode.Tag is not SolutionNodeInfo selectedItem)
        {
            return;
        }

        if (selectedItem.fullPath is null)
        {
            return;
        }

        Process.Start("explorer.exe", $"/select, {selectedItem.fullPath}");
    }
    private void TsmiFileCopyPath_Click(object sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }
        SolutionNodeInfo tag = (SolutionNodeInfo)SelectedNode.Tag;
        Clipboard.SetDataObject(((DatapackFileInfo?)tag.fileInfo)?.FullName!);
    }
    private void TsmiFileCopyRelativePath_Click(object sender, EventArgs e)
    {
        if (Solution is null)
        {
            return;
        }
        SolutionNodeInfo tag = (SolutionNodeInfo)SelectedNode.Tag;
        Clipboard.SetDataObject(Path.GetRelativePath(Solution.Path, ((DatapackFileInfo)tag.fileInfo!).FullName));
    }
}
