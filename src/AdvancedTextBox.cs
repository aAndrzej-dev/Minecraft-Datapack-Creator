using System.Diagnostics;

namespace MinecraftDatapackCreator;
internal sealed class AdvancedTextBox : RichTextBox
{
    public AdvancedTextBox()
    {

    }
    [DebuggerStepThrough]
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 15) //WM_PAINT
        {
            base.WndProc(ref m);
            Graphics g = Graphics.FromHwnd(Handle);
            DrawSelectedLineBackGround(g);
        }
        else
            base.WndProc(ref m);
    }
    protected override void OnSelectionChanged(EventArgs e)
    {
        base.OnSelectionChanged(e);
        Invalidate();


        string selectedWord = SelectedText;
        if (selectedWord.Length < 3)
            return;


    }
    private void DrawSelectedLineBackGround(Graphics g)
    {
        if (SelectionLength != 0)
            return;

        int selectedLine = GetLineFromCharIndex(SelectionStart);

        int lineHeight = FontHeight;

        Point linePos = GetPositionFromCharIndex(GetFirstCharIndexFromLine(selectedLine));

        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        using Pen pen = new Pen(Color.FromArgb(30, ForeColor), 2);
        g.DrawLine(pen, 0, linePos.Y + 1, Width, linePos.Y + 1);
        g.DrawLine(pen, 0, linePos.Y + lineHeight, Width, linePos.Y + lineHeight);

    }
    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
        base.OnMouseDoubleClick(e);
        SelectWordWithChar(GetCharIndexFromPosition(e.Location));
    }

    private void SelectWordWithChar(int charIndex)
    {
        int startPos = charIndex;
        int c = Text[charIndex];

        if (c is (not >= 48 or not <= 57) and (not >= 65 or not <= 90) and (not >= 97 or not <= 122))
        {
            Select(charIndex, 1);
            return;
        }
        while (true)
        {
            if (startPos == 0)
                break;
            int cc = Text[startPos - 1];

            if (cc is (>= 48 and <= 57) or (>= 65 and <= 90) or (>= 97 and <= 122))
            {
                startPos--;
            }
            else
                break;
        }
        int endPos = charIndex;
        while (true)
        {
            if (endPos == Text.Length - 1)
                break;
            int cc = Text[endPos + 1];

            if (cc is (>= 48 and <= 57) or (>= 65 and <= 90) or (>= 97 and <= 122))
            {
                endPos++;
            }
            else
                break;
        }
        if (startPos != endPos + 1)
        {
            Select(startPos, endPos - startPos + 1);
        }
    }
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Control && e.KeyCode == Keys.D)
        {
            int selectionStart = SelectionStart;
            int selectionLength = SelectionLength;
            if (selectionLength == 0)
            {
                int linestart = GetFirstCharIndexOfCurrentLine();
                int lineEnd = GetFirstCharIndexFromLine(GetLineFromCharIndex(selectionStart) + 1);
                if (lineEnd == -1)
                    lineEnd = Text.Length;
                Text = Text.Insert(lineEnd, Text.AsSpan(linestart, Math.Min(lineEnd - linestart, Text.Length)).ToString());
                SelectionStart = lineEnd + lineEnd - linestart - 1;
                SelectionLength = 0;
            }
            else
            {
                int selectionEnd = selectionStart + selectionLength;
                Text = Text.Insert(selectionEnd, SelectedText);
                SelectionStart = selectionEnd;
                SelectionLength = selectionLength;
            }
        }
    }
}
