﻿namespace MinecraftDatapackCreator.Forms;

public class DarkToolStripRenderer : ToolStripProfessionalRenderer
{
    public DarkToolStripRenderer() : base(new DarkColorTable())
    {

    }
    private class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(80, 80, 80);
        public override Color MenuBorder => Color.FromArgb(30, 30, 30);
        public override Color MenuItemBorder => Color.FromArgb(30, 30, 30);
        public override Color ToolStripDropDownBackground => Color.FromArgb(50, 50, 50);
        public override Color ImageMarginGradientBegin => Color.FromArgb(50, 50, 50);
        public override Color ImageMarginGradientEnd => Color.FromArgb(50, 50, 50);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(50, 50, 50);
        public override Color MenuStripGradientBegin => Color.FromArgb(50, 50, 50);
        public override Color MenuStripGradientEnd => Color.FromArgb(50, 50, 50);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(50, 50, 50);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(50, 50, 50);
        public override Color MenuItemPressedGradientMiddle => Color.FromArgb(50, 50, 50);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(50, 50, 50);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(50, 50, 50);
        public override Color SeparatorLight => Color.FromArgb(100, 100, 100);
        public override Color ButtonSelectedGradientBegin => Color.FromArgb(80, 80, 80);
        public override Color ButtonSelectedGradientMiddle => Color.FromArgb(80, 80, 80);
        public override Color ButtonSelectedGradientEnd => Color.FromArgb(80, 80, 80);

    }
}