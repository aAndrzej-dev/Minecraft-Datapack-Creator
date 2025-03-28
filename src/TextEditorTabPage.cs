﻿using MinecraftDatapackCreator.FileStructure;
using System.IO;
using System.Reflection;
using static MinecraftDatapackCreator.PInvoke;
using System.ComponentModel;

namespace MinecraftDatapackCreator;

internal sealed partial class TextEditorTabPage : EditorTabPage
{
    private readonly AdvancedTextBox editor;
    private readonly bool readOnly;
    private bool isNotSaved;

    public override event EventHandler? SavedStateChanged;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool IsNotSaved { get => isNotSaved; protected set { if (isNotSaved == value) return; isNotSaved = value; SavedStateChanged?.Invoke(this, EventArgs.Empty); } }

    public override bool ReadOnly => readOnly;

    public override bool CanUndo => editor.CanUndo;
    public override bool CanRedo => editor.CanRedo;

    public TextEditorTabPage(Controller controller, DatapackFileInfo fileInfo) : base(controller, fileInfo)
    {
        FileInfo fi = new FileInfo(fileInfo.FullName);

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

        editor = new AdvancedTextBox()
        {
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(50, 50, 50),
            ForeColor = Color.White,
            Padding = new Padding(20),
            Dock = DockStyle.Fill,
            Font = controller.Settings.TextEditorFont,
            ContextMenuStrip = cms,
            WordWrap = false,
            DetectUrls = false,
            ReadOnly = readOnly,
            HideSelection = false,
        };
        editor.LoadFile(fileInfo.FullName, RichTextBoxStreamType.PlainText);
        editor.MouseDown += (sender, e) =>
            {
                editor.SelectionStart = editor.GetCharIndexFromPosition(e.Location);
                editor.SelectionLength = 0;
            };

        Text = $"{fileInfo.Name.SetStringLengthMiddle(25)}{(readOnly ? " (ReadOnly)" : "")}";
        SetPadding(editor, 10, 10, 10, 10);

        editor.TextChanged += (s, ev) => IsNotSaved = true;
        Controls.Add(editor);

    }
    public override void Save()
    {
        if (readOnly)
            return;
        suspendChangeEvent = true;
        File.WriteAllText(FileInfo.FullName, editor.Text);

        IsNotSaved = false;
        suspendChangeEvent = false;
    }

    private static void SetPadding(TextBoxBase textbox, int left, int top, int right, int bottom)
    {
        Rect rect = new();
        _ = SendMessage(textbox.Handle, 0xB2, 0, ref rect);
        rect = new(left, top, rect.width - left - right, rect.height - top - bottom);
        _ = SendMessage(textbox.Handle, 0xB3, 0, ref rect);
    }




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
        if (disposing)
        {
            editor.Dispose();
        }
        base.Dispose(disposing);
    }

    public override void Reload(bool askToSave)
    {
        editor.LoadFile(FileInfo.FullName, RichTextBoxStreamType.PlainText);
        IsNotSaved = false;
    }

    internal static EditorTabPage Create(Controller controller, DatapackFileInfo fileInfo) => new TextEditorTabPage(controller, fileInfo);
}
