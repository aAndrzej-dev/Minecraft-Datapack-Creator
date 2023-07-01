namespace MinecraftDatapackCreator.Forms;

partial class SaveFilesForm
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
        label1 = new Label();
        lbFiles = new ListBox();
        btnCancel = new Button();
        btnNo = new Button();
        btnYes = new Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(17, 9);
        label1.Name = "label1";
        label1.Size = new Size(38, 15);
        label1.TabIndex = 0;
        label1.Text = "label1";
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
        lbFiles.Location = new Point(17, 30);
        lbFiles.Margin = new Padding(8, 6, 8, 0);
        lbFiles.Name = "lbFiles";
        lbFiles.Size = new Size(350, 175);
        lbFiles.Sorted = true;
        lbFiles.TabIndex = 0;
        lbFiles.DrawItem += LbFiles_DrawItem;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(122, 122, 122);
        btnCancel.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
        btnCancel.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
        btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(287, 218);
        btnCancel.Margin = new Padding(3, 4, 8, 8);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(80, 26);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // btnNo
        // 
        btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnNo.FlatAppearance.BorderColor = Color.FromArgb(122, 122, 122);
        btnNo.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
        btnNo.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
        btnNo.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
        btnNo.FlatStyle = FlatStyle.Flat;
        btnNo.Location = new Point(196, 218);
        btnNo.Margin = new Padding(3, 4, 8, 8);
        btnNo.Name = "btnNo";
        btnNo.Size = new Size(80, 26);
        btnNo.TabIndex = 2;
        btnNo.Text = "Don't save";
        btnNo.UseVisualStyleBackColor = true;
        btnNo.Click += BtnNo_Click;
        // 
        // btnYes
        // 
        btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnYes.FlatAppearance.BorderColor = Color.FromArgb(122, 122, 122);
        btnYes.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
        btnYes.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
        btnYes.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
        btnYes.FlatStyle = FlatStyle.Flat;
        btnYes.Location = new Point(105, 218);
        btnYes.Margin = new Padding(3, 4, 8, 8);
        btnYes.Name = "btnYes";
        btnYes.Size = new Size(80, 26);
        btnYes.TabIndex = 1;
        btnYes.Text = "Save";
        btnYes.UseVisualStyleBackColor = true;
        btnYes.Click += BtnYes_Click;
        // 
        // SaveFilesForm
        // 
        AcceptButton = btnYes;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(30, 30, 30);
        CancelButton = btnCancel;
        ClientSize = new Size(384, 261);
        Controls.Add(btnYes);
        Controls.Add(btnNo);
        Controls.Add(btnCancel);
        Controls.Add(lbFiles);
        Controls.Add(label1);
        ForeColor = Color.White;
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(400, 300);
        Name = "SaveFilesForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Save Files";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private ListBox lbFiles;
    private Button btnCancel;
    private Button btnNo;
    private Button btnYes;
}