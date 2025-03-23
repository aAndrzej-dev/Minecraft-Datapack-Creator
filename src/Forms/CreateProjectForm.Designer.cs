
namespace MinecraftDatapackCreator.Forms
{
    partial class CreateProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateProjectForm));
            folderBrowserDialog = new FolderBrowserDialog();
            btnFiles = new Button();
            txtPath = new TextBox();
            txtName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            txtNamespace = new TextBox();
            btnCreate = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // btnFiles
            // 
            btnFiles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFiles.FlatAppearance.BorderColor = Color.FromArgb(122, 122, 122);
            btnFiles.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
            btnFiles.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            btnFiles.FlatStyle = FlatStyle.Flat;
            btnFiles.ImageAlign = ContentAlignment.TopCenter;
            btnFiles.Location = new Point(411, 40);
            btnFiles.Name = "btnFiles";
            btnFiles.Size = new Size(45, 23);
            btnFiles.TabIndex = 2;
            btnFiles.Text = "...";
            btnFiles.UseCompatibleTextRendering = true;
            btnFiles.UseVisualStyleBackColor = true;
            btnFiles.TextChanged += BtnFiles_Click;
            btnFiles.Click += BtnFiles_Click;
            // 
            // txtPath
            // 
            txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPath.BackColor = Color.FromArgb(50, 50, 50);
            txtPath.BorderStyle = BorderStyle.FixedSingle;
            txtPath.ForeColor = Color.White;
            txtPath.Location = new Point(61, 40);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(344, 23);
            txtPath.TabIndex = 1;
            txtPath.TextChanged += TxtName_TextChanged;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.BackColor = Color.FromArgb(50, 50, 50);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.CharacterCasing = CharacterCasing.Lower;
            txtName.ForeColor = Color.White;
            txtName.Location = new Point(61, 11);
            txtName.Name = "txtName";
            txtName.Size = new Size(395, 23);
            txtName.TabIndex = 0;
            txtName.TextChanged += TxtName_TextChanged;
            txtName.KeyPress += TxtName_KeyPress;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(16, 44);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 8;
            label2.Text = "Path";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(16, 13);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 9;
            label1.Text = "Name";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(16, 71);
            label3.Name = "label3";
            label3.Size = new Size(102, 15);
            label3.TabIndex = 8;
            label3.Text = "Namespace name";
            // 
            // txtNamespace
            // 
            txtNamespace.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNamespace.BackColor = Color.FromArgb(50, 50, 50);
            txtNamespace.BorderStyle = BorderStyle.FixedSingle;
            txtNamespace.ForeColor = Color.White;
            txtNamespace.Location = new Point(124, 69);
            txtNamespace.Name = "txtNamespace";
            txtNamespace.Size = new Size(332, 23);
            txtNamespace.TabIndex = 3;
            txtNamespace.TextChanged += TxtNamespace_TextChanged;
            txtNamespace.KeyPress += TxtName_KeyPress;
            // 
            // btnCreate
            // 
            btnCreate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCreate.BackColor = Color.RoyalBlue;
            btnCreate.FlatAppearance.BorderColor = Color.RoyalBlue;
            btnCreate.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
            btnCreate.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
            btnCreate.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.Location = new Point(285, 114);
            btnCreate.Margin = new Padding(3, 4, 8, 8);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(80, 26);
            btnCreate.TabIndex = 10;
            btnCreate.Text = "Create";
            btnCreate.UseVisualStyleBackColor = false;
            btnCreate.Click += BtnCreate_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(122, 122, 122);
            btnCancel.FlatAppearance.CheckedBackColor = Color.FromArgb(64, 64, 64);
            btnCancel.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 64, 64);
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(376, 114);
            btnCancel.Margin = new Padding(3, 4, 8, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 26);
            btnCancel.TabIndex = 11;
            btnCancel.Text = Properties.Resources.BtnCancel;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // CreateProjectForm
            // 
            AcceptButton = btnCreate;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(30, 30, 30);
            CancelButton = btnCancel;
            ClientSize = new Size(473, 157);
            Controls.Add(btnCreate);
            Controls.Add(btnCancel);
            Controls.Add(btnFiles);
            Controls.Add(txtNamespace);
            Controls.Add(txtPath);
            Controls.Add(label3);
            Controls.Add(txtName);
            Controls.Add(label2);
            Controls.Add(label1);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CreateProjectForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Create New Project";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FolderBrowserDialog folderBrowserDialog;
        private Button btnFiles;
        private TextBox txtPath;
        private TextBox txtName;
        private Label label2;
        private Label label1;
        private Label label3;
        private TextBox txtNamespace;
        private Button btnCreate;
        private Button btnCancel;
    }
}