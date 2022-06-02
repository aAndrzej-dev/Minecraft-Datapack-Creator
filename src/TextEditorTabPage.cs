using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MinecraftDatapackCreator;

internal class TextEditorTabPage : TabPage, ITabPage
{
    public string Filename { get; }
    private readonly RichTextBox editor;
    private bool isNotSaved;

    public event EventHandler? SavedStateChanged;

    public bool IsNotSaved { get => isNotSaved; private set { isNotSaved = value; SavedStateChanged?.Invoke(this, EventArgs.Empty); } }
    public TextEditorTabPage(string filename)
    {
        Filename = filename;
        editor = new()
        {
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(50, 50, 50),
            ForeColor = Color.White,
            Padding = new Padding(20),
            Dock = DockStyle.Fill,
            Text = File.ReadAllText(filename),
            Font = new Font(new FontFamily("Cascadia Mono"), Font.Size)
        };
        Text = Path.GetFileName(Filename);

        SetPadding(editor, 10, 10, 10, 10);

        editor.TextChanged += (s, ev) => IsNotSaved = true;
        Controls.Add(editor);

    }

    public void Save()
    {
        File.WriteAllText(Filename, editor.Text);

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
}
