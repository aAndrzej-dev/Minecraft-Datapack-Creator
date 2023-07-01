using System.IO;
using MinecraftDatapackCreator.Forms;

namespace MinecraftDatapackCreator;

internal sealed class MyTabControl : TabControl
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

    internal bool DisableSelect { get; set; }
    protected override void OnSelecting(TabControlCancelEventArgs e)
    {
        if (DisableSelect)
        {
            e.Cancel = true;
        }
        base.OnSelecting(e);
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
        EditorTabPage selectedTabPage = (EditorTabPage)SelectedTab;
        using SolidBrush selectedTabPageBackBrush = new SolidBrush(selectedTabPage.TabBackColor);
        if (Alignment is TabAlignment.Top or TabAlignment.Bottom)
        {
            g.FillRectangle(selectedTabPageBackBrush, 0, rect.Y + rect.Height, Width, DividerSize);
        }
        else
        {
            g.FillRectangle(selectedTabPageBackBrush, rect.X + rect.Width, 0, DividerSize, Height);

        }

        for (int i = 0; i < TabCount; i++)
        {
            Rectangle tabRect = GetTabRect(i);
            tabRect.Inflate(-2, 0);
            SizeF textSize = g.MeasureString(TabPages[i].Text, Font);

            EditorTabPage editorTabPage = (EditorTabPage)TabPages[i];

            using SolidBrush editorTabBackBrush = new SolidBrush(editorTabPage.TabBackColor);

            if (SelectedIndex == i || tabRect.Contains(mouseLoc))
            {
                using SolidBrush editorTabForeBrush = new SolidBrush(editorTabPage.TabForeColor);

                g.FillRectangle(editorTabBackBrush, tabRect);

                Rectangle close = new(tabRect.X + tabRect.Width - 20, tabRect.Y + (tabRect.Height / 2) - 8, 16, 16);

                if (close.Contains(mouseLoc))
                {
                    using SolidBrush hoverTabBackBrush = new SolidBrush(Color.FromArgb(50, editorTabPage.TabForeColor));
                    g.FillRectangle(hoverTabBackBrush, close);
                }
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                if (TabPages[i] is EditorTabPage itp && itp.IsNotSaved && !close.Contains(mouseLoc))
                {
                    g.FillEllipse(editorTabForeBrush, new(tabRect.X + tabRect.Width - 16, tabRect.Y + (tabRect.Height / 2) - 4, 8, 8));
                }
                else
                {
                    using Pen editorTabPageForePen = new Pen(editorTabPage.TabForeColor);

                    g.DrawLine(editorTabPageForePen, tabRect.X + tabRect.Width - 16, tabRect.Y + (tabRect.Height / 2) - 4, tabRect.X + tabRect.Width - 8, tabRect.Y + (tabRect.Height / 2) + 4);
                    g.DrawLine(editorTabPageForePen, tabRect.X + tabRect.Width - 16, tabRect.Y + (tabRect.Height / 2) + 4, tabRect.X + tabRect.Width - 8, tabRect.Y + (tabRect.Height / 2) - 4);

                }
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;


                g.DrawString(TabPages[i].Text, Font, editorTabForeBrush, new PointF(tabRect.X + 7, tabRect.Y + (tabRect.Height / 2) - (textSize.Height / 2) + 1));
            }
            else
            {
                using SolidBrush inactiveTabForeBrush = new SolidBrush(InActiveTabForeColor);

                g.FillRectangle(editorTabBackBrush, new Rectangle(tabRect.X, tabRect.Y, 4, tabRect.Height));
                g.DrawString(TabPages[i].Text, base.Font, inactiveTabForeBrush, new PointF(tabRect.X + 7, tabRect.Y + (tabRect.Height / 2) - (textSize.Height / 2) + 1));
                if (TabPages[i] is EditorTabPage itp && itp.IsNotSaved)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.FillEllipse(inactiveTabForeBrush, new(tabRect.X + tabRect.Width - 16, tabRect.Y + (tabRect.Height / 2) - 4, 8, 8));
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
        if (e.Control is not EditorTabPage itp)
        {
            return;
        }

        itp.SavedStateChanged += (s, ev) => Invalidate();

    }
    protected override void OnControlRemoved(ControlEventArgs e)
    {
        base.OnControlRemoved(e);
        if (e.Control is not EditorTabPage itp)
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

                Rectangle close = new(r.X + r.Width - 20, r.Y + (r.Height / 2) - 8, 16, 16);

                if (close.Contains(mouseLoc))
                {
                    TabPage tab = TabPages[i];

                    if (tab is not EditorTabPage tabPage)
                    {
                        TabPages.Remove(tab);
                        return;
                    }

                    if (tabPage.IsNotSaved)
                    {
                        string[] array = new string[1] { Path.GetRelativePath(tabPage.FileInfo.Datapack.Path, tabPage.FileInfo.FullName) };
                        using SaveFilesForm sff = new SaveFilesForm(array);
                        DialogResult dr = sff.ShowDialog();
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