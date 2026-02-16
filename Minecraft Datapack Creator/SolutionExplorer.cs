using MinecraftDatapackCreator.FileStructure;
using MinecraftDatapackCreator.Forms;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using static MinecraftDatapackCreator.PInvoke;
using System.ComponentModel;


namespace MinecraftDatapackCreator;
internal sealed partial class SolutionExplorer : TreeView
{
    private Datapack? solution;
    private IDatapackItemInfo? focusOnChanged;
    private readonly Controller controller;
    private TreeNode? underRemove;

    public event EventHandler<DatapackFileInfo>? FileOpened;
    public event EventHandler<DatapackFileInfo>? FileSelected;

    private Point ScrollPosition
    {
        get => new Point(GetScrollPos(Handle, Orientation.Horizontal), GetScrollPos(Handle, Orientation.Vertical));
        set
        {
            SetScrollPos(Handle, Orientation.Horizontal, value.X, true);
            SetScrollPos(Handle, Orientation.Vertical, value.Y, true);
        }
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Datapack? Solution { get => solution; set { solution = value; LoadSolution(); } }


    public SolutionExplorer(Controller controller)
    {
        this.controller = controller;
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        ImageList = new ImageList
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
                Properties.Resources.Icon

            }
        };
        ShowRootLines = false;
        TreeViewNodeSorter = new DatapackFileStructureComparer();
        InitializeComponent();
        cmsSolution.Renderer = DarkToolStripRenderer.Instance;
        cmsStructure.Renderer = DarkToolStripRenderer.Instance;
        cmsDirectory.Renderer = DarkToolStripRenderer.Instance;
        cmsFile.Renderer = DarkToolStripRenderer.Instance;
        cmsNamespace.Renderer = DarkToolStripRenderer.Instance;

    }


    public bool SelectItem(IDatapackItemInfo? itemInfo)
    {
        if (Solution is null || itemInfo is null)
            return false;

        TreeNode? node = GetNodeByPath(Nodes[Solution.Name], itemInfo.PathRelativeToSolution);
        if (node is null)
            return false;
        else
        {
            SelectedNode = node;
            return true;
        }
    }

    public void CopySelectedItemToClipboard(bool cut)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag || tag.Type is not DatapackItemType.File
            and not DatapackItemType.Directory and not DatapackItemType.Namespace)
        {
            return;
        }

        Clipboard.SetData("Aadev.MinecraftDatapackCreator.FileStructure.ClipboardItemInfo", new ClipboardItemInfo(tag, cut));
    }

    private bool CanPaste([NotNullWhen(true)] out ClipboardItemInfo clipboardItemInfo, DatapackStructureFolder? structureFolder)
    {
        if (!Clipboard.ContainsData("Aadev.MinecraftDatapackCreator.FileStructure.ClipboardItemInfo"))
        {
            clipboardItemInfo = default;
            return false;
        }


        object? obj = Clipboard.GetData("Aadev.MinecraftDatapackCreator.FileStructure.ClipboardItemInfo");
        if (obj is not ClipboardItemInfo cii)
        {
            clipboardItemInfo = default;
            return false;
        }

        if (!cii.IsValid(Solution!, structureFolder))
        {
            clipboardItemInfo = default;
            return false;
        }
        clipboardItemInfo = cii;
        return true;
    }

    public void PasteItemFormClipboardToSelectedItem()
    {
        //TODO: Add option to paste when file is selected
        if (Solution is null)
        {
            return;
        }
        DatapackDirectoryInfo? destinationDirectory = null;
        if (SelectedNode.Tag is DatapackDirectoryInfo ddi)
            destinationDirectory = ddi;
        else if (SelectedNode.Tag is DatapackFileInfo dfi)
            destinationDirectory = dfi.Parent;
        if (destinationDirectory is null)
            return;



        if (!CanPaste(out ClipboardItemInfo cii, destinationDirectory.DatapackStructureFolder))
            return;


        DatapackDirectoryInfo? parent = Solution.FileStructure.RootFolder.GetAbsoluteDirectory(Path.GetDirectoryName(cii.fullPath));

        if (parent is null)
        {
            return;
        }


        if (parent.GetRelativeDirectory(Path.GetFileName(cii.fullPath)) is DatapackDirectoryInfo originDirectory)
        {
            if (cii.cut)
            {
                focusOnChanged = originDirectory;
                DatapackFsOperationResult errorCode = originDirectory.MoveTo(destinationDirectory, originDirectory.Name);
                switch (errorCode)
                {
                    case DatapackFsOperationResult.Success:
                        Clipboard.Clear();
                        return;
                    case DatapackFsOperationResult.DestinationAlreadyExist:
                        MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExistInDestination, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case DatapackFsOperationResult.DestinationInsideSubject:
                        MessageBox.Show(this, "Cannot move directory inside itself", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    default:
                        Clipboard.Clear();
                        return;
                }
            }
            string name = $"{originDirectory.Name}_copy";

            if (destinationDirectory.GetRelativeDirectory(name) is not null)
            {
                int index = 1;
                Span<char> nameBuffer = stackalloc char[name.Length + 5]; // max 99999 dirs

                name.AsSpan().CopyTo(nameBuffer);
                nameBuffer[name.Length] = index.ToString(CultureInfo.InvariantCulture)[0];
                int indexLength = 1;
                while (destinationDirectory.GetRelativeDirectory(nameBuffer[..(name.Length + indexLength)]) is not null)
                {
                    index++;
                    indexLength = (int)(Math.Log10(index) + 1);
                    string stringIndex = index.ToString(CultureInfo.InvariantCulture);
                    stringIndex.CopyTo(nameBuffer[name.Length..]);
                }




                focusOnChanged = originDirectory;
                originDirectory.CopyTo(destinationDirectory, nameBuffer[..(name.Length + indexLength)]);
                return;
            }

            focusOnChanged = originDirectory;
            originDirectory.CopyTo(destinationDirectory, name);
        }
        else if (parent.GetRelativeFile(Path.GetFileName(cii.fullPath)) is DatapackFileInfo originFile)
        {
            if (cii.cut)
            {
                focusOnChanged = originFile;
                DatapackFsOperationResult errorCode = originFile.MoveTo(destinationDirectory, originFile.Name);
                switch (errorCode)
                {
                    case DatapackFsOperationResult.Success:
                        Clipboard.Clear();
                        return;
                    case DatapackFsOperationResult.DestinationAlreadyExist:
                        MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExistInDestination, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    default:
                        Clipboard.Clear();
                        return;
                }
            }

            string extension = Path.GetExtension(originFile.Name);
            string name = $"{Path.GetFileNameWithoutExtension(originFile.Name)}_copy{extension}";



            if (destinationDirectory.GetRelativeFile(name) is not null)
            {
                int index = 1;
                int indexLength = 1;
                Span<char> nameBuffer = stackalloc char[name.Length + 5]; // max 99999 files

                name.AsSpan().CopyTo(nameBuffer);
                extension.AsSpan().CopyTo(nameBuffer[(name.Length + 1 - extension.Length)..]);
                nameBuffer[name.Length - extension.Length] = index.ToString(CultureInfo.InvariantCulture)[0];


                while (destinationDirectory.GetRelativeFile(nameBuffer[..(name.Length + indexLength)]) is not null)
                {
                    if (index >= Math.Pow(10, indexLength))
                    {
                        indexLength++;
                        extension.AsSpan().CopyTo(nameBuffer[(name.Length + indexLength - extension.Length)..]);
                    }

                    index.TryFormat(nameBuffer[(name.Length - extension.Length)..], out int charWritten, provider: CultureInfo.InvariantCulture);
                    index++;
                }


                focusOnChanged = originFile;
                originFile.CopyTo(destinationDirectory, nameBuffer[..(name.Length + indexLength)]);
                return;
            }
            focusOnChanged = originFile;
            originFile.CopyTo(destinationDirectory, name);
        }
        else
            throw new UnreachableException();
    }
    public void AddNewFilePlaceholder(TreeNode parent)
    {
        if (Solution is null || parent.Tag is not ISolutionItemInfo tag)
        {
            return;
        }

        TreeNode tmpNode = new TreeNode(string.Format(CultureInfo.CurrentCulture, CompositeFormats.NewFilePlaceholder, tag.DatapackStructureFolder?.DisplayName.TrimEnd('s')))
        {
            Tag = new SolutionVirtualItemInfo(SolutionNodeType.File | SolutionNodeType.Creating, null, tag.DatapackStructureFolder),
            ImageIndex = 2,
            SelectedImageIndex = 2
        };

        parent.Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();
    }
    public void AddNewFolderPlaceholder(TreeNode parent)
    {
        if (Solution is null || parent.Tag is not ISolutionItemInfo tag)
        {
            return;
        }

        TreeNode tmpNode = new TreeNode(Properties.Resources.NewFolderPlaceholder)
        {
            Tag = new SolutionVirtualItemInfo(SolutionNodeType.Directory | SolutionNodeType.Creating, null, tag.DatapackStructureFolder),
            ImageIndex = 1,
            SelectedImageIndex = 1
        };

        parent.Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();
    }
    public void AddNewNamespacePlaceholder()
    {
        if (Solution is null)
        {
            return;
        }

        TreeNode tmpNode = new TreeNode(Properties.Resources.NewNamespacePlaceholder)
        {
            Tag = new SolutionVirtualItemInfo(SolutionNodeType.Namespace | SolutionNodeType.Creating, null, null),
            ImageIndex = 3,
            SelectedImageIndex = 3
        };

        Nodes[Solution.Name].Nodes[Datapack.DATA_FOLDER_NAME].Nodes.Add(tmpNode);

        SelectedNode = tmpNode;
        tmpNode.BeginEdit();
    }

    private TreeNode CreateNamespaceNode(DatapackDirectoryInfo namespaceInfo, TreeNode dataNode)
    {
        TreeNode node = dataNode.Nodes.Add(namespaceInfo.Name, namespaceInfo.Name, 3, 3);
        node.Tag = namespaceInfo;
        node.ContextMenuStrip = cmsNamespace;
        CreateDatapackStructure(node, Solution!.Sources.DatapackStructure, namespaceInfo);
        CreateDatapackFileStructure(node, namespaceInfo, true);
        return node;
    }
    private TreeNode CreateDirectoryNode(DatapackDirectoryInfo directoryInfo, TreeNode parent)
    {
        TreeNode node = parent.Nodes.Add(directoryInfo.Name, directoryInfo.Name, 1, 1);
        node.Tag = directoryInfo;
        node.ContextMenuStrip = cmsDirectory;
        CreateDatapackFileStructure(node, directoryInfo);
        return node;
    }
    private TreeNode CreateFileNode(DatapackFileInfo fileInfo, TreeNode parent)
    {
        int index = 2;
        if (fileInfo.Name.AsSpan().SequenceEqual(Datapack.PACK_MCMETA_FILE)) // Any meta file
            index = 7;
        if (fileInfo.IsNameInvalid)
            index = 6;
        TreeNode node = parent.Nodes.Add(fileInfo.Name, fileInfo.Name, index, index);
        node.Tag = fileInfo;
        node.ContextMenuStrip = cmsFile;
        return node;
    }
    private TreeNode CreateItemNode(IDatapackItemInfo itemInfo, TreeNode parent)
    {
        if (itemInfo.Type is DatapackItemType.File)
            return CreateFileNode((DatapackFileInfo)itemInfo, parent);
        else if (itemInfo.Type is DatapackItemType.Directory)
            return CreateDirectoryNode((DatapackDirectoryInfo)itemInfo, parent);
        else if (itemInfo.Type is DatapackItemType.Namespace)
            return CreateNamespaceNode((DatapackDirectoryInfo)itemInfo, parent);
        else
            throw new UnreachableException();
    }

    private void CreateDatapackStructure(TreeNode parent, DatapackStructureFoldersCollection? folders, string path)
    {
        if (folders is null)
            return;
        foreach (DatapackStructureFolder folder in folders)
        {
            TreeNode node = parent.Nodes.Add(folder.Name, folder.Name, 5, 5);
            string nodePath = Path.Join(path, folder.Name);

            node.Tag = new SolutionVirtualItemInfo(SolutionNodeType.StructureFolder, nodePath, folder);
            node.ToolTipText = $"{folder.DisplayName}\n{folder.Description}";
            node.ContextMenuStrip = cmsStructure;

            CreateDatapackStructure(node, folder.TryGetChildren(), nodePath);
        }
    }
    private void CreateDatapackStructure(TreeNode parent, DatapackStructureFoldersCollection? folders, DatapackDirectoryInfo parentDirectory)
    {
        if (folders is null)
            return;
        foreach (DatapackStructureFolder folder in folders)
        {
            DatapackDirectoryInfo? nodeDir = parentDirectory.GetRelativeDirectory(folder.Name);
            TreeNode node = parent.Nodes.Add(folder.Name, folder.Name, 5, 5);
            node.ToolTipText = $"{folder.DisplayName}\n{folder.Description}";
            node.ContextMenuStrip = cmsStructure;
            if (nodeDir is null)
            {
                string nodePath = Path.Join(parentDirectory.FullName, folder.Name);
                node.Tag = new SolutionVirtualItemInfo(SolutionNodeType.StructureFolder, nodePath, folder);
                CreateDatapackStructure(node, folder.TryGetChildren(), nodePath);
            }
            else
            {
                node.Tag = nodeDir;
                CreateDatapackStructure(node, folder.TryGetChildren(), nodeDir);
                CreateDatapackFileStructure(node, nodeDir);
            }
        }
    }
    private void CreateDatapackFileStructure(TreeNode parent, DatapackDirectoryInfo? parentDirectory, bool inNamespace = false)
    {
        if (parentDirectory is null)
            return;

        List<DatapackDirectoryInfo>? folders = parentDirectory.TryGetDirectories();
        if (folders is not null)
        {
            DatapackStructureFolder? folder = parentDirectory.DatapackStructureFolder;
            DatapackStructureFoldersCollection? children = inNamespace ? Solution?.Sources.DatapackStructure : folder?.TryGetChildren();
            bool checkChildren = children is not null;
            foreach (DatapackDirectoryInfo item in folders)
            {
                if (checkChildren)
                    if (children!.Any(y => y.Name == item.Name))
                        continue;
                CreateDirectoryNode(item, parent);
            }
        }

        List<DatapackFileInfo>? files = parentDirectory.TryGetFiles();
        if (files is not null)
            foreach (DatapackFileInfo item in files)
            {
                CreateFileNode(item, parent);
            }
    }

    private static TreeNode? GetNodeByPath(TreeNode? parent, ReadOnlySpan<char> path)
    {
        if (path.Length == 0)
        {
            return parent;
        }
        if (parent is null)
            return null;
        int indexOfPathSeparator = path.IndexOf('\\');
        if (indexOfPathSeparator == -1)
        {
            TreeNode? node = parent.GetNodeByKey(path);
            return node;
        }
        return GetNodeByPath(parent.GetNodeByKey(path[..indexOfPathSeparator]), path[(indexOfPathSeparator + 1)..]);
    }
    private bool OpenSelectedFile()
    {
        if (Solution is null || SelectedNode?.Tag is not DatapackFileInfo dfi)
        {
            return false;
        }

        FileOpened?.Invoke(this, dfi);
        return true;
    }
    private void ProcessLabelEdit(NodeLabelEditEventArgs e)
    {
        if (Solution is null || e.Node?.Tag is not ISolutionItemInfo tag || e.Node == underRemove)
            return;
        bool isCreating = (tag.SolutionNodeType & SolutionNodeType.Creating) == SolutionNodeType.Creating;
        if (string.IsNullOrWhiteSpace(e.Label))
        {
            Abort();
            return;
        }

        if ((tag.SolutionNodeType & SolutionNodeType.SolutionFolder) is SolutionNodeType.Namespace or SolutionNodeType.Directory)
        {
            ReadOnlySpan<char> name = e.Label.AsSpan().Trim();

            Span<char> fn = stackalloc char[name.Length];

            if (!Datapack.TryGetValidResourceName(name, fn))
            {
                Abort();
                MessageBox.Show(this, "Name has illegal characters", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (!isCreating)
            {
                if (tag.ItemInfo is null)
                {
                    throw new UnreachableException("Internal error: Directory without FolderInfo?");
                }
                DatapackFsOperationResult errorCode = tag.ItemInfo.Rename(fn);

                focusOnChanged = tag.ItemInfo;
                switch (errorCode)
                {
                    case DatapackFsOperationResult.Success:
                        return;
                    case DatapackFsOperationResult.DestinationAlreadyExist:
                        MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExist, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                        return;
                    default:
                        throw new UnreachableException();
                }
            }

            TreeNode? parentNode = e.Node.Parent ?? throw new UnreachableException();
            if (parentNode.Tag is not ISolutionItemInfo pTag)
            {
                throw new UnreachableException();
            }


            if (pTag.ItemInfo is not DatapackDirectoryInfo folder)
            {
                folder = Solution.FileStructure.RootFolder.CreateAbsoluteDirectory(pTag.FullName);
                if (folder is null)
                    throw new UnreachableException();
            }

            if (folder.GetRelativeDirectory(fn) != null)
            {
                Abort();
                MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExist, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SelectedNode = e.Node.Parent;
            var newlyCreated = folder.CreateRelativeDirectory(fn, false);
            focusOnChanged = newlyCreated;
            focusOnChanged.EnsureExist();
            underRemove = e.Node;
            e.Node.Remove();
        }
        else if ((tag.SolutionNodeType & SolutionNodeType.SolutionFolder) is SolutionNodeType.File)
        {
            ReadOnlySpan<char> filename = e.Label.AsSpan().Trim();

            if (filename.Length is 0)
            {
                Abort();
                return;
            }

            ReadOnlySpan<char> extension = tag.DatapackStructureFolder?.FilesExtension ?? "txt";
            if (!Helpers.HasFileExtension(filename, extension))
            {
                filename = string.Concat(filename, ".", extension);
            }

            Span<char> fn = stackalloc char[filename.Length];

            if (!Datapack.TryGetValidResourceName(filename, fn))
            {
                Abort();
                MessageBox.Show(this, "Name has illegal characters", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!isCreating)
            {
                if (tag.ItemInfo is null)
                {
                    throw new UnreachableException("Internal error: File without FileInfo?");
                }
                DatapackFsOperationResult errorCode = tag.ItemInfo.Rename(fn);

                focusOnChanged = tag.ItemInfo;
                switch (errorCode)
                {
                    case DatapackFsOperationResult.Success:
                        return;
                    case DatapackFsOperationResult.DestinationAlreadyExist:
                        MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExist, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                        return;
                    default:
                        throw new UnreachableException();
                }

            }

            TreeNode? parentNode = e.Node.Parent ?? throw new UnreachableException();
            if (parentNode.Tag is not ISolutionItemInfo pTag)
            {
                throw new UnreachableException();
            }
            if (pTag.ItemInfo is not DatapackDirectoryInfo folder)
            {
                folder = Solution.FileStructure.RootFolder.CreateAbsoluteDirectory(pTag.FullName);
                if (folder is null)
                    throw new UnreachableException();
            }
            if (folder.GetRelativeFile(fn) != null)
            {
                Abort();
                MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExist, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SelectedNode = e.Node.Parent;
            DatapackFileInfo fileInfo = folder.CreateRelativeFile(fn, false);
            focusOnChanged = fileInfo;
            fileInfo.EnsureExist();
            FileOpened?.Invoke(this, fileInfo);
            underRemove = e.Node;
            e.Node.Remove();
        }
        void Abort()
        {
            e.CancelEdit = true;
            if (isCreating)
            {
                SelectedNode = e.Node!.Parent;
                underRemove = e.Node;
                e.Node.Remove();
            }
        }
    }

    private void LoadSolution()
    {
        if (Solution is null)
        {
            Nodes.Clear();
            return;
        }
        BeginUpdate();
        Nodes.Clear();
        TreeNode rootNode = Nodes.Add(Solution.Name, Solution.Name, 4, 4);
        rootNode.Tag = Solution.FileStructure.RootFolder;
        rootNode.ContextMenuStrip = cmsSolution;




        TreeNode packmemeta = rootNode.Nodes.Add(Datapack.PACK_MCMETA_FILE, Datapack.PACK_MCMETA_FILE, 7, 7);
        DatapackFileInfo? packFile = Solution.FileStructure.RootFolder.GetRelativeFile(Datapack.PACK_MCMETA_FILE);
        if (packFile is not null)
            packmemeta.Tag = packFile;
        else
            packmemeta.Tag = new SolutionVirtualItemInfo(SolutionNodeType.MetaFile, Path.Join(Solution.Path, Datapack.PACK_MCMETA_FILE), null);


        TreeNode dataNode = rootNode.Nodes.Add(Datapack.DATA_FOLDER_NAME, "data", 5, 5);
        DatapackDirectoryInfo? datadir = Solution.FileStructure.RootFolder.GetRelativeDirectory(Datapack.DATA_FOLDER_NAME);
        if (datadir is not null)
            dataNode.Tag = datadir;
        else
            dataNode.Tag = new SolutionVirtualItemInfo(SolutionNodeType.DataFolder, Solution.DataFolderPath, null);
        rootNode.Expand();

        List<DatapackDirectoryInfo>? namespaces = Solution.TryGetNamespaces();
        if (namespaces is not null)
        {
            for (int i = 0; i < namespaces.Count; i++)
            {
                CreateNamespaceNode(namespaces[i], dataNode);
            }
        }



        EndUpdate();
        Sort();
        Solution.FileStructure.ItemCreated -= FileStructure_ItemCreated;
        Solution.FileStructure.ItemDelated -= FileStructure_ItemDelated;
        Solution.FileStructure.ItemRenamed -= FileStructure_ItemRenamed;
        Solution.FileStructure.ItemCreated += FileStructure_ItemCreated;
        Solution.FileStructure.ItemDelated += FileStructure_ItemDelated;
        Solution.FileStructure.ItemRenamed += FileStructure_ItemRenamed;

    }

    private void FileStructure_ItemRenamed(object? sender, DatapackItemRenamedEventArgs e)
    {
        if (Solution is null)
            return;
        if (e.Subject.PathRelativeToSolution.SequenceEqual(Datapack.DATA_FOLDER_NAME))
            return;

        TreeNode? parentNode = GetNodeByPath(Nodes[Solution.Name], Path.GetDirectoryName(e.Subject.PathRelativeToSolution)) ?? throw new UnreachableException("Internal Error: parent node is null");

        Invoke(() =>
        {
            SuspendLayout();
            BeginUpdate();
            Point spos = ScrollPosition;
            TreeNode? node = parentNode.Nodes[Path.GetFileName(e.OldPath)];
            if (node is null)
            {
                CreateItemNode(e.Subject, parentNode);
            }
            else
            {
                int index = 2;
                if (e.Subject.Name.AsSpan().SequenceEqual(Datapack.PACK_MCMETA_FILE)) // Any meta file
                    index = 7;
                if (e.Subject.IsNameInvalid)
                    index = 6;
                node.Name = e.Subject.Name;
                node.Text = e.Subject.Name;
                node.ImageIndex = index;
                node.SelectedImageIndex = index;
            }
            Sort();
            ScrollPosition = spos;
            if (focusOnChanged == e.Subject)
            {
                SelectedNode = node;
            }
            EndUpdate();
            ResumeLayout();
        });
    }
    private void FileStructure_ItemDelated(object? sender, DatapackItemChangedEventArgs e)
    {
        if (Solution is null)
            return;
        if (e.Subject.PathRelativeToSolution.SequenceEqual(Datapack.DATA_FOLDER_NAME))
            return;

        TreeNode parentNode = GetNodeByPath(Nodes[Solution.Name], Path.GetDirectoryName(e.Subject.PathRelativeToSolution)) ?? throw new UnreachableException("Internal Error: parent node is null");
        Invoke(() =>
        {
            SuspendLayout();
            BeginUpdate();
            Point spos = ScrollPosition;
            int indexOf = parentNode.Nodes.IndexOfKey(e.Subject.Name);
            if (indexOf != -1)
                parentNode?.Nodes.RemoveAt(indexOf);

            Sort();
            ScrollPosition = spos;
            SelectedNode = parentNode;
            EndUpdate();
            ResumeLayout(false);
        });
    }
    private void FileStructure_ItemCreated(object? sender, DatapackItemChangedEventArgs e)
    {
        if (Solution is null)
            return;
        if (e.Subject.Type is DatapackItemType.MetaFile)
        {
            Nodes[Solution.Name].Nodes[Datapack.DATA_FOLDER_NAME].Tag = e.Subject;
            return;
        }


        TreeNode parentNode = GetNodeByPath(Nodes[Solution.Name], Path.GetDirectoryName(e.Subject.PathRelativeToSolution)) ?? throw new UnreachableException("Internal Error: parent node is null");
        bool isDir = e.Subject is DatapackDirectoryInfo;

        if (isDir)
        {
            DatapackStructureFoldersCollection? children = e.Subject.Parent?.Type == DatapackItemType.Namespace ? Solution.Sources.DatapackStructure : e.Subject.Parent?.DatapackStructureFolder?.TryGetChildren();
            if (children is not null)
            {
                foreach (DatapackStructureFolder item in children)
                {
                    if (item.Name == e.Subject.Name)
                    {
                        TreeNode? structureNode = parentNode.GetNodeByKey(item.Name);
                        if (structureNode is null)
                            return;
                        structureNode.Tag = e.Subject;
                        return;
                    }
                }

            }
        }

        Invoke(() =>
        {
            SuspendLayout();
            BeginUpdate();
            if (parentNode.Nodes.ContainsKey(e.Subject.Name))
            {
                EndUpdate();
                return;
            }


            Point spos = ScrollPosition;
            TreeNode node;
            if (e.Subject.Type is DatapackItemType.Namespace)
                node = CreateNamespaceNode((DatapackDirectoryInfo)e.Subject, parentNode);
            else if (e.Subject.Type is DatapackItemType.Directory)
                node = CreateDirectoryNode((DatapackDirectoryInfo)e.Subject, parentNode);
            else if (e.Subject.Type is DatapackItemType.File)
                node = CreateFileNode((DatapackFileInfo)e.Subject, parentNode);
            else
                throw new UnreachableException();
            Sort();
            ScrollPosition = spos;
            if (focusOnChanged == e.Subject)
            {
                SelectedNode = node;
            }
            EndUpdate();
            ResumeLayout(false);
        });
    }


    protected override void OnBeforeCollapse(TreeViewCancelEventArgs e)
    {
        if (e.Node?.Tag is ISolutionItemInfo sii)
        {
            if (sii.SolutionNodeType is SolutionNodeType.SolutionFolder)
            {
                e.Cancel = true;
            }
        }
        base.OnBeforeCollapse(e);
    }

    protected override void OnItemDrag(ItemDragEventArgs e)
    {
        if (e.Button is not MouseButtons.Left)
            return;
        if (e.Item is not TreeNode treeNode) return;
        if (treeNode.Tag is not ISolutionItemInfo tag)
            return;
        if ((tag.SolutionNodeType & SolutionNodeType.Creating) == SolutionNodeType.Creating)
            return;
        if ((tag.SolutionNodeType & SolutionNodeType.SolutionFolder) is not SolutionNodeType.File and not SolutionNodeType.Directory)
            return;


        DoDragDrop(e.Item, DragDropEffects.Move);

        base.OnItemDrag(e);
    }
    protected override void OnDragDrop(DragEventArgs drgevent)
    {
        if (Solution is null)
            return;
        object? item = drgevent.Data?.GetData(typeof(TreeNode));
        if (item is not TreeNode treeNode)
            return;
        if (treeNode.Tag is not IDatapackItemInfo tag)
            return;

        Point targetPoint = PointToClient(new Point(drgevent.X, drgevent.Y));

        TreeNode overNode = GetNodeAt(targetPoint);
        if (overNode?.Tag is not ISolutionItemInfo sTag)
            return;
        if (sTag.DatapackStructureFolder != tag.DatapackStructureFolder)
        {
            drgevent.Effect = DragDropEffects.None;
            return;
        }

        if ((sTag.SolutionNodeType is not SolutionNodeType.Directory && overNode.Parent == treeNode.Parent) || overNode == treeNode.Parent || overNode == treeNode)
        {
            MessageBox.Show(this, Properties.Resources.DialogMoveToSameLocation, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if (sTag.SolutionNodeType is SolutionNodeType.SolutionFolder)
            return;
        if (sTag.SolutionNodeType is not SolutionNodeType.Directory and not SolutionNodeType.StructureFolder and not SolutionNodeType.Namespace)
        {
            if (overNode.Parent?.Tag is not ISolutionItemInfo newTag)
                return;
            sTag = newTag;
        }


        if (sTag.ItemInfo is not DatapackDirectoryInfo folder)
        {
            folder = Solution.FileStructure.RootFolder.CreateAbsoluteDirectory(sTag.FullName);
        }

        focusOnChanged = tag;

        DatapackFsOperationResult errorCode = tag.MoveTo(folder);
        switch (errorCode)
        {
            case DatapackFsOperationResult.Success:
                return;
            case DatapackFsOperationResult.DestinationAlreadyExist:
                MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExistInDestination, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            case DatapackFsOperationResult.DestinationInsideSubject:
                MessageBox.Show(this, "Cannot move directory inside itself", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
        }
    }
    protected override void OnDragOver(DragEventArgs drgevent)
    {
        object? item = drgevent.Data?.GetData(typeof(TreeNode));
        if (item is not TreeNode treeNode) return;
        if (treeNode.Tag is not ISolutionItemInfo tag)
            return;

        Point targetPoint = PointToClient(new Point(drgevent.X, drgevent.Y));

        TreeNode overNode = GetNodeAt(targetPoint);

        if (overNode?.Tag is not ISolutionItemInfo sTag)
            return;
        SelectedNode = overNode;


        if (sTag.DatapackStructureFolder != tag.DatapackStructureFolder || sTag.SolutionNodeType is SolutionNodeType.SolutionFolder)
        {
            drgevent.Effect = DragDropEffects.None;
        }
        else
            drgevent.Effect = DragDropEffects.Move;
        base.OnDragOver(drgevent);
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

        if (Solution is null || e.Node?.Tag is not DatapackFileInfo { Type: DatapackItemType.File } tag)
        {
            return;
        }

        FileSelected?.Invoke(this, tag);

    }
    protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
    {
        Debug.WriteLine(e.KeyCode);
        base.OnPreviewKeyDown(e);
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
            e.SuppressKeyPress = OpenSelectedFile();
        }
        if (e.Control && e.KeyCode is Keys.C)
        {
            if (SelectedNode?.Tag is not IDatapackItemInfo tag)
                return;

            if (tag.Type is DatapackItemType.File or DatapackItemType.Directory or DatapackItemType.Namespace)
            {
                e.SuppressKeyPress = true;
                CopySelectedItemToClipboard(false);
            }

        }
        if (e.Control && e.KeyCode is Keys.X)
        {
            if (SelectedNode?.Tag is not IDatapackItemInfo tag)
                return;

            if (tag.Type is DatapackItemType.File or DatapackItemType.Directory or DatapackItemType.Namespace)
            {
                e.SuppressKeyPress = true;
                CopySelectedItemToClipboard(true);
            }

        }
        if (e.Control && e.KeyCode is Keys.V)
        {
            if (SelectedNode?.Tag is not IDatapackItemInfo tag)
                return;

            if (tag.Type is DatapackItemType.File or DatapackItemType.Directory or DatapackItemType.Namespace or DatapackItemType.DataDirectory or DatapackItemType.StructureFolder)
            {
                e.SuppressKeyPress = true;
                PasteItemFormClipboardToSelectedItem();
            }

        }
        if (e.KeyCode is Keys.F2)
        {
            SelectedNode?.BeginEdit();
        }
        base.OnKeyDown(e);
    }
    protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
    {
        base.OnNodeMouseDoubleClick(e);

        if (e.Button != MouseButtons.Left)
        {
            return;
        }
        OpenSelectedFile();
    }

    protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
    {
        base.OnBeforeLabelEdit(e);
        if (e.Node?.Tag is not ISolutionItemInfo tag)
        {
            e.CancelEdit = true;
            return;
        }


        if (tag.SolutionNodeType is not SolutionNodeType.File and not SolutionNodeType.Directory and not SolutionNodeType.Namespace && (tag.SolutionNodeType & SolutionNodeType.Creating) == 0)
        {
            e.CancelEdit = true;
            return;
        }

    }
    protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
    {
        base.OnAfterLabelEdit(e);
        if (Solution is null)
            return;

        BeginUpdate();

        ProcessLabelEdit(e);

        EndUpdate();
    }

    private void TsmiSlnAddNamespace_Click(object? sender, EventArgs e) => AddNewNamespacePlaceholder();
    private void TsmiStrAddFile_Click(object? sender, EventArgs e) => AddNewFilePlaceholder(SelectedNode);
    private void TsmiStrAddFolder_Click(object? sender, EventArgs e) => AddNewFolderPlaceholder(SelectedNode);
    private void TsmiNsRenameNamespace_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiFileRename_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiDirRename_Click(object? sender, EventArgs e) => SelectedNode.BeginEdit();
    private void TsmiNsDelateNamespace_Click(object? sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag)
        {
            return;
        }

        DialogResult dr = MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, CompositeFormats.DialogNamespaceDeleteQuestion, tag.Name), Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        if (dr is not DialogResult.Yes)
        {
            return;
        }
        SelectedNode.Collapse(false);
        Directory.Delete(tag.FullName, true);

    }
    private void TsmiFileDelate_Click(object? sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag)
        {
            return;
        }

        if (MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, CompositeFormats.DialogFileDeleteQuestion, controller.Settings.AlwaysShowFullFilePathInDialogs ? tag.FullName : tag.PathRelativeToSolution.ToString()), Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) is not DialogResult.Yes)
        {
            return;
        }

        File.Delete(tag.FullName);
    }
    private void TsmiFileCopyNamespacedId_Click(object? sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not DatapackFileInfo tag)
        {
            return;
        }

        Clipboard.SetDataObject(tag.NamespacedId!);
    }
    private void TsmiDirDelate_Click(object? sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag)
        {
            return;
        }

        if (MessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, CompositeFormats.DialogDirectoryDeleteQuestion, controller.Settings.AlwaysShowFullFilePathInDialogs ? tag.FullName : tag.PathRelativeToSolution.ToString()), Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) is not DialogResult.Yes)
        {
            return;
        }
        Directory.Delete(tag.FullName, true);
    }
    private void TsmiSlnOpenInExplorer_Click(object sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag)
        {
            return;
        }

        Process.Start("explorer.exe", tag.FullName);

    }
    private void TsmiFileShowInExplorer_Click(object sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag)
        {
            return;
        }


        Process.Start("explorer.exe", $"/select, {tag.FullName}");
    }
    private void TsmiFileCopyPath_Click(object sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag)
        {
            return;
        }

        Clipboard.SetDataObject(tag.FullName);
    }
    private void TsmiFileCopyRelativePath_Click(object sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not IDatapackItemInfo tag)
        {
            return;
        }

        Clipboard.SetDataObject(tag.PathRelativeToSolution.ToString());
    }
    private void TsmiStrAddOverride_Click(object sender, EventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not ISolutionItemInfo tag)
        {
            return;
        }

        using OverrideMinecraftFileForm omff = new OverrideMinecraftFileForm(Solution.Sources.MinecraftStructure, tag);
        if (omff.ShowDialog() == DialogResult.OK)
        {
            string? item = omff.SelectedItem;
            if (item is null)
                return;
            if (tag.ItemInfo is null)
            {
                DatapackDirectoryInfo folder = Solution.FileStructure.RootFolder.CreateAbsoluteDirectory(tag.FullName);
                DatapackFileInfo fileInfo = folder.CreateRelativeFile(item, false);
                focusOnChanged = fileInfo;
                fileInfo.EnsureExist();
                FileOpened?.Invoke(this, fileInfo);
            }
            else
            {
                if (tag is DatapackDirectoryInfo dfi)
                {
                    DatapackFileInfo fileInfo = dfi.CreateRelativeFile(item, false);
                    focusOnChanged = fileInfo;
                    fileInfo.EnsureExist();
                    FileOpened?.Invoke(this, fileInfo);
                }
                else
                    throw new UnreachableException();
            }
        }
    }
    private void TsmiFileRepair_Click(object sender, EventArgs e)
    {
        if (SelectedNode.Tag is not IDatapackItemInfo tag)
            return;


        int oldNameLength = tag.Name.Length;
        string? extension = tag.DatapackStructureFolder?.FilesExtension;

        int newLength = Helpers.HasFileExtension(tag.Name, extension) ? oldNameLength : oldNameLength + extension!.Length + 1;

        Span<char> newName = stackalloc char[newLength];

        if (!Datapack.TryGetValidResourceName(tag.Name, newName[..oldNameLength]))
        {
            MessageBox.Show(this, $"Cannot repair file name.", Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            return;
        }

        if (extension?.Length > 0)
        {
            newName[oldNameLength] = '.';
            extension.CopyTo(newName[(oldNameLength + 1)..]);
        }

        DialogResult dr = MessageBox.Show(this, $"Change name to: {newName}", Program.ProductTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        if (dr is DialogResult.Yes)
        {
            DatapackFsOperationResult errorCode = tag.Rename(newName);

            focusOnChanged = tag.ItemInfo;
            switch (errorCode)
            {
                case DatapackFsOperationResult.Success:
                    return;
                case DatapackFsOperationResult.DestinationAlreadyExist:
                    MessageBox.Show(this, Properties.Resources.DialogFileAllreadyExist, Program.ProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                default:
                    throw new UnreachableException();
            }
        }
    }
    private void CmsStructure_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (Solution is null || SelectedNode.Tag is not ISolutionItemInfo tag)
        {
            return;
        }

        bool firstBlockVisible = tag.DatapackStructureFolder?.AllowFilesAndDirectories is true;
        bool secondBlockVisible = CanPaste(out ClipboardItemInfo _, tag.DatapackStructureFolder);
        bool thirdBlockVisible = tag is not SolutionVirtualItemInfo;


        toolStripSeparator4.Visible = firstBlockVisible && (secondBlockVisible || thirdBlockVisible);
        toolStripSeparator8.Visible = secondBlockVisible && thirdBlockVisible;



        if (firstBlockVisible)
        {
            DatapackDirectoryInfo? ns = Solution.FileStructure.RootFolder.GetAbsoluteDirectory(tag.FullName.AsSpan(0, tag.FullName!.Length - tag.DatapackStructureFolder!.Path.Length));


            if (ns?.Name == "minecraft")
            {
                tsmiStrAddOverride.Visible = true;
            }
            else
            {
                tsmiStrAddOverride.Visible = false;
            }
            tsmiStrAddFile.Visible = true;
            tsmiStrAddFolder.Visible = true;
        }
        else
        {
            tsmiStrAddFile.Visible = false;
            tsmiStrAddFolder.Visible = false;
            tsmiStrAddOverride.Visible = false;
        }
        tsmiStrPaste.Visible = secondBlockVisible;
        tsmiStrOpenInExplorer.Visible = thirdBlockVisible;

        e.Cancel = !(firstBlockVisible || secondBlockVisible || thirdBlockVisible);
    }
    private void CmsDirectory_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        tsmiDirPaste.Visible = CanPaste(out ClipboardItemInfo _, (SelectedNode.Tag as ISolutionItemInfo)?.DatapackStructureFolder);
    }
    private void CmsFile_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (SelectedNode.Tag is not IDatapackItemInfo tag)
            return;
        tsmiFileRepair.Visible = tag.IsNameInvalid;
        toolStripSeparator9.Visible = tag.IsNameInvalid;
    }
    private void TsmiFileCopy_Click(object sender, EventArgs e) => CopySelectedItemToClipboard(false);
    private void TsmiFileCut_Click(object sender, EventArgs e) => CopySelectedItemToClipboard(true);
    private void TsmiDirPaste_Click(object sender, EventArgs e) => PasteItemFormClipboardToSelectedItem();
    private void TsmiStrPaste_Click(object sender, EventArgs e) => PasteItemFormClipboardToSelectedItem();
    private void TsmiDirCopy_Click(object sender, EventArgs e) => CopySelectedItemToClipboard(false);
    private void TsmiDirCut_Click(object sender, EventArgs e) => CopySelectedItemToClipboard(true);
}