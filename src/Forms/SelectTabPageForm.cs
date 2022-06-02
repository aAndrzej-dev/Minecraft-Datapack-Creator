namespace MinecraftDatapackCreator.Forms;

internal partial class SelectTabPageForm : Form
{
    private TabPage[] TabPages { get; }
    public int SelectedIndex { get; set; }
    private readonly Form parent;
    public SelectTabPageForm(TabPage[] tabPages, int selectedIndex, Form parent)
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

        for (int i = 0; i < TabPages.Length; i++)
        {
            g.FillRectangle(new SolidBrush(SelectedIndex == i ? Color.RoyalBlue : BackColor), x, y, 200, 20);
            g.DrawString(TabPages[i].Text, Font, new SolidBrush(ForeColor), x + 8, y + 10 - Font.Height / 2);
            y += 20;

            if (i % 15 == 0 && i != 0)
            {

                my = y;
                y = 16;
                x = 232;
            }
        }
        if (my == 0)
        {
            my = y;
        }

        Height = my + 16;
        Width = x + 216;


        int locX = parent.Location.X + (parent.Width / 2 - Width / 2);
        int locY = parent.Location.Y + 64;

        Location = new Point(locX, locY);
    }
}
