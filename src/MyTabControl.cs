namespace MinecraftDatapackCreator;

internal class MyTabControl : TabControl
{
    private Color backColor = DefaultBackColor;
    private Color inActiveTabBackColor;
    private Color inActiveTabForeColor;
    private Color divider;
    private int dividerSize;


    public MyTabControl()
    {
        DrawMode = TabDrawMode.OwnerDrawFixed;
        AllowDrop = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.CacheText, true);

    }

    public Color BackgroundColor { get => backColor; set { backColor = value; Invalidate(); } }
    public Color InActiveTabBackColor { get => inActiveTabBackColor; set { inActiveTabBackColor = value; Invalidate(); } }
    public Color InActiveTabForeColor { get => inActiveTabForeColor; set { inActiveTabForeColor = value; Invalidate(); } }
    public Color Divider { get => divider; set { divider = value; Invalidate(); } }
    public int DividerSize { get => dividerSize; set { dividerSize = value; Invalidate(); } }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (!(e.KeyCode == Keys.Tab && e.Control))
        {
            base.OnKeyDown(e);
        }

    }




    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;
        g.Clear(BackgroundColor);

        Padding = new Point(20, 0);

        if (TabCount <= 0 || SelectedIndex < 0)
        {
            return;
        }
        Rectangle rect = GetTabRect(SelectedIndex);
        ITabPage selectedTabPage = (ITabPage)SelectedTab;
        if (Alignment is TabAlignment.Top or TabAlignment.Bottom)
        {
            g.FillRectangle(new SolidBrush(selectedTabPage.TabBackColor), 0, rect.Y + rect.Height, Width, DividerSize);
        }
        else
        {
            g.FillRectangle(new SolidBrush(selectedTabPage.TabBackColor), rect.X + rect.Width, 0, DividerSize, Height);

        }

        for (int i = 0; i < TabCount; i++)
        {
            Rectangle tabRect = GetTabRect(i);
            tabRect.Inflate(-2, 0);
            SizeF textSize = g.MeasureString(TabPages[i].Text, Font);

            ITabPage iTabPage = (ITabPage)TabPages[i];


            if (SelectedIndex == i || tabRect.Contains(mouseLoc))
            {
                g.FillRectangle(new SolidBrush(iTabPage.TabBackColor), tabRect);

                Rectangle close = new(tabRect.X + tabRect.Width - 20, tabRect.Y + tabRect.Height / 2 - 8, 16, 16);

                if (close.Contains(mouseLoc))
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(50, iTabPage.TabForeColor)), close);
                }
                if (TabPages[i] is ITabPage itp && itp.IsNotSaved && !close.Contains(mouseLoc))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.FillEllipse(new SolidBrush(iTabPage.TabForeColor), new(tabRect.X + tabRect.Width - 16, tabRect.Y + tabRect.Height / 2 - 4, 8, 8));
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                }
                else
                {
                    g.DrawLine(new Pen(iTabPage.TabForeColor), tabRect.X + tabRect.Width - 16, tabRect.Y + tabRect.Height / 2 - 4, tabRect.X + tabRect.Width - 8, tabRect.Y + tabRect.Height / 2 + 4);
                    g.DrawLine(new Pen(iTabPage.TabForeColor), tabRect.X + tabRect.Width - 16, tabRect.Y + tabRect.Height / 2 + 4, tabRect.X + tabRect.Width - 8, tabRect.Y + tabRect.Height / 2 - 4);

                }


                g.DrawString(TabPages[i].Text, Font, new SolidBrush(iTabPage.TabForeColor), new PointF(tabRect.X + 7, tabRect.Y + tabRect.Height / 2 - textSize.Height / 2 + 1));
            }
            else
            {
                g.FillRectangle(new SolidBrush(iTabPage.TabBackColor), new Rectangle(tabRect.X, tabRect.Y, 4, tabRect.Height));
                g.DrawString(TabPages[i].Text, Font, new SolidBrush(InActiveTabForeColor), new PointF(tabRect.X + 7, tabRect.Y + tabRect.Height / 2 - textSize.Height / 2 + 1));
                if (TabPages[i] is ITabPage itp && itp.IsNotSaved)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.FillEllipse(new SolidBrush(InActiveTabForeColor), new(tabRect.X + tabRect.Width - 16, tabRect.Y + tabRect.Height / 2 - 4, 8, 8));
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                }

            }

        }

    }

    private Point mouseLoc = new(-1, -1);

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        mouseLoc = e.Location;
        Invalidate();

    }
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        mouseLoc = new Point(-1, -1);
        Invalidate();
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is not ITabPage itp)
        {
            return;
        }

        itp.SavedStateChanged += (s, ev) => Invalidate();

    }
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        if (e.Control is not ITabPage itp)
        {
            return;
        }

        itp.SavedStateChanged -= (s, ev) => Invalidate();
    }





    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);



        for (int i = 0; i < TabCount; i++)
        {
            if (GetTabRect(i).Contains(e.Location))
            {
                Rectangle r = GetTabRect(i);

                Rectangle close = new(r.X + r.Width - 20, r.Y + r.Height / 2 - 8, 16, 16);

                if (close.Contains(mouseLoc))
                {
                    TabPage tab = TabPages[i];

                    if (tab is not ITabPage tabPage)
                    {
                        TabPages.Remove(tab);
                        return;
                    }

                    if (tabPage.IsNotSaved)
                    {
                        DialogResult dr = MessageBox.Show(this, $"Do you want to save changes in {Path.GetFileName(tabPage.Filename)}", "Minecraft Datapack Creator", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (dr == DialogResult.Cancel)
                            return;
                        if (dr == DialogResult.Yes)
                            tabPage.Save();
                    }
                    TabPages.Remove(tab);
                }
                return;
            }
        }
    }
}