using System.Diagnostics;

namespace MinecraftDatapackCreator;
internal sealed class AdvancedTextBox : RichTextBox
{
    public AdvancedTextBox()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        
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

        float lineHeight = FontHeight * ZoomFactor;

        Point linePos = GetPositionFromCharIndex(GetFirstCharIndexFromLine(selectedLine));

        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using Pen pen = new Pen(Color.FromArgb(30, ForeColor), 2);
        g.DrawLine(pen, 0, linePos.Y, Width, linePos.Y);
        g.DrawLine(pen, 0, linePos.Y + lineHeight, Width, linePos.Y + lineHeight);

    }
    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
        base.OnMouseDoubleClick(e);
        SelectWordWithChar(GetCharIndexFromPosition(e.Location));
    }

    private void SelectWordWithChar(int charIndex)
    {
        if (Text.Length == 0)
            return;
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
                int lineStart = GetFirstCharIndexOfCurrentLine();
                int lineEnd = GetFirstCharIndexFromLine(GetLineFromCharIndex(selectionStart) + 1);
                if (lineEnd == -1)
                    lineEnd = Text.Length;
                ReadOnlySpan<char> textSpan = Text.AsSpan(lineStart, Math.Min(lineEnd - lineStart, Text.Length));

                if (!textSpan.EndsWith("\n"))
                {
                    Text = Text.Insert(lineEnd, "\n" + textSpan.ToString());
                    lineEnd++;
                }
                else
                    Text = Text.Insert(lineEnd, textSpan.ToString());
                SelectionStart = lineEnd + lineEnd - lineStart - 1;
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
        if (e.Alt && e.KeyCode is Keys.Up or Keys.Down)
        {
            bool up = e.KeyCode is Keys.Up;
            int selectionStart = SelectionStart;
            int selectionLength = SelectionLength;
            string[] lines = Lines;


            int startLineIndex = GetLineFromCharIndex(selectionStart);
            int endLineIndex = GetLineFromCharIndex(selectionStart + selectionLength);

            int selectionStartCharPos = selectionStart - GetFirstCharIndexFromLine(startLineIndex);

            if ((up && startLineIndex == 0) || (!up && endLineIndex == lines.Length - 1))
                return;

            int replacementLineIndex = up ? startLineIndex - 1 : endLineIndex + 1;
            string replacementText = lines[replacementLineIndex];

            int dir = up ? -1 : 1;


           
            if (up)
            {
                for (int i = 0; i <= endLineIndex - startLineIndex; i++)
                {
                    lines[i + startLineIndex + dir] = lines[i + startLineIndex];
                }
                lines[endLineIndex] = replacementText;
            }
            else
            {
                for (int i = endLineIndex - startLineIndex; i >= 0; i--)
                {
                    lines[i + startLineIndex + dir] = lines[i + startLineIndex];
                }
                lines[startLineIndex] = replacementText;
            }

            Lines = lines;

            Select(GetFirstCharIndexFromLine(dir + startLineIndex) + selectionStartCharPos, selectionLength);
        }

    }
}
