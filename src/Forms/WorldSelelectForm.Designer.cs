namespace MinecraftDatapackCreator.Forms;

partial class WorldSelelectForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.lbWorlds = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbWorlds
            // 
            this.lbWorlds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.lbWorlds.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbWorlds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbWorlds.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbWorlds.ForeColor = System.Drawing.Color.White;
            this.lbWorlds.FormattingEnabled = true;
            this.lbWorlds.ItemHeight = 24;
            this.lbWorlds.Location = new System.Drawing.Point(0, 0);
            this.lbWorlds.Name = "lbWorlds";
            this.lbWorlds.Size = new System.Drawing.Size(800, 450);
            this.lbWorlds.TabIndex = 2;
            this.lbWorlds.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LbWorlds_DrawItem);
            this.lbWorlds.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LbWorlds_MouseDoubleClick);
            // 
            // WorldSelelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbWorlds);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "WorldSelelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select World";
            this.ResumeLayout(false);

    }

    #endregion
    private ListBox lbWorlds;
}