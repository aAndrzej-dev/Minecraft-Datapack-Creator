using System.Windows.Forms.Integration;

namespace MinecraftDatapackCreator.Forms;

partial class GoToFileForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoToFileForm));
        host = new ElementHost();
        SuspendLayout();
        // 
        // host
        // 
        host.Dock = DockStyle.Fill;
        host.Location = new Point(0, 0);
        host.Name = "host";
        host.Size = new Size(514, 198);
        host.TabIndex = 0;
        // 
        // GoToFileForm
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(30, 30, 30);
        ClientSize = new Size(514, 198);
        Controls.Add(host);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "GoToFileForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Go To File";
        ResumeLayout(false);
    }

    #endregion


    private ElementHost host;
}