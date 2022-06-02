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

        if ((((SolutionNodeType)e.Node.Tag) & (SolutionNodeType.Namespace | SolutionNodeType.Directory | SolutionNodeType.File)) == 0)
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

        if (((SolutionNodeType?)node.Tag) is not SolutionNodeType nodeType)
            throw new Exception();


        bool isCreating = nodeType.HasFlag(SolutionNodeType.Creating);
        if (e.Label is null)
        {
            if (isCreating)
                node.Remove();
            e.CancelEdit = true;

            return;
        }
        if (nodeType.HasFlag(SolutionNodeType.Namespace))
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
        else if (nodeType.HasFlag(SolutionNodeType.Directory))
        {
            //string value = e.Label.TrimEnd().TrimStart().Replace(" ", "_").ToLower();
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
        else if (nodeType.HasFlag(SolutionNodeType.File))
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

        if (Solution is null || DatapackStructure is null || e.Node?.Tag?.Equals(SolutionNodeType.File) is false)
        {
            return;
        }

        if (e.Node is null)
            return;


        string[] splited = e.Node.FullPath.Split('\\');



        string @namespace = splited[2];
        //string relativeName = string.Join("\\", splited, 3, splited.Length - 3);
        string path = "";
        int lastIndex = 0;
        DatapackStructureFolder? structureFolder = null;
        for (int i = 3; i < splited.Length; i++)
        {
            if (GetNodeByPath(string.Join("\\", splited, 0, i + 1))?.Tag?.Equals(SolutionNodeType.Structure) is true)
            {
                if (string.IsNullOrEmpty(path))
                {
                    path += splited[i];
                }
                else
                {
                    path += "\\" + splited[i];
                }

                structureFolder = DatapackStructure.GetDatapackStructureItemByName(path) as DatapackStructureFolder;
            }
            else
            {
                lastIndex = i;
                break;
            }
        }
        FileSelected?.Invoke(this, new SolutionFileEventArgs(@namespace, string.Join("\\", splited!, 3, splited!.Length - 3), structureFolder));

    }
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode is Keys.Space)
        {
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

            if (Solution is null || DatapackStructure is null || !SelectedNode.Tag.Equals(SolutionNodeType.File))
            {
                return;
            }

            string[] splited = SelectedNode.FullPath.Split('\\');

            string @namespace = splited[2];

            string path = "";
            int lastIndex = 0;
            DatapackStructureFolder? structureFolder = null;
            for (int i = 3; i < splited.Length; i++)
            {
                if (GetNodeByPath(string.Join("\\", splited, 0, i + 1))?.Tag?.Equals(SolutionNodeType.Structure) is true)
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        path += splited[i];
                    }
                    else
                    {
                        path += "\\" + splited[i];
                    }

                    structureFolder = DatapackStructure.GetDatapackStructureItemByName(path) as DatapackStructureFolder;
                }
                else
                {
                    lastIndex = i;
                    break;
                }
            }

            FileOpened?.Invoke(this, new SolutionFileEventArgs(@namespace, string.Join("\\", splited, 3, splited.Length - 3), structureFolder));

        }
    }
    protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
    {
        base.OnNodeMouseDoubleClick(e);

        if (Solution == null || DatapackStructure == null || e.Button != MouseButtons.Left || (!e.Node.Tag.Equals(SolutionNodeType.File) && !e.Node.Tag.Equals(SolutionNodeType.MetaFile)))
        {
            return;
        }
        if (e.Node.Tag.Equals(SolutionNodeType.MetaFile))
        {
            MetaFileOpened?.Invoke(this, new FileEventArgs(e.Node.FullPath));
            return;
        }

        string[] splited = e.Node.FullPath.Split('\\');

        string @namespace = splited[2];

        //string relativeName = string.Join("\\", splited, 3, splited.Length - 3);
        string path = "";
        int lastIndex = 0;
        DatapackStructureFolder? structureFolder = null;
        for (int i = 3; i < splited.Length; i++)
        {
            if (GetNodeByPath(string.Join("\\", splited, 0, i + 1))?.Tag?.Equals(SolutionNodeType.Structure) == true)
            {
                if (string.IsNullOrEmpty(path))
                {
                    path += splited[i];
                }
                else
                {
                    path += "\\" + splited[i];
                }

                structureFolder = DatapackStructure.GetDatapackStructureItemByName(path) as DatapackStructureFolder;
            }
            else
            {
                lastIndex = i;
                break;
            }
        }

        FileOpened?.Invoke(this, new SolutionFileEventArgs(@namespace, string.Join("\\", splited, 3, splited.Length - 3), structureFolder));

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
            Tag = SolutionNodeType.Solution
        });

        Nodes[Solution.Name].Nodes.Add("pack.mcmeta", "pack.mcmeta", 2, 2);
        Nodes[Solution.Name].Nodes["pack.mcmeta"].Tag = SolutionNodeType.MetaFile;

        Nodes[Solution.Name].Nodes.Add("data", "data", 5, 5);
        Nodes[Solution.Name].Nodes["data"].Tag = SolutionNodeType.Blank;

        foreach (string @namespace in Solution.GetNamespaces())
        {
            Nodes[Solution.Name].Nodes["data"].Nodes.Add(@namespace, @namespace, 3, 3);
            Nodes[Solution.Name].Nodes["data"].Nodes[@namespace].Tag = SolutionNodeType.Namespace;
            Nodes[Solution.Name].Nodes["data"].Nodes[@namespace].ContextMenuStrip = cmsNamespace;


            foreach (DatapackStructureFolder structure in DatapackStructure)
            {

                CreateStructureFolders(structure, Nodes[Solution.Name].Nodes["data"].Nodes[@namespace], Path.Combine(Solution.Path, "data", @namespace));
            }

        }

        EndUpdate();

    }
    private void CreateStructureFolders(DatapackStructureFolder folder, TreeNode parent, string path)
    {
        parent.Nodes.Add(folder.Name, folder.Name, 5, 5);
        parent.Nodes[folder.Name].Tag = SolutionNodeType.Structure;
        parent.Nodes[folder.Name].ContextMenuStrip = cmsStructure;



        if (folder.Children?.Count > 0)
        {
            foreach (DatapackStructureFolder item in folder.Children!)
            {
                CreateStructureFolders(item, parent.Nodes[folder.Name], Path.Combine(path, folder.Name));
            }
            return;
        }

        if (!Directory.Exists(Path.Combine(path, folder.Name)))
            return;

        foreach (string dir in Directory.GetDirectories(Path.Combine(path, folder.Name)))
        {
            CreateDirectoryStructure(dir, parent.Nodes[folder.Name], folder);
        }
        if (!ShowFiles)
            return;

        foreach (string file in Directory.GetFiles(Path.Combine(path, folder.Name)))
        {
            int fileIcon = 2;

            if (Path.GetExtension(file)[1..] != folder.FileExtension || !Datapack.IsValidResourceName(Path.GetFileName(file)))
                fileIcon = 6;

            parent.Nodes[folder.Name].Nodes.Add(Path.GetFileName(file), Path.GetFileName(file), fileIcon, fileIcon);
            parent.Nodes[folder.Name].Nodes[Path.GetFileName(file)].Tag = SolutionNodeType.File | (fileIcon == 6 ? SolutionNodeType.Corrupted : 0);
            parent.Nodes[folder.Name].Nodes[Path.GetFileName(file)].ContextMenuStrip = cmsFile;







        }


    }
    private void CreateDirectoryStructure(string path, TreeNode parent, DatapackStructureFolder folder)
    {
        string name = Path.GetFileName(path);
        parent.Nodes.Add(name, name, 1, 1);
        parent.Nodes[name].Tag = SolutionNodeType.Directory;
        parent.Nodes[name].ContextMenuStrip = cmsDirectory;

        if (!Directory.Exists(path))
            return;

        foreach (string dir in Directory.GetDirectories(path))
        {
            CreateDirectoryStructure(dir, parent.Nodes[name], folder);
        }

        if (!ShowFiles) return;

        foreach (string file in Directory.GetFiles(path))
        {

            int fileIcon = 2;

            if (Path.GetExtension(file)[1..] != folder.FileExtension || !Datapack.IsValidResourceName(Path.GetFileName(file)))
            {
                fileIcon = 6;

            }


            parent.Nodes[name].Nodes.Add(Path.GetFileName(file), Path.GetFileName(file), fileIcon, fileIcon);
            parent.Nodes[name].Nodes[Path.GetFileName(file)].Tag = SolutionNodeType.File | (fileIcon == 6 ? SolutionNodeType.Corrupted : 0);
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
            Tag = SolutionNodeType.Namespace | SolutionNodeType.Creating,
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
            Tag = SolutionNodeType.File | SolutionNodeType.Creating,
            ImageIndex = 2,
            SelectedImageIndex = 2
        };

        SelectedNode.Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();

    }
    private void TsmiFileDelate_Click(object? sender, EventArgs e)
    {

    }
    private void TsmiFileRename_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiStrAddFolder_Click(object? sender, EventArgs e)
    {
        if (Solution is null || DatapackStructure is null)
        {
            return;
        }

        TreeNode tmpNode = new("New Folder")
        {
            Tag = SolutionNodeType.Directory | SolutionNodeType.Creating,
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
        return;
        if (Solution is null || DatapackStructure is null)
        {
            return;
        }
        string[] splited = SelectedNode.FullPath.Split('\\');


        string path = "";
        int lastIndex = 0;
        DatapackStructureFolder? structureFolder = null;
        for (int i = 3; i < splited.Length; i++)
        {
            if (GetNodeByPath(string.Join("\\", splited, 0, i + 1))?.Tag?.Equals(SolutionNodeType.Structure) == true)
            {
                if (string.IsNullOrEmpty(path))
                {
                    path += splited[i];
                }
                else
                {
                    path += "\\" + splited[i];
                }

                structureFolder = DatapackStructure.GetDatapackStructureItemByName(path) as DatapackStructureFolder;
            }
            else
            {
                lastIndex = i;
                break;
            }
        }

    }
    private void CmsFile_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (((SolutionNodeType?)SelectedNode.Tag)?.HasFlag(SolutionNodeType.Corrupted) is true)
        {
            tsmiFileRepair.Visible = true;
            MessageBox.Show(((int)SelectedNode.Tag).ToString());
        }
        else
        {
            tsmiFileRepair.Visible = false;
        }
    }
}
