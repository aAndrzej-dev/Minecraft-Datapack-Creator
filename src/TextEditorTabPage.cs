using System.IO;

namespace MinecraftDatapackCreator;

internal sealed class TextEditorTabPage : EditorTabPage
{
    private readonly AdvancedTextBox editor;
    private readonly bool readOnly;
    private bool isNotSaved;

    public override event EventHandler? SavedStateChanged;

    public override bool IsNotSaved { get => isNotSaved; protected set { isNotSaved = value; SavedStateChanged?.Invoke(this, EventArgs.Empty); } }

    public override bool ReadOnly => readOnly;

    public override bool CanUndo => editor.CanUndo;
    public override bool CanRedo => editor.CanRedo;

    public TextEditorTabPage(DatapackFileInfo fileInfo, Settings settings) : base(fileInfo)
    {
        System.IO.FileInfo fi = new FileInfo(fileInfo.FullName);

        readOnly = fi.IsReadOnly;
        ContextMenuStrip cms = new ContextMenuStrip();
        ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut") { ShortcutKeys = Keys.Control | Keys.X };
        tsmiCut.Click += (sender, e) => Cut();
        ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy") { ShortcutKeys = Keys.Control | Keys.C };
        tsmiCopy.Click += (sender, e) => Copy();
        ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste") { ShortcutKeys = Keys.Control | Keys.V };
        tsmiPaste.Click += (sender, e) => Paste();

        cms.Items.Add(tsmiCut);
        cms.Items.Add(tsmiCopy);
        cms.Items.Add(tsmiPaste);

        editor = new()
        {
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(50, 50, 50),
            ForeColor = Color.White,
            Padding = new Padding(20),
            Dock = DockStyle.Fill,
            Text = File.ReadAllText(fileInfo.FullName),
            Font = settings.TextEditorFont,
            ContextMenuStrip = cms,
            WordWrap = false,
            DetectUrls = false,
            ReadOnly = readOnly,
            HideSelection = false,
        };
        editor.MouseDown += (sender, e) =>
            {
                editor.SelectionStart = editor.GetCharIndexFromPosition(e.Location);
                editor.SelectionLength = 0;
            };

        Text = $"{fileInfo.Name.SetStringLenghtMiddle(25)}{(readOnly ? " (ReadOnly)" : "")}";
        SetPadding(editor, 10, 10, 10, 10);

        editor.TextChanged += (s, ev) => IsNotSaved = true;
        Controls.Add(editor);

    }
    public override void Save()
    {
        if (readOnly)
            return;
        File.WriteAllText(FileInfo.FullName, editor.Text);

        IsNotSaved = false;
    }

    private static void SetPadding(TextBoxBase textbox, int left, int top, int right, int bottom)
    {
        Rectangle rect = new();
        _ = SendMessage(textbox.Handle, 0xB2, 0, ref rect);
        rect = new(left, top, rect.Width - left - right, rect.Height - top - bottom);
        _ = SendMessage(textbox.Handle, 0xB3, 0, ref rect);
    }



    [System.Runtime.InteropServices.DllImport(@"user32.dll")]
    private static extern int SendMessage(nint hwnd, int wMsg, nint wParam, ref Rectangle lParam);
    public override void Undo() => editor.Undo();
    public override void Redo() => editor.Redo();

    public void Paste() => editor.Paste();
    public void Copy()
    {
        if (editor.SelectionLength > 0)
        {
            editor.Copy();
        }
        else
        {
            int currentIndex = editor.SelectionStart;
            int startIndex = editor.GetFirstCharIndexOfCurrentLine();
            int endIndex = editor.Text.IndexOf('\n', startIndex);
            if (endIndex == -1)
                endIndex = editor.TextLength;
            editor.SelectionStart = startIndex;
            editor.SelectionLength = endIndex - startIndex;
            editor.Copy();
            editor.SelectionStart = currentIndex;
            editor.SelectionLength = 0;
        }
    }
    public void Cut() => editor.Cut();
    public void SelectAll() => editor.SelectAll();

    protected override void Dispose(bool disposing)
    {
        if(disposing)
        {
            editor.Dispose();
        }
        base.Dispose(disposing);
    }
}
