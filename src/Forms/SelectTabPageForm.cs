namespace MinecraftDatapackCreator.Forms;

internal partial class SelectTabPageForm : Form
{
    private EditorTabPage[] TabPages { get; }
    public int SelectedIndex { get; set; }
    private readonly Form parent;
    public SelectTabPageForm(EditorTabPage[] tabPages, int selectedIndex, Form parent)
    {
        this.parent = parent;
        TabPages = tabPages ?? throw new ArgumentNullException(nameof(tabPages));
        SelectedIndex = (selectedIndex + 1) % TabPages.Length;
        InitializeComponent();
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }

    private void SelectTabPageForm_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
    {
        if (!e.Control)
        {
            Close();
        }
    }

    private void SelectTabPageForm_KeyUp(object? sender, KeyEventArgs e)
    {
        if (!e.Control)
        {
            Close();
        }
    }

    private void SelectTabPageForm_KeyDown(object? sender, KeyEventArgs e)
    {
        SelectedIndex = (SelectedIndex + 1) % TabPages.Length;
        Invalidate();
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        int y = 16;
        int x = 16;
        int my = 0;

        int w = 0;

        for (int i = 0; i < TabPages.Length; i++)
        {
            SizeF size = g.MeasureString(TabPages[i].Text, Font);

            w = (int)MathF.Max(size.Width, w);
        }
        for (int i = 0; i < TabPages.Length; i++)
        {
            if (i % 15 == 0 && i != 0)
            {
                my = y;
                y = 16;
                x += w + 18;
            }

            using SolidBrush backBrush = new SolidBrush(SelectedIndex == i ? Color.RoyalBlue : BackColor);
            using SolidBrush accentBrush = new SolidBrush(TabPages[i].TabBackColor);
            using SolidBrush foreBrush = new SolidBrush(ForeColor);

            g.FillRectangle(backBrush, x, y, w + 16, 20);

            g.FillRectangle(accentBrush, x, y, 4, 20);
            g.DrawString(TabPages[i].Text, base.Font, foreBrush, x + 10, y + 10 - base.Font.Height / 2);
            y += 20;


        }
        if (my == 0)
        {
            my = y;
        }

        Height = my + 16;
        Width = x + w + 32;


        int locX = parent.Location.X + (parent.Width / 2 - Width / 2);
        int locY = parent.Location.Y + 64;

        Location = new Point(locX, locY);
    }
}
