namespace MinecraftDatapackCreator.Forms
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            label1 = new Label();
            fontDialog = new FontDialog();
            btnTextEditorFont = new Button();
            label2 = new Label();
            btnJtfEditorFont = new Button();
            cbJsonFormatting = new CheckBox();
            label3 = new Label();
            txtMinecraftDir = new TextBox();
            btnMinecraftDir = new Button();
            fbdMinecraftDir = new FolderBrowserDialog();
            btnStructureFolder = new Button();
            label4 = new Label();
            txtStructureFolder = new TextBox();
            fbdStructureFolder = new FolderBrowserDialog();
            cbFileFullPath = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            cbDataAutoUpdate = new CheckBox();
            btnDataUpdate = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 25);
            label1.Margin = new Padding(16, 16, 8, 8);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 0;
            label1.Text = "Text editor font";
            // 
            // btnTextEditorFont
            // 
            btnTextEditorFont.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnTextEditorFont.FlatStyle = FlatStyle.Flat;
            btnTextEditorFont.Font = new Font("Segoe UI", 9F);
            btnTextEditorFont.Location = new Point(123, 21);
            btnTextEditorFont.Name = "btnTextEditorFont";
            btnTextEditorFont.Size = new Size(350, 23);
            btnTextEditorFont.TabIndex = 0;
            btnTextEditorFont.UseVisualStyleBackColor = true;
            btnTextEditorFont.Click += BtnTextEditorFont_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 56);
            label2.Margin = new Padding(16, 8, 3, 8);
            label2.Name = "label2";
            label2.Size = new Size(89, 15);
            label2.TabIndex = 0;
            label2.Text = "Json editor font";
            // 
            // btnJtfEditorFont
            // 
            btnJtfEditorFont.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnJtfEditorFont.FlatStyle = FlatStyle.Flat;
            btnJtfEditorFont.Location = new Point(123, 52);
            btnJtfEditorFont.Name = "btnJtfEditorFont";
            btnJtfEditorFont.Size = new Size(350, 23);
            btnJtfEditorFont.TabIndex = 1;
            btnJtfEditorFont.UseVisualStyleBackColor = true;
            btnJtfEditorFont.Click += BtnJtfEditorFont_Click;
            // 
            // cbJsonFormatting
            // 
            cbJsonFormatting.AutoSize = true;
            cbJsonFormatting.Location = new Point(25, 145);
            cbJsonFormatting.Margin = new Padding(16, 8, 3, 8);
            cbJsonFormatting.Name = "cbJsonFormatting";
            cbJsonFormatting.Size = new Size(214, 19);
            cbJsonFormatting.TabIndex = 3;
            cbJsonFormatting.Text = "Remove whitespaces form json files";
            cbJsonFormatting.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 85);
            label3.Margin = new Padding(16, 8, 3, 8);
            label3.Name = "label3";
            label3.Size = new Size(94, 15);
            label3.TabIndex = 0;
            label3.Text = "Minecraft Folder";
            // 
            // txtMinecraftDir
            // 
            txtMinecraftDir.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtMinecraftDir.BackColor = Color.FromArgb(30, 30, 30);
            txtMinecraftDir.BorderStyle = BorderStyle.FixedSingle;
            txtMinecraftDir.ForeColor = Color.White;
            txtMinecraftDir.Location = new Point(123, 82);
            txtMinecraftDir.Name = "txtMinecraftDir";
            txtMinecraftDir.Size = new Size(308, 23);
            txtMinecraftDir.TabIndex = 6;
            // 
            // btnMinecraftDir
            // 
            btnMinecraftDir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinecraftDir.FlatStyle = FlatStyle.Flat;
            btnMinecraftDir.Location = new Point(437, 82);
            btnMinecraftDir.Name = "btnMinecraftDir";
            btnMinecraftDir.Size = new Size(33, 23);
            btnMinecraftDir.TabIndex = 1;
            btnMinecraftDir.Text = "...";
            btnMinecraftDir.UseVisualStyleBackColor = true;
            btnMinecraftDir.Click += BtnMinecraftDir_Click;
            // 
            // fbdMinecraftDir
            // 
            fbdMinecraftDir.ShowNewFolderButton = false;
            // 
            // btnStructureFolder
            // 
            btnStructureFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStructureFolder.FlatStyle = FlatStyle.Flat;
            btnStructureFolder.Location = new Point(437, 111);
            btnStructureFolder.Name = "btnStructureFolder";
            btnStructureFolder.Size = new Size(33, 23);
            btnStructureFolder.TabIndex = 1;
            btnStructureFolder.Text = "...";
            btnStructureFolder.UseVisualStyleBackColor = true;
            btnStructureFolder.Click += BtnStructureFolder_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 114);
            label4.Margin = new Padding(16, 8, 3, 8);
            label4.Name = "label4";
            label4.Size = new Size(170, 15);
            label4.TabIndex = 0;
            label4.Text = "Datapack Structure Data Folder";
            // 
            // txtStructureFolder
            // 
            txtStructureFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtStructureFolder.BackColor = Color.FromArgb(30, 30, 30);
            txtStructureFolder.BorderStyle = BorderStyle.FixedSingle;
            txtStructureFolder.ForeColor = Color.White;
            txtStructureFolder.Location = new Point(201, 111);
            txtStructureFolder.Name = "txtStructureFolder";
            txtStructureFolder.Size = new Size(230, 23);
            txtStructureFolder.TabIndex = 6;
            txtStructureFolder.TextChanged += TxtStructureFolder_TextChanged;
            // 
            // fbdStructureFolder
            // 
            fbdStructureFolder.ShowNewFolderButton = false;
            // 
            // cbFileFullPath
            // 
            cbFileFullPath.AutoSize = true;
            cbFileFullPath.Location = new Point(25, 171);
            cbFileFullPath.Margin = new Padding(16, 8, 3, 8);
            cbFileFullPath.Name = "cbFileFullPath";
            cbFileFullPath.Size = new Size(214, 19);
            cbFileFullPath.TabIndex = 3;
            cbFileFullPath.Text = "Always show full file path in dialogs";
            cbFileFullPath.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.BackColor = Color.RoyalBlue;
            btnSave.FlatAppearance.BorderColor = Color.RoyalBlue;
            btnSave.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Location = new Point(297, 376);
            btnSave.Margin = new Padding(3, 4, 8, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 26);
            btnSave.TabIndex = 12;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(122, 122, 122);
            btnCancel.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
            btnCancel.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(388, 376);
            btnCancel.Margin = new Padding(3, 4, 8, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 26);
            btnCancel.TabIndex = 13;
            btnCancel.Text = Properties.Resources.BtnCancel;
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbDataAutoUpdate
            // 
            cbDataAutoUpdate.AutoSize = true;
            cbDataAutoUpdate.Location = new Point(25, 236);
            cbDataAutoUpdate.Name = "cbDataAutoUpdate";
            cbDataAutoUpdate.Size = new Size(223, 19);
            cbDataAutoUpdate.TabIndex = 14;
            cbDataAutoUpdate.Text = "Auto Update Datapack Structure Data";
            cbDataAutoUpdate.UseVisualStyleBackColor = true;
            cbDataAutoUpdate.Visible = false;
            // 
            // btnDataUpdate
            // 
            btnDataUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDataUpdate.FlatStyle = FlatStyle.Flat;
            btnDataUpdate.Location = new Point(308, 233);
            btnDataUpdate.Name = "btnDataUpdate";
            btnDataUpdate.Size = new Size(165, 23);
            btnDataUpdate.TabIndex = 1;
            btnDataUpdate.Text = "Check For Updates";
            btnDataUpdate.UseVisualStyleBackColor = true;
            btnDataUpdate.Visible = false;
            //btnDataUpdate.Click += BtnDataUpdate_Click;
            // 
            // SettingsForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(30, 30, 30);
            CancelButton = btnCancel;
            ClientSize = new Size(485, 419);
            Controls.Add(cbDataAutoUpdate);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(txtStructureFolder);
            Controls.Add(txtMinecraftDir);
            Controls.Add(label4);
            Controls.Add(cbFileFullPath);
            Controls.Add(cbJsonFormatting);
            Controls.Add(btnDataUpdate);
            Controls.Add(btnStructureFolder);
            Controls.Add(label3);
            Controls.Add(btnMinecraftDir);
            Controls.Add(btnJtfEditorFont);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnTextEditorFont);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnJtfEditorFont;
        private Label label2;
        private Button btnTextEditorFont;
        private Label label1;
        private FontDialog fontDialog;
        private CheckBox cbJsonFormatting;
        private Label label3;
        private TextBox txtMinecraftDir;
        private Button btnMinecraftDir;
        private FolderBrowserDialog fbdMinecraftDir;
        private Button btnStructureFolder;
        private Label label4;
        private TextBox txtStructureFolder;
        private FolderBrowserDialog fbdStructureFolder;
        private CheckBox cbFileFullPath;
        private Button btnSave;
        private Button btnCancel;
        private CheckBox cbDataAutoUpdate;
        private Button btnDataUpdate;
    }
}