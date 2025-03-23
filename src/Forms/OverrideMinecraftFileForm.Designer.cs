namespace MinecraftDatapackCreator.Forms;

partial class OverrideMinecraftFileForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverrideMinecraftFileForm));
        btnAddOverride = new Button();
        btnCancel = new Button();
        lbFiles = new ListBox();
        label1 = new Label();
        SuspendLayout();
        // 
        // btnAddOverride
        // 
        btnAddOverride.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAddOverride.BackColor = Color.RoyalBlue;
        btnAddOverride.FlatAppearance.BorderColor = Color.RoyalBlue;
        btnAddOverride.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
        btnAddOverride.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
        btnAddOverride.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
        btnAddOverride.FlatStyle = FlatStyle.Flat;
        btnAddOverride.Location = new Point(484, 443);
        btnAddOverride.Margin = new Padding(3, 4, 8, 8);
        btnAddOverride.Name = "btnAddOverride";
        btnAddOverride.Size = new Size(114, 26);
        btnAddOverride.TabIndex = 6;
        btnAddOverride.Text = "Add Override";
        btnAddOverride.UseVisualStyleBackColor = false;
        btnAddOverride.Click += BtnAddOverride_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(122, 122, 122);
        btnCancel.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
        btnCancel.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
        btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(609, 443);
        btnCancel.Margin = new Padding(3, 4, 8, 8);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(80, 26);
        btnCancel.TabIndex = 8;
        btnCancel.Text = Properties.Resources.BtnCancel;
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // lbFiles
        // 
        lbFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lbFiles.BackColor = Color.FromArgb(40, 40, 40);
        lbFiles.BorderStyle = BorderStyle.FixedSingle;
        lbFiles.DrawMode = DrawMode.OwnerDrawFixed;
        lbFiles.ForeColor = Color.White;
        lbFiles.FormattingEnabled = true;
        lbFiles.IntegralHeight = false;
        lbFiles.ItemHeight = 24;
        lbFiles.Location = new Point(17, 34);
        lbFiles.Margin = new Padding(8, 6, 8, 0);
        lbFiles.Name = "lbFiles";
        lbFiles.Size = new Size(672, 396);
        lbFiles.TabIndex = 4;
        lbFiles.DrawItem += LbFiles_DrawItem;
        lbFiles.MouseDoubleClick += LbFiles_MouseDoubleClick;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(17, 13);
        label1.Name = "label1";
        label1.Size = new Size(117, 15);
        label1.TabIndex = 5;
        label1.Text = "Select file to override";
        // 
        // OverrideMinecraftFileForm
        // 
        AcceptButton = btnAddOverride;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(30, 30, 30);
        CancelButton = btnCancel;
        ClientSize = new Size(706, 482);
        Controls.Add(btnAddOverride);
        Controls.Add(btnCancel);
        Controls.Add(lbFiles);
        Controls.Add(label1);
        ForeColor = Color.White;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MinimizeBox = false;
        Name = "OverrideMinecraftFileForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Override Minecraft File";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnAddOverride;
    private Button btnCancel;
    private ListBox lbFiles;
    private Label label1;
}