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
            this.label1 = new System.Windows.Forms.Label();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.btnTextEditorFont = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnJtfEditorFont = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbJsonFormatting = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMinecraftDir = new System.Windows.Forms.TextBox();
            this.btnMinecraftDir = new System.Windows.Forms.Button();
            this.fbdMinecraftDir = new System.Windows.Forms.FolderBrowserDialog();
            this.btnStructureFolder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStructureFolder = new System.Windows.Forms.TextBox();
            this.fbdStructureFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(16, 16, 8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text editor font";
            // 
            // btnTextEditorFont
            // 
            this.btnTextEditorFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTextEditorFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTextEditorFont.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTextEditorFont.Location = new System.Drawing.Point(123, 21);
            this.btnTextEditorFont.Name = "btnTextEditorFont";
            this.btnTextEditorFont.Size = new System.Drawing.Size(350, 23);
            this.btnTextEditorFont.TabIndex = 0;
            this.btnTextEditorFont.UseVisualStyleBackColor = true;
            this.btnTextEditorFont.Click += new System.EventHandler(this.BtnTextEditorFont_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(16, 8, 3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Json editor font";
            // 
            // btnJtfEditorFont
            // 
            this.btnJtfEditorFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJtfEditorFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJtfEditorFont.Location = new System.Drawing.Point(123, 52);
            this.btnJtfEditorFont.Name = "btnJtfEditorFont";
            this.btnJtfEditorFont.Size = new System.Drawing.Size(350, 23);
            this.btnJtfEditorFont.TabIndex = 1;
            this.btnJtfEditorFont.UseVisualStyleBackColor = true;
            this.btnJtfEditorFont.Click += new System.EventHandler(this.BtnJtfEditorFont_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(317, 384);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "OK";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(398, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbJsonFormatting
            // 
            this.cbJsonFormatting.AutoSize = true;
            this.cbJsonFormatting.Location = new System.Drawing.Point(25, 145);
            this.cbJsonFormatting.Margin = new System.Windows.Forms.Padding(16, 8, 3, 8);
            this.cbJsonFormatting.Name = "cbJsonFormatting";
            this.cbJsonFormatting.Size = new System.Drawing.Size(214, 19);
            this.cbJsonFormatting.TabIndex = 3;
            this.cbJsonFormatting.Text = "Remove whitespaces form json files";
            this.cbJsonFormatting.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(16, 8, 3, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Minecraft Folder";
            // 
            // txtMinecraftDir
            // 
            this.txtMinecraftDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinecraftDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtMinecraftDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMinecraftDir.ForeColor = System.Drawing.Color.White;
            this.txtMinecraftDir.Location = new System.Drawing.Point(123, 82);
            this.txtMinecraftDir.Name = "txtMinecraftDir";
            this.txtMinecraftDir.Size = new System.Drawing.Size(308, 23);
            this.txtMinecraftDir.TabIndex = 6;
            // 
            // btnMinecraftDir
            // 
            this.btnMinecraftDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinecraftDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinecraftDir.Location = new System.Drawing.Point(437, 82);
            this.btnMinecraftDir.Name = "btnMinecraftDir";
            this.btnMinecraftDir.Size = new System.Drawing.Size(33, 23);
            this.btnMinecraftDir.TabIndex = 1;
            this.btnMinecraftDir.Text = "...";
            this.btnMinecraftDir.UseVisualStyleBackColor = true;
            this.btnMinecraftDir.Click += new System.EventHandler(this.BtnMinecraftDir_Click);
            // 
            // fbdMinecraftDir
            // 
            this.fbdMinecraftDir.ShowNewFolderButton = false;
            // 
            // btnStructureFolder
            // 
            this.btnStructureFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStructureFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStructureFolder.Location = new System.Drawing.Point(437, 111);
            this.btnStructureFolder.Name = "btnStructureFolder";
            this.btnStructureFolder.Size = new System.Drawing.Size(33, 23);
            this.btnStructureFolder.TabIndex = 1;
            this.btnStructureFolder.Text = "...";
            this.btnStructureFolder.UseVisualStyleBackColor = true;
            this.btnStructureFolder.Click += new System.EventHandler(this.BtnStructureFolder_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 114);
            this.label4.Margin = new System.Windows.Forms.Padding(16, 8, 3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Datapack Structure Data Folder";
            // 
            // txtStructureFolder
            // 
            this.txtStructureFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStructureFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtStructureFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStructureFolder.ForeColor = System.Drawing.Color.White;
            this.txtStructureFolder.Location = new System.Drawing.Point(201, 111);
            this.txtStructureFolder.Name = "txtStructureFolder";
            this.txtStructureFolder.Size = new System.Drawing.Size(230, 23);
            this.txtStructureFolder.TabIndex = 6;
            // 
            // fbdStructureFolder
            // 
            this.fbdStructureFolder.ShowNewFolderButton = false;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(485, 419);
            this.Controls.Add(this.txtStructureFolder);
            this.Controls.Add(this.txtMinecraftDir);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbJsonFormatting);
            this.Controls.Add(this.btnStructureFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnMinecraftDir);
            this.Controls.Add(this.btnJtfEditorFont);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTextEditorFont);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnJtfEditorFont;
        private Label label2;
        private Button btnTextEditorFont;
        private Label label1;
        private FontDialog fontDialog;
        private Button btnSave;
        private Button btnCancel;
        private CheckBox cbJsonFormatting;
        private Label label3;
        private TextBox txtMinecraftDir;
        private Button btnMinecraftDir;
        private FolderBrowserDialog fbdMinecraftDir;
        private Button btnStructureFolder;
        private Label label4;
        private TextBox txtStructureFolder;
        private FolderBrowserDialog fbdStructureFolder;
    }
}