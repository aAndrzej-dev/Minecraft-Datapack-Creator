using System.Windows.Forms.Integration;

namespace MinecraftDatapackCreator.Forms;

partial class WorldSelectForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldSelectForm));
        host = new ElementHost();
        SuspendLayout();
        // 
        // host
        // 
        host.Dock = DockStyle.Fill;
        host.Location = new Point(0, 0);
        host.Name = "host";
        host.Size = new Size(800, 450);
        host.TabIndex = 0;
        // 
        // WorldSelectForm
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(30, 30, 30);
        ClientSize = new Size(800, 450);
        Controls.Add(host);
        ForeColor = Color.White;
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "WorldSelectForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Select World";
        ResumeLayout(false);
    }

    #endregion

    private ElementHost host;
}