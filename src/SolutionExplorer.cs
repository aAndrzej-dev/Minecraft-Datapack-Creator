namespace MinecraftDatapackCreator;

internal partial class SolutionExplorer : TreeView
{
    public bool IsLabelEditing { get; private set; }


    private Datapack? solution;
    private IDatapackStructureItemsCollection? datapackStructure;


    public Datapack? Solution { get => solution; set { solution = value; LoadSolution(); } }
    public IDatapackStructureItemsCollection? DatapackStructure { get => datapackStructure; set { datapackStructure = value; LoadSolution(); } }
    public bool ShowFiles { get; set; } = true;
    //public bool ShowDirectories { get; set; }

    public event EventHandler<SolutionFileEventArgs>? FileOpened;
    public event EventHandler<SolutionFileEventArgs>? FileSelected;
    public event EventHandler<FileEventArgs>? MetaFileOpened;

    public SolutionExplorer()
    {
        InitializeComponent();
    }

    public void RefreshSolution()
    {
        if (Solution is null || DatapackStructure is null)
        {
            return;
        }

        string? sNode = SelectedNode?.FullPath;
        List<string> ExpandedNodes = new();

        foreach (TreeNode node in Nodes)
        {
            if (!node.IsExpanded)
                continue;

            ExpandedNodes.Add(node.FullPath);
            ExpandedNodes.AddRange(GetExpandedNodes(node));

        }
        LoadSolution(true);

        foreach (string item in ExpandedNodes)
        {
            TreeNode? node = GetNodeByPath(item);
            node?.Expand();

        }
        SelectedNode = GetNodeByPath(sNode);



    }


    protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
    {
        base.OnBeforeLabelEdit(e);
        if (e.Node == null)
        {
            e.CancelEdit = true;
            return;
        }

        SolutionNodeInfo tag = (SolutionNodeInfo)e.Node.Tag;

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
        if (e.Node is not TreeNode node)
            return;
        IsLabelEditing = false;

        SolutionNodeInfo tag = (SolutionNodeInfo)e.Node.Tag;


        bool isCreating = tag.solutionNodeType.HasFlag(SolutionNodeType.Creating);
        if (e.Label is null)
        {
            if (isCreating)
                node.Remove();
            e.CancelEdit = true;

            return;
        }
        if (tag.solutionNodeType.HasFlag(SolutionNodeType.Namespace))
        {
            string value = e.Label.TrimEnd().TrimStart().Replace(" ", "_").ToLower();
            if (string.IsNullOrWhiteSpace(e.Label) || string.IsNullOrWhiteSpace(value))
            {
                if (isCreating)
                    node.Remove();
                e.CancelEdit = true;
                return;
            }
            if (!Solution!.IsValidNamespace(value))
            {
                if (isCreating)
                    node.Remove();

                MessageBox.Show(this, $"Invalid name or this namespace allready exist.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                e.CancelEdit = true;

                return;

            }

            DirectoryInfo di = new(Path.Combine(Solution.Path, "data", SelectedNode.Text));
            if (di.Exists)
            {
                di.MoveTo(Path.Combine(Solution.Path, "data", value));
            }
            else
            {
                DirectoryInfo ndi = new(Path.Combine(Solution.Path, "data", value));
                ndi.Create();
            }

        }
        else if (tag.solutionNodeType.HasFlag(SolutionNodeType.Directory))
        {
            if (string.IsNullOrWhiteSpace(e.Label))
            {
                if (isCreating)
                    node.Remove();
                e.CancelEdit = true;
                return;
            }
            if (!Datapack.TryGetValidResourceName(e.Label, out string? value))
            {
                e.CancelEdit = true;
                if (isCreating)
                {
                    node.Remove();
                }
                MessageBox.Show(this, $"Invalid name", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }
            string fullpath = Path.Combine(Path.GetDirectoryName(Solution!.Path)!, Path.GetDirectoryName(node.FullPath)!, value);


            DirectoryInfo di = new(Path.Combine(Path.GetDirectoryName(Solution!.Path)!, node.FullPath));
            if (di.Exists)
            {
                di.MoveTo(Path.Combine(fullpath));

            }
            else
            {
                DirectoryInfo ndi = new(fullpath);
                ndi.Create();
            }
        }
        else if (tag.solutionNodeType.HasFlag(SolutionNodeType.File))
        {
            if (string.IsNullOrWhiteSpace(e.Label))
            {
                if (isCreating)
                    node.Remove();
                e.CancelEdit = true;
                return;
            }
            string[]? pathSplitted = node.FullPath.Replace("/", "\\").Split("\\");
            DatapackStructureFolder? folder = DatapackStructure!.GetDatapackStructureItemByName(string.Join("\\", pathSplitted, 3, pathSplitted.Length - 3)) as DatapackStructureFolder;

            string filename = e.Label.TrimEnd().TrimStart();

            if (e.Label is null || filename is null)
            {
                e.CancelEdit = true;
                return;
            }

            if (!filename.EndsWith("." + folder?.FileExtension!))
            {
                filename += "." + folder?.FileExtension!;
            }



            if (!Datapack.TryGetValidResourceName(filename, out string? fn))
            {



                if (isCreating)
                    node.Remove();
                e.CancelEdit = true;
                MessageBox.Show(this, $"Invalid name", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;

            }


            string newFullpath = Path.Combine(Solution!.Path, string.Join("\\", pathSplitted, 1, pathSplitted.Length - 2), fn);
            if (File.Exists(newFullpath))
            {
                MessageBox.Show("File allready exist!");
                if (isCreating)
                {
                    node.Remove();

                }
                e.CancelEdit = true;
                return;
            }
            DirectoryInfo di = new(Path.GetDirectoryName(newFullpath)!);
            if (!di.Exists)
                di.Create();
            if (isCreating)
            {

                FileInfo fi = new(newFullpath);
                fi.Create().Close();
            }
            else
            {
                string oldFullPath = Path.Combine(Solution.Path, string.Join("\\", pathSplitted, 1, pathSplitted.Length - 1));
                FileInfo fi = new(oldFullPath);

                fi.MoveTo(newFullpath, false);
            }
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

        if (Solution is null || DatapackStructure is null)
        {
            return;
        }

        if (e.Node is null)
            return;

        SolutionNodeInfo tag = (SolutionNodeInfo)e.Node.Tag;

        if (tag.solutionNodeType is not SolutionNodeType.File)
            return;

        FileSelected?.Invoke(this, new SolutionFileEventArgs(tag.@namespace!, tag.relativePath!, tag.structureFolder!, tag.fullPath!));

    }
    protected override void OnKeyDown(KeyEventArgs e)
    {

        if (e.KeyCode is Keys.Space)
        {
            e.SuppressKeyPress = true;
            if (SelectedNode.IsExpanded)
            {
                SelectedNode.Collapse();
            }
            else
            {
                SelectedNode.Expand();
            }
        }

        if (e.KeyCode is Keys.Enter)
        {
            if (Solution is null || DatapackStructure is null)
            {
                return;
            }


            SolutionNodeInfo tag = (SolutionNodeInfo)SelectedNode.Tag;
            if (tag.solutionNodeType.HasFlag(SolutionNodeType.MetaFile))
            {
                e.SuppressKeyPress = true;
                MetaFileOpened?.Invoke(this, new FileEventArgs(tag.fullPath!));
                return;
            }
            if (tag.solutionNodeType is not SolutionNodeType.File)
                return;
            e.SuppressKeyPress = true;

            FileOpened?.Invoke(this, new SolutionFileEventArgs(tag.@namespace!, tag.relativePath!, tag.structureFolder!, tag.fullPath!));

        }
        base.OnKeyDown(e);
    }
    protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
    {
        base.OnNodeMouseDoubleClick(e);

        if (Solution == null || DatapackStructure == null || e.Button != MouseButtons.Left)
        {
            return;
        }



        SolutionNodeInfo tag = (SolutionNodeInfo)e.Node.Tag;
        if (tag.solutionNodeType.HasFlag(SolutionNodeType.MetaFile))
        {
            MetaFileOpened?.Invoke(this, new FileEventArgs(tag.fullPath!));
            return;
        }
        if (tag.solutionNodeType is not SolutionNodeType.File)
            return;

        FileOpened?.Invoke(this, new SolutionFileEventArgs(tag.@namespace!, tag.relativePath!, tag.structureFolder!, tag.fullPath!));
    }

    private void LoadSolution(bool onlyReload = false)
    {



        if (Solution is null || DatapackStructure is null)
        {
            Nodes.Clear();
            return;
        }

        if (!onlyReload || solutionWatcher is null)
        {
            solutionWatcher = new FileSystemWatcher(Solution.Path)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
                SynchronizingObject = this,
            };



            solutionWatcher.Created += SolutionWatcher_Changed;
            solutionWatcher.Deleted += SolutionWatcher_Changed;
            solutionWatcher.Renamed += SolutionWatcher_Changed;

            solutionWatcher.EnableRaisingEvents = true;
        }



        BeginUpdate();
        Nodes.Clear();


        Nodes.Add(new TreeNode()
        {
            Name = Solution.Name,
            Text = Solution.Name,
            ImageIndex = 4,
            SelectedImageIndex = 4,
            ContextMenuStrip = cmsSolution,
            Tag = new SolutionNodeInfo(SolutionNodeType.Solution, Solution.Path)
        });

        Nodes[Solution.Name].Nodes.Add("pack.mcmeta", "pack.mcmeta", 2, 2);
        Nodes[Solution.Name].Nodes["pack.mcmeta"].Tag = new SolutionNodeInfo(SolutionNodeType.MetaFile, Path.Combine(Solution.Path, "pack.mcmeta"));

        Nodes[Solution.Name].Nodes.Add("data", "data", 5, 5);
        Nodes[Solution.Name].Nodes["data"].Tag = new SolutionNodeInfo(SolutionNodeType.Blank, Path.Combine(Solution.Path, "data"));

        foreach (string @namespace in Solution.GetNamespaces())
        {
            Nodes[Solution.Name].Nodes["data"].Nodes.Add(@namespace, @namespace, 3, 3);
            Nodes[Solution.Name].Nodes["data"].Nodes[@namespace].Tag = new SolutionNodeInfo(SolutionNodeType.Namespace, Path.Combine(Solution.Path, "data", @namespace), @namespace);
            Nodes[Solution.Name].Nodes["data"].Nodes[@namespace].ContextMenuStrip = cmsNamespace;


            foreach (DatapackStructureFolder structure in DatapackStructure)
            {

                CreateStructureFolders(structure, Nodes[Solution.Name].Nodes["data"].Nodes[@namespace], Path.Combine(Solution.Path, "data", @namespace), @namespace);
            }

        }

        EndUpdate();

    }
    private void CreateStructureFolders(DatapackStructureFolder folder, TreeNode parent, string path, string @namespace)
    {
        parent.Nodes.Add(folder.Name, folder.Name, 5, 5);
        parent.Nodes[folder.Name].Tag = new SolutionNodeInfo(SolutionNodeType.Structure, Path.Combine(path, folder.Name), @namespace, folder);
        parent.Nodes[folder.Name].ContextMenuStrip = cmsStructure;



        if (folder.Children?.Count > 0)
        {
            foreach (DatapackStructureFolder item in folder.Children!)
            {
                CreateStructureFolders(item, parent.Nodes[folder.Name], Path.Combine(path, folder.Name), @namespace);
            }
            return;
        }

        if (!Directory.Exists(Path.Combine(path, folder.Name)))
            return;

        foreach (string dir in Directory.GetDirectories(Path.Combine(path, folder.Name)))
        {
            CreateDirectoryStructure(dir, parent.Nodes[folder.Name], folder, @namespace);
        }
        if (!ShowFiles)
            return;

        foreach (string file in Directory.GetFiles(Path.Combine(path, folder.Name)))
        {
            int fileIcon = 2;

            if (Path.GetExtension(file)[1..] != folder.FileExtension || !Datapack.IsValidResourceName(Path.GetFileName(file)))
                fileIcon = 6;

            parent.Nodes[folder.Name].Nodes.Add(Path.GetFileName(file), Path.GetFileName(file), fileIcon, fileIcon);
            parent.Nodes[folder.Name].Nodes[Path.GetFileName(file)].Tag = new SolutionNodeInfo(SolutionNodeType.File | (fileIcon == 6 ? SolutionNodeType.Corrupted : 0), file, @namespace, folder, Path.GetFileNameWithoutExtension(file));
            parent.Nodes[folder.Name].Nodes[Path.GetFileName(file)].ContextMenuStrip = cmsFile;







        }


    }
    private void CreateDirectoryStructure(string path, TreeNode parent, DatapackStructureFolder folder, string @namespace, string relativePath = "")
    {
        relativePath = Path.Combine(relativePath, Path.GetFileName(path));
        string name = Path.GetFileName(path);
        parent.Nodes.Add(name, name, 1, 1);
        parent.Nodes[name].Tag = new SolutionNodeInfo(SolutionNodeType.Directory, path, @namespace, folder, relativePath);
        parent.Nodes[name].ContextMenuStrip = cmsDirectory;

        if (!Directory.Exists(path))
            return;

        foreach (string dir in Directory.GetDirectories(path))
        {
            CreateDirectoryStructure(dir, parent.Nodes[name], folder, @namespace, relativePath);
        }

        if (!ShowFiles)
            return;

        foreach (string file in Directory.GetFiles(path))
        {

            int fileIcon = 2;

            if (Path.GetExtension(file)[1..] != folder.FileExtension || !Datapack.IsValidResourceName(Path.GetFileName(file)))
            {
                fileIcon = 6;

            }


            parent.Nodes[name].Nodes.Add(Path.GetFileName(file), Path.GetFileName(file), fileIcon, fileIcon);
            parent.Nodes[name].Nodes[Path.GetFileName(file)].Tag = new SolutionNodeInfo(SolutionNodeType.File | (fileIcon == 6 ? SolutionNodeType.Corrupted : 0), file, @namespace, folder, Path.Combine(relativePath, Path.GetFileNameWithoutExtension(file)));
            parent.Nodes[name].Nodes[Path.GetFileName(file)].ContextMenuStrip = cmsFile;


        }



    }
    private TreeNode? GetNodeByPath(string? path)
    {
        if (path is null)
            return null;
        if (!path.Contains('\\'))
        {
            TreeNode node = Nodes[path];
            return node;
        }
        string[] splited = path.Split('\\');
        TreeNode treeNode = Nodes[splited[0]];
        for (int i = 1; i < splited.Length; i++)
        {
            treeNode = treeNode.Nodes[splited[i]];
        }
        return treeNode;
    }
    private string[] GetExpandedNodes(TreeNode parent)
    {
        List<string> ExpandedNodes = new();
        foreach (TreeNode node in parent.Nodes)
        {
            if (!node.IsExpanded)
                continue;

            ExpandedNodes.Add(node.FullPath);
            ExpandedNodes.AddRange(GetExpandedNodes(node));

        }
        return ExpandedNodes.ToArray();
    }

    public void AddNewNamespace()
    {
        if (Solution is null || DatapackStructure is null)
        {
            return;
        }

        TreeNode tmpNode = new("New Namespace")
        {
            Tag = new SolutionNodeInfo(SolutionNodeType.Namespace | SolutionNodeType.Creating, null),
            ImageIndex = 3,
            SelectedImageIndex = 3
        };

        Nodes[Solution.Name].Nodes["data"].Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();
    }


    private void SolutionWatcher_Changed(object? sender, FileSystemEventArgs e) => RefreshSolution();
    private void TsmiSlnAddNamespace_Click(object? sender, EventArgs e) => AddNewNamespace();
    private void TsmiNsRenameNamespace_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiNsDelateNamespace_Click(object? sender, EventArgs e)
    {
        DialogResult dr = MessageBox.Show(this, $"Do you want to delate namespace: {SelectedNode.Text}", "Minecraft Datapack Creator", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        if (dr is not DialogResult.Yes)
            return;

        string @namespace = SelectedNode.Text;
        SelectedNode.Collapse(false);
        DirectoryInfo di = new(Path.Combine(Solution!.Path, "data", @namespace));
        di.Delete(true);

    }
    private void TsmiStrAddFile_Click(object? sender, EventArgs e)
    {
        TreeNode tmpNode = new("New File")
        {
            Tag = new SolutionNodeInfo(SolutionNodeType.File | SolutionNodeType.Creating, null),
            ImageIndex = 2,
            SelectedImageIndex = 2
        };

        SelectedNode.Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();

    }
    private void TsmiFileDelate_Click(object? sender, EventArgs e)
    {
        if (Solution is null || DatapackStructure is null)
        {
            return;
        }

        SolutionNodeInfo tag = (SolutionNodeInfo)SelectedNode.Tag;

        if (tag.fullPath is null)
        {
            MessageBox.Show("Something went wrong");
        }

        if (MessageBox.Show(this, $"Do you want to delate {Path.GetFileName(tag.fullPath)}", "Minecraft Datapack Creator", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) is not DialogResult.Yes)
            return;



        File.Delete(tag.fullPath!);




    }
    private void TsmiFileRename_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiFileCopyNamespacedId_Click(object? sender, EventArgs e)
    {
        SolutionNodeInfo nodeInfo = (SolutionNodeInfo)SelectedNode.Tag;
        Clipboard.SetDataObject($"{nodeInfo.@namespace}:{nodeInfo.relativePath?.Replace('\\', '/')}");



    }
    private void TsmiStrAddFolder_Click(object? sender, EventArgs e)
    {
        if (Solution is null || DatapackStructure is null)
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
        if (Solution is null || DatapackStructure is null)
        {
            return;
        }
        if (MessageBox.Show(this, "Do you want to delate directory and all files?", "Minecraft Datapack Creator", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) is not DialogResult.Yes)
            return;


        DirectoryInfo di = new(Path.Combine(Path.GetDirectoryName(Solution!.Path)!, SelectedNode.FullPath));


        di.Delete(true);

    }
    private void TsmiDirRename_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiFileRepair_Click(object sender, EventArgs e)
    {

    }

    private void CmsFile_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
    }
}
